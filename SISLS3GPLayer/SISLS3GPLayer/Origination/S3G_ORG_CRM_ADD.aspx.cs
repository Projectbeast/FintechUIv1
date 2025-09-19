#region Namespaces

using System;
using System.Web;
using System.Data;
using System.Text;
using S3GBusEntity.Collection;
using S3GBusEntity;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.Security;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using System.ServiceModel;
using AjaxControlToolkit;

#endregion

public partial class Origination_S3G_ORG_CRM_ADD : ApplyThemeForProject
{
    #region Common Variable declaration
    int intCompanyID, intUserID = 0;
    Dictionary<string, string> ObjDictionary = null;
    int intErrCode = 0;
    int intFollowUpId = 0;
    UserInfo ObjUserInfo = new UserInfo();
    SerializationMode ObjSerMode = SerializationMode.Binary;
    string strDateFormat = string.Empty;
    //string strMode = string.Empty;
    static string strPageName = "Follow Up Instructions";
    static string strSuffix = "";
    FormsAuthenticationTicket Ticket;
    static int intTicketNo = 1;
    //int intTicket = 0;

    ClnDataMgtServicesReference.ClnDataMgtServicesClient objFollowUp_Client;
    S3GBusEntity.Collection.ClnDataMgtServices.S3G_CLN_CRM_HdrDataTable objS3G_CLN_FollowUpDataTable = null;
    S3GBusEntity.Collection.ClnDataMgtServices.S3G_CLN_CRM_HdrRow objS3G_CLN_FollowUpRow = null;

    S3GBusEntity.Collection.ClnDataMgtServices.S3G_CLN_ENQUIRYDataTable objS3G_CLN_EnquiryDataTable = null;
    S3GBusEntity.Collection.ClnDataMgtServices.S3G_CLN_ENQUIRYRow objS3G_CLN_EnquiryRow = null;


    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    string strRedirectPage = "~/Origination/S3G_ORG_CRM_ADD.aspx?qsMode=C";
    string strRedirectPageAdd = "window.location.href='../Origination/S3G_ORG_CRM_ADD.aspx?qsMode=C'";
    string strRedirectPageView = "window.location.href='../Origination/S3GOrgCRM_View.aspx?Code=CRM';";
    string strErrorMessagePrefix = @"Correct the following validation(s): </br></br>   ";

    string strNewWinPricingIFrm = "S3GOrgPricing_Add.aspx?qsMode=C&qsCRMID=";
    //string strNewWinPricingIFrm = "S3GOrgPricing_Add.aspx?Popup=Yes&qsMode=C&qsCRMID=";
    //string strNewWinApplicationIFrm = "S3G_ORG_ApplicationProcessing.aspx?Popup=Yes&qsMode=C&qsCRMID=";
    string strNewWinApplicationIFrm = "S3G_ORG_ApplicationProcessing.aspx?qsMode=C&qsCRMID=";

    string strNewWinPricing = " window.open('../Origination/S3GOrgPricing_Add.aspx?Popup=Yes&qsMode=C&qsCRMID=";
    //string strNewWinPricing = " window.showModalDialog('../Origination/S3GOrgPricing_Add.aspx?Popup=Yes&qsMode=C&qsCRMID=";
    string strNewWinApplication = " window.open('../Origination/S3G_ORG_ApplicationProcessing.aspx?Popup=Yes&qsMode=C&qsCRMID=";
    //string strNewWinAttributes = "', 'CRM', 'toolbar:no;menubar:no;statusbar:no;dialogwidth:950px;dialogHeight:850px;');";
    string strNewWinAttributes = "', 'newwindow','center= yes;toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,directories=no;modal=no;')";

    bool bCreate = false;
    bool bClearList = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bMakerChecker = false;
    S3GSession ObjS3GSession = new S3GSession();
    int strDecMaxLength = 0;
    int strPrefixLength = 0;
    static Int32 iDocumentID = 0;
    static string strDeletedDocumentID = "0";
    static Int32 iLeadID = 0;
    #endregion

    #region Events

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //Included by Sathish.G Starts
            //if (!IsPostBack)
            //{

            //    ucPopUp.FunPubPageLoad();
            //}
            //string id = PostBackControlId;
            ////if (id != null && (id.Contains("txtHeaderSearch")) || id.Contains("lnk"))
            //if (id != null)
            //{
            //    ucPopUp.FunPubPageLoad();
            //}
            ////Included by Sathish.G Ends
            FunPriPageLoad();
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    private List<StringBuilder> FunFormXml(out bool IsCheck, out int intCount, out string strPANUM)
    {
        List<StringBuilder> objList = new List<StringBuilder>();
        StringBuilder strGrvXML = new StringBuilder();
        StringBuilder strAllXML = new StringBuilder();
        strGrvXML.Append("<Root>");
        strAllXML.Append("<Root>");
        IsCheck = false;
        intCount = 0;
        strPANUM = "";

        foreach (GridViewRow gvRow in grvMain.Rows)
        {
            CheckBox chkSelect = (CheckBox)gvRow.FindControl("chkSelectAcc");
            Label txtPrimeAccountNo = (Label)gvRow.FindControl("txtPrimeAccountNo");
            Label txtSubAccountNo = (Label)gvRow.FindControl("txtSubAccountNo");

            if (chkSelect.Checked)
            {
                IsCheck = true;
                intCount = intCount + 1;
                strPANUM = txtPrimeAccountNo.Text;
                strGrvXML.Append("<Details  PANum='" + txtPrimeAccountNo.Text.Trim() + "' SANum ='" + txtSubAccountNo.Text.Trim() + "' /> ");
            }
            strAllXML.Append("<Details  PANum='" + txtPrimeAccountNo.Text.Trim() + "' SANum ='" + txtSubAccountNo.Text.Trim() + "' /> ");
        }
        strGrvXML.Append("</Root>");
        strAllXML.Append("</Root>");
        objList.Add(strGrvXML);
        objList.Add(strAllXML);
        return objList;
    }

    private List<StringBuilder> FunFormXmlDC(out bool IsCheck, out int intCount, out string strPANUM)
    {
        List<StringBuilder> objList = new List<StringBuilder>();
        StringBuilder strGrvXML = new StringBuilder();
        StringBuilder strAllXML = new StringBuilder();
        strGrvXML.Append("<Root>");
        strAllXML.Append("<Root>");
        IsCheck = false;
        intCount = 0;
        strPANUM = "";

        foreach (GridViewRow gvRow in gvLeadDetails.Rows)
        {
            CheckBox chkLeadSelect = (CheckBox)gvRow.FindControl("chkLeadSelect");
            Label lblLeadPanum = (Label)gvRow.FindControl("lblLeadPanum");
          

            if (chkSelect.Checked)
            {
                IsCheck = true;
                intCount = intCount + 1;
                strPANUM = lblLeadPanum.Text;
                strGrvXML.Append("<Details  Panum='" + lblLeadPanum.Text.Trim() + "' /> ");
            }
            strAllXML.Append("<Details  Panum='" + lblLeadPanum.Text.Trim() + "' /> ");
        }
        strGrvXML.Append("</Root>");
        strAllXML.Append("</Root>");
        objList.Add(strGrvXML);
        objList.Add(strAllXML);
        return objList;
    }

    #region "Buttons"

    protected void btnLeadSource_OnClick(object sender, EventArgs e)
    {
        try
        {
            txtLeadSource.Text = ucLead.SelectedText;
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    protected void btnLoadCustomer_OnClick(object sender, EventArgs e)
    {
        try
        {
            iDocumentID = 0;
            strDeletedDocumentID = "0";
            funGridClear(grvMain);
            funGridClear(grvAssetDetails);
            ucdCustomer.ClearCustomerDetails();
            funGridClear(gvLeadDetails);
            funGridClear(grvAccountDetails);
            funGridClear(grvFollowUp);
            //pnlFollowUp.Visible = true;
            pnlAddFollow.Visible = false;
            FunPubClearPopUp();
            FunProProspectClear();
            FunPriLoadLeadLov();
            FunProClearLead();
            ViewState["NewFollowup"] = ViewState["dtFollow"] = ViewState["dtFollowAll"] = ViewState["Lead_ID"] = ViewState["LeadStatusDetails"] = null;
            ViewState["Lead_Panum"] = null;
            FunProClearAllTabs();
            btnAddLead.Visible = true;
            for (int i = 1; i <= ddlDocumentType.Items.Count - 1; i++)
            {
                ViewState.Remove(ddlDocumentType.Items[i].Text);
            }

            tcCRM.ActiveTabIndex = 0;
            tcCRM.Tabs[3].Enabled = tcCRM.Tabs[4].Enabled = tcCRM.Tabs[5].Enabled = (Convert.ToInt32(ddlType.SelectedValue) == 8) ? false : true;
            if (ucPopUp.SelectedText.Trim() != "" && ddlType.SelectedValue != "0")
            {
                txtName.Text = ucPopUp.SelectedText;
                DataSet dsFollow = FunPriGetCRMProspectInfo();
                if (dsFollow.Tables[0] != null && dsFollow.Tables[0].Rows.Count > 0)
                {
                    grvMain.DataSource = dsFollow.Tables[0];
                    grvMain.DataBind();
                }

                if (dsFollow.Tables[1] != null && dsFollow.Tables[1].Rows.Count > 0)
                {
                    if (ddlType.SelectedValue == "9")
                    {
                        txtName.Text = dsFollow.Tables[1].Rows[0]["Entity_name"].ToString();
                    }
                    else
                    {
                        txtName.Text = dsFollow.Tables[1].Rows[0]["Customer_Name"].ToString();
                        ucdCustomer.SetCustomerDetails(dsFollow.Tables[1].Rows[0], true);
                        hidCustomerId.Text = dsFollow.Tables[1].Rows[0]["Customer_ID"].ToString();
                        hidCustomerId.Text = dsFollow.Tables[1].Rows[0]["Customer_ID"].ToString();
                        FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(hidCustomerId.Text, false, 0);
                        btnUp.Attributes.Add("OnClick", "if(funUpValidate()){ window.open('../Origination/S3GOrgCustomerMaster_Add.aspx?IsFromEnquiry=Yes&qsCustomerId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=Q', 'null','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');return false;}");
                    }
                }

                if (dsFollow.Tables[2] != null && dsFollow.Tables[2].Rows.Count > 0)
                {
                    if (ddlType.SelectedValue == "9")
                    {
                    }
                    else
                    {
                        btnPrspctClear.Enabled = false;
                        ddlTitle.SelectedValue = dsFollow.Tables[2].Rows[0]["Prospect_Title_ID"].ToString();
                        if (ddlType.SelectedValue == "8")
                        {
                            ucPopUp.SelectedText = txtName.Text = dsFollow.Tables[2].Rows[0]["Prospect_Name"].ToString();

                            txtProspectName.Text = dsFollow.Tables[2].Rows[0]["Prospect_Name"].ToString();
                            txtComAddress1.Text = dsFollow.Tables[2].Rows[0]["Address1"].ToString();
                            txtCOmAddress2.Text = dsFollow.Tables[2].Rows[0]["Address2"].ToString();
                            txtComCity.SelectedItem.Text = dsFollow.Tables[2].Rows[0]["City"].ToString();
                            txtComState.SelectedItem.Text = dsFollow.Tables[2].Rows[0]["State"].ToString();
                            txtComCountry.SelectedItem.Text = dsFollow.Tables[2].Rows[0]["Country"].ToString();
                            txtComPincode.Text = dsFollow.Tables[2].Rows[0]["Pincode"].ToString();
                            txtComTelephone.Text = dsFollow.Tables[2].Rows[0]["Telephone"].ToString();
                            txtComMobile.Text = dsFollow.Tables[2].Rows[0]["Mobile"].ToString();
                            txtComEmail.Text = dsFollow.Tables[2].Rows[0]["EMail"].ToString();
                            txtComWebsite.Text = dsFollow.Tables[2].Rows[0]["Website"].ToString();
                            ddlRefType.SelectedValue = dsFollow.Tables[2].Rows[0]["Reference_Type"].ToString();
                            ddlCustomerType.SelectedValue = Convert.ToString(dsFollow.Tables[2].Rows[0]["Customer_Type"]);
                            ddlCompanyType.SelectedValue = Convert.ToString(dsFollow.Tables[2].Rows[0]["Company_Type"]);
                            //ddlPostingGLCode.SelectedValue = Convert.ToString(dsFollow.Tables[2].Rows[0]["Posting_GL_Code"]);
                            ddlIndustry.SelectedValue = Convert.ToString(dsFollow.Tables[2].Rows[0]["Industry_ID"]);
                            ddlConstitutionName.SelectedValue = Convert.ToString(dsFollow.Tables[2].Rows[0]["Constitution_ID"]);
                            txtCntPer.Text = Convert.ToString(dsFollow.Tables[2].Rows[0]["Contact_Person"]);
                            txtCntPerPh.Text = Convert.ToString(dsFollow.Tables[2].Rows[0]["Contact_Person_Ph"]);
                            txtCntPerDesig.Text = Convert.ToString(dsFollow.Tables[2].Rows[0]["Contact_Person_Desig"]);
                        }
                        if (ddlType.SelectedValue == "8")
                        {
                            if (ddlRefType.SelectedValue == "0")
                            {
                                tdInitiate.Visible = true;
                            }
                            else
                            {
                                tdInitiate.Visible = false;
                            }

                        }
                    }
                    //txtBranchSearch.Text = dsFollow.Tables[2].Rows[0]["Location"].ToString();
                    //hdnBranchID.Value = dsFollow.Tables[2].Rows[0]["Location_Id"].ToString();
                    //ddlAccountStatus.SelectedValue = dsFollow.Tables[2].Rows[0]["Account_Stat"].ToString();
                    //ddlCustomerStatus.SelectedValue = dsFollow.Tables[2].Rows[0]["Customer_Stat"].ToString();
                }

                if (dsFollow.Tables[3] != null && dsFollow.Tables[3].Rows.Count > 0)
                {
                    if (ddlType.SelectedValue == "9")
                    {
                    }
                    else
                    {
                        hidCustomerId.Text = dsFollow.Tables[3].Rows[0]["Customer_ID"].ToString();
                        if (hidCustomerId.Text != "0")
                        {
                            FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(hidCustomerId.Text, false, 0);
                            btnUp.Attributes.Add("OnClick", "if(funUpValidate()){ window.open('../Origination/S3GOrgCustomerMaster_Add.aspx?IsFromEnquiry=Yes&qsCustomerId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=Q', 'null','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');return false;}");
                        }
                        ucPopUp.SelectedValue = dsFollow.Tables[3].Rows[0]["CRM_ID"].ToString();
                    }
                }

                if (dsFollow.Tables[4] != null && dsFollow.Tables[4].Rows.Count > 0)
                {

                    FunBindGrid(gvLeadDetails, dsFollow.Tables[4]);
                    pnlLeadViewDetails.Visible = true;
                    ViewState["Prospect_LeadDetails"] = dsFollow.Tables[4];
                    hdnLeadSourceType.Value = dsFollow.Tables[4].Rows[0]["Lead_Source_Type"].ToString();
                }
                else
                {
                    FunBindGrid(gvLeadDetails, dsFollow.Tables[4]);
                    pnlLeadViewDetails.Visible = true;
                }

                FunPriClearGroupDetails();
                if (dsFollow.Tables.Count > 5)
                {
                    if (dsFollow.Tables[5] != null && dsFollow.Tables[5].Rows.Count > 0)
                    {
                        txtGroupName.Text = Convert.ToString(dsFollow.Tables[5].Rows[0]["Group_Name"]);
                        txtGroupCode.Text = Convert.ToString(dsFollow.Tables[5].Rows[0]["Group_Code"]);
                        grvGroupDetails.DataSource = dsFollow.Tables[5];
                        grvGroupDetails.DataBind();
                    }
                }

                //pnlFollowUp.Visible = true;
                //DataTable dtFromDB = dsFollow.Tables[3];
                //DataTable dtExists = new DataTable();
                //ViewState["dtFollow"] = ViewState["dtFollowAll"] = dsFollow.Tables[3];
                //if (ViewState["NewFollowup"] == null)
                //{
                //    ViewState["NewFollowup"] = dsFollow.Tables[3].Clone();
                //    dtExists = dtFromDB;
                //}
                //else
                //{
                //    dtExists = ((DataTable)ViewState["NewFollowup"]).Copy();
                //    dtExists.Merge(dtFromDB);
                //}
                //if (dtExists != null && dtExists.Rows.Count > 0)
                //{
                //pnlFollowUp.Visible = true;
                //grvFollowUp.DataSource = dtExists;
                //grvFollowUp.DataBind();
                //grvFollowUp.Rows[0].Visible = true;
                //}
                //else
                //{
                //    grvFollowUp.DataSource = funAddRow();
                //    grvFollowUp.DataBind();
                //    grvFollowUp.Rows[0].Visible = false;
                //}

                //ViewState["dtFollow"] = ViewState["dtFollowAll"] = dtExists;
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    protected void btnDocAdd_Click(object sender, EventArgs e)
    {
        try
        {
            string strErrorMsg = string.Empty;
            DataTable dtDocs = (DataTable)ViewState["Docs" + ddlDocumentType.SelectedValue];
            if (chkSelect.Checked == false && chkIsCollected.Checked == false)
            {
                strErrorMsg = strErrorMsg + "</br> No document is Collected / Scanned";
            }

            if (chkIsCollected.Checked == true)
            {
                if (Convert.ToInt32(ddlCollectedBy.SelectedValue) == 0 || ddlCollectedBy.SelectedText == "")
                {
                    strErrorMsg = strErrorMsg + "</br> Enter Collected By";
                }
                if (Convert.ToString(txtCollectedDate.Text) == "")
                {
                    strErrorMsg = strErrorMsg + "</br> Enter Collected Date";
                }
            }

            if (chkSelect.Checked == true)
            {
                if (Convert.ToInt32(ddlScannedBy.SelectedValue) == 0 || ddlScannedBy.SelectedText == "")
                {
                    strErrorMsg = strErrorMsg + "</br> Enter Scanned By";
                }
                if (Convert.ToString(txtscannedDate.Text) == "")
                {
                    strErrorMsg = strErrorMsg + "</br> Enter Scanned Date";
                }
            }

            if ((Convert.ToInt32(ddlCollectedBy.SelectedValue) > 0 || Convert.ToString(txtCollectedDate.Text) != "") && chkIsCollected.Checked == false)
            {
                strErrorMsg = strErrorMsg + "</br> Check Collected";
            }

            if ((Convert.ToInt32(ddlScannedBy.SelectedValue) > 0 || Convert.ToString(txtscannedDate.Text) != "") && chkSelect.Checked == false)
            {
                strErrorMsg = strErrorMsg + "</br> Upload Scanned Document";
            }

            if (Convert.ToString(strErrorMsg) != "")
            {
                cvFollowUp.ErrorMessage = strErrorMessagePrefix + strErrorMsg;
                cvFollowUp.IsValid = false;
                return;
            }

            FunPriSaveDocumentDetails();

            #region "Comments"

            //if (dtDocs != null)
            //{
            //    DataRow dRow = dtDocs.NewRow();

            //    dRow["Documents_ID"] = "0";
            //    dRow["Doc_Tran_ID"] = "0";
            //    dRow["Company_ID"] = intCompanyID.ToString(); ;
            //    dRow["Doc_Cat_ID"] = ddlDocument.SelectedValue;
            //    dRow["Doc_Description"] = ddlDocument.SelectedItem.Text;

            //    if (!string.IsNullOrEmpty(txtCollectedDate.Text))
            //        //dRow["Collected_Date"] = DateTime.Parse(txtCollectedDate.Text, CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
            //        dRow["Collected_Date"] = Utility.StringToDate(txtCollectedDate.Text);

            //    if (!string.IsNullOrEmpty(txtscannedDate.Text))
            //        dRow["Scanned_Date"] = Utility.StringToDate(txtscannedDate.Text);
            //    dRow["Trans"] = chkSelect.Checked; ;
            //    if (chkSelect.Checked)
            //    {
            //        dRow["Scanned_Ref_No"] = hdnSelectedPath.Value;
            //        dRow["ViewDoc"] = "1";
            //    }
            //    else
            //    {
            //        dRow["ViewDoc"] = "0";
            //    }

            //    if (ddlCollectedBy.SelectedValue != "-1")
            //    {
            //        dRow["Collected_By"] = ddlCollectedBy.SelectedValue;
            //        dRow["Collected_By_Name"] = ddlCollectedBy.SelectedItem.Text;
            //    }
            //    if (ddlScannedBy.SelectedValue != "-1")
            //    {
            //        dRow["Scanned_By"] = ddlScannedBy.SelectedValue;
            //        dRow["Scanned_By_Name"] = ddlScannedBy.SelectedItem.Text;
            //    }
            //    dRow["Document_Path"] = lblActualPath.Text;
            //    dRow["Remarks"] = txtDocRemarks.Text;
            //    dRow["Value"] = txtValue.Text;
            //    dtDocs.Rows.InsertAt(dRow, 0);
            //}

            //FunProClearDocumentControls();
            //ViewState["Docs" + ddlDocumentType.SelectedValue] = dtDocs;

            //bool IsCheck = false;
            //int intCount = 0;
            //string strPANUM = "";
            //List<StringBuilder> objList = FunFormXml(out IsCheck, out intCount, out strPANUM);

            //FunProGetDocs(strPANUM);
            #endregion
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    protected void btnAddLead_Click(object sender, EventArgs e)
    {
        try
        {
            if ((ucPopUp.FindControl("hdnText") as HiddenField).Value.Trim() != "" && ddlType.SelectedValue != "0")
            {
                FunPriEnableDisableLeadDtls(1);
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    protected void btnMoveEnquiry_Click(object sender, EventArgs e)
    {
        try
        {
            string strErrorMsg = cvEnquiry.ErrorMessage = string.Empty;
            string strAlert = string.Empty;

            if (ViewState["Lead_ID"] == null || Convert.ToInt32(ViewState["Lead_ID"]) == 0)
            {
                strErrorMsg = "Select Lead Information to initiate";
            }
            else
            {
                DataTable dtlead = (DataTable)ViewState["Prospect_LeadDetails"];
                DataRow[] dr = dtlead.Select("Lead_ID=" + Convert.ToString(ViewState["Lead_ID"]));
                if (Convert.ToInt32(dr[0]["Lead_Status_ID"]) != 3)
                {
                    strErrorMsg = strErrorMsg + "Initiation can be done for Approved Leads only";
                }

                if (Convert.ToInt32(dr[0]["Pricing_ID"]) > 0)
                {
                    switch (Convert.ToInt32(dr[0]["Pricing_ID"]))
                    {
                        case 1:
                            strErrorMsg = strErrorMsg + "Enquiry already initiated for this Lead";
                            break;
                        case 2:
                            strErrorMsg = strErrorMsg + "Pricing already initiated for this Lead";
                            break;
                        case 3:
                            strErrorMsg = strErrorMsg + "Application already initiated for this Lead";
                            break;
                    }
                }
            }

            if (!string.IsNullOrEmpty(strErrorMsg))
            {
                cvEnquiry.IsValid = false;
                cvEnquiry.ErrorMessage = strErrorMsg;
                return;
            }

            string strDocNo = Convert.ToString(ViewState["Lead_ID"]);
            if (Convert.ToInt32(ddlPrograms.SelectedValue) == 1)
            {
                if (Convert.ToInt32(ddlType.SelectedValue) == 8)
                {
                    if (FunPriCheckEnq() == false)
                    {
                        return;
                    }
                }

                objFollowUp_Client = new ClnDataMgtServicesReference.ClnDataMgtServicesClient();
                objS3G_CLN_EnquiryDataTable = new ClnDataMgtServices.S3G_CLN_ENQUIRYDataTable();
                objS3G_CLN_EnquiryRow = objS3G_CLN_EnquiryDataTable.NewS3G_CLN_ENQUIRYRow();

                objS3G_CLN_EnquiryRow.Company_ID = Convert.ToInt32(intCompanyID);
                objS3G_CLN_EnquiryRow.Created_By = Convert.ToInt32(intUserID);
                objS3G_CLN_EnquiryRow.Lead_ID = Convert.ToInt32(strDocNo);
                objS3G_CLN_EnquiryRow.Is_MoveEnquiry = Convert.ToInt32(ddlPrograms.SelectedValue);

                objS3G_CLN_EnquiryDataTable.AddS3G_CLN_ENQUIRYRow(objS3G_CLN_EnquiryRow);

                Int32 intErrorCode;

                intErrCode = objFollowUp_Client.FunPubCreateEnquiry(out intErrorCode, out strDocNo, ObjSerMode, ClsPubSerialize.Serialize(objS3G_CLN_EnquiryDataTable, ObjSerMode));


                switch (intErrCode)
                {
                    case 0:
                        {
                            strAlert = "Prospect mapped successfully with " + ddlPrograms.SelectedItem.Text + " number " + strDocNo;
                            strAlert += Resources.ValidationMsgs.S3G_ValMsg_FollowUp_Confirm;// "\\n\\n Would you like to add one more Follow Up details?";
                            strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                            strRedirectPageView = "";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
                            break;
                        }
                    case -1:
                        {
                            Utility.FunShowAlertMsg(this.Page, "Document Number control not defined for Customer Master");
                            break;
                        }
                    case -2:
                        {
                            Utility.FunShowAlertMsg(this.Page, "Document Number control exceeded for Customer Master");
                            break;
                        }
                    case -3:
                        {
                            Utility.FunShowAlertMsg(this.Page, "Customer Document Number exceeding 12 charecters");
                            break;
                        }
                    case -4:
                        {
                            Utility.FunShowAlertMsg(this.Page, "Document Number control not defined for Enquiry Updation");
                            break;
                        }
                    case -5:
                        {
                            Utility.FunShowAlertMsg(this.Page, "Document Number control exceeded for Enquiry Updation");
                            break;
                        }
                    case 50:
                        {
                            Utility.FunShowAlertMsg(this.Page, "Error in saving details");
                            break;
                        }
                }
            }
            else
            {
                FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(strDocNo, false, 0);
                ifrmCRM.Visible = true;
                if (Convert.ToInt32(ddlPrograms.SelectedValue) == 2)
                {
                    Response.Redirect(strNewWinPricingIFrm + FormsAuthentication.Encrypt(Ticket), false);
                    //ifrmCRM.Attributes.Add("src", strNewWinPricingIFrm + FormsAuthentication.Encrypt(Ticket));
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", strNewWinPricing + FormsAuthentication.Encrypt(Ticket) + strNewWinAttributes, true);
                }
                else if (Convert.ToInt32(ddlPrograms.SelectedValue) == 3)
                {
                    Response.Redirect(strNewWinApplicationIFrm + FormsAuthentication.Encrypt(Ticket), false);
                    //ifrmCRM.Attributes.Add("src", strNewWinApplicationIFrm + FormsAuthentication.Encrypt(Ticket));
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", strNewWinApplication + FormsAuthentication.Encrypt(Ticket) + strNewWinAttributes, true);
                }
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    protected void btnFrameCancel_Click(object sender, EventArgs e)
    {
        ifrmCRM.Visible = false;
    }

    protected void btnDocClear_Click(object sender, EventArgs e)
    {
        try
        {
            FunProClearDocumentControls();
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    protected void btnDown_Click(object sender, EventArgs e)
    {
        try
        {
            if (hidCustomerId.Text.Trim() == "")
            {
                cvFollowUp.ErrorMessage = strErrorMessagePrefix + " Select the search description";
                cvFollowUp.IsValid = false;
                return;
            }
            funGridClear(grvAccountDetails);
            //chkSelect_CheckedChanged(null, null);
            bool IsCheck = false;
            int intCount = 0;
            string strPANUM = "";
            List<StringBuilder> objList = FunFormXml(out IsCheck, out intCount, out strPANUM);

            if (ObjDictionary != null)
                ObjDictionary.Clear();
            else
                ObjDictionary = new Dictionary<string, string>();

            ObjDictionary.Add("@Company_ID", intCompanyID.ToString());
            if (IsCheck) ObjDictionary.Add("@StrXml", objList[0].ToString());// strGrvXML.ToString());
            else ObjDictionary.Add("@StrXml", objList[1].ToString());// strAllXML.ToString());

            grvAccountDetails.DataSource = Utility.GetDefaultData("S3G_CLN_GetFollowUp_AccountDetails", ObjDictionary);
            grvAccountDetails.DataBind();

            if (grvAccountDetails.Rows.Count == 0)
            {
                Utility.FunShowAlertMsg(this, "Account Information does not exists");
                return;
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    protected void btnView_Click(object sender, EventArgs e)
    {
        try
        {
            FunPubClearPopUp();
            ddlQuery.Items.Clear();
            DataSet dsLookUp = ViewState["CRMLookUp"] as DataSet;
            FunFillGrid(ddlQuery, dsLookUp.Tables[3], "Lookup_Code", "Lookup_Description");
            ddlQuery.Items.RemoveAt(9);
            ddlQuery.Items.RemoveAt(8);
            ddlQuery.Items.RemoveAt(7);
            ddlQuery.Items.RemoveAt(6);

            funClearPopUp();
            //            MPE.Show();

            pnlAddFollow.Visible = true;

            LinkButton btnView = (LinkButton)sender;
            GridViewRow gvRow = (GridViewRow)btnView.Parent.Parent;
            DataTable dtFollowUp = (DataTable)ViewState["dtFollow"];
            Label txtTicket = (Label)gvRow.FindControl("txtTicketNo");
            HiddenField hidQueryType = (HiddenField)gvRow.FindControl("hidQueryType");
            HiddenField hidFollowup_Detail_ID = (HiddenField)gvRow.FindControl("hidFollowup_Detail_ID");
            hdnView.Value = gvRow.RowIndex.ToString();

            //DataRow[] drlist = dtFollowUp.Select("TicketNo= " + Convert.ToInt32(txtTicket.Text.Trim()) + " and QueryType='" + hidQueryType.Value + "'  ");
            DataRow[] drlist1 = dtFollowUp.Select("TicketNo= " + Convert.ToInt32(txtTicket.Text.Trim()) + " and QueryType <> '" + hidQueryType.Value + "'  ");
            //DataRow[] drStatus = dtFollowUp.Select("TicketNo= " + Convert.ToInt32(txtTicket.Text.Trim()) + " ");
            DataRow[] drlist = dtFollowUp.Select("Followup_Detail_ID = " + Convert.ToInt64(hidFollowup_Detail_ID.Value));

            if (drlist.Length > 0)
            {
                txtTicketNo.Text = hdnTicketNo.Value = drlist[0]["TicketNo"].ToString();
                //txtDate.Text = FormatDate(drlist[0]["Date"].ToString());
                //txtDescription.Text = drlist[0]["Description"].ToString();
                //txtNotifyDt.Text = FormatDate(drlist[0]["NotifyDate"].ToString());
                txtNotifyDt.Text = txtDate.Text = DateTime.Now.ToString(strDateFormat);
                ddlFrom.SelectedValue = drlist[0]["To_Type"].ToString();
                ddlTo.SelectedValue = drlist[0]["From_Type"].ToString();
                if (btnView.Text.ToUpper() == "EDIT")
                {
                    ddlQuery.SelectedValue = drlist[0]["QueryType"].ToString();
                    ddlQuery.ClearDropDownList();
                }
                //ddlMode.SelectedValue = drlist[0]["Mode"].ToString();

                TextBox txtFromName = ucFrom.FindControl("txtName") as TextBox;
                HiddenField hdnIDFrom = ucFrom.FindControl("hdnID") as HiddenField;
                TextBox txtToName = ucTo.FindControl("txtName") as TextBox;
                HiddenField hdnIDTo = ucTo.FindControl("hdnID") as HiddenField;
                txtFromName.Text = drlist[0]["To_UserName"].ToString();
                txtToName.Text = drlist[0]["From_UserName"].ToString();
                hdnIDFrom.Value = drlist[0]["To"].ToString();
                hdnIDTo.Value = drlist[0]["From"].ToString();
                //ddlStatus.SelectedValue = drStatus[0]["Status_Code"].ToString();
                funAssignUser(ddlFrom, ucFrom);
                funAssignUser(ddlTo, ucTo);
                txtTrackDetailID.Text = drlist[0]["Followup_Detail_ID"].ToString();

                //switch (drlist[0]["QueryType"].ToString())
                //{
                //    case "1":
                //        ddlStatus.SelectedValue = "1";
                //        break;
                //    case "2":
                //        ddlStatus.SelectedValue = "3";
                //        break;
                //    case "4":
                //        ddlStatus.SelectedValue = "3";
                //        break;
                //    case "3":
                //        ddlStatus.SelectedValue = "1";
                //        break;
                //    case "5":
                //        ddlStatus.SelectedValue = "2";
                //        break;
                //}
            }

            if (btnView.Text.ToUpper() != "EDIT")
            {
                hdnView.Value = "-1";
                if (drlist.Length > 0)
                {
                    switch (drlist[0]["QueryType"].ToString())
                    {
                        case "1":
                            ddlQuery.Items.RemoveAt(4);
                            ddlQuery.Items.RemoveAt(3);
                            ddlQuery.Items.RemoveAt(1);
                            //ddlStatus.SelectedValue = "1";
                            break;
                        case "2":
                            ddlQuery.Items.RemoveAt(4);
                            ddlQuery.Items.RemoveAt(3);
                            ddlQuery.Items.RemoveAt(1);
                            //ddlStatus.SelectedValue = "3";
                            break;
                        case "4":
                            ddlQuery.Items.RemoveAt(2);
                            ddlQuery.Items.RemoveAt(3);
                            ddlQuery.Items.RemoveAt(1);
                            //ddlStatus.SelectedValue = "3";
                            break;
                        case "3":
                            ddlQuery.Items.RemoveAt(3);
                            ddlQuery.Items.RemoveAt(2);
                            ddlQuery.Items.RemoveAt(1);
                            //ddlStatus.SelectedValue = "1";
                            break;
                        case "5":
                            if (drlist1.Length > 0)
                            {
                                if (drlist1[0]["QueryType"].ToString() == "1")
                                {
                                    ddlQuery.Items.RemoveAt(4);
                                    ddlQuery.Items.RemoveAt(3);
                                    ddlQuery.Items.RemoveAt(1);
                                }
                                else if (drlist1[0]["QueryType"].ToString() == "3")
                                {
                                    ddlQuery.Items.RemoveAt(1);
                                    ddlQuery.Items.RemoveAt(2);
                                    ddlQuery.Items.RemoveAt(1);
                                }
                            }
                            else
                            {
                                ddlQuery.Items.RemoveAt(3);
                                ddlQuery.Items.RemoveAt(1);
                            }
                            break;
                    }
                }
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        FunPubClearPopUp();
        try
        {
            if (Convert.ToInt32(ddlQuery.SelectedValue) != 7)
            {
                if (((TextBox)ucFrom.FindControl("txtName")).Text.Trim() == "")
                {
                    FunPriTrackErrorLog("Select From Type Name");
                    return;
                }
                if (ddlFrom.SelectedValue == ddlTo.SelectedValue)
                {
                    if (ddlFrom.SelectedValue != "1")
                    {
                        FunPriTrackErrorLog("From Type and To Type should be different");
                        return;
                    }
                    if (((HiddenField)ucFrom.FindControl("hdnID")).Value.Trim() == ((HiddenField)ucTo.FindControl("hdnID")).Value.Trim())
                    {
                        FunPriTrackErrorLog("From Type Name and To Type Name should be different");
                        return;
                    }
                }
                //if (ddlTo.SelectedValue == "1")
                //{
                //    if (((HiddenField)ucTo.FindControl("hdnID")).Value.Trim() == intUserID.ToString())
                //    {
                //        FunPriTrackErrorLog("To Type Name should be differed from Logged User");
                //        return;
                //    }
                //}
                if (ddlStatus.SelectedValue == "0")
                {
                    FunPriTrackErrorLog("Select the Status");
                    return;
                }
            }
            //else if (Convert.ToInt32(ddlQuery.SelectedValue) == 7 && Convert.ToString(txtDescription.Text) == "")
            if (Convert.ToString(txtDescription.Text) == "")
            {
                FunPriTrackErrorLog("Enter the Description");
                return;
            }
            FunPriSaveTrackDetails();

            #region "Comments"

            //DataTable dtFollow = new DataTable();
            //DataTable dt = ViewState["dtFollow"] as DataTable;
            //DataTable dtNewFollowup = ViewState["NewFollowup"] as DataTable;
            //dtFollow = dt.Clone();
            //DataTable dtCurrData = dtNewFollowup.Clone();
            //DataRow drFollow;

            //if (hdnView.Value == "" || hdnView.Value == "-1")
            //{
            //    /* Newly add*/
            //    DataRow[] drList = dt.Select(" TicketNo = " + Convert.ToInt32(hdnTicketNo.Value) + " ");
            //    foreach (DataRow dr in drList)
            //    {
            //        dr.BeginEdit();
            //        dr["isMax"] = false;
            //        if (dr["Version_No"].ToString() == "1")
            //        {
            //            dr["Status"] = ddlStatus.SelectedValue == "0" ? "" : ddlStatus.SelectedItem.Text.Trim();
            //            dr["Status_Code"] = ddlStatus.SelectedValue;
            //        }
            //        else
            //        {
            //            dr["Status"] = "";
            //            dr["Status_Code"] = "0";
            //        }
            //        dr.AcceptChanges();
            //    }

            //    drFollow = dtFollow.NewRow();
            //    drFollow = funAssignDataRow(drFollow);
            //    if (drList.Length > 0)
            //        drFollow["Version_No"] = (Convert.ToInt32(drList[0]["Version_No"]) + 1).ToString();
            //    else
            //        drFollow["Version_No"] = "1";

            //    drFollow["isMax"] = true;

            //    //Comment By Vijay
            //    #region
            //    //Delete the ticket value For Query Type Follow Up

            //    if (hdnView.Value == "" && ddlQuery.SelectedValue == "5")
            //    {
            //        drFollow["Status"] = "";
            //        drFollow["Status_Code"] = "0";
            //        drFollow["TicketNo"] = "0";
            //    }
            //    else if (ddlQuery.SelectedValue == "7" || ddlQuery.SelectedValue == "8")
            //    {
            //        drFollow["TicketNo"] = "0";
            //    }
            //    #endregion

            //    if (hdnView.Value == "-1")
            //    {
            //        drFollow["Status"] = "";
            //        drFollow["Status_Code"] = "0";
            //    }
            //    //drFollow["Followup_Detail_ID"] = 0;

            //    drFollow["Status"] = ddlStatus.SelectedValue == "0" ? "" : ddlStatus.SelectedItem.Text.Trim();
            //    drFollow["Status_Code"] = ddlStatus.SelectedValue;

            //    dtFollow.Rows.Add(drFollow);
            //}
            //dtCurrData = dtFollow.Copy();
            //dtCurrData.Merge(dtNewFollowup);

            //dtFollow.Merge(dt);
            //ViewState["NewFollowup"] = dtCurrData;

            //if (hdnView.Value != "" && hdnView.Value != "-1")
            //{
            //    int intRowIndex = Convert.ToInt32(hdnView.Value);
            //    drFollow = dtFollow.Rows[intRowIndex];
            //    drFollow.BeginEdit();
            //    drFollow = funAssignDataRow(drFollow);
            //    dtFollow.AcceptChanges();
            //}

            //FunBindTrackDetails(dtFollow);
            //ViewState["dtFollow"] = dtFollow;
            //funClearPopUp();
            ////            MPE.Hide();

            //ViewState["Followup"] = dtFollow;
            //txtSDate.Text = txtSTicketNo.Text = string.Empty;

            #endregion
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    protected void btnRemove_Click(object sender, EventArgs e)
    {
        try
        {
            FunPubClearPopUp();
            DataTable dtFollowUp = (DataTable)ViewState["dtFollow"];
            LinkButton btnRemove = (LinkButton)sender;
            GridViewRow gvRow = (GridViewRow)btnRemove.Parent.Parent;
            Label txtTicketNo = (Label)gvRow.FindControl("txtTicketNo");
            HiddenField hidVersionNo = (HiddenField)gvRow.FindControl("hidVersionNo");

            dtFollowUp.Rows.RemoveAt(gvRow.RowIndex);
            DataRow[] drList = dtFollowUp.Select(" TicketNo > " + Convert.ToInt32(txtTicketNo.Text) + " ");
            DataRow[] drList1 = dtFollowUp.Select(" TicketNo = " + Convert.ToInt32(txtTicketNo.Text) + " and Version_No = '" + (Convert.ToInt32(hidVersionNo.Value) - 1) + "' ");
            DataRow[] drQry = dtFollowUp.Select(" TicketNo = " + Convert.ToInt32(txtTicketNo.Text) + " ");
            if (drQry.Length > 0)
            {
                string strQryType = drQry[drQry.Length - 1]["QueryType"].ToString();
                drQry[0].BeginEdit();
                if (strQryType == "1" || strQryType == "3")
                {
                    drQry[0]["Status"] = "Open";
                    drQry[0]["Status_Code"] = "1";
                }
                else if (strQryType == "5")
                {
                    drQry[0]["Status"] = "Process";
                    drQry[0]["Status_Code"] = "2";
                }
                else
                {
                    drQry[0]["Status"] = "Closed";
                    drQry[0]["Status_Code"] = "3";
                }
                drQry[0].AcceptChanges();
            }
            foreach (DataRow dr in drList)
            {
                if (drList1.Length == 0)
                {
                    dr.BeginEdit();
                    dr["TicketNo"] = Convert.ToInt32(dr["TicketNo"]) - 1;
                    dr.AcceptChanges();
                }
            }
            if (drList1.Length == 1)
            {
                drList1[0].BeginEdit();
                drList1[0]["isMax"] = true;
                drList1[0].AcceptChanges();
            }

            grvFollowUp.DataSource = dtFollowUp;
            grvFollowUp.DataBind();

            ViewState["Followup"] = dtFollowUp;
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            //PopulateType();
            if (ObjDictionary != null)
                ObjDictionary.Clear();
            else
                ObjDictionary = new Dictionary<string, string>();

            ObjDictionary.Add("@Company_ID", intCompanyID.ToString());
            if (ddlType.SelectedValue == "8")
            {
                ObjDictionary.Add("@Type", "2");
                ObjDictionary.Add("@Customer_ID", (ucPopUp.FindControl("hdnID") as HiddenField).Value);
            }
            else
            {
                ObjDictionary.Add("@Type", "1");
                ObjDictionary.Add("@Customer_ID", hidCustomerId.Text.Trim());
            }

            ObjDictionary.Add("@LEAD_ID", Convert.ToString(ViewState["Lead_ID"]));
            ObjDictionary.Add("@Panum", Convert.ToString(ViewState["Lead_Panum"]));

            DataTable dtTicket = Utility.GetDefaultData("S3G_CLN_GetCRM_TicketNo", ObjDictionary);
            int dcTicketNo = 0;
            if (dtTicket != null && dtTicket.Rows.Count > 0)
            {
                dcTicketNo = Convert.ToInt32(dtTicket.Rows[0]["TicketNo"].ToString()) + 1;
            }

            FunPubClearPopUp();
            funClearPopUp();
            hdnView.Value = "";

            DataSet dsLookUp = ViewState["CRMLookUp"] as DataSet;
            ddlQuery.Items.Clear();
            FunFillGrid(ddlQuery, dsLookUp.Tables[3], "Lookup_Code", "Lookup_Description");
            ddlQuery.Items.RemoveAt(6);
            ddlQuery.Items.RemoveAt(4);
            ddlQuery.Items.RemoveAt(2);

            ddlStatus.SelectedValue = "1";
            ddlFrom.SelectedValue = "1";
            TextBox txtFrom = ucFrom.FindControl("txtName") as TextBox;
            HiddenField hdnFrom = ucFrom.FindControl("hdnID") as HiddenField;
            txtFrom.Text = ObjUserInfo.ProUserNameRW;
            hdnFrom.Value = intUserID.ToString();
            funAssignUser(ddlFrom, ucFrom);

            TextBox txtTo = ucTo.FindControl("txtName") as TextBox;
            HiddenField hdnTo = ucTo.FindControl("hdnID") as HiddenField;

            if (ddlType.SelectedValue == "8")
            {
                ddlTo.SelectedValue = "10";
                txtTo.Text = txtName.Text;
                hdnTo.Value = (ucPopUp.FindControl("hdnID") as HiddenField).Value;
            }
            else if (ddlType.SelectedValue == "9")
            {
                ddlTo.SelectedValue = "6";
                txtTo.Text = txtName.Text;
                hdnTo.Value = (ucPopUp.FindControl("hdnID") as HiddenField).Value;
            }
            else
            {
                ddlTo.SelectedValue = "2";
                txtTo.Text = (ucdCustomer.FindControl("txtCustomerName") as TextBox).Text.Trim();
                hdnTo.Value = hidCustomerId.Text.Trim();
            }

            funAssignUser(ddlTo, ucTo);

            //MPE.Show();

            string strSearchFilter = "Lead_ID =" + ((ViewState["Lead_ID"] != null) ? Convert.ToString(ViewState["Lead_ID"]) : "0");
            strSearchFilter = strSearchFilter + " and TicketNo >  0";

            DataTable dtFollowAll = ViewState["dtFollowAll"] as DataTable;
            //DataRow[] drListAll = dtFollowAll.Select(" TicketNo >  0 ");
            DataRow[] drListAll = dtFollowAll.Select(strSearchFilter);
            int intTicketNoAll = 0;
            if (drListAll.Length > 0)
                intTicketNoAll = Convert.ToInt32(drListAll[drListAll.Length - 1]["TicketNo"].ToString());

            DataTable dtFollow = ViewState["NewFollowup"] as DataTable;
            //DataRow[] drList = dtFollow.Select(" TicketNo >  0 ");
            DataRow[] drList = dtFollow.Select(strSearchFilter);
            int intTicket = 0;
            if (drList.Length > 0)
                intTicket = Convert.ToInt32(drList[drList.Length - 1]["TicketNo"].ToString());

            if (intTicket > intTicketNoAll)
                intTicketNo = 1 + intTicket;// Convert.ToInt32(drList[drList.Length - 1]["TicketNo"].ToString());
            else
                intTicketNo = 1 + intTicketNoAll;

            txtDate.Text = txtNotifyDt.Text = DateTime.Now.ToString(strDateFormat);
            if (intTicketNo > dcTicketNo)
                txtTicketNo.Text = hdnTicketNo.Value = Convert.ToString(intTicketNo);
            else
                txtTicketNo.Text = hdnTicketNo.Value = Convert.ToString(dcTicketNo);

            ViewState["pnlIndex"] = 0;
            ddlQuery.SelectedValue = txtTrackDetailID.Text = "0";

            pnlAddFollow.Visible = true;
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            FunPubClearPopUp();
            string strErrorMsg = string.Empty;
            //if (hidCustomerId.Text.Trim() == "")
            //{
            //    cvFollowUp.ErrorMessage = strErrorMessagePrefix + " Select the search description";
            //    cvFollowUp.IsValid = false;
            //    return;
            //}

            //if (ViewState["IS_NewLead"] == null || Convert.ToInt32(ViewState["IS_NewLead"]) > 0)
            //{
            //    strErrorMsg = strErrorMsg + "Add Lead Deatils" + "<br/>";
            //}
            //else if (((DataTable)ViewState["Assets"]).Rows.Count == 0)
            //{
            //    strErrorMsg = strErrorMsg + "Add minimum one asset information";
            //}
            //else
            //{
            //    if (Convert.ToInt32(ddlFinanceMode.SelectedValue) == 0)
            //    {
            //        strErrorMsg = strErrorMsg + rfvLeadFinanceMode.ErrorMessage + "<br/>";
            //    }
            //    if (Convert.ToInt32(ddlLOB.SelectedValue) == 0)
            //    {
            //        strErrorMsg = strErrorMsg + rfvddlLOB.ErrorMessage + "<br/>";
            //    }
            //    if (Convert.ToInt32(ddlLeadSourceType.SelectedValue) == 0)
            //    {
            //        strErrorMsg = strErrorMsg + rfvLeadSourceType.ErrorMessage + "<br/>";
            //    }
            //    if (Convert.ToString(hdnBranchID.Value) == "" || Convert.ToString(hdnBranchID.Value) == "0")
            //    {
            //        strErrorMsg = strErrorMsg + rfvLeadLocation.ErrorMessage + "<br/>";
            //    }
            //    if (Convert.ToString(txtLeadInformation.Text) == "")
            //    {
            //        strErrorMsg = strErrorMsg + rfvLeadInformation.ErrorMessage + "<br/>";
            //    }
            //}

            if (!string.IsNullOrEmpty(strErrorMsg))
            {
                cvEnquiry.IsValid = false;
                cvEnquiry.ErrorMessage = strErrorMsg;
                return;
            }

            FunPriSaveRecord(0);
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            //FunPubClearPopUp();
            //ddlType.SelectedIndex = 0;
            //ddlSearch.Items.Clear();
            //ddlSearch.Text = "";
            //(ucPopUp.FindControl("hdnID") as HiddenField).Value = "";
            //(ucPopUp.FindControl("hdnText") as HiddenField).Value = "";
            //ucdCustomer.ClearCustomerDetails();
            //funGridClear(grvMain);
            //funGridClear(grvAccountDetails);
            //funGridClear(grvAssetDetails);
            //funGridClear(grvFollowUp);
            //ViewState["Followup"] = null;
            //pnlAccount.Visible =
            //    pnlFollowUp.Visible = pnlAccountInformation.Visible = false;
            //Session.Remove("FU_TYPE");
            //FunProProspectClear();
            //txtName.Text = "";

            Response.Redirect(strRedirectPage, false);
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    protected void btnCan_Click(object sender, EventArgs e)
    {
        try
        {
            funClearPopUp();
            FunPubClearPopUp();
            pnlAddFollow.Visible = false;
            //            MPE.Hide();
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    protected void btnAccordion_Click(object sender, EventArgs e)
    {

        //ScriptManager.RegisterStartupScript(this, this.GetType(), "Txt", "javascript: document.getElementById('<%= " + Button2.ClientID + ".ClientID %>').click();", true);

        //if (Accordion1.SelectedIndex == 0)
        //    Accordion1.SelectedIndex = -1;
        //else
        //    Accordion1.SelectedIndex = 0;
        //Accordion1.SelectedIndex = 0;
    }

    protected void btnLeadOk_OnClick(object sender, EventArgs e)
    {
        try
        {
            //if (ViewState["Assets"] == null || ((DataTable)ViewState["Assets"]).Rows.Count <= 0)
            //{
            //    Utility.FunShowAlertMsg(this, "Add atleast one asset value");
            //    return;
            //}
            if (Convert.ToString(hdnSalesPersonID.Value) == "" && Convert.ToString(txtSalesPerson.Text) != "")
            {
                Utility.FunShowAlertMsg(this, "Invalid sales person");
                return;
            }
            if (Convert.ToString(hdnBranchID.Value) == "" && Convert.ToString(txtBranchSearch.Text) != "")
            {
                Utility.FunShowAlertMsg(this, "Invalid Location");
                return;
            }

            if (Convert.ToString(txtOtherLead.Text) == "" && Convert.ToInt32(ddlLeadSourceType.SelectedValue) == 16)
            {
                Utility.FunShowAlertMsg(this, "Lead Source should not be blank");
                return;
            }
            if (Convert.ToInt32(ddlLeadSourceType.SelectedValue) <= 10 && Convert.ToInt32(ddlLeadSourceType.SelectedValue) != 3 && Convert.ToString(txtLeadSource.Text).Trim() == "")
            {
                Utility.FunShowAlertMsg(this, "Lead Source should not be blank");
                return;
            }
            if (ViewState["Assets"] != null && ((DataTable)ViewState["Assets"]).Rows.Count > 0)
            {
                double dblAstFinAmt = 0;
                for (Int32 i = 0; i < ((DataTable)ViewState["Assets"]).Rows.Count; i++)
                {
                    dblAstFinAmt += Convert.ToDouble(((DataTable)ViewState["Assets"]).Rows[i]["Finance_Amount"]);
                }
                if (dblAstFinAmt != Convert.ToDouble(txtLdFinanceAmt.Text))
                {
                    Utility.FunShowAlertMsg(this, "Asset Total Finance Amount should be equal to Lead Finance Amount");
                    return;
                }
            }

            FunPriSaveLeadDeatils();
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    protected void btnLeadView_OnClick(object sender, EventArgs e)
    {
        try
        {
            pnlLeadView.Style["display"] = "none";
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
        //ObjDictionary.Clear();
        //ObjDictionary.Add("@Company_ID", intCompanyID.ToString());
        //ObjDictionary.Add("@Type", ddlType.SelectedValue.ToString());
        //if (ddlType.SelectedValue == "8")
        //{
        //    ObjDictionary.Add("@SearchValue", (ucPopUp.FindControl("hdnID") as HiddenField).Value.Trim());
        //}
        //else
        //{
        //    ObjDictionary.Add("@SearchValue", (ucPopUp.FindControl("hdnText") as HiddenField).Value.Trim());
        //}
        //ObjDictionary.Add("@UserId", intUserID.ToString());
        //if (ddlType.SelectedValue == "1" || ddlType.SelectedValue == "2" || ddlType.SelectedValue == "3")
        //    ObjDictionary.Add("@SearchType", "Customer");
        //else
        //    ObjDictionary.Add("@SearchType", "Account");

        //DataSet dsFollow;
        //if (ddlType.SelectedValue == "8")
        //{
        //    dsFollow = Utility.GetDataset("S3G_CLN_GetCRMProspectList", ObjDictionary);
        //}
        //else
        //{
        //    dsFollow = Utility.GetDataset("S3G_CLN_GetCRMFollowUpList", ObjDictionary);
        //}

        //if (ViewState["NewFollowup"] != null)
        //{
        //    grvFollowUp.DataSource = (DataTable)ViewState["NewFollowup"];
        //    grvFollowUp.DataBind();
        //}

        //else
        //{
        //    grvFollowUp.DataSource = funAddRow();
        //    grvFollowUp.DataBind();
        //    grvFollowUp.Rows[0].Visible = false;
        //}

        //ViewState["dtFollow"] = ViewState["dtFollowAll"] = dsFollow.Tables[3];
    }

    protected void btnBrowse_OnClick(object sender, EventArgs e)
    {
        try
        {
            if (ddlDocumentType.SelectedValue != "0")
            {
                DataTable dtDocs = (DataTable)ViewState["Docs" + ddlDocumentType.SelectedValue];

                HttpFileCollection hfc = Request.Files;

                if (string.IsNullOrEmpty(lblActualPath.Text))
                {
                    Utility.FunShowAlertMsg(this, "Document path not available to Upload");
                    return;
                }

                if (hfc.Count > 0)
                {
                    HttpPostedFile hpf = hfc[0];
                    if (hpf.ContentLength > 0)
                    {
                        chkSelect.Enabled = true;
                        chkSelect.Checked = true;
                        chkSelect.ToolTip = flUpload.ToolTip = hdnSelectedPath.Value;
                        lblCurrentPath.Text = hpf.FileName;

                        Cache["Docs_" + ddlDocumentType.SelectedValue + "_File_" + ddlDocument.SelectedValue] = hpf;
                    }
                }
            }
            else
            {
                Utility.FunShowAlertMsg(this, "Select Document type");
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    protected void btnProspectView_Click(object sender, EventArgs e)
    {
        try
        {
            FunPriSaveProspectDtl();    //Added on 24Jul2015
            //pnlProspectView.Visible = false;
            //pnlProspectView.Style["display"] = "none";

            //tcCRM.ActiveTabIndex = 0;
            //pnlFollowUp.Visible = true;
            //(ucPopUp.FindControl("hdnText") as HiddenField).Value = txtName.Text;
            ////tcCRM_ActiveTabChanged(null, null);
            //FunPriLoadCRMLeadDetails();
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    protected void Button3_OnClick(object sender, EventArgs e)
    {
        try
        {
            FunPubClearPopUp();
            moeNOD.Hide();
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    protected void btnDocUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            string strErrorMsg = string.Empty;
            DataTable dtDocs = (DataTable)ViewState["Docs" + ddlDocumentType.SelectedValue];
            if (chkSelect.Checked == false && chkIsCollected.Checked == false)
            {
                strErrorMsg = strErrorMsg + "</br> No document is Collected / Scanned";
            }

            if (chkIsCollected.Checked == true)
            {
                if (Convert.ToInt32(ddlCollectedBy.SelectedValue) == 0 || ddlCollectedBy.SelectedText == "")
                {
                    strErrorMsg = strErrorMsg + "</br> Enter Collected By";
                }
                if (Convert.ToString(txtCollectedDate.Text) == "")
                {
                    strErrorMsg = strErrorMsg + "</br> Enter Collected Date";
                }
            }

            if (chkSelect.Checked == true)
            {
                if (Convert.ToInt32(ddlScannedBy.SelectedValue) == 0 || ddlScannedBy.SelectedText == "")
                {
                    strErrorMsg = strErrorMsg + "</br> Enter Scanned By";
                }
                if (Convert.ToString(txtscannedDate.Text) == "")
                {
                    strErrorMsg = strErrorMsg + "</br> Enter Scanned Date";
                }
            }

            if (((Convert.ToInt32(ddlCollectedBy.SelectedValue) > 0 && ddlCollectedBy.SelectedText != "") || Convert.ToString(txtCollectedDate.Text) != "") && chkIsCollected.Checked == false)
            {
                strErrorMsg = strErrorMsg + "</br> Check Collected";
            }

            if (((Convert.ToInt32(ddlScannedBy.SelectedValue) > 0 && ddlScannedBy.SelectedText != "") || Convert.ToString(txtscannedDate.Text) != "") && chkSelect.Checked == false)
            {
                strErrorMsg = strErrorMsg + "</br> Upload Scanned Document";
            }

            if (Convert.ToString(strErrorMsg) != "")
            {
                cvFollowUp.ErrorMessage = strErrorMessagePrefix + strErrorMsg;
                cvFollowUp.IsValid = false;
                return;
            }

            FunPriSaveDocumentDetails();

            #region "Hidden"

            //Label lblDocumentID = (Label)gvPRDDT.Rows[iDocRowIdx].FindControl("lblDocumentID");
            //Label lblDocumentCategoryID = (Label)gvPRDDT.Rows[iDocRowIdx].FindControl("lblPRTID");
            //Label lblDesc = (Label)gvPRDDT.Rows[iDocRowIdx].FindControl("lblDesc");
            //Label lblValue = (Label)gvPRDDT.Rows[iDocRowIdx].FindControl("lblValue");
            //Label lblCollectedBy = (Label)gvPRDDT.Rows[iDocRowIdx].FindControl("lblCollectedBy");
            //Label lblCollectedDate = (Label)gvPRDDT.Rows[iDocRowIdx].FindControl("lblCollectedDate");
            //Label lblScannedBy = (Label)gvPRDDT.Rows[iDocRowIdx].FindControl("lblScannedBy");
            //Label lblScannedDate = (Label)gvPRDDT.Rows[iDocRowIdx].FindControl("lblScannedDate");
            //ImageButton imgDocPath = (ImageButton)gvPRDDT.Rows[iDocRowIdx].FindControl("hyplnkView");
            //Label lblCollectedByName = (Label)gvPRDDT.Rows[iDocRowIdx].FindControl("lblCollectedByName");
            //Label lblScannedByName = (Label)gvPRDDT.Rows[iDocRowIdx].FindControl("lblScannedByName");
            //Label lblDocPath = (Label)gvPRDDT.Rows[iDocRowIdx].FindControl("lblDocPath");
            //Label lblViewDoc = (Label)gvPRDDT.Rows[iDocRowIdx].FindControl("lblCanView");
            //Label lblRemarks = (Label)gvPRDDT.Rows[iDocRowIdx].FindControl("lblRemarks");
            //Label lblProgramName = (Label)gvPRDDT.Rows[iDocRowIdx].FindControl("lblProgramName");

            //if (dtDocs != null)
            //{
            //    if (dtDocs.Rows.Count > 0)
            //    {
            //        string strFilter = "Documents_ID = " + Convert.ToString(iDocumentID);
            //        DataRow[] dtrow = dtDocs.Select(strFilter);
            //        if (dtrow.Length > 0)
            //        {
            //            dtrow[0].Delete();
            //        }
            //        dtDocs.AcceptChanges();
            //    }

            //    DataRow dRow = dtDocs.NewRow();

            //    dRow["Documents_ID"] = Convert.ToString(iDocumentID);
            //    dRow["Doc_Tran_ID"] = "0";
            //    dRow["Company_ID"] = intCompanyID.ToString(); ;
            //    dRow["Doc_Cat_ID"] = ddlDocument.SelectedValue;
            //    dRow["Doc_Description"] = ddlDocument.SelectedItem.Text;

            //    if (!string.IsNullOrEmpty(txtCollectedDate.Text))
            //        dRow["Collected_Date"] = DateTime.Parse(txtCollectedDate.Text, CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);

            //    if (!string.IsNullOrEmpty(txtscannedDate.Text))
            //        dRow["Scanned_Date"] = DateTime.Parse(txtscannedDate.Text, CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
            //    dRow["Trans"] = chkSelect.Checked; ;
            //    if (chkSelect.Checked)
            //    {
            //        dRow["Scanned_Ref_No"] = hdnSelectedPath.Value;
            //        dRow["ViewDoc"] = "1";
            //    }
            //    else
            //    {
            //        dRow["ViewDoc"] = "0";
            //    }

            //    if (ddlCollectedBy.SelectedValue != "-1")
            //    {
            //        dRow["Collected_By"] = ddlCollectedBy.SelectedValue;
            //        dRow["Collected_By_Name"] = ddlCollectedBy.SelectedItem.Text;
            //    }
            //    if (ddlScannedBy.SelectedValue != "-1")
            //    {
            //        dRow["Scanned_By"] = ddlScannedBy.SelectedValue;
            //        dRow["Scanned_By_Name"] = ddlScannedBy.SelectedItem.Text;
            //    }
            //    dRow["Document_Path"] = lblActualPath.Text;
            //    dRow["Remarks"] = txtDocRemarks.Text;
            //    dRow["Value"] = txtValue.Text;
            //    dtDocs.Rows.InsertAt(dRow, 0);

            //    lblDocumentID.Text = Convert.ToString(iDocumentID);
            //    lblDocumentCategoryID.Text = ddlDocument.SelectedValue;
            //    lblDesc.Text = ddlDocument.SelectedItem.Text;
            //    lblValue.Text = txtValue.Text;
            //    lblCollectedBy.Text = ddlCollectedBy.SelectedValue;
            //    lblCollectedDate.Text = (Convert.ToString(txtCollectedDate.Text) == "") ? "" : DateTime.Parse(txtCollectedDate.Text, CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
            //    lblScannedBy.Text = ddlScannedBy.SelectedValue;
            //    lblScannedDate.Text = (Convert.ToString(txtscannedDate.Text) == "") ? "" : DateTime.Parse(txtscannedDate.Text, CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
            //    lblCollectedByName.Text = ddlCollectedBy.SelectedItem.Text;
            //    lblScannedByName.Text = ddlScannedBy.SelectedItem.Text;
            //    lblDocPath.Text = lblActualPath.Text;
            //    lblViewDoc.Text = (Convert.ToString(hdnSelectedPath.Value) == "") ? "0" : "1";
            //    //lblScannedRefNo.Text = hdnSelectedPath.Value;
            //    lblRemarks.Text = txtDocRemarks.Text;
            //    imgDocPath.CommandArgument = Convert.ToString(hdnSelectedPath.Value);
            //}

            //FunProClearDocumentControls();
            //ViewState["Docs" + ddlDocumentType.SelectedValue] = dtDocs;

            #endregion
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    protected void btnLeadClear_Click(object sender, EventArgs e)
    {
        try
        {
            FunProClearLead();
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    protected void btnPassword_Click(object sender, EventArgs e)
    {
        S3GAdminServicesReference.S3GAdminServicesClient ObjS3GAdminServices = new S3GAdminServicesReference.S3GAdminServicesClient();
        try
        {
            txtPassword.Text = txtPassword.Text.Replace(",", "");
            if (ObjS3GAdminServices.FunPubPasswordValidation(intUserID, txtPassword.Text.Trim()) > 0)
            {
                Utility.FunShowAlertMsg(this, "Incorrect Password");
                ddlLeadStatus.SelectedValue = "1";
            }
            else
            {
                txtLeadRemarks.Enabled = (Convert.ToInt32(ddlLeadStatus.SelectedValue) > 3) ? true : false;
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
        finally
        {
            ObjS3GAdminServices.Close();
        }
    }

    protected void btnCancelPass_Click(object sender, EventArgs e)
    {
        try
        {
            ddlLeadStatus.SelectedValue = "1";
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            FunPriGetTrackDtl();
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    protected void btnSClear_Click(object sender, EventArgs e)
    {
        try
        {
            txtSTicketNo.Text = txtSDate.Text = string.Empty;
            FunPriGetTrackDtl();
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    protected void btnGetLOV_Click(object sender, EventArgs e)
    {
        try
        {
            funAssignPopupValue();
            ucPopUp.Show();
            funProCloseAllPops();
            ////Included by Sathish.G Starts
            //ucPopUp.FunPubPageLoad();
            ////Included by Sathish.G Ends
            //ucPopUp.btnGetLOV_Click(sender, e);
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    protected void btnGetSource_Click(object sender, EventArgs e)
    {
        try
        {
            //(ucLead.FindControl("pnlLoadLOV") as Panel).Style.Add("min-width", "75%");

            switch (ddlLeadSourceType.SelectedValue)
            {
                case "1":
                    {
                        ucLead.LOVCode = "CRMCM";
                        ucLead.gvDisplay = "Customer_Name";
                    }
                    break;
                case "10":
                    {
                        ucLead.LOVCode = "CRMUSR";
                        ucLead.gvDisplay = "User_Name";
                    }
                    break;
                default:
                    {
                        ucLead.LOVCode = "CRMENT";
                        ucLead.gvDisplay = "Entity_Name";
                        ucLead.ucProcparam = new Dictionary<string, string>();
                        ucLead.ucProcparam.Add("@Location_ID", ddlLeadSourceType.SelectedValue);
                    }
                    break;
            }

            ucLead.Show();

            (ucLead.FindControl("lblHeader") as Label).Text = ddlLeadSourceType.SelectedItem.Text;
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    protected void btnAssetAdd_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dtAsset = (DataTable)ViewState["Assets"];

            DropDownList ddlAssetCategory = (DropDownList)grvAssets.FooterRow.FindControl("ddlAssetCategory");
            TextBox txtAssetCost = (TextBox)grvAssets.FooterRow.FindControl("txtAssetCost");
            TextBox txtFinanceAmount = (TextBox)grvAssets.FooterRow.FindControl("txtFinanceAmt");

            if (Convert.ToDouble(txtFinanceAmount.Text) > Convert.ToDouble(txtAssetCost.Text))
            {
                Utility.FunShowAlertMsg(this, "Finance Amount should be less than or equal to Cost");
                return;
            }

            int qryExists = (from AssetDetails in dtAsset.AsEnumerable()
                             where (AssetDetails.Field<string>("Asset_ID") == (Convert.ToString(ddlAssetCategory.SelectedValue)))
                             select AssetDetails).Count();

            if (qryExists == 0)
            {
                DataRow dRow = dtAsset.NewRow();
                dRow["Asset_ID"] = Convert.ToString(ddlAssetCategory.SelectedValue);
                dRow["Asset_Description"] = Convert.ToString(ddlAssetCategory.SelectedItem.Text);
                dRow["Asset_Cost"] = Convert.ToString(txtAssetCost.Text);
                dRow["Finance_Amount"] = Convert.ToString(txtFinanceAmount.Text);
                dRow["LeadAsset_ID"] = 0;
                dRow["Is_Added"] = 1;

                dtAsset.Rows.Add(dRow);

                grvAssets.DataSource = dtAsset;
                grvAssets.DataBind();

                ViewState["Assets"] = dtAsset;

                FunPriSetFooterAssetCtg();
            }
            else
            {
                Utility.FunShowAlertMsg(this, "Asset Type already Exists");
                return;
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    #region  Cancel Methods

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            divShow.Style["display"] = "none";
            pnlLeadView.Style["display"] = "none";
            btnSave.Enabled = false;
            FunProClearCachedFiles();
            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strRedirectPageView, true);
            //Response.Redirect(strRedirectPage);
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    #endregion

    #endregion

    //protected void imgProspect_OnClick(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (pnlProspectView.Visible)
    //        {
    //            pnlProspectView.Visible = false;
    //        }
    //        else
    //        {
    //            pnlProspectView.Visible = true;
    //        }
    //    }
    //    catch (Exception objException)
    //    {
    //        throw objException;
    //    }
    //}

    #region "CheckBox Events"

    protected void chkLeadSelect_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            pnlLeadView.Style["display"] = "none";
            string strSelectID = ((CheckBox)sender).ClientID;
            int _iRowIdx = Utility.FunPubGetGridRowID("gvLeadDetails", strSelectID);

            Label lblLeadID = (Label)gvLeadDetails.Rows[_iRowIdx].FindControl("lblLeadID");
            Label lblLeadPanum = (Label)gvLeadDetails.Rows[_iRowIdx].FindControl("lblLeadPanum");
            CheckBox cbxChecked = (CheckBox)gvLeadDetails.Rows[_iRowIdx].FindControl("chkLeadSelect");
            Label lblLeadStatusID = (Label)gvLeadDetails.Rows[_iRowIdx].FindControl("lblLeadStatusID");

            if (cbxChecked.Checked == true)
            {
                ViewState["Lead_ID"] = Convert.ToString(lblLeadID.Text);
                ViewState["Lead_Panum"] = Convert.ToString(lblLeadPanum.Text);
            }
            else
            {
                ViewState["Lead_ID"] = 0;
                ViewState["Lead_Panum"] = "";
            }

            DataTable dtProspectLeadDetails = (DataTable)ViewState["Prospect_LeadDetails"];
            dtProspectLeadDetails.Columns.Remove("IS_Checked");
            dtProspectLeadDetails.Columns.Add("IS_Checked");
            dtProspectLeadDetails.Columns["IS_Checked"].DefaultValue = 0;
            dtProspectLeadDetails.AcceptChanges();
            dtProspectLeadDetails.Rows[_iRowIdx]["IS_Checked"] = (cbxChecked.Checked == true) ? 1 : 0;
            dtProspectLeadDetails.AcceptChanges();
            ViewState["Prospect_LeadDetails"] = dtProspectLeadDetails;

            FunBindGrid(gvLeadDetails, dtProspectLeadDetails);
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    protected void chkSelect_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            FunPubClearPopUp();
            /*DataTable dt = ViewState["dtFollowAll"] as DataTable;// ViewState["dtFollow"] as DataTable;        
            string txtPrimeAccountNo = "", txtSubAccountNo = "", SubAccountNo = "";

            foreach (GridViewRow gvRow in grvMain.Rows)
            {
                if (((CheckBox)gvRow.FindControl("chkSelect")).Checked)
                {
                    if (txtPrimeAccountNo == "") txtPrimeAccountNo = "'" + (gvRow.FindControl("txtPrimeAccountNo") as Label).Text.Trim() + "'";
                    else txtPrimeAccountNo = txtPrimeAccountNo + "," + "'" + (gvRow.FindControl("txtPrimeAccountNo") as Label).Text.Trim() + "'";

                    SubAccountNo = (gvRow.FindControl("txtSubAccountNo") as Label).Text.Trim();
                    if (SubAccountNo == "") SubAccountNo = (gvRow.FindControl("txtPrimeAccountNo") as Label).Text.Trim() + "DUMMY";

                    if (txtSubAccountNo == "") txtSubAccountNo = "'" + SubAccountNo + "'";
                    else txtSubAccountNo = txtSubAccountNo + "," + "'" + SubAccountNo + "'";
                }
            }

            if (txtPrimeAccountNo != "" && txtSubAccountNo != "")
            {
                if (dt.Select("Panum in (" + txtPrimeAccountNo + ")and sanum  in (" + txtSubAccountNo + ") ").Length > 0)
                {
                    dt = dt.Select("Panum in (" + txtPrimeAccountNo + ")and sanum  in (" + txtSubAccountNo + ") ").CopyToDataTable();
                    grvFollowUp.DataSource = ViewState["dtFollow"] = dt;
                    grvFollowUp.DataBind();
                }
                else
                {
                    ViewState["dtFollow"] = (ViewState["dtFollowAll"] as DataTable).Clone();
                    dt = funAddRow();
                    grvFollowUp.DataSource = ViewState["dtFollow"] = dt;
                    grvFollowUp.DataBind();
                    grvFollowUp.Rows[0].Visible = false;
                }
            }
            else
            {
                grvFollowUp.DataSource = ViewState["dtFollow"] = ViewState["dtFollowAll"] as DataTable;
                grvFollowUp.DataBind();
            }*/

            bool IsCheck = false;
            int intCount = 0;
            string strPANUM = "";
            List<StringBuilder> objList = FunFormXml(out IsCheck, out intCount, out strPANUM);

            if (ObjDictionary != null)
                ObjDictionary.Clear();
            else
                ObjDictionary = new Dictionary<string, string>();

            ObjDictionary.Add("@Company_ID", intCompanyID.ToString());
            if (IsCheck) ObjDictionary.Add("@StrXml", objList[0].ToString());// strGrvXML.ToString());
            //else ObjDictionary.Add("@StrXml", objList[1].ToString());// strAllXML.ToString());
            ObjDictionary.Add("@Customer_ID", hidCustomerId.Text.ToString());
            ObjDictionary.Add("@User_ID", intUserID.ToString());
            DataTable dsAccountFollow = Utility.GetDefaultData("S3G_CLN_GetFollowUp_AccountWise", ObjDictionary);

            ViewState["dtFollow"] = ViewState["dtFollowAll"] = dsAccountFollow;
            ViewState["Followup"] = dsAccountFollow;
            if (dsAccountFollow != null && dsAccountFollow.Rows.Count > 0)
            {
                pnlFollowUp.Visible = true;
                grvFollowUp.DataSource = dsAccountFollow;
                grvFollowUp.DataBind();
            }
            else
            {
                grvFollowUp.DataSource = funAddRow();
                grvFollowUp.DataBind();
                grvFollowUp.Rows[0].Visible = false;
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    #endregion

    #region "DropDown Events"

    protected void ddlDocumentType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlDocumentType.SelectedValue != "0")
            {
                bool IsCheck = false;
                int intCount = 0;
                string strPANUM = "";
                List<StringBuilder> objList = FunFormXml(out IsCheck, out intCount, out strPANUM);

                if (!string.IsNullOrEmpty(hidCustomerId.Text) && hidCustomerId.Text != "0")
                {
                    if (ddlDocumentType.SelectedValue != "3")
                    {
                        if (grvMain.Rows.Count > 0 && intCount == 0)
                        {
                            Utility.FunShowAlertMsg(this, "No account is mapped to collect documents");
                            ddlDocument.Items.Clear();
                            return;
                        }
                        else if (intCount > 1)
                        {
                            Utility.FunShowAlertMsg(this, "More than one accounts are mapped to collect documents");
                            ddlDocument.Items.Clear();
                            return;
                        }
                    }
                }

                DataSet dSet = FunProGetDocs(strPANUM);

                txtValue.Visible = lblValue.Visible = (Convert.ToInt32(ddlDocumentType.SelectedValue) == 3) ? true : false;

                ddlDocument.FillDLL(dSet.Tables[1]);
                ddlDocument.AddItemToolTip();
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    protected void ddlFrom_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            TextBox txtName = ucFrom.FindControl("txtName") as TextBox;
            HiddenField hdnID = ucFrom.FindControl("hdnID") as HiddenField;
            txtName.Text = hdnID.Value = string.Empty;
            //            MPE.Show();
            funAssignUser(ddlFrom, ucFrom);
            FunPubClearPopUp();
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    protected void ddlLeadSourceType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ucLead.Clear();
            txtLeadSource.Text = txtOtherLead.Text = "";
            btnGetSource.Enabled = true;
            txtOtherLead.Visible = false;
            lblLeadSource.CssClass = (Convert.ToInt64(ddlLeadSourceType.SelectedValue) == 13 || Convert.ToInt64(ddlLeadSourceType.SelectedValue) == 0) ? "styleDisplayLabel" : "styleReqFieldLabel";

            switch (ddlLeadSourceType.SelectedValue)
            {
                case "0":
                case "13":
                case "14":
                case "15":
                case "16":
                    {
                        btnGetSource.Enabled = false;
                        if (Convert.ToInt32(ddlLeadSourceType.SelectedValue) == 16)
                            txtOtherLead.Visible = true;
                    }
                    break;
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    protected void ddlLeadStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToInt32(ddlLeadStatus.SelectedValue) > 1)
            {
                if (Convert.ToInt32(ObjUserInfo.ProUserLevelIdRW) < 3)
                {
                    Utility.FunShowAlertMsg(this, "Logined user dont have access to modify the Lead Status");
                    ddlLeadStatus.SelectedValue = "1";
                    return;
                }

                lblErrorMessagePass.Text = string.Empty;
                if (ModalPopupExtenderPassword.Enabled == false)
                {
                    ModalPopupExtenderPassword.Enabled = true;
                }
                ModalPopupExtenderPassword.Show();
            }
            txtLeadRemarks.Enabled = (Convert.ToInt32(ddlLeadStatus.SelectedValue) > 3) ? true : false;
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    protected void ddlQuery_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPubClearPopUp();
            Int32 intQueryType = Convert.ToInt32(ddlQuery.SelectedValue);
            if (hdnView.Value != "")
            {
                if (intQueryType == 5)
                    ddlStatus.SelectedValue = "2";
                if (intQueryType == 2 || intQueryType == 4)
                    ddlStatus.SelectedValue = "3";
            }
            else
            {
                txtTicketNo.Text = (intQueryType != 1 && intQueryType != 3 && intQueryType != 9) ? "" : hdnTicketNo.Value;
                //if (ddlQuery.SelectedValue != "1" && ddlQuery.SelectedValue != "3")
                //    txtTicketNo.Text = "";
                //else
                //    txtTicketNo.Text = hdnTicketNo.Value;
            }


            DataSet dsLookUp = ViewState["CRMLookUp"] as DataSet;
            ddlMode.Items.Clear();
            FunFillGrid(ddlMode, dsLookUp.Tables[2], "Lookup_Code", "Lookup_Description");
            if (Convert.ToInt32(ddlQuery.SelectedValue) == 8)
            {
                ddlMode.Items.RemoveAt(5); ddlMode.Items.RemoveAt(2); ddlMode.Items.RemoveAt(1);
            }
            else if (Convert.ToInt32(ddlQuery.SelectedValue) >= 1 && Convert.ToInt32(ddlQuery.SelectedValue) <= 5)
            {
                ddlMode.Items.RemoveAt(4); ddlMode.Items.RemoveAt(3);
            }

            FunProEnableDisableTrackCtrl(intQueryType);
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    protected void ddlRefType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlRefNumber.Items.Clear();
            if (ddlRefType.SelectedValue != "0")
            {
                ObjDictionary = new Dictionary<string, string>();
                ObjDictionary.Add("@Company_ID", intCompanyID.ToString());
                ObjDictionary.Add("@Option", ddlRefType.SelectedValue);
                if (ddlType.SelectedValue == "8")
                {
                    ObjDictionary.Add("@CRM_ID", ucPopUp.SelectedValue);
                }
                ddlRefNumber.BindDataTable("S3G_CLN_Get_CRMReferences", ObjDictionary, new string[] { "RefDoc_ID", "RefDoc_No" });
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    protected void ddlSearch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPubClearPopUp();
            funGridClear(grvMain);
            funGridClear(grvAssetDetails);
            ucdCustomer.ClearCustomerDetails();
            funGridClear(grvAccountDetails);
            funGridClear(grvFollowUp);
            ViewState["Followup"] = null;
            pnlFollowUp.Visible = true;

            if (ObjDictionary != null)
                ObjDictionary.Clear();
            else
                ObjDictionary = new Dictionary<string, string>();

            ObjDictionary.Add("@Company_ID", intCompanyID.ToString());
            ObjDictionary.Add("@Type", ddlType.SelectedValue.ToString());
            ObjDictionary.Add("@SearchValue", ddlSearch.SelectedValue.ToString());
            ObjDictionary.Add("@UserId", intUserID.ToString());

            DataSet dsFollow = Utility.GetDataset("S3G_CLN_GetFollowUpList", ObjDictionary);
            if (dsFollow.Tables[0] != null && dsFollow.Tables[0].Rows.Count > 0)
            {
                grvMain.DataSource = dsFollow.Tables[0];
                grvMain.DataBind();
                pnlAccount.Visible = true;
            }

            if (dsFollow.Tables[1] != null && dsFollow.Tables[1].Rows.Count > 0)
            {
                ucdCustomer.SetCustomerDetails(dsFollow.Tables[1].Rows[0], true);
                hidCustomerId.Text = dsFollow.Tables[1].Rows[0]["Customer_ID"].ToString();
                FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(hidCustomerId.Text, false, 0);
                btnUp.Attributes.Add("OnClick", "if(funUpValidate()){ window.open('../Origination/S3GOrgCustomerMaster_Add.aspx?IsFromEnquiry=Yes&qsCustomerId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=Q', 'null','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');return false;}");
            }

            if (dsFollow.Tables[2] != null && dsFollow.Tables[2].Rows.Count > 0)
            {
                grvAssetDetails.DataSource = dsFollow.Tables[2];
                grvAssetDetails.DataBind();
            }

            ViewState["dtFollow"] = ViewState["dtFollowAll"] = dsFollow.Tables[3];
            ViewState["Followup"] = dsFollow.Tables[3];
            if (dsFollow.Tables[3] != null && dsFollow.Tables[3].Rows.Count > 0)
            {
                grvFollowUp.DataSource = dsFollow.Tables[3];
                grvFollowUp.DataBind();
            }
            else
            {
                grvFollowUp.DataSource = funAddRow();
                grvFollowUp.DataBind();
                grvFollowUp.Rows[0].Visible = false;
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    protected void ddlTo_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPubClearPopUp();
            TextBox txtName = ucTo.FindControl("txtName") as TextBox;
            HiddenField hdnID = ucTo.FindControl("hdnID") as HiddenField;
            txtName.Text = hdnID.Value = string.Empty;
            funAssignUser(ddlTo, ucTo);

        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPubClearPopUp();
            //(ucPopUp.FindControl("hdnText") as HiddenField).Value = txtName.Text = "";
            //(ucPopUp.FindControl("hdnID") as HiddenField).Value = "";
            //(ucPopUp.FindControl("pnlLoadLOV") as Panel).Visible = false;
            ucPopUp.Clear();
            funAssignPopupValue();
            txtName.Text = "";
            ddlSearch.Items.Clear();
            funGridClear(grvMain);
            funGridClear(grvAssetDetails);
            ucdCustomer.ClearCustomerDetails();
            pnlAccount.Visible = pnlFollowUp.Visible = pnlAccountInformation.Visible = false;
            funGridClear(grvFollowUp);
            ViewState["Followup"] = null;
            funGridClear(grvAccountDetails);
            hidCustomerId.Text = "";
            FunProClearAllTabs();
            FunProProspectClear();
            //FunProClearLead();
            FunPriClearGroupDetails();

            if (ddlType.SelectedValue == "0")
            {
                Session.Remove("FU_TYPE");
            }
            else
            {
                Session["FU_TYPE"] = ddlType.SelectedValue;
            }

            if (ddlType.SelectedValue == "8")
            {
                imgCustomer.Visible = false;
                imgProspect.Visible = true;
            }
            else
            {
                imgCustomer.Visible = true;
                imgProspect.Visible = false;
            }

            //if (ObjDictionary != null)
            //    ObjDictionary.Clear();
            //else
            //    ObjDictionary = new Dictionary<string, string>();

            //ObjDictionary.Add("@Company_ID", intCompanyID.ToString());
            //ObjDictionary.Add("@Type", ddlType.SelectedValue.ToString());
            //ObjDictionary.Add("@UserId", Convert.ToString(intUserID));
            //ddlSearch.BindDataTable("S3G_CLN_GetFollowUpType", ObjDictionary, true, "-- Select --", new string[] { "Searchvalue", "Searchvalue" });
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    #endregion

    #region "GridView Events"

    protected void grvFollowUp_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton btnRemove = (LinkButton)e.Row.FindControl("btnRemove");
                Label txtTicketNo = (Label)e.Row.FindControl("txtTicketNo");
                LinkButton btnView = (LinkButton)e.Row.FindControl("btnView");
                Label txtQuery = (Label)e.Row.FindControl("txtQuery");
                LinkButton lnkQuery = (LinkButton)e.Row.FindControl("lnkQuery");
                HiddenField hidQueryType = (HiddenField)e.Row.FindControl("hidQueryType");
                HiddenField hidVersionNo = (HiddenField)e.Row.FindControl("hidVersionNo");
                HiddenField hidStatus = (HiddenField)e.Row.FindControl("hidStatus");
                HiddenField IsMax = (HiddenField)e.Row.FindControl("IsMax");
                Label txtDescription = (Label)e.Row.FindControl("txtDescription");

                //txtDescription.Text = txtDescription.Text.Replace("~Remarks:~", "<font  color='blue;' Weight='bold;'>Remarks:</font>");

                if (txtTicketNo.Text.Trim() == "-1")
                    e.Row.Visible = false;

                if (txtTicketNo.Text.Trim() == "0" || txtTicketNo.Text.Trim() == "-1")
                {
                    txtTicketNo.Text = "";
                    btnRemove.Enabled = btnView.Enabled = false;
                }

                if (hidVersionNo.Value.Trim() == "" || hidVersionNo.Value.Trim() == "0")
                {
                    btnRemove.Enabled = btnView.Enabled = lnkQuery.Visible = false;
                }
                else
                {
                    txtQuery.Visible = false;
                }

                if (IsMax.Value.Trim().ToUpper() == "TRUE" && txtTicketNo.Text != "")
                {
                    lnkQuery.Visible = btnRemove.Enabled = btnView.Enabled = true;
                    txtQuery.Visible = false;
                }
                else
                {
                    lnkQuery.Visible = btnRemove.Enabled = btnView.Enabled = false;
                    txtQuery.Visible = true;
                }
                if (hidQueryType.Value == "2" || hidQueryType.Value == "4")
                {
                    lnkQuery.Visible = false;
                    txtQuery.Visible = true;
                }

                //if (e.Row.RowIndex == 0)
                //    e.Row.Cells[0].RowSpan = 5;// ((DataTable)grvFollowUp.DataSource).Columns.Count;

                if (hidStatus.Value == "3")
                {
                    lnkQuery.Visible = btnRemove.Enabled = btnView.Enabled = false;
                    txtQuery.Visible = true;
                }

                //if (e.Row.RowIndex == 0)
                //    FunProAddDynamicRow(e.Row);
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    protected void grvPopUp_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                for (int i = 0; i < e.Row.Cells.Count; i++)
                {
                    switch (((System.Data.DataRowView)(e.Row.DataItem)).Row.ItemArray[i].GetType().ToString())
                    {
                        case "System.Decimal":
                            e.Row.Cells[i].HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
                            e.Row.Cells[i].Text = Convert.ToDecimal(e.Row.Cells[i].Text).ToString(strSuffix);
                            break;
                        case "System.Int32":
                            e.Row.Cells[i].HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                            break;
                        case "System.DateTime":
                            {
                                e.Row.Cells[i].HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Left;
                                e.Row.Cells[i].Text = FormatDate(e.Row.Cells[i].Text);
                                break;
                            }
                        case "System.String":
                            e.Row.Cells[i].HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Left;
                            break;
                    }
                    if (grvPopUp.Rows.Count > 0 && (grvPopUp.Rows[0].Cells[0].Text.Trim() == "&nbsp;" || grvPopUp.Rows[0].Cells[0].Text.Trim() == ""))
                    {
                        if (i != 2 && i != 6)
                            grvPopUp.Rows[0].Cells[i].Text = "";
                    }
                }
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    protected void grvAccountDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label txtNOD = e.Row.FindControl("txtNOD") as Label;
                LinkButton lnkNOD = e.Row.FindControl("lnkNOD") as LinkButton;
                if (txtNOD.Text.Trim() == "0")
                    lnkNOD.Visible = false;
                else
                    txtNOD.Visible = false;
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    protected void grvAssets_DeleteClick(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            DataTable dtAsset = (DataTable)ViewState["Assets"];
            dtAsset.Rows[e.RowIndex].Delete();
            dtAsset.AcceptChanges();

            if (dtAsset.Rows.Count == 0)
            {
                FunProInitializeAssetRow();
            }
            else
            {
                grvAssets.DataSource = dtAsset;
                grvAssets.DataBind();

                ViewState["Assets"] = dtAsset;
                FunPriSetFooterAssetCtg();
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    protected void grvAssets_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                TextBox txtAssetCost = (TextBox)e.Row.FindControl("txtAssetCost");
                TextBox txtFinanceAmt = (TextBox)e.Row.FindControl("txtFinanceAmt");
                txtAssetCost.SetDecimalPrefixSuffix(ObjS3GSession.ProGpsPrefixRW, ObjS3GSession.ProGpsSuffixRW, true, false, "Cost");
                txtFinanceAmt.SetDecimalPrefixSuffix(ObjS3GSession.ProGpsPrefixRW, ObjS3GSession.ProGpsSuffixRW, true, false, "Finance Amount");
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    protected void gvPRDDT_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblDocumentID = (Label)e.Row.FindControl("lblDocumentID");
                Label lblCanView = (Label)e.Row.FindControl("lblCanView");
                Label lblCollectedDate = (Label)e.Row.FindControl("lblCollectedDate");
                Label lblScannedDate = (Label)e.Row.FindControl("lblScannedDate");
                Label lblCollectedBy = (Label)e.Row.FindControl("lblCollectedBy");
                ImageButton hyplnkView = (ImageButton)e.Row.FindControl("hyplnkView");
                CheckBox CbxCheck = (CheckBox)e.Row.FindControl("CbxCheck");
                Label lblDocPath = (Label)e.Row.FindControl("lblDocPath");
                Label lblValue = (Label)e.Row.FindControl("lblValue");
                LinkButton lnkbtnDocEdit = (LinkButton)e.Row.FindControl("lnkbtnDocEdit");

                hyplnkView.CssClass = "styleGridEdit";

                //if (lblCollectedDate.Text != "")
                //{
                //    lblCollectedDate.Text = Convert.ToDateTime(lblCollectedDate.Text).ToString(strDateFormat);
                //}
                //if (lblScannedDate.Text != "")
                //{
                //    lblScannedDate.Text = Convert.ToDateTime(lblScannedDate.Text).ToString(strDateFormat);
                //}

                if (lblCanView.Text == "0" || hyplnkView.CommandArgument.Trim().ToUpper() == lblDocPath.Text.ToUpper())
                {
                    hyplnkView.Visible = false;
                }

                if (lblCollectedBy.Text != "" && lblCollectedBy.Text != "-1")
                {
                    CbxCheck.Checked = true;
                }

                lnkbtnDocEdit.Enabled = (Convert.ToString(lblDocumentID.Text) == "0") ? false : true;
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    protected void gvLeadDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblPricingID = (Label)e.Row.FindControl("lblPricingID");
                Label lblLeadNo = (Label)e.Row.FindControl("lblLeadNo");
                CheckBox cbxIsChecked = (CheckBox)e.Row.FindControl("chkLeadSelect");

                LinkButton lnkLeadEdit = (LinkButton)e.Row.FindControl("lnkLeadEdit");
                LinkButton lnkLeadView = (LinkButton)e.Row.FindControl("lnkLeadView");
                Label lblLeadStatusID = (Label)e.Row.FindControl("lblLeadStatusID");

                //cbxIsChecked.Enabled = (Convert.ToInt32(lblPricingID.Text) > 0) ? false : true;

                if (cbxIsChecked.Checked == true)
                {
                    lnkLeadView.Enabled = true;
                    lnkLeadEdit.Enabled = (Convert.ToInt32(lblLeadStatusID.Text) == 5 || Convert.ToInt32(lblLeadStatusID.Text) == 4) ? false : true;
                    if (Convert.ToInt32(lblPricingID.Text) > 0)
                    {
                        lnkLeadEdit.Enabled = false;
                    }
                    if (Convert.ToString(lblLeadNo.Text) == "" || Convert.ToString(lblLeadNo.Text) == "GENERAL")
                    {
                        lnkLeadEdit.Enabled = lnkLeadView.Enabled = false;
                    }
                }
                else
                {
                    lnkLeadEdit.Enabled = lnkLeadView.Enabled = false;
                }
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    #endregion

    #region "Link Events"

    protected void hyplnkView_Click(object sender, EventArgs e)
    {
        try
        {
            string strFieldAtt = ((ImageButton)sender).ClientID;
            int gRowIndex = Utility.FunPubGetGridRowID("gvPRDDT", strFieldAtt);
            Label lblDocPath = (Label)gvPRDDT.Rows[gRowIndex].FindControl("lblDocPath");
            if (((ImageButton)sender).CommandArgument.Trim().ToUpper() != lblDocPath.Text.ToUpper())
            {

                string strFileName = ((ImageButton)sender).CommandArgument.Replace("\\", "/").Trim();
                string strScipt = "window.open('../Common/S3GViewFile.aspx?qsFileName=" + strFileName + "', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strScipt, true);
            }
            else
            {
                Utility.FunShowAlertMsg(this, "File not to be scanned yet");
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    protected void tcCRM_ActiveTabChanged(object sender, EventArgs e)
    {
        try
        {
            FunProClearAllTabs();
            pnlLeadView.Style["display"] = "none";
            int intTabIndex = tcCRM.ActiveTabIndex;
            if (ObjDictionary != null)
                ObjDictionary.Clear();
            else
                ObjDictionary = new Dictionary<string, string>();

            DataSet dsLead = new DataSet();

            if ((ucPopUp.FindControl("hdnText") as HiddenField).Value.Trim() != "" && ddlType.SelectedValue != "0")
            {
                //dsLead = new DataSet();
                ////txtName.Text = (ucPopUp.FindControl("hdnText") as HiddenField).Value;
                ObjDictionary.Add("@Company_ID", Convert.ToString(intCompanyID));
                ObjDictionary.Add("@Type", ddlType.SelectedValue.ToString());
                if (ddlType.SelectedValue == "8")
                {
                    ObjDictionary.Add("@SearchValue", ucPopUp.SelectedValue);
                }
                else
                {
                    if (ddlType.SelectedValue == "1" || ddlType.SelectedValue == "2" || ddlType.SelectedValue == "3")
                    {
                        ObjDictionary.Add("@SearchValue", hidCustomerId.Text);
                    }
                    else
                    {
                        ObjDictionary.Add("@SearchValue", ucPopUp.SelectedText);
                    }
                }
                ObjDictionary.Add("@UserId", intUserID.ToString());
                if (ddlType.SelectedValue == "1" || ddlType.SelectedValue == "2" || ddlType.SelectedValue == "3")
                    ObjDictionary.Add("@SearchType", "Customer");
                else
                    ObjDictionary.Add("@SearchType", "Account");

                if (tcCRM.ActiveTabIndex == 1)
                {
                    if (Convert.ToInt32(ViewState["Lead_ID"]) > 0 || Convert.ToString(ViewState["Lead_Panum"]) != "")
                    {
                        ObjDictionary.Add("@Lead_ID", Convert.ToString(ViewState["Lead_ID"]));
                        ObjDictionary.Add("@Lead_Panum", Convert.ToString(ViewState["Lead_Panum"]));
                    }
                    else
                    {
                        Utility.FunShowAlertMsg(this, "Select Lead No");
                        tcCRM.ActiveTabIndex = 0;
                        intTabIndex = 0;
                    }
                }


                if (tcCRM.ActiveTabIndex == 2 && hdnLeadSourceType.Value == "8")
                {


                    if (Convert.ToInt32(ViewState["Lead_ID"]) > 0 || Convert.ToString(ViewState["Lead_Panum"]) != "")
                    {
                        ObjDictionary.Add("@Lead_ID", Convert.ToString(ViewState["Lead_ID"]));
                        ObjDictionary.Add("@Lead_Panum", Convert.ToString(ViewState["Lead_Panum"]));
                    }
                    else
                    {
                        Utility.FunShowAlertMsg(this, "Select Lead No");
                        tcCRM.ActiveTabIndex = 0;
                        intTabIndex = 0;
                    }
                }

                if (tcCRM.ActiveTabIndex == 1 && Convert.ToString(ViewState["Lead_Panum"]) == "GENERAL")
                {
                    ddlPrograms.Visible = btnMoveEnquiry.Visible = false;
                }
                else
                {
                    ddlPrograms.Visible = btnMoveEnquiry.Visible = true;
                }


                if (ddlType.SelectedValue == "8" || ddlType.SelectedValue == "9")
                {
                    dsLead = Utility.GetDataset("CN_GET_CRM_PROSPECTLIST", ObjDictionary);
                }
                else if (intTabIndex <= 3)
                {
                    dsLead = Utility.GetDataset("S3G_CLN_GetCRMFollowUpList", ObjDictionary);
                }
            }
            else if (tcCRM.ActiveTabIndex > 0)
            {
                Utility.FunShowAlertMsg(this, "Select Prospect/Customer");
                tcCRM.ActiveTabIndex = 0;
                btnAddLead.Visible = false;
                intTabIndex = 0;
            }

            switch (intTabIndex)
            {
                case 0: //Lead Details
                    {
                        lblCRMDetail.Text = tcCRM.Tabs[intTabIndex].HeaderText;
                        FunBindGrid(gvLeadDetails, ((ViewState["Prospect_LeadDetails"] != null) ? (DataTable)ViewState["Prospect_LeadDetails"] : null));
                        pnlLeadViewDetails.Visible = true;
                        ucLead.Hide();
                    }
                    break;
                case 1: //Track Details
                    {
                        if (dsLead != null)
                        {
                            lblCRMDetail.Text = tcCRM.Tabs[intTabIndex].HeaderText;
                            pnlFollowUp.Visible = true;
                            DataTable dtExists = new DataTable();

                            if (ViewState["NewFollowup"] == null || ((DataTable)ViewState["NewFollowup"]).Rows.Count == 0)
                            {
                                DataTable dtFromDB = dsLead.Tables[3];

                                ViewState["dtFollow"] = ViewState["dtFollowAll"] = dsLead.Tables[3];

                                if (ViewState["NewFollowup"] == null)
                                {
                                    ViewState["NewFollowup"] = dsLead.Tables[3].Clone();
                                    dtExists = dtFromDB;
                                }
                                else
                                {
                                    dtExists = ((DataTable)ViewState["NewFollowup"]).Copy();
                                    dtExists.Merge(dtFromDB);
                                }

                                ViewState["dtFollow"] = ViewState["dtFollowAll"] = dtExists;
                            }
                            else
                            {
                                //dtExists = ((DataTable)ViewState["NewFollowup"]).Copy();
                                dtExists.Merge((DataTable)ViewState["dtFollow"]);
                            }
                            if (dtExists != null && dtExists.Rows.Count > 0)
                            {
                                FunBindTrackDetails(dtExists);
                            }
                            else
                            {
                                grvFollowUp.DataSource = funAddRow();
                                grvFollowUp.DataBind();
                                grvFollowUp.Rows[0].Visible = false;
                            }
                        }
                    }
                    break;
                case 2:         /*Document Details*/
                    {
                        lblCRMDetail.Text = tcCRM.Tabs[intTabIndex].HeaderText;
                        //gvPRDDT.DataSource = dsFollow.Tables[4];
                        //gvPRDDT.DataBind();
                        pnlDocuments.Visible = true;
                        lblValue.Visible = txtValue.Visible = false;
                        FunProClearDocumentControls();
                    }
                    break;
                case 3:         /*Account Details*/
                    {
                        lblCRMDetail.Text = tcCRM.Tabs[intTabIndex].HeaderText;
                        if (dsLead.Tables[0] != null && dsLead.Tables[0].Rows.Count > 0)
                        {
                            grvMain.DataSource = dsLead.Tables[0];
                            grvMain.DataBind();
                        }
                        pnlAccount.Visible = true;
                    }
                    break;
                case 4:         /*Status Details*/
                    {
                        lblCRMDetail.Text = tcCRM.Tabs[intTabIndex].HeaderText;

                        DataTable dtStatusDtl = Utility.GetDefaultData("S3G_ORG_GETCRMSTATUSDTL", ObjDictionary);

                        //if (dsLead.Tables[2] != null && dsLead.Tables[2].Rows.Count > 0)
                        //{
                        //    grvAssetDetails.DataSource = dsLead.Tables[2];
                        //    grvAssetDetails.DataBind();
                        //    pnlStatus.Visible = true;
                        //}
                        if (dtStatusDtl != null && dtStatusDtl.Rows.Count > 0)
                        {
                            grvAssetDetails.DataSource = dtStatusDtl;
                            grvAssetDetails.DataBind();
                            pnlStatus.Visible = true;
                        }
                    }
                    break;
                case 5:     /*Group Details*/
                    {
                        DataSet dtGroupDtls = Utility.GetDataset("S3G_ORG_GETGROUPDTLS", ObjDictionary);
                        lblCRMDetail.Text = tcCRM.Tabs[intTabIndex].HeaderText;
                        pnlGroupDetails.Visible = true;
                        grvGroupDtls.DataSource = dtGroupDtls.Tables[0];
                        grvGroupDtls.DataBind();
                        grvCustDtls.DataSource = dtGroupDtls.Tables[1];
                        grvCustDtls.DataBind();
                    }
                    break;
                case 6:  /* DC  Dairy*/
                    {
                        lblCRMDetail.Text = tcCRM.Tabs[intTabIndex].HeaderText;
                        bool IsCheck = false;
                        int intCount = 0;
                        string strPANUM = "";
                        List<StringBuilder> objList = FunFormXmlDC(out IsCheck, out intCount, out strPANUM);

                        if (ObjDictionary != null)
                            ObjDictionary.Clear();
                        else
                            ObjDictionary = new Dictionary<string, string>();

                        ObjDictionary.Add("@Company_ID", intCompanyID.ToString());
                        if (IsCheck) ObjDictionary.Add("@StrXml", objList[0].ToString());// strGrvXML.ToString());
                        else ObjDictionary.Add("@StrXml", objList[1].ToString());// strAllXML.ToString());
                        grvDcDairyAccountDetails.DataSource = Utility.GetDefaultData("CN_GET_FLW_DCDAIRY_AC_DTL", ObjDictionary);
                        grvDcDairyAccountDetails.DataBind();
                        if (grvDcDairyAccountDetails.Rows.Count == 0)
                        {
                            Utility.FunShowAlertMsg(this, "DC Dairy does not exists");
                            return;
                        }
                    }
                    break;
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    protected void txtBilledAmount_Click(object sender, EventArgs e)
    {
        try
        {
            FunPubClearPopUp();
            funShowPopUp(sender, e, "Billed Amount");
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    protected void txtCollectedAmount_Click(object sender, EventArgs e)
    {
        try
        {
            FunPubClearPopUp();
            funShowPopUp(sender, e, "Collected Amount");
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    protected void lnkLeadView_Click(object sender, EventArgs e)
    {
        try
        {
            string strSelectID = ((LinkButton)sender).ClientID;
            int _iRowIdx = Utility.FunPubGetGridRowID("gvLeadDetails", strSelectID);
            LinkButton lnkLeadView = (LinkButton)gvLeadDetails.Rows[_iRowIdx].FindControl("lnkLeadView");
            Label lblLeadID = (Label)gvLeadDetails.Rows[_iRowIdx].FindControl("lblLeadID");

            if (ObjDictionary != null)
                ObjDictionary.Clear();
            else
                ObjDictionary = new Dictionary<string, string>();

            ObjDictionary.Add("@Company_ID", Convert.ToString(intCompanyID));
            ObjDictionary.Add("@Lead_ID", Convert.ToString(lblLeadID.Text));

            DataSet dsLeadDetails = Utility.GetDataset("S3G_ORG_GETLEADDETAILS", ObjDictionary);
            FunPriLoadLeadDetails(dsLeadDetails);
            FunPriEnableDisableLeadDtls(2);
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    protected void lnkbtnDocEdit_Click(object sender, EventArgs e)
    {
        try
        {
            string strSelectID = ((LinkButton)sender).ClientID;
            int _iRowIdx = Utility.FunPubGetGridRowID("gvPRDDT", strSelectID);
            LinkButton lnkbtnDocEdit = (LinkButton)gvPRDDT.Rows[_iRowIdx].FindControl("lnkbtnDocEdit");
            Label lblDocumentID = (Label)gvPRDDT.Rows[_iRowIdx].FindControl("lblDocumentID");

            DataTable dtDoc = (DataTable)ViewState["Documents"];

            DataRow[] drDoc = dtDoc.Select("Documents_ID = " + lblDocumentID.Text);

            iDocumentID = Convert.ToInt32(lblDocumentID.Text);
            txtCRMDocumentID.Text = Convert.ToString(iDocumentID);
            ddlDocument.SelectedValue = Convert.ToString(drDoc[0]["Doc_Cat_ID"]);
            ddlCollectedBy.SelectedValue = Convert.ToString(drDoc[0]["Collected_By"]);
            ddlCollectedBy.SelectedText = Convert.ToString(drDoc[0]["Collected_By_Name"]);
            ddlScannedBy.SelectedValue = Convert.ToString(drDoc[0]["Scanned_By"]);
            ddlScannedBy.SelectedText = Convert.ToString(drDoc[0]["Scanned_By_Name"]);
            txtCollectedDate.Text = Convert.ToString(drDoc[0]["Collected_Date"]);
            txtscannedDate.Text = Convert.ToString(drDoc[0]["Scanned_Date"]);
            //hdnSelectedPath.Value = Convert.ToString(lblDocPath.Text);
            lblCurrentPath.Text = Convert.ToString(drDoc[0]["Scanned_Ref_No"]);
            chkSelect.Checked = (Convert.ToInt32(drDoc[0]["ViewDoc"]) == 1) ? true : false;
            txtValue.Text = Convert.ToString(drDoc[0]["Value"]);
            txtDocRemarks.Text = Convert.ToString(drDoc[0]["Remarks"]);
            chkIsCollected.Checked = Convert.ToBoolean(drDoc[0]["Is_Collected"]);

            btnDocUpdate.Visible = true;
            ddlDocumentType.Enabled = btnDocAdd.Visible = false;
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    protected void lnkbtnDocRemove_Click(object sender, EventArgs e)
    {
        try
        {
            string strSelectID = ((LinkButton)sender).ClientID;
            int _iRowIdx = Utility.FunPubGetGridRowID("gvPRDDT", strSelectID);
            Label lblDocumentID = (Label)gvPRDDT.Rows[_iRowIdx].FindControl("lblDocumentID");

            //DataTable dtDoc = (DataTable)ViewState["Docs" + ddlDocumentType.SelectedValue];
            //Label lblDocumentCategoryID = (Label)gvPRDDT.Rows[_iRowIdx].FindControl("lblPRTID");

            //if (Convert.ToString(lblDocumentID.Text) != "0")
            //{
            //    strDeletedDocumentID = strDeletedDocumentID + "," + Convert.ToString(lblDocumentID.Text);
            //}
            //if (dtDoc != null && dtDoc.Rows.Count > 0)
            //{
            //    string strFilter = "Documents_ID = " + Convert.ToString(lblDocumentID.Text);
            //    strFilter = strFilter + " and Doc_Cat_ID = " + Convert.ToString(lblDocumentCategoryID.Text);

            //    DataRow[] drRow = dtDoc.Select(strFilter);
            //    foreach (DataRow dr in drRow)
            //    {
            //        dr.Delete();
            //    }
            //    dtDoc.AcceptChanges();
            //    ViewState["Docs" + ddlDocumentType.SelectedValue] = dtDoc;
            //}

            Dictionary<string, string> Procparam = new Dictionary<string, string>();
            Procparam.Add("@Option", "9");
            Procparam.Add("@CRM_DOC_ID", Convert.ToString(lblDocumentID.Text));
            Utility.GetDefaultData("S3G_ORG_CRMCMNLST", Procparam);

            FunProGetDocs("");
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    protected void lnkLeadEdit_Click(object sender, EventArgs e)
    {
        try
        {
            FunPriEnableDisableLeadDtls(3);
            string strSelectID = ((LinkButton)sender).ClientID;
            int _iRowIdx = Utility.FunPubGetGridRowID("gvLeadDetails", strSelectID);
            LinkButton lnkLeadView = (LinkButton)gvLeadDetails.Rows[_iRowIdx].FindControl("lnkLeadView");
            Label lblLeadID = (Label)gvLeadDetails.Rows[_iRowIdx].FindControl("lblLeadID");

            if (ObjDictionary != null)
                ObjDictionary.Clear();
            else
                ObjDictionary = new Dictionary<string, string>();

            ObjDictionary.Add("@Company_ID", Convert.ToString(intCompanyID));
            ObjDictionary.Add("@Lead_ID", Convert.ToString(lblLeadID.Text));
            iLeadID = Convert.ToInt32(lblLeadID.Text);

            DataSet dsLeadDetails = Utility.GetDataset("S3G_ORG_GETLEADDETAILS", ObjDictionary);
            FunPriLoadLeadDetails(dsLeadDetails);
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    protected void lnkNOD_Click(object sender, EventArgs e)
    {
        try
        {
            FunPubClearPopUp();
            funShowPopUp(sender, e, "NOD");
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    protected void lnkJE_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            FunPubClearPopUp();
            LinkButton lnkJE = (sender) as LinkButton;
            //foreach (GridViewRow gvRow in grvAccountDetails.Rows)
            //{
            //    LinkButton lnkJE1 = gvRow.FindControl("lnkJE") as LinkButton;
            //    if (lnkJE1.ClientID != lnkJE.ClientID)
            //        rdoJE1.Checked = false;
            //}
            //LinkButton lnkNOD = (LinkButton)rdoJE.Parent.Parent.FindControl("lnkNOD");
            funShowPopUp(lnkJE, e, "Ledger Entry");
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    protected void lnkgdStatusPanum_Click(object sender, EventArgs e)
    {
        try
        {
            divShow.Style["display"] = "block";
            string strSelectID = ((LinkButton)sender).ClientID;
            int _iRowIdx = Utility.FunPubGetGridRowID("grvAssetDetails", strSelectID);

            LinkButton lblPanum = (LinkButton)grvAssetDetails.Rows[_iRowIdx].FindControl("lnkgdStatusPanum");
            Dictionary<string, string> Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            Procparam.Add("@Panum", Convert.ToString(lblPanum.Text));
            DataTable dtTrackDtl = Utility.GetDefaultData("S3G_ORG_GETCRMACCOUNTDTLS", Procparam);

            StringBuilder strHtml = new StringBuilder();
            strHtml.Append("<html>");
            strHtml.Append("<head>");
            string strStyle = string.Empty;
            strStyle = strStyle + "<style>";
            strStyle = strStyle + "table#tblMain {border: 1px solid black;border-collapse: collapse;border-spacing: 10px; align:center;}";
            strStyle = strStyle + "table#tblMain th	{background-color: #F2F2F2 ;color:#003d9e;font-family: calibri, Arial, Sans-Serif;font-size:14px;";
            strStyle = strStyle + "border: 1px solid #003d9e;border-collapse: collapse;}";
            strStyle = strStyle + "table#tblMain td	{background-color: #F0FFF0 ;color:#003d9e;font-family: calibri, Arial, Sans-Serif;font-size:15px;";
            strStyle = strStyle + "border: 1px solid #003d9e;border-collapse: collapse;}";
            //strStyle = strStyle + "table#tblMain tr:nth-child(even) {background-color: #EBD699;}";
            //strStyle = strStyle + "table#tblMain tr:nth-child(odd) {background-color : red;}";
            strStyle = strStyle + "</style>";

            strHtml.Append(strStyle);
            strHtml.Append("</head>");
            strHtml.Append("<body>");
            strHtml.Append("<table id=tblMain width=95% align = center>");
            strHtml.Append("<tr width=10% >");
            strHtml.Append("<th width=10%>Program</th>");
            strHtml.Append("<th width=10%>Document Number</th>");
            strHtml.Append("<th width=20%>Document Date</th>");
            strHtml.Append("<th width=10%>Created User / Approved User</th>");
            strHtml.Append("</tr>");

            for (Int32 i = 0; i < dtTrackDtl.Rows.Count; i++)
            {
                strHtml.Append("<tr width=100%>");
                strHtml.Append("<td width=25%>" + Convert.ToString(dtTrackDtl.Rows[i]["Program"]) + "</td>");
                strHtml.Append("<td width=25%>" + Convert.ToString(dtTrackDtl.Rows[i]["Document_Number"]) + "</td>");
                strHtml.Append("<td width=20% align=center>" + Convert.ToString(dtTrackDtl.Rows[i]["Document_Date"]) + "</td>");
                strHtml.Append("<td width=30%>" + Convert.ToString(dtTrackDtl.Rows[i]["User_Name"]) + "</td>");
                strHtml.Append("</tr>");
            }

            strHtml.Append("</table>");
            strHtml.Append("</body>");
            strHtml.Append("</html>");
            divhtml.InnerHtml = Convert.ToString(strHtml);
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    #endregion

    #endregion


    [System.Web.Services.WebMethod]
    public static string[] GetBranchList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        Procparam.Clear();
        Procparam.Add("@Company_ID", System.Web.HttpContext.Current.Session["AutoSuggestCompanyID"].ToString());
        Procparam.Add("@User_ID", System.Web.HttpContext.Current.Session["AutoSuggestUserID"].ToString());
        Procparam.Add("@Program_Id", "241");
        Procparam.Add("@Is_Active", "1");
        Procparam.Add("@PrefixText", prefixText);
        if (System.Web.HttpContext.Current.Session["LOBAutoSuggestValue"] != null)
        {
            if (System.Web.HttpContext.Current.Session["LOBAutoSuggestValue"].ToString() != "0")
                Procparam.Add("@LOB_ID", System.Web.HttpContext.Current.Session["LOBAutoSuggestValue"].ToString());
            else
                Procparam.Add("@LOB_ID", "0");
        }
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_SYSAD_GET_BRANCHLIST_AGT", Procparam));
        return suggetions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetSalesPersonList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        Procparam.Clear();
        Procparam.Add("@Company_ID", System.Web.HttpContext.Current.Session["AutoSuggestCompanyID"].ToString());
        Procparam.Add("@Prefix", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_Get_Userlist_AGT", Procparam));

        return suggetions.ToArray();
    }

    #region "Methods"

    private void funAssignUser(DropDownList ddl, UserControls_LOBMasterView uc)
    {
        try
        {
            switch (ddl.SelectedValue)
            {
                case "1":
                    uc.strLOV_Code = "USM";
                    break;
                case "2":
                    uc.strLOV_Code = "CMD";
                    break;
                case "3":
                    uc.strLOV_Code = "UTPA";
                    break;
                case "4":
                    uc.strLOV_Code = "DCE";
                    break;
                case "5":
                    uc.strLOV_Code = "DCT";
                    break;
                case "6":
                    uc.strLOV_Code = "ENT";
                    break;
                case "10":
                    uc.strLOV_Code = "PROS";
                    break;
                default:
                    uc.strLOV_Code = "USM";
                    break;
            }
            uc.strControlID = uc.ClientID;
            HiddenField hdnLovCode = uc.FindControl("hdnLovCode") as HiddenField;
            HiddenField hdnCtrlId = uc.FindControl("hdnCtrlId") as HiddenField;
            hdnCtrlId.Value = uc.ClientID;
            hdnLovCode.Value = uc.strLOV_Code;
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    private string FunLoadXml()
    {
        StringBuilder strBuilder = new StringBuilder();
        try
        {
            strBuilder.Append("<Root>");

            int intRowCount = ((DataTable)ViewState["NewFollowup"]).Rows.Count;

            foreach (GridViewRow gvRow in grvFollowUp.Rows)
            {
                HiddenField hidQueryType = (HiddenField)gvRow.FindControl("hidQueryType");
                //if (intRowCount > gvRow.RowIndex)
                if (Convert.ToString(hidQueryType.Value) != "" && Convert.ToInt32(hidQueryType.Value) > 0)
                {
                    HiddenField hidFromID = (HiddenField)gvRow.FindControl("hidFromID");
                    HiddenField hidFromType = (HiddenField)gvRow.FindControl("hidFromType");
                    HiddenField hidToID = (HiddenField)gvRow.FindControl("hidToID");
                    HiddenField hidToType = (HiddenField)gvRow.FindControl("hidToType");
                    HiddenField hidMode = (HiddenField)gvRow.FindControl("hidMode");
                    HiddenField hidVersionNo = (HiddenField)gvRow.FindControl("hidVersionNo");
                    HiddenField IsMax = (HiddenField)gvRow.FindControl("IsMax");
                    HiddenField hidStatus = (HiddenField)gvRow.FindControl("hidStatus");
                    HiddenField hidFollowup_Detail_ID = (HiddenField)gvRow.FindControl("hidFollowup_Detail_ID");

                    Label txtDate = (Label)gvRow.FindControl("txtDate");
                    Label txtDescription = (Label)gvRow.FindControl("txtDescription");
                    Label txtNotifyDate = (Label)gvRow.FindControl("txtNotifyDate");
                    Label txtTicketNo = (Label)gvRow.FindControl("txtTicketNo");
                    Label lblTrackLeadID = (Label)gvRow.FindControl("lblTrackLeadID");
                    Label lblTrackPanum = (Label)gvRow.FindControl("lblTrackPanum");
                    string strTktNo = txtTicketNo.Text.Trim();
                    if (txtTicketNo.Text == "") strTktNo = "0";

                    if (hidMode.Value.Trim() != "6" && hidMode.Value.Trim() != "")
                    {
                        strBuilder.Append("<Details Ticket_No='" + strTktNo + "' Date ='" + Utility.StringToDate(txtDate.Text.Trim()) + "'  ");
                        strBuilder.Append("User_FromType='" + hidFromType.Value + "'  User_From='" + hidFromID.Value + "'  ");
                        strBuilder.Append("User_ToType='" + hidToType.Value + "'  User_To='" + hidToID.Value + "'  ");
                        strBuilder.Append("QueryType_Code='70'  Query_Code='" + hidQueryType.Value + "'  ");
                        strBuilder.Append("Description='" + txtDescription.Text.Trim() + "'  Notify_Date='" + Utility.StringToDate(txtNotifyDate.Text.Trim()) + "'  ");
                        strBuilder.Append("ModeType_Code='69'  Mode_Code='" + hidMode.Value + "'  ");
                        strBuilder.Append("StatusType_Code='71'  Status_Code='" + hidStatus.Value + "'  Version_No='" + hidVersionNo.Value + "' IsMax='" + IsMax.Value + "' DocRef_No='' Followup_Detail_ID= '" + hidFollowup_Detail_ID.Value + "' Lead_ID = '" + lblTrackLeadID.Text + "' ");
                        strBuilder.Append("Panum='" + Convert.ToString(lblTrackPanum.Text) + "'  ");
                        strBuilder.Append("/>");
                    }
                }
            }
            strBuilder.Append("</Root>");
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
        return strBuilder.ToString();
    }

    private void FunPriPageLoad()
    {
        S3GSession ObjS3GSession = null;
        try
        {
            this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);

            //Date Format
            ObjS3GSession = new S3GSession();
            strDateFormat = ObjS3GSession.ProDateFormatRW;

            //End
            //User Authorization
            bCreate = ObjUserInfo.ProCreateRW;
            bModify = ObjUserInfo.ProModifyRW;
            bQuery = ObjUserInfo.ProViewRW;
            //Code end

            if (intCompanyID == 0)
                intCompanyID = ObjUserInfo.ProCompanyIdRW;
            if (intUserID == 0)
                intUserID = ObjUserInfo.ProUserIdRW;

            if (Request.QueryString["qsViewId"] != null)
            {
                FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString["qsViewId"]);
                intFollowUpId = Convert.ToInt32(fromTicket.Name);
            }

            txtscannedDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtscannedDate.ClientID + "','" + strDateFormat + "',true,  false);");
            txtCollectedDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtCollectedDate.ClientID + "','" + strDateFormat + "',true,  false);");
            txtNotifyDt.Attributes.Add("onblur", "fnDoDate(this,'" + txtNotifyDt.ClientID + "','" + strDateFormat + "',false,  false);");

            Button btnGetLOVTo = ucTo.FindControl("btnGetLOV") as Button;
            Button btnGetLOVFrom = ucFrom.FindControl("btnGetLOV") as Button;

            System.Web.HttpContext.Current.Session["AutoSuggestCompanyID"] = intCompanyID.ToString();
            System.Web.HttpContext.Current.Session["AutoSuggestUserID"] = intUserID.ToString();
            if (ddlLOB.SelectedValue != string.Empty && ddlLOB.SelectedValue != "0")
            {
                System.Web.HttpContext.Current.Session["LOBAutoSuggestValue"] = ddlLOB.SelectedValue;
            }
            else
            {
                System.Web.HttpContext.Current.Session["LOBAutoSuggestValue"] = null;
            }
            if (hdnBranchID.Value != string.Empty && hdnBranchID.Value != "0")
            {
                System.Web.HttpContext.Current.Session["BranchAutoSuggestValue"] = hdnBranchID.Value;
            }
            else
            {
                System.Web.HttpContext.Current.Session["BranchAutoSuggestValue"] = null;
            }

            FunSetComboBoxAttributes(txtComCity, "City", "30");
            FunSetComboBoxAttributes(txtComState, "State", "60");
            FunSetComboBoxAttributes(txtComCountry, "Country", "60");


            strSuffix = Utility.SetSuffix();
            //txtNotifyDt.Attributes.Add("ReadOnly", "true");
            txtName.Attributes.Add("ReadOnly", "true");

            flUpload.Attributes.Add("onchange", "fnAssignPath('" + flUpload.ClientID + "','" + hdnSelectedPath.ClientID + "'); fnLoadPath('" + btnBrowse.ClientID + "');");
            btnDlg.OnClientClick = "fnLoadPath('" + flUpload.ClientID + "');";
            strPrefixLength = ObjS3GSession.ProGpsPrefixRW;
            strDecMaxLength = ObjS3GSession.ProGpsSuffixRW;

            if (!IsPostBack)
            {
                ceNotifyDt.Format = cetxtCollectedDate.Format = cetxtscannedDate.Format = ceFlwupDate.Format =
                calStartDate.Format = strDateFormat;
                TextBox txtFromName = ucFrom.FindControl("txtName") as TextBox;
                TextBox txtToName = ucTo.FindControl("txtName") as TextBox;
                txtToName.Width = txtFromName.Width = Unit.Pixel(200);
                iDocumentID = iLeadID = 0;
                strDeletedDocumentID = "0";

                FunPriLoadLov();

                if (Request.QueryString["Report"] != null)
                {
                    FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString["Report"]);
                    intFollowUpId = Convert.ToInt32(fromTicket.Name);
                    if (ObjDictionary != null)
                        ObjDictionary.Clear();
                    else
                        ObjDictionary = new Dictionary<string, string>();

                    ObjDictionary.Add("@Company_ID", Convert.ToString(intCompanyID));
                    ObjDictionary.Add("@Lead_ID", Convert.ToString(Convert.ToString(intFollowUpId)));

                    DataSet dsLeadDetails = Utility.GetDataset("S3G_ORG_GETLEADDETAILS", ObjDictionary);
                    FunPriLoadLeadDetails(dsLeadDetails);
                    FunPriEnableDisableLeadDtls(2);
                    btnCancel.Visible = btnLeadCancel.Enabled = false;
                    tpTrackDetails.Visible = tpDocumentDocument.Visible = tpAccountDetails.Visible =
                    tpStatusDetails.Visible = tpGroupDetails.Visible = pnlProspectView.Visible = false;
                    lblType.Visible = false;
                    ucPopUp.Visible = false;
                    ddlType.Visible = false;
                    txtName.Visible = false;
                    fldButton.Visible = false;
                    imgCustomer.Visible = false;
                    lblHeading.Visible = false;
                    tpLeadDetails.Visible = false;
                    Fieldset1.Visible = false;
                    btnGetSource.Visible = false;
                    ucPopUp.Width = 1000;
                    return;
                }

                FunProInitializeAssetRow();
                funAssignPopupValue();

                btnUp.Attributes.Add("OnClick", "return funUpValidate()");
                txtFromName.Attributes.Add("readOnly", "true");
                txtToName.Attributes.Add("readOnly", "true");
                txtSDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtSDate.ClientID + "','" + strDateFormat + "',false,  false);");
                txtDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtDate.ClientID + "','" + strDateFormat + "',true,  false);");
                txtProspectName.Attributes.Add("onkeyup", "fnSetProspectName('" + txtProspectName.ClientID + "', '" + txtName.ClientID + "');");
                txtProspectName.Attributes.Add("onfocusout", "fnSetProspectName('" + txtProspectName.ClientID + "', '" + txtName.ClientID + "');");
                txtLeadRate.SetDecimalPrefixSuffix(2, 2, true, false, "Rate");
                txtLdFinanceAmt.SetDecimalPrefixSuffix(strPrefixLength, strDecMaxLength, true, false, "Finance Amount");

                Session.Remove("FU_TYPE");

                bClearList = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ClearListValues"));
                if (Request.QueryString["qsMode"] == "Q")
                {
                    FunFollowProcessForModification(intFollowUpId);
                    rfvddlType.Enabled = RequiredFieldValidator1.Enabled = false;
                    FunPriDisableControls(-1);
                }
                else if (Request.QueryString["qsMode"] == "M")
                {
                    FunFollowProcessForModification(intFollowUpId);
                    FunPriDisableControls(1);
                    rfvddlType.Enabled = RequiredFieldValidator1.Enabled = false;
                }
                else
                {
                    FunPriDisableControls(0);
                }

                if (Request.QueryString["qsCustomer"] != null)
                {
                    FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString["qsCustomer"]);
                    ddlType.SelectedValue = "2";
                    ucPopUp.SelectedValue = fromTicket.Name;
                    ucPopUp.SelectedText = "CRM";
                    btnLoadCustomer_OnClick(null, null);
                }
                else
                {
                    ddlType.SelectedValue = "8";
                    ddlType_SelectedIndexChanged(null, null);
                    Session["CRMEntity"] = null;
                }
                FunProClearCachedFiles();
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
        finally
        {
            ObjS3GSession = null;
        }
    }

    private void FunFollowProcessForModification(int intFollowUpId)
    {
        pnlFollowUp.Visible = true;
        trHead.Visible = false;
        try
        {
            if (ObjDictionary != null)
                ObjDictionary.Clear();
            else
                ObjDictionary = new Dictionary<string, string>();

            ObjDictionary.Add("@Company_ID", Convert.ToString(intCompanyID));
            ObjDictionary.Add("@FollowUp_ID", intFollowUpId.ToString());
            ObjDictionary.Add("@UserId", intUserID.ToString());

            DataSet dsFollow = Utility.GetDataset("S3G_CLN_GetFollowUp", ObjDictionary);

            if (dsFollow.Tables[0] != null && dsFollow.Tables[0].Rows.Count > 0)
            {
                grvMain.DataSource = dsFollow.Tables[0];
                grvMain.DataBind();
                pnlAccount.Visible = true;
            }

            if (dsFollow.Tables[1] != null && dsFollow.Tables[1].Rows.Count > 0)
            {
                ucdCustomer.SetCustomerDetails(dsFollow.Tables[1].Rows[0], true);
                hidCustomerId.Text = dsFollow.Tables[1].Rows[0]["Customer_ID"].ToString();
                FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(hidCustomerId.Text, false, 0);
                btnUp.Attributes.Add("OnClick", "if(funUpValidate()){ window.open('../Origination/S3GOrgCustomerMaster_Add.aspx?IsFromEnquiry=Yes&qsCustomerId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=Q', 'null','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');return false;}");
            }

            if (dsFollow.Tables[2] != null && dsFollow.Tables[2].Rows.Count > 0)
            {
                grvAssetDetails.DataSource = dsFollow.Tables[2];
                grvAssetDetails.DataBind();
            }

            ViewState["dtFollow"] = ViewState["dtFollowAll"] = dsFollow.Tables[3];
            ViewState["Followup"] = dsFollow.Tables[3];
            if (dsFollow.Tables[3] != null && dsFollow.Tables[3].Rows.Count > 0)
            {
                grvFollowUp.DataSource = dsFollow.Tables[3];
                grvFollowUp.DataBind();
                if (dsFollow.Tables[3].Select(" FollowUp_ID = " + intFollowUpId + " ").Length > 0)
                    txtFollowUpNo.Text = dsFollow.Tables[3].Select(" FollowUp_ID = " + intFollowUpId + " ").CopyToDataTable().Rows[0]["FollowUp_No"].ToString();
            }
            else
            {
                grvFollowUp.DataSource = funAddRow();
                grvFollowUp.DataBind();
                grvFollowUp.Rows[0].Visible = false;
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    #region "User Authorization"
    //This is used to implement User Authorization
    private void FunPriDisableControls(int intModeID)
    {
        try
        {
            switch (intModeID)
            {
                case 0: // Create Mode

                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);
                    if (!bCreate)
                    {
                        //btnListing.Enabled = false;
                    }
                    break;
                case 1: // Modify Mode

                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                    if (!bModify)
                    {

                    }
                    btnClear.Enabled = false;
                    foreach (GridViewRow gvRow in grvMain.Rows)
                    {
                        (gvRow.FindControl("chkSelectAcc") as CheckBox).Enabled = false;
                    }
                    break;

                case -1:// Query Mode
                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);

                    if (!bQuery)
                    {
                        Response.Redirect(strRedirectPage, false);
                    }
                    btnClear.Enabled = btnSave.Enabled = false;
                    if (grvFollowUp.HeaderRow != null) (grvFollowUp.HeaderRow.FindControl("btnAdd") as Button).Enabled = false;

                    foreach (GridViewRow gvRow in grvFollowUp.Rows)
                    {
                        LinkButton btnView = gvRow.FindControl("btnView") as LinkButton;
                        LinkButton btnRemove = gvRow.FindControl("btnRemove") as LinkButton;
                        LinkButton lnkQuery = gvRow.FindControl("lnkQuery") as LinkButton;
                        btnView.Enabled = btnRemove.Enabled = lnkQuery.Enabled = false;
                    }
                    foreach (GridViewRow gvRow in grvMain.Rows)
                    {
                        CheckBox chkSelect = gvRow.FindControl("chkSelectAcc") as CheckBox;
                        chkSelect.Enabled = false;
                    }

                    txtSDate.Attributes.Remove("onblur");
                    txtSDate.ReadOnly = true;

                    break;
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }
    //Code end
    #endregion

    private DataTable funAddRow()
    {
        DataTable dtFollow = new DataTable();
        try
        {
            if (ViewState["dtFollow"] != null)
            {
                dtFollow = ViewState["dtFollow"] as DataTable;
            }
            DataRow drFollow = dtFollow.NewRow();
            drFollow["Lead_ID"] = (ViewState["Lead_ID"] != null) ? Convert.ToInt32(ViewState["Lead_ID"]) : 0;
            drFollow["TicketNo"] = -1;
            drFollow["Date"] = DateTime.Now.ToString();
            drFollow["From"] = 0;
            drFollow["From_Type"] = "0";
            drFollow["From_UserName"] = "";
            drFollow["To"] = 0;
            drFollow["To_Type"] = "0";
            drFollow["To_UserName"] = "";
            drFollow["QueryType"] = "0";
            drFollow["Description"] = "";
            drFollow["NotifyDate"] = DateTime.Now.ToString();
            drFollow["Mode"] = 0;
            drFollow["Status"] = 0;
            drFollow["QueryTxt"] = "";
            drFollow["ModeTxt"] = "";
            drFollow["isMax"] = false;
            drFollow["Panum"] = "";
            dtFollow.Rows.Add(drFollow);
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
        return dtFollow;
    }

    private DataRow funAssignDataRow(DataRow drFollow)
    {
        try
        {
            TextBox txtFromName = ucFrom.FindControl("txtName") as TextBox;
            HiddenField hdnIDFrom = ucFrom.FindControl("hdnID") as HiddenField;
            TextBox txtToName = ucTo.FindControl("txtName") as TextBox;
            HiddenField hdnIDTo = ucTo.FindControl("hdnID") as HiddenField;

            drFollow["Lead_ID"] = (ViewState["Lead_ID"] != null) ? Convert.ToInt32(ViewState["Lead_ID"]) : 0;
            drFollow["TicketNo"] = Convert.ToInt32(hdnTicketNo.Value);
            drFollow["Date"] = DateTime.Today;
            drFollow["From"] = (Convert.ToString(hdnIDFrom.Value) == "") ? 0 : Convert.ToInt32(hdnIDFrom.Value);
            drFollow["From_Type"] = ddlFrom.SelectedValue;
            drFollow["From_UserName"] = txtFromName.Text.Trim();
            drFollow["To"] = (Convert.ToString(hdnIDTo.Value) == "") ? 0 : Convert.ToInt32(hdnIDTo.Value);
            drFollow["To_Type"] = Convert.ToInt32(ddlTo.SelectedValue);
            drFollow["To_UserName"] = txtToName.Text.Trim();
            drFollow["QueryType"] = ddlQuery.SelectedValue;
            drFollow["Description"] = txtDescription.Text.Trim();
            drFollow["NotifyDate"] = Utility.StringToDate(txtNotifyDt.Text.Trim());
            drFollow["Mode"] = ddlMode.SelectedValue;
            drFollow["ModeTxt"] = ddlMode.SelectedValue == "0" ? "" : ddlMode.SelectedItem.Text.Trim();
            drFollow["QueryTxt"] = ddlQuery.SelectedValue == "0" ? "" : ddlQuery.SelectedItem.Text.Trim();
            drFollow["Status"] = "Open";
            drFollow["Status_Code"] = "1";
            drFollow["Followup_Detail_ID"] = 0;
            drFollow["Panum"] = (ViewState["Lead_Panum"] != null) ? Convert.ToString(ViewState["Lead_Panum"]) : "";
            string strMailId = "";
            if (hdnIDTo.Value != "" && ddlMode.SelectedValue == "2")
            {
                try
                {
                    if (ObjDictionary != null)
                        ObjDictionary.Clear();
                    else
                        ObjDictionary = new Dictionary<string, string>();

                    ObjDictionary.Add("@Company_ID", Convert.ToString(intCompanyID));
                    ObjDictionary.Add("@ToType", ddlTo.SelectedValue);
                    ObjDictionary.Add("@User_To", hdnIDTo.Value);

                    DataTable dtMail = Utility.GetDefaultData("S3G_CLN_GetFollowUp_ToMail", ObjDictionary);
                    if (dtMail != null && dtMail.Rows.Count > 0)
                        strMailId = dtMail.Rows[0]["ToMailId"].ToString();
                }
                catch (Exception objException)
                {
                    throw objException;
                }
            }
            drFollow["ToMailId"] = strMailId;
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
        return drFollow;
    }

    #region Save

    protected void FunProUploadFiles(DataTable dtDocs, string strDocType, string strPath)
    {
        try
        {
            for (int i = 0; i <= dtDocs.Rows.Count - 1; i++)
            {
                string strDocument = dtDocs.Rows[i]["Doc_Cat_ID"].ToString();
                string strCacheFile = "Docs_" + strDocType + "_File_" + strDocument;
                if (Cache[strCacheFile] != null)
                {
                    HttpPostedFile hpf = (HttpPostedFile)Cache[strCacheFile];

                    string strFolderName = @"\COMPANY" + intCompanyID.ToString();
                    string strFilePath = strPath + strFolderName;

                    if (!Directory.Exists(strFilePath))
                    {
                        Directory.CreateDirectory(strFilePath);
                    }
                    strFilePath += @"\" + System.IO.Path.GetFileName(hpf.FileName).Split('.')[0].ToString() + DateTime.Now.ToLocalTime().ToString().Replace(" ", "").Replace("/", "").Replace(":", "") + "." + System.IO.Path.GetFileName(hpf.FileName).Split('.')[1].ToString();
                    hpf.SaveAs(strFilePath);

                    dtDocs.Rows[i]["Document_Path"] = strFilePath;
                    dtDocs.AcceptChanges();
                }
                else
                {
                    dtDocs.Rows[i]["Document_Path"] = "";
                }
            }

            ViewState["Docs" + strDocType] = dtDocs;
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    protected void FunProClearCachedFiles()
    {
        try
        {
            for (int i = 1; i <= ddlDocumentType.Items.Count - 1; i++)
            {
                string strDocType = ddlDocumentType.Items[i].Value;
                if (ViewState["Docs" + strDocType] != null)
                {
                    for (int j = 0; j <= ((DataTable)ViewState["Docs" + strDocType]).Rows.Count - 1; j++)
                    {
                        string strCacheFile = "Docs_" + strDocType + "_File_" + j.ToString();
                        if (Cache[strCacheFile] != null)
                        {
                            Cache.Remove(strCacheFile);
                        }
                    }
                }
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    private void FunPriSaveRecord(int intMoveEnquiry)
    {
        //FunPubSentMail();       
        string strPANum = string.Empty;
        string strSANum = string.Empty;
        int chkCount = 0;
        return;
        if (ddlType.SelectedValue == "8" && string.IsNullOrEmpty(txtProspectName.Text.Trim()))
        {
            cvEnquiry.IsValid = false;
            cvEnquiry.ErrorMessage = "Enter Prospect name to proceed.";
            return;
        }
        else if (ddlType.SelectedValue != "8" && string.IsNullOrEmpty(ucPopUp.SelectedValue))
        {
            cvEnquiry.IsValid = false;
            cvEnquiry.ErrorMessage = "Select " + ddlType.SelectedItem.Text + " to proceed.";
            return;
        }

        if (ddlType.SelectedValue == "8" && Convert.ToInt32(ddlConstitutionName.SelectedValue) == 0)
        {
            cvEnquiry.IsValid = false;
            cvEnquiry.ErrorMessage = "Select the Constitution";
            return;
        }

        foreach (GridViewRow gvRow in grvMain.Rows)
        {
            CheckBox chkSelect = gvRow.FindControl("chkSelectAcc") as CheckBox;
            if (chkSelect.Checked)
            {
                if (chkCount == 0)
                {
                    strPANum = (gvRow.FindControl("txtPrimeAccountNo") as Label).Text.Trim();
                    strSANum = (gvRow.FindControl("txtSubAccountNo") as Label).Text.Trim();
                    if (strSANum == "") strSANum = strPANum + "DUMMY";
                }
                chkCount++;
                if (chkCount > 1)
                {
                    strPANum = strSANum = "";
                    break;
                }
            }
        }
        //Requirement for Finance Amount greater than Zero Through Enquiry initiate on 21/02/2014 by Palani Kumar.A
        if (ddlPrograms.SelectedValue == "1")
        {
            decimal dcFinanceAmount = 0;
            DataTable dt = (DataTable)ViewState["Assets"];
            foreach (DataRow dtrow in dt.Rows)
            {
                if (string.IsNullOrEmpty(dtrow["FinanceAmount"].ToString()))//For FinanceAmount -- ItemArray[4]
                {
                    Utility.FunShowAlertMsg(this, "Finance Amount should not be Empty and greater than Zero (0)");
                    return;
                }
                if (!string.IsNullOrEmpty(dtrow["FinanceAmount"].ToString()))
                {
                    if (Convert.ToDecimal(dtrow["FinanceAmount"].ToString()) <= dcFinanceAmount)
                    {
                        Utility.FunShowAlertMsg(this, "Finance Amount should be greater than Zero (0) ");
                        return;
                    }
                }
            }
        }
        //-------------- End
        int intFollowId, intCustomerID = 0;
        int intDCID = 0;
        
        string strDocNo = string.Empty;
        objFollowUp_Client = new ClnDataMgtServicesReference.ClnDataMgtServicesClient();
        try
        {
            if (Page.IsValid)
            {
                objS3G_CLN_FollowUpDataTable = new ClnDataMgtServices.S3G_CLN_CRM_HdrDataTable();
                objS3G_CLN_FollowUpRow = objS3G_CLN_FollowUpDataTable.NewS3G_CLN_CRM_HdrRow();

                //(ucPopUp.FindControl("hdnID") as HiddenField).Value

                //if (ddlType.SelectedValue == "8")
                //{
                //    if (!string.IsNullOrEmpty((ucPopUp.FindControl("hdnID") as HiddenField).Value))
                //    {
                //        objS3G_CLN_FollowUpRow.CRM_ID = Convert.ToInt32((ucPopUp.FindControl("hdnID") as HiddenField).Value);
                //    }
                //    else
                //    {
                //        objS3G_CLN_FollowUpRow.CRM_ID = 0;
                //    }
                //}
                //else
                //{
                if (!string.IsNullOrEmpty(ucPopUp.SelectedValue))
                {
                    objS3G_CLN_FollowUpRow.CRM_ID = Convert.ToInt32(ucPopUp.SelectedValue);
                }
                else
                {
                    objS3G_CLN_FollowUpRow.CRM_ID = 0;
                }
                //}
                objS3G_CLN_FollowUpRow.Company_ID = intCompanyID;
                objS3G_CLN_FollowUpRow.Group_ID = 0;
                if (!string.IsNullOrEmpty(hidCustomerId.Text))
                {
                    objS3G_CLN_FollowUpRow.Customer_ID = Convert.ToInt32(hidCustomerId.Text);
                }
                else
                {
                    objS3G_CLN_FollowUpRow.Customer_ID = 0;
                }
                objS3G_CLN_FollowUpRow.Location_Code = hdnBranchID.Value;

                objS3G_CLN_FollowUpRow.Prospect_Title = Convert.ToInt32(ddlTitle.SelectedValue);
                objS3G_CLN_FollowUpRow.Prospect_Name = txtProspectName.Text;
                objS3G_CLN_FollowUpRow.Customer_Type = Convert.ToInt32(ddlCustomerType.SelectedValue);
                objS3G_CLN_FollowUpRow.Posting_GL_Code = Convert.ToInt32(ddlPostingGLCode.SelectedValue);
                objS3G_CLN_FollowUpRow.Industry_Type = Convert.ToInt32(ddlIndustry.SelectedValue);
                objS3G_CLN_FollowUpRow.Company_Type = Convert.ToInt32(ddlCompanyType.SelectedValue);
                objS3G_CLN_FollowUpRow.Address1 = txtComAddress1.Text;
                objS3G_CLN_FollowUpRow.Address2 = txtCOmAddress2.Text;
                TextBox _txtState = (TextBox)txtComState.FindControl("TextBox");
                TextBox _txtCity = (TextBox)txtComCity.FindControl("TextBox");
                TextBox _txtCountry = (TextBox)txtComCountry.FindControl("TextBox");
                objS3G_CLN_FollowUpRow.City = Convert.ToString(_txtCity.Text);
                objS3G_CLN_FollowUpRow.State = Convert.ToString(_txtState.Text);
                objS3G_CLN_FollowUpRow.Country = Convert.ToString(_txtCountry.Text);
                objS3G_CLN_FollowUpRow.Pincode = txtComPincode.Text;
                objS3G_CLN_FollowUpRow.Mobile = txtComMobile.Text;
                objS3G_CLN_FollowUpRow.Telephone = txtComTelephone.Text;
                objS3G_CLN_FollowUpRow.EMail = txtComEmail.Text;
                objS3G_CLN_FollowUpRow.Website = txtComWebsite.Text;
                objS3G_CLN_FollowUpRow.SE_Ref = "";
                objS3G_CLN_FollowUpRow.Account_Stat_Type = 109;
                objS3G_CLN_FollowUpRow.Account_Stat = (Convert.ToString(ddlAccountStatus.SelectedValue) == "") ? 0 : Convert.ToInt32(ddlAccountStatus.SelectedValue);
                objS3G_CLN_FollowUpRow.Customer_Stat_Type = 110;
                objS3G_CLN_FollowUpRow.Customer_Stat = (Convert.ToString(ddlCustomerStatus.SelectedValue) == "") ? 0 : Convert.ToInt32(ddlCustomerStatus.SelectedValue);
                objS3G_CLN_FollowUpRow.Created_By = intUserID;
                objS3G_CLN_FollowUpRow.Created_On = DateTime.Now;
                objS3G_CLN_FollowUpRow.Finance_Mode = (Convert.ToString(ddlFinanceMode.SelectedValue) == "") ? 0 : Convert.ToInt32(ddlFinanceMode.SelectedValue);
                objS3G_CLN_FollowUpRow.LOB = (Convert.ToString(ddlLOB.SelectedValue) == "") ? 0 : Convert.ToInt32(ddlLOB.SelectedValue);
                objS3G_CLN_FollowUpRow.Product_ID = (Convert.ToString(ddlProductType.SelectedValue) == "") ? 0 : Convert.ToInt32(ddlProductType.SelectedValue);
                objS3G_CLN_FollowUpRow.LeadSourceType = (Convert.ToString(ddlLeadSourceType.SelectedValue) == "") ? 0 : Convert.ToInt32(ddlLeadSourceType.SelectedValue);
                objS3G_CLN_FollowUpRow.Cnt_Per = txtCntPer.Text;
                objS3G_CLN_FollowUpRow.Cnt_Per_Ph = txtCntPerPh.Text;
                objS3G_CLN_FollowUpRow.Cnt_Per_Desig = txtCntPerDesig.Text;
                objS3G_CLN_FollowUpRow.Funding_Type = Convert.ToInt32(ddlFundingType.SelectedValue);
                objS3G_CLN_FollowUpRow.Competitor_info = Convert.ToString(txtCompetitorInfo.Text);
                objS3G_CLN_FollowUpRow.IS_NewLead = (ViewState["IS_NewLead"] != null) ? (Int32)ViewState["IS_NewLead"] : 1;
                objS3G_CLN_FollowUpRow.Lead_Update_ID = iLeadID;
                objS3G_CLN_FollowUpRow.Lead_Remarks = Convert.ToString(txtLeadRemarks.Text);
                objS3G_CLN_FollowUpRow.Sales_Person = (Convert.ToString(hdnSalesPersonID.Value) == "") ? 0 : Convert.ToInt32(hdnSalesPersonID.Value);
                objS3G_CLN_FollowUpRow.Enquiry_Number = "";
                objS3G_CLN_FollowUpRow.Offer_Number = "";
                objS3G_CLN_FollowUpRow.Application_Number = "";
                objS3G_CLN_FollowUpRow.PANUM = "";
                objS3G_CLN_FollowUpRow.SANUM = "";

                objS3G_CLN_FollowUpRow.Constitution_ID = Convert.ToInt32(ddlConstitutionName.SelectedValue);
                objS3G_CLN_FollowUpRow.Is_MoveEnquiry = intMoveEnquiry;

                if (ucLead.SelectedValue == "")
                {
                    objS3G_CLN_FollowUpRow.LeadSource = 0;
                }
                else
                {
                    objS3G_CLN_FollowUpRow.LeadSource = Convert.ToInt32(ucLead.SelectedValue);
                }
                objS3G_CLN_FollowUpRow.Lead_Status = (Convert.ToString(ddlLeadStatus.SelectedValue) == "") ? 0 : Convert.ToInt32(ddlLeadStatus.SelectedValue);
                objS3G_CLN_FollowUpRow.Lead_Information = Convert.ToString(txtLeadInformation.Text);
                objS3G_CLN_FollowUpRow.Deleted_Doc = Convert.ToString(strDeletedDocumentID);
                objS3G_CLN_FollowUpRow.Lead_Cnt_Name = Convert.ToString(txtLeadContactPerson.Text);
                objS3G_CLN_FollowUpRow.Lead_Cnt_Ph_No = Convert.ToString(txtLeadContactNumber.Text);
                objS3G_CLN_FollowUpRow.Lead_Cnt_Designation = Convert.ToString(txtLeadContactDesg.Text);
                objS3G_CLN_FollowUpRow.Lead_Cnt_Email = Convert.ToString(txtLeadContactEmail.Text);
                objS3G_CLN_FollowUpRow.Tenure = (Convert.ToString(txtLeadTenure.Text) == "") ? 0 : Convert.ToInt32(txtLeadTenure.Text);
                objS3G_CLN_FollowUpRow.Rate = (Convert.ToString(txtLeadRate.Text) == "") ? 0 : Convert.ToDecimal(txtLeadRate.Text);

                ObjDictionary = new Dictionary<string, string>();
                ObjDictionary.Add("@Company_ID", intCompanyID.ToString());
                if (ddlLOB.SelectedValue != "0")
                    ObjDictionary.Add("@LOB_ID", ddlLOB.SelectedValue);
                ObjDictionary.Add("@Program_ID", "241");
                DataTable dtPath = Utility.GetDefaultData("S3G_LOANAD_GetDocPath", ObjDictionary);
                string strFilePath = "";

                for (int i = 1; i <= ddlDocumentType.Items.Count - 1; i++)
                {
                    string strDocType = ddlDocumentType.Items[i].Value;
                    if (ViewState["Docs" + strDocType] != null)
                    {
                        if (dtPath != null && dtPath.Rows.Count > 0)
                        {
                            strFilePath = dtPath.Rows[0]["DOCUMENT_PATH"].ToString();
                        }
                        else
                        {
                            Utility.FunShowAlertMsg(this, "Define Scan Image Path in Document path setup for CRM");
                            return;
                        }

                        FunProUploadFiles((DataTable)ViewState["Docs" + strDocType], strDocType, strFilePath);
                    }
                }

                if (ViewState["NewFollowup"] != null)
                {
                    //objS3G_CLN_FollowUpRow.XML_Followup = ((DataTable)ViewState["NewFollowup"]).FunPubFormXml();
                    objS3G_CLN_FollowUpRow.XML_Followup = FunLoadXml();
                }
                else
                {
                    objS3G_CLN_FollowUpRow.XML_Followup = "<Root></Root>";
                }
                if (ViewState["Assets"] != null)
                {
                    objS3G_CLN_FollowUpRow.XML_Lead_Asset = ((DataTable)ViewState["Assets"]).FunPubFormXml();
                }

                if (ViewState["Docs2"] != null)
                {
                    objS3G_CLN_FollowUpRow.XML_POD_Docs = ((DataTable)ViewState["Docs2"]).FunPubFormXml();
                }
                else
                {
                    objS3G_CLN_FollowUpRow.XML_POD_Docs = "<Root></Root>";
                }
                if (ViewState["Docs1"] != null)
                {
                    objS3G_CLN_FollowUpRow.XML_PRD_Docs = ((DataTable)ViewState["Docs1"]).FunPubFormXml();
                }
                else
                {
                    objS3G_CLN_FollowUpRow.XML_PRD_Docs = "<Root></Root>";
                }

                if (ViewState["Docs4"] != null)
                {
                    objS3G_CLN_FollowUpRow.XML_FIR_Docs = ((DataTable)ViewState["Docs4"]).FunPubFormXml();
                }
                else
                {
                    objS3G_CLN_FollowUpRow.XML_FIR_Docs = "<Root></Root>";
                }

                if (ViewState["Docs3"] != null)
                {
                    objS3G_CLN_FollowUpRow.XML_Cons_Docs = ((DataTable)ViewState["Docs3"]).FunPubFormXml();
                }
                else
                {
                    objS3G_CLN_FollowUpRow.XML_Cons_Docs = "<Root></Root>";
                }

                if (ViewState["LeadStatusDetails"] != null)
                {
                    objS3G_CLN_FollowUpRow.XML_Status_Details = ((DataTable)ViewState["LeadStatusDetails"]).FunPubFormXml();
                }
                else
                {
                    objS3G_CLN_FollowUpRow.XML_Status_Details = "<Root></Root>";
                }

                objS3G_CLN_FollowUpDataTable.AddS3G_CLN_CRM_HdrRow(objS3G_CLN_FollowUpRow);

                intErrCode = objFollowUp_Client.FunPubCreateCRM(out intFollowId, out strDocNo, out intCustomerID, ObjSerMode, ClsPubSerialize.Serialize(objS3G_CLN_FollowUpDataTable, ObjSerMode));
                switch (intErrCode)
                {
                    case 0:
                        {
                            FunProClearCachedFiles();

                            string strMessage = "";
                            int intMail = 0;
                            hidCustomerId.Text = intCustomerID.ToString();
                            foreach (GridViewRow gvRow in grvFollowUp.Rows)
                            {
                                //Mail:
                                HiddenField hidMode = (HiddenField)gvRow.FindControl("hidMode");
                                HiddenField hidFollowup_Detail_ID = (HiddenField)gvRow.FindControl("hidFollowup_Detail_ID");
                                Label txtDescription = (Label)gvRow.FindControl("txtDescription");
                                Label txtTicketNo = (Label)gvRow.FindControl("txtTicketNo");
                                TextBox hidToMailId = (TextBox)gvRow.FindControl("hidToMailId");
                                Label txtNotifyDate = (Label)gvRow.FindControl("txtNotifyDate");
                                Label hidToUserName = (Label)gvRow.FindControl("hidToUserName");
                                Label hidFromUserName = (Label)gvRow.FindControl("hidFromUserName");
                                Label txtQuery = (Label)gvRow.FindControl("txtQuery");

                                string strTktNo = txtTicketNo.Text.Trim();
                                if (txtTicketNo.Text == "") strTktNo = "0";

                                if (hidFollowup_Detail_ID.Value == "0" && strTktNo != "0" && hidMode.Value == "2")
                                {
                                    try
                                    {
                                        //FunPubSentMail(txtDescription.Text.Trim(), strTktNo, hidToMailId.Text.Trim(), txtNotifyDate.Text.Trim(), hidToUserName.Text.Trim(), txtQuery.Text.Trim(), hidFromUserName.Text.Trim());
                                        Dictionary<string, string> dictMail = new Dictionary<string, string>();
                                        dictMail.Add("FromMail", "s3g@sundaraminfotech.in");
                                        dictMail.Add("ToMail", hidToMailId.Text.Trim());
                                        dictMail.Add("Subject", "Follow up");
                                        //dictMail.Add("ToCC", "");
                                        //dictMail.Add("ToBCC", "");
                                        ArrayList arrMailAttachement = new ArrayList();
                                        StringBuilder strBody = new StringBuilder();
                                        strBody = GetHTMLText(txtDescription.Text.Trim(), strTktNo, txtNotifyDate.Text.Trim(), hidToUserName.Text.Trim(), txtQuery.Text.Trim(), hidFromUserName.Text.Trim());

                                        Utility.FunPubSentMail(dictMail, arrMailAttachement, strBody);
                                    }
                                    catch (Exception exMail)
                                    {
                                        if (intMail == 0)
                                            strMessage = "Invalid EMail ID. Mail not sent to the Ticket No ( " + strTktNo;
                                        else
                                            strMessage = strMessage + "," + strTktNo;

                                        intMail++;
                                        //Utility.FunShowAlertMsg(this, strMessage);
                                    }
                                }
                            }

                            if (strMessage != "") strMessage = "\\n\\n " + strMessage + ")";

                            else if (intFollowUpId == 0)
                            {
                                //To avoid double save click
                                btnSave.Enabled = false;
                                //End here
                                if (intMoveEnquiry == 0)
                                {
                                    strAlert = "Follow Up " + Resources.ValidationMsgs.S3G_ValMsg_Save;
                                }
                                else
                                {
                                    if (intMoveEnquiry == 1)
                                    {
                                        strAlert = "Prospect mapped successfully with " + ddlPrograms.SelectedItem.Text + " number " + strDocNo;
                                        //strAlert = "Enquiry Updation " + strDocNo + " Created Successfully";
                                    }
                                    else
                                    {
                                        FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(strDocNo, false, 0);
                                        ifrmCRM.Visible = true;
                                        if (intMoveEnquiry == 2)
                                        {
                                            Response.Redirect(strNewWinPricingIFrm + FormsAuthentication.Encrypt(Ticket), false);
                                            //ifrmCRM.Attributes.Add("src", strNewWinPricingIFrm + FormsAuthentication.Encrypt(Ticket));
                                            //ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", strNewWinPricing + FormsAuthentication.Encrypt(Ticket) + strNewWinAttributes, true);
                                        }
                                        else if (intMoveEnquiry == 3)
                                        {
                                            Response.Redirect(strNewWinApplicationIFrm + FormsAuthentication.Encrypt(Ticket), false);
                                            //ifrmCRM.Attributes.Add("src", strNewWinApplicationIFrm + FormsAuthentication.Encrypt(Ticket));
                                            //ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", strNewWinApplication + FormsAuthentication.Encrypt(Ticket) + strNewWinAttributes, true);
                                        }
                                        Session["ddlType"] = ddlType.SelectedValue;
                                        Session["ucPopUpValue"] = hidCustomerId.Text;
                                        return;
                                    }
                                }
                                strAlert += strMessage;
                                strAlert += Resources.ValidationMsgs.S3G_ValMsg_FollowUp_Confirm;// "\\n\\n Would you like to add one more Follow Up details?";
                                strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                                strRedirectPageView = "";
                                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
                            }
                            else
                            {
                                //To avoid double save click
                                btnSave.Enabled = false;
                                //End here

                                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert(' Follow Up " + Resources.ValidationMsgs.S3G_ValMsg_Update + strMessage + "');" + strRedirectPageView, true);   //Details Updated succesfully
                            }

                            break;
                        }
                    case -1:
                        {
                            Utility.FunShowAlertMsg(this.Page, "Document Number control not defined for Customer Master");
                            break;
                        }
                    case -2:
                        {
                            Utility.FunShowAlertMsg(this.Page, "Document Number control exceeded for Customer Master");
                            break;
                        }
                    case -3:
                        {
                            Utility.FunShowAlertMsg(this.Page, "Customer Document Number exceeding 12 charecters");
                            break;
                        }
                    case -4:
                        {
                            Utility.FunShowAlertMsg(this.Page, "Document Number control not defined for Enquiry Updation");
                            break;
                        }
                    case -5:
                        {
                            Utility.FunShowAlertMsg(this.Page, "Document Number control exceeded for Enquiry Updation");
                            break;
                        }
                    case 50:
                        {
                            Utility.FunShowAlertMsg(this.Page, "Error in saving details");
                            break;
                        }
                }
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
        finally
        {
            if (objFollowUp_Client != null)
                objFollowUp_Client.Close();
        }
    }
    #endregion

    #region  DateFormat
    public string FormatDate(string strDate)
    {
        try
        {
            if (strDate != "")
                return DateTime.Parse(strDate, CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
            //return Utility.StringToDate(strDate).ToString(strDateFormat);
            else
                return "";
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }
    #endregion

    private void funClearPopUp()
    {
        try
        {
            TextBox txtFromName = ucFrom.FindControl("txtName") as TextBox;
            HiddenField hdnIDFrom = ucFrom.FindControl("hdnID") as HiddenField;
            TextBox txtToName = ucTo.FindControl("txtName") as TextBox;
            HiddenField hdnIDTo = ucTo.FindControl("hdnID") as HiddenField;

            txtDescription.Text = txtTicketNo.Text = hdnTicketNo.Value = txtDate.Text = txtNotifyDt.Text = "";
            txtToName.Text = txtFromName.Text = hdnIDFrom.Value = hdnIDTo.Value = txtTrackDetailID.Text = "";
            ddlQuery.SelectedIndex = ddlFrom.SelectedIndex = ddlTo.SelectedIndex = ddlMode.SelectedIndex = 0;
            lblMode.CssClass = "styleDisplayLabel";
            rfvMode.Enabled = ddlMode.Enabled = false;
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    private void funShowPopUp(object sender, EventArgs e, string strType)
    {
        try
        {
            if (ObjDictionary != null)
                ObjDictionary.Clear();
            else
                ObjDictionary = new Dictionary<string, string>();

            LinkButton lnkNOD = (LinkButton)sender;
            GridViewRow gvRow = lnkNOD.Parent.Parent as GridViewRow;
            HiddenField hidLOB = gvRow.FindControl("hidLOB") as HiddenField;
            HiddenField hidBranch = gvRow.FindControl("hidBranch") as HiddenField;
            Label txtPANum = gvRow.FindControl("txtPANum") as Label;
            Label txtSANum = gvRow.FindControl("txtSANum") as Label;

            ObjDictionary.Add("@Company_ID", intCompanyID.ToString());
            ObjDictionary.Add("@LOB_ID", hidLOB.Value);
            ObjDictionary.Add("@Location_ID", hidBranch.Value);
            ObjDictionary.Add("@PANum", txtPANum.Text.Trim());
            ObjDictionary.Add("@SANum", txtSANum.Text.Trim());
            ObjDictionary.Add("@Customer_ID", hidCustomerId.Text.Trim());
            ObjDictionary.Add("@Type", strType);
            if (strType == "Ledger Entry")
            {
                DataSet dsJnEntry = Utility.GetDataset("S3G_CLN_GetFollowUp_PopUp", ObjDictionary);
                if (dsJnEntry.Tables[1].Rows.Count > 0)
                {
                    DataTable dtJnEntry = dsJnEntry.Tables[0];
                    dtJnEntry.Merge(dsJnEntry.Tables[1]);
                    grvPopUp.DataSource = dtJnEntry;// dsJnEntry.Tables[0].Merge(dsJnEntry.Tables[1]); ;
                    grvPopUp.DataBind();
                }
            }
            else
            {
                grvPopUp.DataSource = Utility.GetDefaultData("S3G_CLN_GetFollowUp_PopUp", ObjDictionary);
                grvPopUp.DataBind();
            }
            moeNOD.Show();
            lblNodHead.Text = strType;
            if (grvPopUp.Rows.Count == 0 && strType == "Ledger Entry")
            {
                Utility.FunShowAlertMsg(this, "Ledger Entry does not exists ");
                moeNOD.Hide();
                return;
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    private void funGridClear(GridView gv)
    {
        try
        {
            gv.DataSource = null;
            gv.DataBind();
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    private StringBuilder GetHTMLText(string strDesc, string strTktNo, string strNotifyDt, string strUserName, string strQueryType, string strFrom)
    {
        StringBuilder strBody = new StringBuilder();
        try
        {
            strBody.Append(
               "<font size=\"1\"  color=\"black\" face=\"Times New Roman\">" +

              " <table width=\"100%\">" +

           "<tr >" +
               "<td  align=\"Left\" >" +
                   "<font size=\"1\"  color=\"Black\" face=\"Times New Roman\" >" + "Dear " + strUserName + ",</font> " + "</b>" +
                   "</br>" +
               "</td>" +
          " </tr>" +

           "<tr >" +
               "<td  align=\"Left\" >" +
                   "<font size=\"1\"  color=\"Black\" face=\"Times New Roman\" >" + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Ticket No was generated successfully .  </font> " +
               "</td>" +
          " </tr>" +

           "<tr >" +
               "<td  align=\"Left\" >" +
                   "<font size=\"1\"  color=\"Black\" face=\"Times New Roman\" >" + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Ticket No : " + strTktNo + "</font> " +
               "</td>" +
          " </tr>" +

            "<tr >" +
               "<td  align=\"Left\" >" +
                   "<font size=\"1\"  color=\"Black\" face=\"Times New Roman\" >" + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Query Type  : " + strQueryType + "</font> " +
                    "</br>" +
               "</td>" +
          " </tr>" +

          "<tr >" +
               "<td  align=\"Left\" >" +
                   "<font size=\"1\"  color=\"Black\" face=\"Times New Roman\" >" + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Notify Date : " + strNotifyDt + "</font> " +
               "</td>" +
          " </tr>" +

            "<tr >" +
               "<td  align=\"Left\" >" +
                   "<font size=\"1\"  color=\"Black\" face=\"Times New Roman\" >" + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Description : " + strDesc + "</font> " +
                    "</br>" +
               "</td>" +
          " </tr>" +

             "<tr >" +
               "<td  align=\"Left\" >" +
                   "<font size=\"1\"  color=\"Black\" face=\"Times New Roman\" >" + " Yours Truly," + "</font> " +
               "</td>" +
          " </tr>" +


             "<tr >" +
               "<td  align=\"Left\" >" +
                   "<font size=\"1\"  color=\"Black\" face=\"Times New Roman\" >" + strFrom + "</font> " +
               "</td>" +
          " </tr>" +

       "</table>" + "</font>");
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
        return strBody;
    }

    private void FunPubClearPopUp()
    {
        try
        {
            ModalPopupExtender ucMPE = ((ModalPopupExtender)ucPopUp.FindControl("ucMPE"));
            if (ucMPE != null) ucMPE.Hide();
            GridView gvList = ((GridView)ucPopUp.FindControl("gvList"));
            gvList.ClearGrid();
            pnlProspectView.Style["display"] = "none";
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    private void funAssignPopupValue()
    {
        try
        {
            switch (ddlType.SelectedValue)
            {
                case "1":
                    {
                        ucPopUp.LOVCode = "FWUPD";
                        ucPopUp.gvDisplay = "ID";
                        break;
                    }
                case "2":
                    {
                        ucPopUp.LOVCode = "FWUPD";
                        ucPopUp.gvDisplay = "ID";
                        break;
                    }
                case "3":
                    {
                        ucPopUp.LOVCode = "FWUPD";
                        ucPopUp.gvDisplay = "ID";
                        break;
                    }
                case "4":
                    {
                        ucPopUp.LOVCode = "FWUPD";
                        ucPopUp.gvDisplay = "Prime_Account_Number";
                        break;
                    }
                case "5":
                    {
                        ucPopUp.LOVCode = "FWUPD";
                        ucPopUp.gvDisplay = "Sub_Account_Number";
                        break;
                    }
                case "6":
                    {
                        ucPopUp.LOVCode = "FWUPD";
                        ucPopUp.gvDisplay = "SERIAL_NUMBER";
                        break;
                    }
                case "7":
                    {
                        ucPopUp.LOVCode = "FWUPD";
                        ucPopUp.gvDisplay = "Registration_Number";
                        break;
                    }
                case "8":
                    {
                        ucPopUp.LOVCode = "PROS";
                        ucPopUp.gvDisplay = "Prospect_Name";
                        break;
                    }
                case "0":
                    {
                        ucPopUp.LOVCode = "";
                        ucPopUp.gvDisplay = "";
                        break;
                    }
                case "9":
                    {
                        ucPopUp.LOVCode = "FWUPD";
                        ucPopUp.gvDisplay = "ID";
                        break;
                    }
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    private void FunPubSentMail(string strDesc, string strTktNo, string MailId, string strNotifyDate, string strUserName, string strQueryType, string strFrom)
    {


        /*if (MailId.Trim() != "")
        {
            CommonMailServiceReference.CommonMailClient ObjCommonMail = new CommonMailServiceReference.CommonMailClient();
            try
            {
                string body = "";
                body = GetHTMLText(strDesc, strTktNo, strNotifyDate, strUserName, strQueryType, strFrom);
                ClsPubCOM_Mail ObjCom_Mail = new ClsPubCOM_Mail();
                ObjCom_Mail.ProFromRW = "s3g@sundaraminfotech.in";
                ObjCom_Mail.ProTORW = MailId; // (ucdCustomer.FindControl("txtEmail") as TextBox).Text.Trim();
                ObjCom_Mail.ProSubjectRW = "Follow up";
                ObjCom_Mail.ProMessageRW = strBody.ToString();
                ObjCommonMail.FunSendMail(ObjCom_Mail);
            }
            catch (FaultException<CommonMailServiceReference.ClsPubFaultException> ex)
            {
                if (ObjCommonMail != null)
                    ObjCommonMail.Close();
                  ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                if (ObjCommonMail != null)
                    ObjCommonMail.Close();
                  ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
                throw ex;
            }
            finally
            {
                if (ObjCommonMail != null)
                    ObjCommonMail.Close();
            }
        }*/
    }

    protected void funProCloseAllPops()
    {
        try
        {
            pnlLeadView.Style["display"] = "none";
            pnlProspectView.Style["display"] = "none";
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    protected void FunSetComboBoxAttributes(AjaxControlToolkit.ComboBox cmb, string Type, string maxLength)
    {
        try
        {
            TextBox textBox = cmb.FindControl("TextBox") as TextBox;

            if (textBox != null)
            {
                textBox.Attributes.Add("onkeyup", "maxlengthfortxt('" + maxLength + "');");
                textBox.Attributes.Add("onblur", "funCheckFirstLetterisNumeric(this, '" + Type + "');");
                textBox.Attributes.Add("onpaste", "return false");
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    protected DataTable FunProAddAddrColumns(DataTable dt)
    {
        try
        {
            dt.Columns.Add("ID");
            dt.Columns.Add("Name");
            dt.Columns.Add("Category");

            return dt;
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    protected void FunProInitializeAssetRow()
    {
        try
        {
            DataTable dtAsset = new DataTable();
            dtAsset.Columns.Add("Asset_ID");
            dtAsset.Columns.Add("Asset_Description");
            dtAsset.Columns.Add("Asset_Cost");
            dtAsset.Columns.Add("Finance_Amount");
            dtAsset.Columns.Add("LeadAsset_ID");
            dtAsset.Columns.Add("Is_Added");

            DataRow dRow = dtAsset.NewRow();
            dtAsset.Rows.Add(dRow);

            grvAssets.DataSource = dtAsset;
            grvAssets.DataBind();
            grvAssets.Rows[0].Visible = false;

            dtAsset.Rows[0].Delete();

            ViewState["Assets"] = dtAsset;

            FunPriSetFooterAssetCtg();

        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    private void FunPriSetFooterAssetCtg()
    {
        try
        {
            if (grvAssets != null)
            {
                if (grvAssets.FooterRow != null)
                {
                    DataTable dtAssetCtg = new DataTable();
                    if (ViewState["dtAssetsctg"] != null)
                        dtAssetCtg = (DataTable)ViewState["dtAssetsctg"];
                    DropDownList ddlAssetCategory = (DropDownList)grvAssets.FooterRow.FindControl("ddlAssetCategory");
                    ddlAssetCategory.DataSource = dtAssetCtg;
                    ddlAssetCategory.DataValueField = "Asset_ID";
                    ddlAssetCategory.DataTextField = "Asset_Description";
                    ddlAssetCategory.DataBind();
                    ddlAssetCategory.Items.Insert(0, new ListItem("--Select--", "0"));
                }
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    protected DataSet FunProGetDocs(string strPANUM)
    {
        try
        {
            DataSet dSet = new DataSet();

            ObjDictionary = new Dictionary<string, string>();
            ObjDictionary.Add("@Customer_ID", hidCustomerId.Text);
            ObjDictionary.Add("@Company_ID", intCompanyID.ToString());
            if (ddlLOB.SelectedValue != "0")
            {
                ObjDictionary.Add("@LOB_ID", ddlLOB.SelectedValue);
            }
            ObjDictionary.Add("@PROGRAM_ID", "241");

            ObjDictionary.Add("@CRM_ID", Convert.ToString(ucPopUp.SelectedValue));
            if (ViewState["Lead_ID"] != null)
                ObjDictionary.Add("@lead_id", ViewState["Lead_ID"].ToString());
            ObjDictionary.Add("@type", ddlType.SelectedValue.ToString());
            if (ddlDocumentType.SelectedValue == "1")
                dSet = Utility.GetDataset("S3G_CLN_GetCRM_PRDDDocDetails", ObjDictionary);
            else if (ddlDocumentType.SelectedValue == "2")
                dSet = Utility.GetDataset("S3G_CLN_GetCRM_PODDDocDetails", ObjDictionary);
            else if (ddlDocumentType.SelectedValue == "3")
                dSet = Utility.GetDataset("S3G_CLN_GetCRM_ConstDocDetails", ObjDictionary);
            else if (ddlDocumentType.SelectedValue == "4")
                dSet = Utility.GetDataset("S3G_CLN_GetCRM_FIRDocDetails", ObjDictionary);

            gvPRDDT.DataSource = dSet.Tables[0];
            gvPRDDT.DataBind();

            if (dSet != null && dSet.Tables[2].Rows.Count > 0)
            {
                lblActualPath.Text = dSet.Tables[2].Rows[0]["DOCUMENT_PATH"].ToString();
            }
            else
            {
                lblActualPath.Text = "";
            }
            ViewState["Documents"] = dSet.Tables[0];
            return dSet;
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    protected void FunProClearDocumentControls()
    {
        try
        {
            Cache.Remove("File");
            ddlDocumentType.SelectedValue = "0";
            ddlDocument.SelectedValue = "-1";
            txtCollectedDate.Text = txtscannedDate.Text = hdnSelectedPath.Value = lblCurrentPath.Text = txtDocRemarks.Text = "";
            chkSelect.Checked = chkIsCollected.Checked = false;
            chkSelect.Enabled = btnDocUpdate.Visible = false;
            txtValue.Text = "";
            btnDocAdd.Visible = ddlDocumentType.Enabled = true;
            txtCRMDocumentID.Text = "0";
            ddlCollectedBy.Clear();
            ddlScannedBy.Clear();
            gvPRDDT.DataSource = null;
            gvPRDDT.DataBind();
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    protected void FunProToggleProspectControls(bool blEnable)
    {
        try
        {
            txtProspectName.ReadOnly = txtComAddress1.ReadOnly = txtCOmAddress2.ReadOnly =
                txtComPincode.ReadOnly = txtComTelephone.ReadOnly = txtComMobile.ReadOnly = txtComEmail.ReadOnly = txtComWebsite.ReadOnly = !blEnable;
            txtComCity.Enabled = txtComState.Enabled = txtComCountry.Enabled = blEnable;
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    protected void FunProProspectClear()
    {
        try
        {
            ddlTitle.SelectedValue = "0";
            ddlRefType.SelectedValue = "0";
            txtComAddress1.Text = txtCOmAddress2.Text = txtComPincode.Text =
            txtComTelephone.Text = txtComMobile.Text = txtComEmail.Text = txtComWebsite.Text = txtProspectName.Text = txtComCity.SelectedItem.Text =
            txtComCountry.SelectedItem.Text = txtCntPer.Text = txtCntPerPh.Text = txtCntPerDesig.Text = string.Empty;
            ddlCompanyType.SelectedValue = ddlPostingGLCode.SelectedValue = ddlConstitutionName.SelectedValue =
            ddlIndustry.SelectedValue = txtComState.SelectedValue = "0";

            ddlRefNumber.Items.Clear();
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    protected void FunProClearLead()
    {
        try
        {
            ddlFinanceMode.SelectedValue = ddlLOB.SelectedValue =
            ddlLeadSourceType.SelectedValue = ddlAccountStatus.SelectedValue = ddlCustomerStatus.SelectedValue =
            ddlLeadStatus.SelectedValue = txtLeadID.Text = "0";

            //ddlFundingType.SelectedValue = ddlLeadConstitution.SelectedValue = 0;

            ucLead.Clear();
            txtLeadInformation.Text = txtLeadSource.Text = txtCompetitorInfo.Text = txtBranchSearch.Text = txtLdFinanceAmt.Text =
            txtLeadRemarks.Text = txtSalesPerson.Text = txtLeadContactPerson.Text = txtLeadContactNumber.Text =
            txtLeadContactDesg.Text = txtLeadContactEmail.Text = txtLeadTenure.Text = txtLeadRate.Text = txtOtherLead.Text = "";
            txtOtherLead.Visible = false;
            ddlLeadAssigned.Clear();

            FunProInitializeAssetRow();
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    protected void FunProClearAllTabs()
    {
        try
        {
            funGridClear(grvFollowUp);
            funGridClear(gvPRDDT);
            funGridClear(grvAssetDetails);
            //funGridClear(grvMain);
            funGridClear(grvAccountDetails);
            funGridClear(grvDcDairyAccountDetails);
            pnlFollowUp.Visible = pnlDocuments.Visible = pnlStatus.Visible = pnlGroupDetails.Visible =
                pnlAccount.Visible = pnlLeadViewDetails.Visible = false;

            btnSave.Enabled = true;
            ifrmCRM.Attributes.Remove("src");
            ifrmCRM.Visible = false;
            divShow.Style["display"] = "none";
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    //New
    private void FunPriLoadLov()
    {
        try
        {
            if (ObjDictionary != null)
            {
                ObjDictionary.Clear();
            }
            else
            {
                ObjDictionary = new Dictionary<string, string>();
            }

            ObjDictionary.Add("@Company_ID", Convert.ToString(intCompanyID));
            ObjDictionary.Add("@User_ID", Convert.ToString(intUserID));
            DataSet dsLov = Utility.GetDataset("S3G_ORG_GetCRM_LookUp", ObjDictionary);

            ViewState["CRMLookUp"] = dsLov;

            //Search Type
            ddlType.DataSource = dsLov.Tables[0];
            ddlType.DataValueField = "Lookup_Code";
            ddlType.DataTextField = "Lookup_Description";
            ddlType.DataBind();
            ddlType.Items.Insert(0, new ListItem("--Select--", "0"));

            //From Type
            ddlFrom.DataSource = dsLov.Tables[1];
            ddlFrom.DataValueField = "Lookup_Code";
            ddlFrom.DataTextField = "Lookup_Description";
            ddlFrom.DataBind();
            ddlFrom.Items.Insert(0, new ListItem("--Select--", "0"));

            //To Type
            ddlTo.DataSource = dsLov.Tables[1];
            ddlTo.DataValueField = "Lookup_Code";
            ddlTo.DataTextField = "Lookup_Description";
            ddlTo.DataBind();
            ddlTo.Items.Insert(0, new ListItem("--Select--", "0"));

            //Mode Type
            //ddlMode.DataSource = dsLov.Tables[2];
            //ddlMode.DataValueField = "Lookup_Code";
            //ddlMode.DataTextField = "Lookup_Description";
            //ddlMode.DataBind();
            ddlMode.Items.Insert(0, new ListItem("--Select--", "0"));

            //Query Type
            ddlQuery.DataSource = dsLov.Tables[3];
            ddlQuery.DataValueField = "Lookup_Code";
            ddlQuery.DataTextField = "Lookup_Description";
            ddlQuery.DataBind();
            ddlQuery.Items.Insert(0, new ListItem("--Select--", "0"));
            ddlQuery.Items.RemoveAt(6);

            //Status Type
            ddlStatus.DataSource = dsLov.Tables[4];
            ddlStatus.DataValueField = "Lookup_Code";
            ddlStatus.DataTextField = "Lookup_Description";
            ddlStatus.DataBind();
            ddlStatus.Items.Insert(0, new ListItem("--Select--", "0"));

            //Document Type
            ddlDocumentType.DataSource = dsLov.Tables[6];
            ddlDocumentType.DataValueField = "Lookup_Code";
            ddlDocumentType.DataTextField = "Lookup_Description";
            ddlDocumentType.DataBind();
            ddlDocumentType.Items.Insert(0, new ListItem("--Select--", "0"));

            DataTable dtAddress = dsLov.Tables[7];
            DataTable dtSource = new DataTable();
            if (dtAddress.Select("Category = 1").Length > 0)
            {
                dtSource = dtAddress.Select("Category = 1").CopyToDataTable();
            }
            else
            {
                dtSource = FunProAddAddrColumns(dtSource);
            }
            txtComCity.FillDataTable(dtSource, "Name", "Name", false);

            dtSource = new DataTable();
            if (dtAddress.Select("Category = 2").Length > 0)
            {
                dtSource = dtAddress.Select("Category = 2").CopyToDataTable();
            }
            else
            {
                dtSource = FunProAddAddrColumns(dtSource);
            }
            txtComState.FillDataTable(dsLov.Tables[24], "Name", "Name", false);

            dtSource = new DataTable();
            if (dtAddress.Select("Category = 3").Length > 0)
            {
                dtSource = dtAddress.Select("Category = 3").CopyToDataTable();
            }
            else
            {
                dtSource = FunProAddAddrColumns(dtSource);
            }
            txtComCountry.FillDataTable(dtSource, "Name", "Name", false);

            //Title Type
            ddlTitle.DataSource = dsLov.Tables[8];
            ddlTitle.DataValueField = "ID";
            ddlTitle.DataTextField = "Name";
            ddlTitle.DataBind();
            ddlTitle.Items.Insert(0, new ListItem("--Select--", "0"));

            //Constitution Type
            ddlConstitutionName.DataSource = dsLov.Tables[9];
            ddlConstitutionName.DataValueField = "ID";
            ddlConstitutionName.DataTextField = "ConstitutionName";
            ddlConstitutionName.DataBind();
            ddlConstitutionName.Items.Insert(0, new ListItem("--Select--", "0"));

            //Reference Type
            ddlRefType.DataSource = dsLov.Tables[12];
            ddlRefType.DataValueField = "Lookup_Code";
            ddlRefType.DataTextField = "Lookup_Description";
            ddlRefType.DataBind();
            ddlRefType.Items.Insert(0, new ListItem("--Select--", "0"));

            //Initiate Program Type
            ddlPrograms.DataSource = dsLov.Tables[15];
            ddlPrograms.DataValueField = "ID";
            ddlPrograms.DataTextField = "Caption";
            ddlPrograms.DataBind();
            ddlPrograms.Items.Insert(0, new ListItem("--Select--", "0"));

            ViewState["dtAssetsctg"] = dsLov.Tables[16].Copy();

            //Customer Type
            FunFillGrid(ddlCustomerType, dsLov.Tables[17], Convert.ToString(dsLov.Tables[17].Columns[0].ColumnName), Convert.ToString(dsLov.Tables[17].Columns[1].ColumnName));

            //Posting GL Code
            FunFillGrid(ddlPostingGLCode, dsLov.Tables[18], Convert.ToString(dsLov.Tables[18].Columns[0].ColumnName), Convert.ToString(dsLov.Tables[18].Columns[1].ColumnName));

            //industry Type
            FunFillGrid(ddlIndustry, dsLov.Tables[19], Convert.ToString(dsLov.Tables[19].Columns[0].ColumnName), Convert.ToString(dsLov.Tables[19].Columns[1].ColumnName));

            //ViewState["dtAssetRateType"] = dsLov.Tables[21].Copy();

            FunFillGrid(ddlCompanyType, dsLov.Tables[25], "ID", "Name");

            FunPriLoadLeadLov();

        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    private void FunPriLoadLeadLov()
    {
        try
        {
            DataSet dsLeadLov = (DataSet)ViewState["CRMLookUp"];
            //Finance Mode Type
            FunFillGrid(ddlFinanceMode, dsLeadLov.Tables[10], "Lookup_Code", "Lookup_Description");

            //LOB Type
            FunFillGrid(ddlLOB, dsLeadLov.Tables[20], Convert.ToString(dsLeadLov.Tables[20].Columns[0].ColumnName), Convert.ToString(dsLeadLov.Tables[20].Columns[2].ColumnName));
            //ddlLOB.SelectedIndex = 1;
            //ddlLOB.ClearDropDownList();

            //Lead Source Type
            FunFillGrid(ddlLeadSourceType, dsLeadLov.Tables[11], "Lookup_Code", "Lookup_Description");

            //Account Status Type
            FunFillGrid(ddlAccountStatus, dsLeadLov.Tables[13], "Lookup_Code", "Lookup_Description");

            //Customer Status Type
            FunFillGrid(ddlCustomerStatus, dsLeadLov.Tables[14], "Lookup_Code", "Lookup_Description");

            //Lead Status Type
            ddlLeadStatus.Items.Clear();
            FunFillGrid(ddlLeadStatus, dsLeadLov.Tables[5], "Lookup_Code", "Lookup_Description");

            //Funding Type
            FunFillGrid(ddlFundingType, dsLeadLov.Tables[23], Convert.ToString(dsLeadLov.Tables[23].Columns[0].ColumnName), Convert.ToString(dsLeadLov.Tables[23].Columns[1].ColumnName));

            //Lead Constitution Type
            FunFillGrid(ddlLeadConstitution, dsLeadLov.Tables[9], Convert.ToString(dsLeadLov.Tables[9].Columns[0].ColumnName), Convert.ToString(dsLeadLov.Tables[9].Columns[1].ColumnName));

        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    private void FunPriLoadCRMLeadDetails()
    {
        try
        {
            FunProClearAllTabs();
            DataSet dsFollow = null;

            if ((ucPopUp.FindControl("hdnText") as HiddenField).Value.Trim() != "" && ddlType.SelectedValue != "0")
            {
                lblCRMDetail.Text = tcCRM.Tabs[0].HeaderText;
                dsFollow = new DataSet();
                dsFollow = FunPriGetCRMProspectInfo();

                if (dsFollow != null)
                {
                    FunBindGrid(gvLeadDetails, dsFollow.Tables[4]);
                    pnlLeadViewDetails.Visible = true;
                    ucLead.Hide();

                    ViewState["Prospect_LeadDetails"] = dsFollow.Tables[4];
                    hdnLeadSourceType.Value = dsFollow.Tables[4].Rows[0]["Lead_Source_Type"].ToString();
                }
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    protected void FunFillGrid(DropDownList ddlList, DataTable dt, string strValueField, string strDisplayFeild)
    {
        try
        {
            if (ddlList != null && ddlList.Items.Count > 0)
            {
                ddlList.Items.Clear();
            }

            ddlList.DataSource = dt;
            ddlList.DataValueField = strValueField;
            ddlList.DataTextField = strDisplayFeild;
            ddlList.DataBind();
            ddlList.Items.Insert(0, new ListItem("--Select--", "0"));
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    protected void FunBindGrid(GridView gv, DataTable dt)
    {
        try
        {
            if (dt != null)
            {
                gv.DataSource = dt;
                gv.DataBind();
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    protected void FunBindTrackDetails(DataTable dt)
    {
        try
        {
            DataTable dtLeadTrack = dt;
            DataView dvTrack = new DataView(dt);
            dvTrack.RowFilter = "Lead_ID =" + ((ViewState["Lead_ID"] != null) ? Convert.ToString(ViewState["Lead_ID"]) : "0");
            dtLeadTrack = dvTrack.ToTable();
            if (dtLeadTrack.Rows.Count > 0)
            {
                FunBindGrid(grvFollowUp, dtLeadTrack);
                grvFollowUp.Rows[0].Visible = true;
            }
            else
            {
                grvFollowUp.DataSource = funAddRow();
                grvFollowUp.DataBind();
                grvFollowUp.Rows[0].Visible = false;
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    protected void FunProEnableDisableTrackCtrl(Int32 _iValue)
    {
        try
        {
            TextBox txtFrom = ucFrom.FindControl("txtName") as TextBox;
            HiddenField hdnFrom = ucFrom.FindControl("hdnID") as HiddenField;
            TextBox txtTo = ucTo.FindControl("txtName") as TextBox;
            HiddenField hdnTo = ucTo.FindControl("hdnID") as HiddenField;

            if (_iValue == 7)      //Remarks
            {
                ddlFrom.Enabled = ddlTo.Enabled = ddlMode.Enabled = ucFrom.Visible = ucTo.Visible = ddlStatus.Enabled = rfvMode.Enabled = false;
                txtFrom.Text = txtTo.Text = hdnFrom.Value = hdnTo.Value = string.Empty;
                ddlFrom.SelectedValue = ddlTo.SelectedValue = ddlMode.SelectedValue = ddlStatus.SelectedValue = "0";
                lblMode.CssClass = " styleDisplayLabel";
            }
            else
            {
                lblMode.CssClass = " styleReqFieldLabel";
                ddlFrom.Enabled = ddlTo.Enabled = ddlMode.Enabled = ucFrom.Visible = ucTo.Visible = rfvMode.Enabled = true;
                ddlStatus.Enabled = (_iValue == 5 || _iValue == 2 || _iValue == 4) ? false : true;
                txtFrom.Text = ObjUserInfo.ProUserNameRW;
                hdnFrom.Value = intUserID.ToString();
                funAssignUser(ddlFrom, ucFrom);

                if (ddlType.SelectedValue == "8")
                {
                    ddlTo.SelectedValue = "10";
                    txtTo.Text = txtName.Text;
                    hdnTo.Value = (ucPopUp.FindControl("hdnID") as HiddenField).Value;
                }
                else if (ddlType.SelectedValue == "9")
                {
                    ddlTo.SelectedValue = "6";
                    txtTo.Text = (ucdCustomer.FindControl("txtCustomerName") as TextBox).Text.Trim();
                    hdnTo.Value = hidCustomerId.Text.Trim();
                }
                else
                {
                    ddlTo.SelectedValue = "2";
                    txtTo.Text = (ucdCustomer.FindControl("txtCustomerName") as TextBox).Text.Trim();
                    hdnTo.Value = hidCustomerId.Text.Trim();
                }
                funAssignUser(ddlTo, ucTo);

                if (Convert.ToInt32(ddlQuery.SelectedValue) == 5)
                {
                    ddlStatus.SelectedValue = (Convert.ToString(txtTicketNo.Text) == "") ? "3" : "2";
                }
                else
                {
                    ddlStatus.SelectedValue = (Convert.ToInt32(ddlQuery.SelectedValue) == 4 || Convert.ToInt32(ddlQuery.SelectedValue) == 2) ? "3" : "1";
                }

                //ddlMode.Enabled = (Convert.ToInt32(_iValue) == 5) ? false : true;

                ddlFrom.SelectedValue = "1";
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    private void FunPriEnableDisableLeadDtls(Int32 _iMode)
    {
        try
        {
            if (_iMode == 1)         //Create
            {
                FunPriLoadLeadLov();
                FunProClearLead();
                grvAssets.Enabled = btnLeadOk.Enabled = btnLeadClear.Enabled = true;
                pnlLeadView.Style["display"] = "Block";
                ddlLeadStatus.SelectedValue = "1";  //Pending
                ddlLeadStatus.Enabled = false;
                //if (ViewState["Prospect_LeadDetails"] != null)
                //{
                //    DataTable dtProspectLeadDetails = (DataTable)ViewState["Prospect_LeadDetails"];
                //    dtProspectLeadDetails.Columns.Remove("IS_Checked");
                //    dtProspectLeadDetails.Columns.Add("IS_Checked");
                //    dtProspectLeadDetails.Columns["IS_Checked"].DefaultValue = 0;
                //    dtProspectLeadDetails.AcceptChanges();
                //    ViewState["Prospect_LeadDetails"] = dtProspectLeadDetails;

                //    FunBindGrid(gvLeadDetails, dtProspectLeadDetails);
                //}

                txtSalesPerson.ReadOnly = txtBranchSearch.ReadOnly = txtLeadInformation.ReadOnly = txtCompetitorInfo.ReadOnly =
                txtLeadContactPerson.ReadOnly = txtLeadContactNumber.ReadOnly = txtLeadTenure.ReadOnly = txtLeadRate.ReadOnly =
                txtLeadContactDesg.ReadOnly = txtLeadContactEmail.ReadOnly = txtLeadRemarks.ReadOnly = txtLdFinanceAmt.ReadOnly = false;
                ddlFinanceMode.Enabled = ddlLeadSourceType.Enabled = ddlAccountStatus.Enabled = ddlCustomerStatus.Enabled =
                ddlLeadStatus.Enabled = ddlFundingType.Enabled = ddlLeadConstitution.Enabled = btnGetSource.Enabled = ddlLeadAssigned.Enabled = true;
            }
            else if (_iMode == 2)   //Query
            {
                ddlFinanceMode.ClearDropDownList();
                ddlLeadSourceType.ClearDropDownList();
                ddlAccountStatus.ClearDropDownList();
                ddlCustomerStatus.ClearDropDownList();
                ddlLeadStatus.ClearDropDownList();
                ddlFundingType.ClearDropDownList();
                ddlLeadConstitution.ClearDropDownList();
                ddlLOB.ClearDropDownList();
                txtSalesPerson.ReadOnly = txtBranchSearch.ReadOnly = txtLeadInformation.ReadOnly = txtCompetitorInfo.ReadOnly =
                txtLeadContactPerson.ReadOnly = txtLeadContactNumber.ReadOnly = txtLeadTenure.ReadOnly = txtLeadRate.ReadOnly =
                txtLeadContactDesg.ReadOnly = txtLeadContactEmail.ReadOnly = txtLeadRemarks.ReadOnly = txtLdFinanceAmt.ReadOnly = true;
                grvAssets.Enabled = btnLeadClear.Enabled = btnLeadOk.Enabled = ddlLeadAssigned.Enabled = ddlLeadStatus.Enabled = false;
            }
            else if (_iMode == 3)         //Modify
            {
                FunPriLoadLeadLov();
                FunProClearLead();
                grvAssets.Enabled = btnLeadOk.Enabled = true;
                pnlLeadView.Style["display"] = "Block";
                txtSalesPerson.ReadOnly = txtBranchSearch.ReadOnly = txtLeadInformation.ReadOnly = txtCompetitorInfo.ReadOnly =
                txtLeadContactPerson.ReadOnly = txtLeadContactNumber.ReadOnly = txtLeadTenure.ReadOnly = txtLeadRate.ReadOnly = txtLdFinanceAmt.ReadOnly =
                txtLeadContactDesg.ReadOnly = txtLeadContactEmail.ReadOnly = txtLeadRemarks.ReadOnly = btnLeadClear.Enabled = false;
                ddlFinanceMode.Enabled = ddlLeadSourceType.Enabled = ddlAccountStatus.Enabled = ddlCustomerStatus.Enabled =
                ddlLeadStatus.Enabled = ddlFundingType.Enabled = ddlLeadConstitution.Enabled = btnGetSource.Enabled = ddlLeadAssigned.Enabled = true;
            }

        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    #region "New Customization Methods"

    private void FunPriSaveProspectDtl()
    {
        try
        {
            objFollowUp_Client = new ClnDataMgtServicesReference.ClnDataMgtServicesClient();
            S3GBusEntity.Collection.ClnDataMgtServices.S3G_ORG_ProspectDataTable objPspctDT = new ClnDataMgtServices.S3G_ORG_ProspectDataTable();
            S3GBusEntity.Collection.ClnDataMgtServices.S3G_ORG_ProspectRow objPspctRw = objPspctDT.NewS3G_ORG_ProspectRow();

            TextBox txtProspectCity = (TextBox)txtComCity.FindControl("TextBox");
            TextBox txtProspectCountry = (TextBox)txtComCountry.FindControl("TextBox");
            objPspctRw.Address1 = Convert.ToString(txtComAddress1.Text);
            objPspctRw.Address2 = Convert.ToString(txtCOmAddress2.Text);
            objPspctRw.City = (Convert.ToString(txtProspectCity.Text) == "" || Convert.ToString(txtProspectCity.Text) == "0") ? Convert.ToString(txtComCity.Text).Trim() : Convert.ToString(txtProspectCity.Text).Trim();
            objPspctRw.Company_ID = Convert.ToInt32(intCompanyID);
            objPspctRw.Company_Type = Convert.ToInt32(ddlCompanyType.SelectedValue);
            objPspctRw.Country = (Convert.ToString(txtProspectCountry.Text) == "" || Convert.ToString(txtProspectCountry.Text) == "0") ? Convert.ToString(txtComCountry.Text).Trim() : Convert.ToString(txtProspectCountry.Text).Trim();
            objPspctRw.Created_By = Convert.ToInt32(intUserID);
            objPspctRw.CRM_ID = (Convert.ToString(ucPopUp.SelectedValue) != "") ? Convert.ToInt64(ucPopUp.SelectedValue) : 0;
            objPspctRw.Cutsomer_Type = Convert.ToInt32(ddlCustomerType.SelectedValue);
            objPspctRw.EMail = Convert.ToString(txtComEmail.Text);
            objPspctRw.Industry_Type = Convert.ToInt32(ddlIndustry.SelectedValue);
            objPspctRw.Mobile = Convert.ToString(txtComMobile.Text);
            objPspctRw.Option = 1;
            objPspctRw.Pincode = Convert.ToString(txtComPincode.Text);
            objPspctRw.Posting_GL_Code = 0;
            objPspctRw.Prospect_Cnt_Designation = Convert.ToString(txtCntPerDesig.Text);
            objPspctRw.Prospect_Cnt_No = Convert.ToString(txtCntPerPh.Text);
            objPspctRw.Prospect_Cnt_Person = Convert.ToString(txtCntPer.Text);
            objPspctRw.Prospect_Constitution_ID = Convert.ToInt32(ddlConstitutionName.SelectedValue);
            objPspctRw.Prospect_Name = Convert.ToString(txtProspectName.Text);
            objPspctRw.Prospect_Title = Convert.ToInt32(ddlTitle.SelectedValue);
            objPspctRw.State = Convert.ToString(txtComState.SelectedValue);
            objPspctRw.Telephone = Convert.ToString(txtComTelephone.Text);
            objPspctRw.Website = Convert.ToString(txtComWebsite.Text);

            objPspctDT.AddS3G_ORG_ProspectRow(objPspctRw);

            Int64 intCRMID_Out = 0;

            intErrCode = objFollowUp_Client.FunPubCreateProspect(out intCRMID_Out, ObjSerMode, ClsPubSerialize.Serialize(objPspctDT, ObjSerMode));
            switch (intErrCode)
            {
                case 0:
                    {
                        string strMessage = (Convert.ToString(ucPopUp.SelectedValue) == "") ? "Prospect Created Successfully" : "Prospect Updated Successfully";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('" + strMessage + "');" + strRedirectPageView, true);
                        break;
                    }
                case 1:
                    {
                        Utility.FunShowAlertMsg(this.Page, "Prospect name already exists");
                        break;
                    }
                case 50:
                    {
                        Utility.FunShowAlertMsg(this.Page, "Error in Prospect Creation");
                        break;
                    }
            }
        }
        catch (Exception objException)
        {
            throw objException;
            //ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            
        }
    }

    //private void FunPriErrorMsg(Exception objException)
    //{
    //    try
    //    {
    //        cvFollowUp.ErrorMessage = objException.Message;
    //        cvFollowUp.IsValid = false;
    //    }
    //    catch (Exception objException)
    //    {
    //        ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
    //        throw objException;
    //    }
    //}

    private void FunPriSaveLeadDeatils()
    {
        try
        {
            objFollowUp_Client = new ClnDataMgtServicesReference.ClnDataMgtServicesClient();
            ClnDataMgtServices.S3G_ORG_LEADDTLDataTable obj_LeadDT = new ClnDataMgtServices.S3G_ORG_LEADDTLDataTable();
            ClnDataMgtServices.S3G_ORG_LEADDTLRow Obj_LDRw = obj_LeadDT.NewS3G_ORG_LEADDTLRow();

            if (Convert.ToInt32(ddlType.SelectedValue) == 8)
            {
                Obj_LDRw.CRM_ID = Convert.ToInt64(ucPopUp.SelectedValue);
                Obj_LDRw.Customer_ID = 0;
            }
            else
            {
                Obj_LDRw.CRM_ID = 0;
                Obj_LDRw.Customer_ID = Convert.ToInt64(hidCustomerId.Text);

            }

            Obj_LDRw.Created_By = Convert.ToInt32(intUserID);
            Obj_LDRw.Company_ID = Convert.ToInt32(intCompanyID);
            Obj_LDRw.Competitor_Info = Convert.ToString(txtCompetitorInfo.Text);
            Obj_LDRw.CP_Designation = Convert.ToString(txtLeadContactDesg.Text);
            Obj_LDRw.CP_Email = Convert.ToString(txtLeadContactEmail.Text);
            Obj_LDRw.CP_Name = Convert.ToString(txtLeadContactPerson.Text);
            Obj_LDRw.CP_Phone_No = Convert.ToString(txtLeadContactNumber.Text);
            Obj_LDRw.Drop_Remarks = Convert.ToString(txtLeadRemarks.Text);
            Obj_LDRw.Finance_Mode = Convert.ToInt32(ddlFinanceMode.SelectedValue);
            Obj_LDRw.Funding_Type = Convert.ToInt32(ddlFundingType.SelectedValue);
            Obj_LDRw.Lead_Id = Convert.ToInt32(txtLeadID.Text);
            Obj_LDRw.Lead_Information = Convert.ToString(txtLeadInformation.Text);
            Obj_LDRw.Lead_source_Type = (Convert.ToString(ddlLeadSourceType.SelectedValue) == "") ? 0 : Convert.ToInt32(ddlLeadSourceType.SelectedValue);
            if (ucLead.SelectedValue == "")
            {
                Obj_LDRw.Lead_Source = 0;
            }
            else
            {
                Obj_LDRw.Lead_Source = Convert.ToInt32(ucLead.SelectedValue);
            }
            if (ddlCustomerStatus.SelectedValue != string.Empty)
            {
                Obj_LDRw.Customer_Stat = Convert.ToInt32(ddlCustomerStatus.SelectedValue);
            }
            else
            {
                Obj_LDRw.Customer_Stat = 0;
            }
            Obj_LDRw.Lead_Status = (Convert.ToString(ddlLeadStatus.SelectedValue) == "") ? 0 : Convert.ToInt32(ddlLeadStatus.SelectedValue);
            Obj_LDRw.Lob_ID = Convert.ToInt32(ddlLOB.SelectedValue);
            Obj_LDRw.Product_Type = (Convert.ToString(ddlProductType.SelectedValue) == "") ? "0" : ddlProductType.SelectedValue;
            Obj_LDRw.Location_ID = Convert.ToInt32(hdnBranchID.Value);
            Obj_LDRw.Other_Source_Type = Convert.ToString(txtOtherLead.Text).Trim();
            Obj_LDRw.Sales_Person = (Convert.ToString(hdnSalesPersonID.Value) == "") ? 0 : Convert.ToInt32(hdnSalesPersonID.Value);
            if (ViewState["Assets"] != null && ((DataTable)ViewState["Assets"]).Rows.Count > 0)
            {
                Obj_LDRw.XML_LeadDetails = ((DataTable)ViewState["Assets"]).FunPubFormXml();
            }
            Obj_LDRw.Account_Status = Convert.ToInt32(ddlAccountStatus.SelectedValue);
            Obj_LDRw.Tenure = (Convert.ToString(txtLeadTenure.Text.Trim()) == "") ? 0 : Convert.ToInt32(txtLeadTenure.Text);
            Obj_LDRw.Rate = (Convert.ToString(txtLeadRate.Text.Trim()) == "") ? 0 : Convert.ToDouble(txtLeadRate.Text);
            Obj_LDRw.Lead_Assigned_To = (Convert.ToString(ddlLeadAssigned.SelectedValue) == "") ? 0 : Convert.ToInt32(ddlLeadAssigned.SelectedValue);
            Obj_LDRw.Finance_Amount = (Convert.ToString(txtLdFinanceAmt.Text.Trim()) == "") ? 0 : Convert.ToDouble(txtLdFinanceAmt.Text);

            obj_LeadDT.AddS3G_ORG_LEADDTLRow(Obj_LDRw);

            Int64 intLeadID = 0;
            string strLeadNo;

            intErrCode = objFollowUp_Client.FunPubCreateLead(out strLeadNo, out intLeadID, ObjSerMode, ClsPubSerialize.Serialize(obj_LeadDT, ObjSerMode));
            switch (intErrCode)
            {
                case 0:
                    {
                        if (Convert.ToInt64(txtLeadID.Text) == 0)
                            Utility.FunShowAlertMsg(this, "Lead " + strLeadNo + " Created Successfully");
                        else
                            Utility.FunShowAlertMsg(this, "Lead " + strLeadNo + " Updated Successfully");
                        FunProClearLead();
                        pnlLeadView.Style["display"] = "none";
                        DataSet dsInfo = FunPriGetCRMProspectInfo();
                        ViewState["Prospect_LeadDetails"] = dsInfo.Tables[4];
                        hdnLeadSourceType.Value = dsInfo.Tables[4].Rows[0]["Lead_Source_Type"].ToString();
                        gvLeadDetails.DataSource = dsInfo.Tables[4];
                        gvLeadDetails.DataBind();
                        break;
                    }
                case -1:
                    {
                        Utility.FunShowAlertMsg(this.Page, "Document Number control not defined for CRM");
                        break;
                    }
                case -2:
                    {
                        Utility.FunShowAlertMsg(this.Page, "Document Number control exceeded for CRM");
                        break;
                    }
                case 50:
                    {
                        Utility.FunShowAlertMsg(this.Page, "Error in Lead Details");
                        break;
                    }
            }

        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    private void FunPriLoadLeadDetails(DataSet dsLeadDetails)
    {
        try
        {
            if (dsLeadDetails != null)
            {
                txtLeadID.Text = Convert.ToString(dsLeadDetails.Tables[1].Rows[0]["Lead_ID"]);
                ddlFinanceMode.SelectedValue = Convert.ToString(dsLeadDetails.Tables[1].Rows[0]["Finance_Mode"]);
                ddlLOB.SelectedValue = Convert.ToString(dsLeadDetails.Tables[1].Rows[0]["LOB_ID"]);
                ddlLOB_SelectedIndexChanged(null, null);
                ddlProductType.SelectedValue = Convert.ToString(dsLeadDetails.Tables[1].Rows[0]["Product_ID"]);
                ddlLeadSourceType.SelectedValue = Convert.ToString(dsLeadDetails.Tables[1].Rows[0]["Lead_Source_Type"]);
                ucLead.SelectedValue = Convert.ToString(dsLeadDetails.Tables[1].Rows[0]["Lead_Source"]);
                txtLeadSource.Text = Convert.ToString(dsLeadDetails.Tables[1].Rows[0]["Lead_Source_Name"]);
                txtBranchSearch.Text = Convert.ToString(dsLeadDetails.Tables[1].Rows[0]["Location"]);
                hdnBranchID.Value = Convert.ToString(dsLeadDetails.Tables[1].Rows[0]["Location_ID"]);
                txtSalesPerson.Text = Convert.ToString(dsLeadDetails.Tables[1].Rows[0]["Sales_Person_Name"]);
                hdnSalesPersonID.Value = Convert.ToString(dsLeadDetails.Tables[1].Rows[0]["Sales_Person"]);
                ddlCustomerStatus.SelectedValue = Convert.ToString(dsLeadDetails.Tables[1].Rows[0]["Customer_Status"]);
                ddlLeadStatus.SelectedValue = Convert.ToString(dsLeadDetails.Tables[1].Rows[0]["Lead_Status"]);
                ddlAccountStatus.SelectedValue = Convert.ToString(dsLeadDetails.Tables[1].Rows[0]["Account_Status"]);
                txtLeadInformation.Text = Convert.ToString(dsLeadDetails.Tables[1].Rows[0]["Lead_Information"]);
                txtCompetitorInfo.Text = Convert.ToString(dsLeadDetails.Tables[1].Rows[0]["Competitor_Info"]);
                ddlFundingType.SelectedValue = Convert.ToString(dsLeadDetails.Tables[1].Rows[0]["Funding_Type"]);
                txtLeadTenure.Text = (Convert.ToInt32(dsLeadDetails.Tables[1].Rows[0]["Tenure"]) == 0) ? "" : Convert.ToString(dsLeadDetails.Tables[1].Rows[0]["Tenure"]);
                txtLeadRate.Text = (Convert.ToInt32(dsLeadDetails.Tables[1].Rows[0]["Rate"]) == 0) ? "" : Convert.ToString(dsLeadDetails.Tables[1].Rows[0]["Rate"]);
                txtLeadContactPerson.Text = Convert.ToString(dsLeadDetails.Tables[1].Rows[0]["Contact_Name"]);
                txtLeadContactNumber.Text = Convert.ToString(dsLeadDetails.Tables[1].Rows[0]["Contact_Number"]);
                txtLeadContactDesg.Text = Convert.ToString(dsLeadDetails.Tables[1].Rows[0]["Contact_Designation"]);
                txtLeadContactEmail.Text = Convert.ToString(dsLeadDetails.Tables[1].Rows[0]["Contact_Mail"]);
                txtOtherLead.Text = Convert.ToString(dsLeadDetails.Tables[1].Rows[0]["Other_Lead_Source"]);
                txtOtherLead.Visible = (Convert.ToString(dsLeadDetails.Tables[1].Rows[0]["Other_Lead_Source"]) == "") ? false : true;
                txtLeadRemarks.Text = Convert.ToString(dsLeadDetails.Tables[1].Rows[0]["Lead_Remarks"]);
                ddlLeadAssigned.SelectedValue = Convert.ToString(dsLeadDetails.Tables[1].Rows[0]["Lead_Assigned_To"]);
                ddlLeadAssigned.SelectedText = Convert.ToString(dsLeadDetails.Tables[1].Rows[0]["Lead_Assigned_Name"]);
                txtLdFinanceAmt.Text = (Convert.ToDouble(dsLeadDetails.Tables[1].Rows[0]["Finance_Amount"]) == 0) ? "" : Convert.ToString(dsLeadDetails.Tables[1].Rows[0]["Finance_Amount"]);

                if (dsLeadDetails.Tables[2] != null && dsLeadDetails.Tables[2].Rows.Count > 0)
                {
                    ViewState["Assets"] = dsLeadDetails.Tables[2];
                    grvAssets.DataSource = dsLeadDetails.Tables[2];
                    grvAssets.DataBind();
                    grvAssets.FooterRow.Visible = true;
                    FunPriSetFooterAssetCtg();
                }
                else
                {
                    FunProInitializeAssetRow();
                }
                pnlLeadView.Style["display"] = "Block";
                lblLeadSource.CssClass = (Convert.ToInt64(ddlLeadSourceType.SelectedValue) == 13 || Convert.ToInt64(ddlLeadSourceType.SelectedValue) == 0) ? "styleDisplayLabel" : "styleReqFieldLabel";
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    private DataSet FunPriGetCRMProspectInfo()
    {
        DataSet dsCRM = new DataSet();
        try
        {
            if (ObjDictionary != null)
                ObjDictionary.Clear();
            else
                ObjDictionary = new Dictionary<string, string>();
            ObjDictionary.Add("@Company_ID", intCompanyID.ToString());
            ObjDictionary.Add("@Type", ddlType.SelectedValue.ToString());
            ObjDictionary.Add("@UserId", intUserID.ToString());
            if (ddlType.SelectedValue == "8" || ddlType.SelectedValue == "9")
            {
                ObjDictionary.Add("@SearchValue", ucPopUp.SelectedValue);
            }
            else
            {
                ObjDictionary.Add("@SearchValue", ucPopUp.SelectedText);
            }
            if (ddlType.SelectedValue == "1" || ddlType.SelectedValue == "2" || ddlType.SelectedValue == "3")
                ObjDictionary.Add("@SearchType", "Customer");
            else if (ddlType.SelectedValue == "9")
                ObjDictionary.Add("@SearchType", "Entity");
            else
                ObjDictionary.Add("@SearchType", "Account");

            dsCRM = Utility.GetDataset("S3G_ORG_GETCRMPROSPECTINFO", ObjDictionary);
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
        return dsCRM;
    }

    private void FunPriTrackErrorLog(string strMsg)
    {
        try
        {
            cvOK.ErrorMessage = strErrorMessagePrefix + strMsg;
            cvOK.IsValid = false;
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    private void FunPriSaveTrackDetails()
    {
        try
        {
            HiddenField hdnIDFrom = ucFrom.FindControl("hdnID") as HiddenField;
            HiddenField hdnIDTo = ucTo.FindControl("hdnID") as HiddenField;
            objFollowUp_Client = new ClnDataMgtServicesReference.ClnDataMgtServicesClient();
            ClnDataMgtServices.S3G_ORG_TRACKDTLDataTable obj_TrackDtl = new ClnDataMgtServices.S3G_ORG_TRACKDTLDataTable();
            ClnDataMgtServices.S3G_ORG_TRACKDTLRow Obj_TrackRw = obj_TrackDtl.NewS3G_ORG_TRACKDTLRow();

            Obj_TrackRw.CRM_ID = (Convert.ToInt32(ddlType.SelectedValue) == 8) ? Convert.ToInt64(ucPopUp.SelectedValue) : 0;
            Obj_TrackRw.Customer_ID = (Convert.ToInt32(ddlType.SelectedValue) == 8 || Convert.ToInt32(ddlType.SelectedValue) == 9) ? 0 : Convert.ToInt32(hidCustomerId.Text);
            Obj_TrackRw.Created_By = Convert.ToInt32(intUserID);
            Obj_TrackRw.Description = Convert.ToString(txtDescription.Text);
            Obj_TrackRw.From_Type = Convert.ToInt32(ddlFrom.SelectedValue);
            Obj_TrackRw.From_User = (Convert.ToString(hdnIDFrom.Value) == "") ? 0 : Convert.ToInt32(hdnIDFrom.Value);
            Obj_TrackRw.Lead_ID = (ViewState["Lead_ID"] != null) ? Convert.ToInt32(ViewState["Lead_ID"]) : 0;
            Obj_TrackRw.Mode = Convert.ToInt32(ddlMode.SelectedValue);
            Obj_TrackRw.Target_Date = Utility.StringToDate(txtNotifyDt.Text);
            Obj_TrackRw.Ticket_No = (Convert.ToString(txtTicketNo.Text) == "") ? 0 : Convert.ToInt32(txtTicketNo.Text);
            Obj_TrackRw.Ticket_Status = Convert.ToInt32(ddlStatus.SelectedValue);
            Obj_TrackRw.To_Type = Convert.ToInt32(ddlTo.SelectedValue);
            Obj_TrackRw.To_User = (Convert.ToString(hdnIDTo.Value) == "") ? 0 : Convert.ToInt32(hdnIDTo.Value);
            Obj_TrackRw.Track_Type = Convert.ToInt32(ddlQuery.SelectedValue);
            Obj_TrackRw.Track_Detail_ID = Convert.ToInt32(txtTrackDetailID.Text);
            Obj_TrackRw.Track_Date = Utility.StringToDate(txtDate.Text);
            Obj_TrackRw.Panum = (ViewState["Lead_Panum"] != null) ? Convert.ToString(ViewState["Lead_Panum"]) : "";
            Obj_TrackRw.Entity_ID = Convert.ToInt32(ddlType.SelectedValue) == 9 ? ucPopUp.SelectedText : "0";

            obj_TrackDtl.AddS3G_ORG_TRACKDTLRow(Obj_TrackRw);

            Int64 intTrackID = 0;

            intErrCode = objFollowUp_Client.FunPubCreateTrackDetails(out intTrackID, ObjSerMode, ClsPubSerialize.Serialize(obj_TrackDtl, ObjSerMode));
            switch (intErrCode)
            {
                case 0:
                    {
                        Utility.FunShowAlertMsg(this, "Track Details added Successfully");
                        funClearPopUp();
                        pnlAddFollow.Visible = false;

                        if (ObjDictionary != null)
                            ObjDictionary.Clear();
                        else
                            ObjDictionary = new Dictionary<string, string>();

                        ObjDictionary.Add("@Company_ID", Convert.ToString(intCompanyID));
                        ObjDictionary.Add("@Type", ddlType.SelectedValue.ToString());
                        if (ddlType.SelectedValue == "8")
                        {
                            ObjDictionary.Add("@SearchValue", ucPopUp.SelectedValue);
                        }
                        else
                        {
                            if (ddlType.SelectedValue == "1" || ddlType.SelectedValue == "2" || ddlType.SelectedValue == "3")
                            {
                                ObjDictionary.Add("@SearchValue", hidCustomerId.Text);
                            }
                            else
                            {
                                ObjDictionary.Add("@SearchValue", ucPopUp.SelectedText);
                            }
                        }
                        ObjDictionary.Add("@UserId", intUserID.ToString());
                        if (ddlType.SelectedValue == "1" || ddlType.SelectedValue == "2" || ddlType.SelectedValue == "3")
                            ObjDictionary.Add("@SearchType", "Customer");
                        else
                            ObjDictionary.Add("@SearchType", "Account");

                        ObjDictionary.Add("@Lead_ID", Convert.ToString(ViewState["Lead_ID"]));
                        ObjDictionary.Add("@Lead_Panum", Convert.ToString(ViewState["Lead_Panum"]));

                        DataSet dsLead = Utility.GetDataset((ddlType.SelectedValue == "8") ? "S3G_CLN_GetCRMProspectList" : "S3G_CLN_GetCRMFollowUpList", ObjDictionary);
                        ViewState["dtFollow"] = dsLead.Tables[3];
                        grvFollowUp.DataSource = dsLead.Tables[3];
                        grvFollowUp.DataBind();
                        break;
                    }
                case 1:
                    {
                        Utility.FunShowAlertMsg(this.Page, "Date should be greater than or Equal to Ticket Generation date");
                        break;
                    }
                case 50:
                    {
                        Utility.FunShowAlertMsg(this.Page, "unable to add Track Details");
                        break;
                    }
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    private void FunPriSaveDocumentDetails()
    {
        try
        {
            if (ObjDictionary != null)
                ObjDictionary.Clear();
            else
                ObjDictionary = new Dictionary<string, string>();
            ObjDictionary.Add("@Company_ID", intCompanyID.ToString());
            ObjDictionary.Add("@LOB_ID", "0");
            ObjDictionary.Add("@Program_ID", "241");
            //ObjDictionary.Add("@User_Id", intUserID.ToString());
            DataTable dtPath = Utility.GetDefaultData("S3G_LOANAD_GetDocPath", ObjDictionary);
            string strFilePath = "";

            if (dtPath != null && dtPath.Rows.Count > 0)
            {
                strFilePath = Convert.ToString(dtPath.Rows[0]["DOCUMENT_PATH"]);
            }
            else
            {
                Utility.FunShowAlertMsg(this, "Define Scan Image Path in Document path setup for CRM");
                return;
            }

            string strDocument = Convert.ToString(ddlDocument.SelectedValue);
            string strCacheFile = "Docs_" + Convert.ToString(ddlDocumentType.SelectedValue) + "_File_" + strDocument;
            if (Cache[strCacheFile] != null)
            {
                HttpPostedFile hpf = (HttpPostedFile)Cache[strCacheFile];

                string strFolderName = @"\COMPANY" + intCompanyID.ToString();
                strFilePath = strFilePath + strFolderName;

                if (!Directory.Exists(strFilePath))
                {
                    Directory.CreateDirectory(strFilePath);
                }
                strFilePath += @"\" + System.IO.Path.GetFileName(hpf.FileName).Split('.')[0].ToString() + DateTime.Now.ToLocalTime().ToString().Replace(" ", "").Replace("/", "").Replace(":", "") + "." + System.IO.Path.GetFileName(hpf.FileName).Split('.')[1].ToString();
                hpf.SaveAs(strFilePath);
            }
            else
            {
                strFilePath = "";
            }

            objFollowUp_Client = new ClnDataMgtServicesReference.ClnDataMgtServicesClient();
            ClnDataMgtServices.S3G_ORG_DocumentDtlDataTable obj_DocDtl = new ClnDataMgtServices.S3G_ORG_DocumentDtlDataTable();
            ClnDataMgtServices.S3G_ORG_DocumentDtlRow Obj_DocRw = obj_DocDtl.NewS3G_ORG_DocumentDtlRow();

            Obj_DocRw.Collected_By = (Convert.ToInt32(ddlCollectedBy.SelectedValue) > 0 && Convert.ToString(ddlCollectedBy.SelectedText) != "") ? Convert.ToInt32(ddlCollectedBy.SelectedValue) : 0;
            Obj_DocRw.Scanned_By = (Convert.ToInt32(ddlScannedBy.SelectedValue) > 0 && Convert.ToString(ddlScannedBy.SelectedText) != "") ? Convert.ToInt32(ddlScannedBy.SelectedValue) : 0;
            if (Convert.ToString(txtCollectedDate.Text) != "")
                Obj_DocRw.Collected_Date = Utility.StringToDate(txtCollectedDate.Text);
            Obj_DocRw.Company_ID = Convert.ToInt32(intCompanyID);
            Obj_DocRw.Created_By = Convert.ToInt32(intUserID);
            Obj_DocRw.CRM_Doc_ID = (Convert.ToString(txtCRMDocumentID.Text) == "") ? 0 : Convert.ToInt64(txtCRMDocumentID.Text);
            if (Convert.ToInt32(ddlType.SelectedValue) == 8)
            {
                Obj_DocRw.CRM_ID = Convert.ToInt64(ucPopUp.SelectedValue);
                Obj_DocRw.Customer_ID = 0;
                if (ViewState["Lead_ID"].ToString() != "0")
                {
                    Obj_DocRw.Lead_id = ViewState["Lead_ID"].ToString();
                }
            }
            else if (Convert.ToInt32(ddlType.SelectedValue) == 9)
            {
                Obj_DocRw.CRM_ID = 0;
                Obj_DocRw.Customer_ID = 0;
                Obj_DocRw.Lead_id = ViewState["Lead_ID"].ToString();
            }
            else
            {
                Obj_DocRw.CRM_ID = 0;
                Obj_DocRw.Customer_ID = Convert.ToInt64(hidCustomerId.Text);
            }

            Obj_DocRw.Document_SubType_ID = Convert.ToInt32(ddlDocument.SelectedValue);
            Obj_DocRw.Document_Type_ID = Convert.ToInt32(ddlDocumentType.SelectedValue);
            Obj_DocRw.IS_Collected = (Convert.ToBoolean(chkIsCollected.Checked) == true) ? 1 : 0;
            Obj_DocRw.Document_Value = Convert.ToString(txtValue.Text);
            Obj_DocRw.Option = 2;
            Obj_DocRw.Remarks = Convert.ToString(txtDocRemarks.Text);
            if (Convert.ToString(strFilePath) != "")
                Obj_DocRw.Scan_Path = Convert.ToString(strFilePath);
            if (Convert.ToString(txtscannedDate.Text) != "")
            {
                Obj_DocRw.Scanned_Date = Utility.StringToDate(txtscannedDate.Text);
            }

            obj_DocDtl.AddS3G_ORG_DocumentDtlRow(Obj_DocRw);

            Int64 intDocID = 0;

            intErrCode = objFollowUp_Client.FunPubCreateDocumentDetails(out intDocID, ObjSerMode, ClsPubSerialize.Serialize(obj_DocDtl, ObjSerMode));
            switch (intErrCode)
            {
                case 0:
                    {
                        if (Convert.ToString(txtCRMDocumentID.Text) == "" || Convert.ToString(txtCRMDocumentID.Text) == "0")
                        {
                            Utility.FunShowAlertMsg(this, "Document Details Added Successfully");
                        }
                        else
                        {
                            Utility.FunShowAlertMsg(this, "Document Details Updated Successfully");
                        }
                        //FunProGetDocs("");
                        FunProClearDocumentControls();
                        break;
                    }
                case 50:
                    {
                        Utility.FunShowAlertMsg(this.Page, "Error in Document Details");
                        break;
                    }
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }

    }

    private void FunPriClearGroupDetails()
    {
        try
        {
            pnlGroupDetails.Visible = false;
            grvGroupDetails.DataSource = null;
            grvGroupDetails.DataBind();
            txtGroupCode.Text = txtGroupName.Text = "";
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    private void FunPriGetTrackDtl()
    {
        try
        {
            if (ObjDictionary != null)
                ObjDictionary.Clear();
            else
                ObjDictionary = new Dictionary<string, string>();

            ObjDictionary.Add("@Option", "1");
            if (ViewState["Lead_ID"] != null && Convert.ToInt32(ViewState["Lead_ID"]) > 0)
                ObjDictionary.Add("@Lead_ID", Convert.ToString(ViewState["Lead_ID"]));
            if (ViewState["Lead_Panum"] != null && Convert.ToString(ViewState["Lead_Panum"]) != "")
                ObjDictionary.Add("@Panum", Convert.ToString(ViewState["Lead_Panum"]));
            if (Convert.ToString(txtSTicketNo.Text).Trim() != "")
                ObjDictionary.Add("@Ticket_No", Convert.ToString(txtSTicketNo.Text));
            if (Convert.ToString(txtSDate.Text).Trim() != "")
                ObjDictionary.Add("@Ticket_Date", Convert.ToString(Utility.StringToDate(txtSDate.Text)));

            DataTable dtTrackDtl = Utility.GetDefaultData("S3G_ORG_CRMCMNLST", ObjDictionary);
            ViewState["dtFollow"] = dtTrackDtl;
            grvFollowUp.DataSource = dtTrackDtl;
            grvFollowUp.DataBind();
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    #endregion

    #endregion

    private bool FunPriCheckEnq()
    {
        bool blnRslt = true;
        try
        {
            TextBox _txtState = (TextBox)txtComState.FindControl("TextBox");
            TextBox _txtCity = (TextBox)txtComCity.FindControl("TextBox");
            TextBox _txtCountry = (TextBox)txtComCountry.FindControl("TextBox");
            string strPrspcError = string.Empty;
            if (Convert.ToInt32(ddlConstitutionName.SelectedValue) == 0)
            {
                strPrspcError += "Select the Prospect Constitution </br>";
            }
            if (Convert.ToString(txtComAddress1.Text) == "")
            {
                strPrspcError += "Enter the Prospect Address1 </br>";
            }
            if (Convert.ToString(_txtCity.Text) == "")
            {
                strPrspcError += "Enter the Prospect City </br>";
            }
            if (Convert.ToString(_txtState.Text) == "")
            {
                strPrspcError += "Enter the Prospect State </br>";
            }
            if (Convert.ToString(_txtCountry.Text) == "")
            {
                strPrspcError += "Enter the Prospect Country </br>";
            }
            if (Convert.ToString(txtComMobile.Text) == "")
            {
                strPrspcError += "Enter the Prospect Mobile";
            }

            if (strPrspcError != "")
            {
                cvEnquiry.ErrorMessage = strPrspcError;
                cvEnquiry.IsValid = false;
                blnRslt = false;
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
        return blnRslt;
    }


    protected void grvGroupDtls_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[7].HorizontalAlign = HorizontalAlign.Right;
        }

    }
    protected void grvCustDtls_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[8].HorizontalAlign = HorizontalAlign.Right;
        }
    }
    protected void ddlLOB_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();

        Dictionary<string, string> objProcedureParameter = new Dictionary<string, string>();
        objProcedureParameter.Add("@Lob_ID", ddlLOB.SelectedValue);
        dt = Utility.GetDefaultData("S3G_APP_GetProducttype", objProcedureParameter);
        FunFillGrid(ddlProductType, dt, "Product_ID", "Product_Name");
    }
   

    protected void grvDCDairyAccounts_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label txtNOD = e.Row.FindControl("txtNOD") as Label;
                LinkButton lnkNOD = e.Row.FindControl("lnkNOD") as LinkButton;
                if (txtNOD.Text.Trim() == "0")
                    lnkNOD.Visible = false;
                else
                    txtNOD.Visible = false;
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    protected void btnkDcSelect_Click(object sender, EventArgs e)
    {
        btndcAddOld.Enabled = true;
        btndcAdd.Enabled = true;
        FunPubClearPopUp();
        funShowPopUpDCDairy(sender, e, "Billed Amount");
        MoedcdairyDetails.Show();
    }
    protected void grvDCDairyPopUp_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                string strTotalDue;
                string strTotalCollectionSum;
                string strTotalBalSum;
                DataTable dt = (DataTable)ViewState["DC_DAIRY_SUM"];
                strTotalDue = dt.Compute("Sum(DUE_AMOUNT_TOT)", "").ToString();
                strTotalCollectionSum = dt.Compute("Sum(COLLECTION_TOT)", "").ToString();
                strTotalBalSum = dt.Compute("Sum(BALANCE_TOT)", "").ToString();

                TextBox txtDueSum = e.Row.FindControl("txtgrvDCDueSum") as TextBox;
                TextBox txtTotalCollection = e.Row.FindControl("txtgrvDCTotalCollection") as TextBox;
                TextBox txtTotalbalSum = e.Row.FindControl("txtgrvDCTotalbalSum") as TextBox;
                txtDueSum.SetDecimalPrefixSuffix(14, 3, true, "Due Aount");
                txtTotalCollection.SetDecimalPrefixSuffix(14, 3, true, "Due Aount");
                txtTotalbalSum.SetDecimalPrefixSuffix(14, 3, true, "Due Aount");
                txtDueSum.Text = Math.Round(Convert.ToDecimal(strTotalDue), 3).ToString("0.000"); // Output: 1c
                txtTotalCollection.Text = Math.Round(Convert.ToDecimal(strTotalCollectionSum), 3).ToString("0.000"); // Output: 1c
                txtTotalbalSum.Text = Math.Round(Convert.ToDecimal(strTotalBalSum), 3).ToString("0.000"); // Output: 1c
                txtDueSum.Enabled = false;
                txtTotalCollection.Enabled = false;
                txtTotalbalSum.Enabled = false;


            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                AjaxControlToolkit.CalendarExtender CEPTPDate = e.Row.FindControl("CEPTPDate") as AjaxControlToolkit.CalendarExtender;
                CEPTPDate.Format = strDateFormat;
                DropDownList ddldcstatus = e.Row.FindControl("ddlgrvDCStatus") as DropDownList;
                Label lblDcStatus = e.Row.FindControl("lblgrvDcStatus") as Label;
                TextBox txtPTPdate = e.Row.FindControl("txtgrvDCPTPdate") as TextBox;
                TextBox txtValue = e.Row.FindControl("txtgrvDCValue") as TextBox;
                TextBox txtRemarks = e.Row.FindControl("txtgrvDCRemarks") as TextBox;
                Dictionary<string, string> ProParm = new Dictionary<string, string>();
                ProParm.Add("@COMPANYID", intCompanyID.ToString());
                ddldcstatus.BindDataTable("CN_GET_DC_DIRSTUS", ProParm, new string[] { "ID", "Name" });
               // txtValue.SetDecimalPrefixSuffix(10, 3, true, "Value");



                if (lblDcStatus.Text == "2")
                {
                    txtPTPdate.ReadOnly = txtValue.ReadOnly = txtRemarks.ReadOnly = true;
                    CEPTPDate.Enabled = false;
                    if (lblDcStatus.Text != "3")
                    {
                        ddldcstatus.SelectedValue = lblDcStatus.Text;
                        ddldcstatus.ClearDropDownList();
                    }
                }
                if (lblDcStatus.Text != "3")
                {
                    ddldcstatus.SelectedValue = lblDcStatus.Text;
                }
                //txtPTPdate.Text = DateTime.Parse(txtPTPdate.Text, CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);


            }

        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }
    protected void txtgrvDCPTPdate_TextChanged(object sender, EventArgs e)
    {
        string strFieldAtt = ((TextBox)sender).ClientID;
        int gRowIndex = Utility.FunPubGetGridRowID("grvDCDairyPopUp", strFieldAtt);
        Label lblUpdateStatus = grvDCDairyPopUp.Rows[gRowIndex].FindControl("lblgrvDCUpdateStatus") as Label;
        lblUpdateStatus.Text = "1";
        btndcAdd.Enabled = true;
    }
    protected void txtgrvDCValue_TextChanged(object sender, EventArgs e)
    {
        string strFieldAtt = ((TextBox)sender).ClientID;
        int gRowIndex = Utility.FunPubGetGridRowID("grvDCDairyPopUp", strFieldAtt);
        Label lblUpdateStatus = grvDCDairyPopUp.Rows[gRowIndex].FindControl("lblgrvDCUpdateStatus") as Label;
        lblUpdateStatus.Text = "1";
        btndcAdd.Enabled = true;
    }
    protected void txtgrvDCRemarks_TextChanged(object sender, EventArgs e)
    {
        string strFieldAtt = ((TextBox)sender).ClientID;
        int gRowIndex = Utility.FunPubGetGridRowID("grvDCDairyPopUp", strFieldAtt);
        Label lblUpdateStatus = grvDCDairyPopUp.Rows[gRowIndex].FindControl("lblgrvDCUpdateStatus") as Label;
        lblUpdateStatus.Text = "1";
        btndcAdd.Enabled = true;
    }
    protected void ddlgrvDCStatus_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void grvDCDairyPopUpOld_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                string strTotalDue;
                string strTotalCollectionSum;
                string strTotalBalSum;
                DataTable dt = (DataTable)ViewState["DC_DAIRY_SUM_OLD"];
                strTotalDue = dt.Compute("Sum(DUE_AMOUNT_TOT)", "").ToString();
                strTotalCollectionSum = dt.Compute("Sum(COLLECTION_TOT)", "").ToString();
                strTotalBalSum = dt.Compute("Sum(BALANCE_TOT)", "").ToString();

                TextBox txtDueSum = e.Row.FindControl("txtgrvDCDueSum") as TextBox;
                TextBox txtTotalCollection = e.Row.FindControl("txtgrvDCTotalCollection") as TextBox;
                TextBox txtTotalbalSum = e.Row.FindControl("txtgrvDCTotalbalSum") as TextBox;
                txtDueSum.SetDecimalPrefixSuffix(14, 3, true, "Due Aount");
                txtTotalCollection.SetDecimalPrefixSuffix(14, 3, true, "Due Aount");
                txtTotalbalSum.SetDecimalPrefixSuffix(14, 3, true, "Due Aount");
                txtDueSum.Text = Math.Round(Convert.ToDecimal(strTotalDue), 3).ToString("0.000"); ; // Output: 1c
                txtTotalCollection.Text = Math.Round(Convert.ToDecimal(strTotalCollectionSum), 3).ToString("0.000"); // Output: 1c
                txtTotalbalSum.Text = Math.Round(Convert.ToDecimal(strTotalBalSum), 3).ToString("0.000"); // Output: 1c
                txtDueSum.Enabled = false;
                txtTotalCollection.Enabled = false;
                txtTotalbalSum.Enabled = false;

            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                AjaxControlToolkit.CalendarExtender CEPTPDate = e.Row.FindControl("CEgvDCOLDPTPDate") as AjaxControlToolkit.CalendarExtender;
                CEPTPDate.Format = strDateFormat;
                DropDownList ddldcstatus = e.Row.FindControl("ddlgvDCOLDStatus") as DropDownList;
                Label lblDcStatus = e.Row.FindControl("lblgvDCOLDUpdateStatus") as Label;
                TextBox txtPTPdate = e.Row.FindControl("txtgvDCOLDPTPdate") as TextBox;
                TextBox txtValue = e.Row.FindControl("txtgvDCOLDValue") as TextBox;
                TextBox txtRemarks = e.Row.FindControl("txtgvDCOLDRemarks") as TextBox;
                Dictionary<string, string> ProParm = new Dictionary<string, string>();
                ProParm.Add("@COMPANYID", intCompanyID.ToString());
                ddldcstatus.BindDataTable("CN_GET_DC_DIRSTUS", ProParm, new string[] { "ID", "Name" });
                Label lblPANUM = e.Row.FindControl("lblgvDCOLDPANUM") as Label;
                Label lblCASHFLOW = e.Row.FindControl("lblgvDCOLDCASHFLOW") as Label;
                Label lblDueDate = e.Row.FindControl("lblgvDCOLDDueDate") as Label;

                if (lblDcStatus.Text == "2")
                {
                    txtPTPdate.ReadOnly = txtValue.ReadOnly = txtRemarks.ReadOnly = true;
                    ddldcstatus.SelectedValue = lblDcStatus.Text;
                    ddldcstatus.ClearDropDownList();
                }
                else
                {
                    ddldcstatus.SelectedValue = lblDcStatus.Text;
                }
                txtValue.SetDecimalPrefixSuffix(10, 3, true, "Value");
                txtPTPdate.Text = DateTime.Parse(txtPTPdate.Text, CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);



            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }
    protected void txtgvDCOLDPTPdate_TextChanged(object sender, EventArgs e)
    {
        string strFieldAtt = ((TextBox)sender).ClientID;
        int gRowIndex = Utility.FunPubGetGridRowID("grvDCDairyPopUpOld", strFieldAtt);
        Label lblUpdateStatus = grvDCDairyPopUpOld.Rows[gRowIndex].FindControl("lblUpdateStatus") as Label;
        lblUpdateStatus.Text = "1";
        btndcAdd.Enabled = true;
    }
    protected void txtgvDCOLDValue_TextChanged(object sender, EventArgs e)
    {

    }
    protected void txtgvDCOLDRemarks_TextChanged(object sender, EventArgs e)
    {
        string strFieldAtt = ((TextBox)sender).ClientID;
        int gRowIndex = Utility.FunPubGetGridRowID("grvDCDairyPopUpOld", strFieldAtt);
        Label lblUpdateStatus = grvDCDairyPopUpOld.Rows[gRowIndex].FindControl("lblUpdateStatus") as Label;
        lblUpdateStatus.Text = "1";
        btndcAdd.Enabled = true;
    }
    protected void ddlgvDCOLDStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strFieldAtt = ((DropDownList)sender).ClientID;
        int gRowIndex = Utility.FunPubGetGridRowID("grvDCDairyPopUpOld", strFieldAtt);
        Label lblUpdateStatus = grvDCDairyPopUpOld.Rows[gRowIndex].FindControl("lblUpdateStatus") as Label;
        lblUpdateStatus.Text = "1";
        btndcAdd.Enabled = true;
    }
    protected void btndcdairyClose_Click(object sender, EventArgs e)
    {
        try
        {
            FunPubClearPopUp();
            MoedcdairyDetails.Hide();
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }
    protected void btndcAdd_Click(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["DCDAIRY"] != null)
            {
                ViewState["DCDAIRY"] = null;
            }
            DataTable dtDcDairy = new DataTable();
            dtDcDairy.Columns.Add("S_No");
            dtDcDairy.Columns.Add("PANUM");
            dtDcDairy.Columns.Add("DUE_FLAG");
            dtDcDairy.Columns.Add("DUE_DATE");
            dtDcDairy.Columns.Add("DUE_AMOUNT");
            dtDcDairy.Columns.Add("CASHFLOWDESCRIPTION");
            dtDcDairy.Columns.Add("COLLECTION");
            dtDcDairy.Columns.Add("BALANCE");
            dtDcDairy.Columns.Add("PTP_DATE");
            dtDcDairy.Columns.Add("VALUE_AMNT");
            dtDcDairy.Columns.Add("REMARKS");
            dtDcDairy.Columns.Add("STATUS_ID");
            dtDcDairy.Columns.Add("DC_NAME");
            dtDcDairy.Columns.Add("User_Id");
            dtDcDairy.Columns.Add("User_Name");
            dtDcDairy.Columns.Add("MODStatus");
            foreach (GridViewRow grv in grvDCDairyPopUp.Rows)
            {
                Label lblSlNo = grv.FindControl("lblgrvDCSlNo") as Label;
                Label lblPANUM = grv.FindControl("lblgrvDCPANUM") as Label;
                Label lblCASHFLOW = grv.FindControl("lblgrvDCCASHFLOW") as Label;
                Label lblCASHFLOW_DESC = grv.FindControl("lblgrvDCCASHFLOW_DESC") as Label;
                Label lblDueDate = grv.FindControl("lblgrvDCDueDate") as Label;
                Label lblDueAmount = grv.FindControl("lblgrvDCDueAmount") as Label;
                Label lblCollection = grv.FindControl("lblgrvDCCollection") as Label;
                Label lblCurrentInstallmentDue = grv.FindControl("lblCurrentInstallmentDue") as Label;
                Label lblArrearInstallmentDue = grv.FindControl("lblArrearInstallmentDue") as Label;
                Label lblCurrentInstallmentCollection = grv.FindControl("lblCurrentInstallmentCollection") as Label;
                Label lblArrearInstallmentCollection = grv.FindControl("lblArrearInstallmentCollection") as Label;
                Label lblRecordedby = grv.FindControl("lblgrvDCRecordedby") as Label;
                Label lblBalance = grv.FindControl("lblgrvDCBalance") as Label;
                TextBox txtPTPdate = grv.FindControl("txtgrvDCPTPdate") as TextBox;
                TextBox txtValue = grv.FindControl("txtgrvDCValue") as TextBox;
                TextBox txtRemarks = grv.FindControl("txtgrvDCRemarks") as TextBox;
                DropDownList ddlStatus = grv.FindControl("ddlgrvDCStatus") as DropDownList;
                Label lblDCName = grv.FindControl("lblgrvDCName") as Label;
                Label lblUserId = grv.FindControl("lblgrvDCUserId") as Label;
                Label lblUpdateStatus = grv.FindControl("lblgrvDCUpdateStatus") as Label;




                if (ViewState["DCDAIRYMAIN"] != null)
                {
                    DataRow[] dr1 = ((DataTable)ViewState["DCDAIRYMAIN"]).Select("PANUM='" + lblPANUM.Text + "' AND S_No='" + lblSlNo.Text + "' ");
                    if (dr1.Count() > 0)
                    {
                        foreach (DataRow drr in dr1)
                        {
                            drr.Delete();
                        }
                    }
                }
                DataRow dr = dtDcDairy.NewRow();
                dr["S_No"] = lblSlNo.Text;
                dr["PANUM"] = lblPANUM.Text;
                dr["DUE_FLAG"] = lblCASHFLOW.Text;
                dr["CASHFLOWDESCRIPTION"] = lblCASHFLOW_DESC.Text;
                dr["COLLECTION"] = lblCollection.Text;
                if (lblDueDate.Text != string.Empty)
                    dr["DUE_DATE"] = Utility.StringToDate(lblDueDate.Text);
                else
                    dr["DUE_DATE"] = "";
                dr["DUE_AMOUNT"] = lblDueAmount.Text;
                dr["BALANCE"] = lblBalance.Text;
                if (txtPTPdate.Text != string.Empty)
                    dr["PTP_DATE"] = Utility.StringToDate(txtPTPdate.Text);
                else
                    dr["PTP_DATE"] = "";
                dr["VALUE_AMNT"] = txtValue.Text;
                dr["REMARKS"] = txtRemarks.Text;
                dr["STATUS_ID"] = ddlStatus.SelectedValue;
                dr["DC_NAME"] = lblDCName.Text;
                dr["User_Id"] = lblUserId.Text;
                dr["User_Name"] = lblRecordedby.Text;
                dr["MODStatus"] = lblUpdateStatus.Text;
                dtDcDairy.Rows.Add(dr);
                if (ViewState["DCDAIRY"] != null)
                {
                    DataTable dt = (DataTable)ViewState["DCDAIRY"];
                    dt.Merge(dtDcDairy);
                    ViewState["DCDAIRY"] = dt;
                }
                else
                    ViewState["DCDAIRY"] = dtDcDairy;
            }
            if (ViewState["DCDAIRYMAIN"] != null)
            {
                DataTable dtt = (DataTable)ViewState["DCDAIRYMAIN"];
                dtt.Merge(dtDcDairy);
                ViewState["DCDAIRYMAIN"] = dtt;
            }
            else
                ViewState["DCDAIRYMAIN"] = ViewState["DCDAIRY"];

            FunPriSaveDCCRM();
        }

        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }


    private void FunPriSaveDCCRM()
    {
        try
        {
            objFollowUp_Client = new ClnDataMgtServicesReference.ClnDataMgtServicesClient();            
            S3GBusEntity.Collection.ClnDataMgtServices.S3G_CLN_DCDataTable ObjDCDT = new ClnDataMgtServices.S3G_CLN_DCDataTable();            
            S3GBusEntity.Collection.ClnDataMgtServices.S3G_CLN_DCRow OBJDCRw = ObjDCDT.NewS3G_CLN_DCRow();
            ObjDictionary = new Dictionary<string, string>();            
            OBJDCRw.Company_ID = Convert.ToInt32(intCompanyID.ToString());

            if (ViewState["DC_DAIRY_SUM"] != null)
            {
                OBJDCRw.XML_DC_DAIRYDETAILS = ((DataTable)ViewState["DC_DAIRY_SUM"]).FunPubFormXml();                
            }
            else
            {
                OBJDCRw.XML_DC_DAIRYDETAILS = "<Root></Root>";
            }
            //if (ViewState["DC_DAIRY_SUM_OLD"] != null)
            //{
            //    OBJDCRw.XML_DC_DAIRYDETAILS_OLD = ((DataTable)ViewState["DC_DAIRY_SUM_OLD"]).FunPubFormXml();
            //}
            //else
            //{
            //    OBJDCRw.XML_DC_DAIRYDETAILS_OLD = "<Root></Root>";
            //}
            string strDcNo = string.Empty;
            Int64 intDCID = 0;
            ObjDCDT.AddS3G_CLN_DCRow(OBJDCRw);            
            intErrCode = objFollowUp_Client.FunPubCreateDC(out strDcNo, out intDCID, ObjSerMode, ClsPubSerialize.Serialize(ObjDCDT, ObjSerMode));
            switch (intErrCode)
            {
                case 0:
                    {
                        string strMessage = (Convert.ToString(ucPopUp.SelectedValue) == "") ? "DC Created Successfully" : "DC Updated Successfully";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('" + strMessage + "');" + strRedirectPageView, true);
                        break;
                    }               
                case 50:
                    {
                        Utility.FunShowAlertMsg(this.Page, "Error in DC Creation");
                        break;
                    }
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }
    private void funShowPopUpDCDairy(object sender, EventArgs e, string strType)//Added by Sathish
    {
        try
        {
            if (ObjDictionary != null)
                ObjDictionary.Clear();
            else
                ObjDictionary = new Dictionary<string, string>();

            Button lnkNOD = (Button)sender;
            GridViewRow gvRow = lnkNOD.Parent.Parent as GridViewRow;
            HiddenField hidLOB = gvRow.FindControl("hidLOB") as HiddenField;
            HiddenField hidBranch = gvRow.FindControl("hidBranch") as HiddenField;
            Label txtPANum = gvRow.FindControl("txtPANum") as Label;
            Label txtSANum = gvRow.FindControl("txtSANum") as Label;
            ObjDictionary.Add("@Company_ID", intCompanyID.ToString());
            ObjDictionary.Add("@User_Id", intUserID.ToString());
            ObjDictionary.Add("@PANUM", txtPANum.Text.Trim());

            DataSet dsJnEntry = Utility.GetDataset("CN_GET_FLWDC_DIRY_POPUP", ObjDictionary);
            if (dsJnEntry.Tables[0].Rows.Count > 0)
            {
                btndcAdd.Enabled = true;
                if (ViewState["DCDAIRYMAIN"] != null)
                {
                    DataTable dt1 = ((DataTable)ViewState["DCDAIRYMAIN"]).Select("PANUM='" + txtPANum.Text.Trim() + "'").CopyToDataTable();
                    grvDCDairyPopUp.DataSource = dt1;
                    grvDCDairyPopUp.DataBind();
                }
                else
                {
                    ViewState["DC_DAIRY_SUM"] = dsJnEntry.Tables[0];
                    ViewState["DC_DAIRY_SUM_OLD"] = dsJnEntry.Tables[0];
                    grvDCDairyPopUp.DataSource = dsJnEntry.Tables[0];
                    grvDCDairyPopUp.DataBind();
                }
                MoedcdairyDetails.Show();
            }
            else
            {
                grvDCDairyPopUp.DataSource = null;
                grvDCDairyPopUp.DataBind();
                btndcAdd.Enabled = false;
            }
            if (dsJnEntry.Tables[1].Rows.Count > 0)
            {
                btndcAdd.Enabled = true;
                if (ViewState["DCDAIRYMAIN_OLD"] != null)
                {
                    DataTable dt1 = ((DataTable)ViewState["DCDAIRYMAIN_OLD"]).Select("PANUM='" + txtPANum.Text.Trim() + "'").CopyToDataTable();
                    grvDCDairyPopUpOld.DataSource = dt1;
                    grvDCDairyPopUpOld.DataBind();
                }
                else
                {

                    ViewState["DC_DAIRY_SUM_OLD"] = dsJnEntry.Tables[1];
                    grvDCDairyPopUpOld.DataSource = dsJnEntry.Tables[1];
                    grvDCDairyPopUpOld.DataBind();
                }
            }
            else
            {
                grvDCDairyPopUpOld.DataSource = null;
                grvDCDairyPopUpOld.DataBind();
            }
            MoedcdairyDetails.Show();
            lblNodHead.Text = strType;
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }
}