#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Orgination 
/// Screen Name			: Entity Master
/// Created By			: Nataraj Y
/// Created Date		: 24-June-2010
/// Purpose	            : 
/// 
/// Modified By			: Nataraj Y
/// Modified Date		: 01-Dec-2010
/// Purpose	            : Bug Fixing - Round 3
#endregion

#region Namespaces
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using S3GBusEntity;
using ORG = S3GBusEntity.Origination;
using ORGSERVICE = OrgMasterMgtServicesReference;
using System.Collections;
using System.Configuration;
using System.Web;
using System.IO;
#endregion

public partial class Origination_S3GOrgEntityMaster : ApplyThemeForProject
{
    #region Initialization
    int intCompanyId = 0;
    int intUserId = 0;
    int intEntityId = 0;
    int intErrorCode = 0;
    int intProgramID = 32;
    string strEntityCode;
    ORG.OrgMasterMgtServices.S3G_ORG_Entity_MasterDataTable ObjEntityMasterDataTable;
    //ORG.OrgMasterMgtServices.S3G_ORG_EntityBankMappingDataTable ObjEntityBankMappingDataTable;
    ORGSERVICE.OrgMasterMgtServicesClient ObjOrgMasterMgtServicesClient;
    UserInfo ObjUserInfo;
    S3GSession ObjS3GSession;
    StringBuilder strbBnkDetails = new StringBuilder();
    StringBuilder strAdditionalParmDet = new StringBuilder();
    static string strPageName = "Entity Master";
    public string strDateFormat;
    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    string strRedirectPageView = "window.location.href='../Origination/S3GOrgEntityMaster_View.aspx';";
    string strRedirectPageAdd = "window.location.href='../Origination/S3GOrgEntityMaster_Add.aspx';";
    string strRedirectPage = "~/Origination/S3GOrgEntityMaster_Add.aspx";
    //User Authorization
    string strMode = string.Empty;
    bool bCreate = false;
    bool bModify = false;
    bool bQuery = false;
    // bool bDelete = false;
    // bool bMakerChecker = false;
    bool bClearList = false;
    public static Origination_S3GOrgEntityMaster obj_Page;
    int strDecMaxLength = 0;
    int strPrefixLength = 0;
    //Code end
    Dictionary<string, string> Procparam = new Dictionary<string, string>();
    DataTable dtOperationHisDet = new DataTable();
    DataTable dtEmployeeHisDet = new DataTable();
    DataTable dtInsurancePolicyDet = new DataTable();
    #endregion

    #region PageLoad
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            ObjUserInfo = new UserInfo();
            intCompanyId = ObjUserInfo.ProCompanyIdRW;
            intUserId = ObjUserInfo.ProUserIdRW;

            ObjS3GSession = new S3GSession();
            strDateFormat = ObjS3GSession.ProDateFormatRW;

            strPrefixLength = ObjS3GSession.ProGpsPrefixRW;
            strDecMaxLength = ObjS3GSession.ProGpsSuffixRW;

            ceFromDate.Format = strDateFormat;                       // assigning the first textbox with the End date
            ceToDate.Format = strDateFormat;


            System.Web.HttpContext.Current.Session["AutoSuggestCompanyID"] = intCompanyId.ToString();
            System.Web.HttpContext.Current.Session["AutoProgramID"] = intProgramID.ToString();

            //User Authorization
            bCreate = ObjUserInfo.ProCreateRW;
            bModify = ObjUserInfo.ProModifyRW;
            bQuery = ObjUserInfo.ProViewRW;
            bClearList = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ClearListValues"));
            //Code end

            obj_Page = this;



            txtTaxNumber.Attributes.Add("maxlength", "10");

            if (Request.QueryString["qsMode"] != null)
                strMode = Request.QueryString["qsMode"];
            if (Request.QueryString["qsEntityId"] != null)
            {
                FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsEntityId"));
                if (fromTicket != null)
                {
                    intEntityId = Convert.ToInt32(fromTicket.Name);
                }
                else
                {
                    strAlert = strAlert.Replace("__ALERT__", "Invalid Entity Details");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                }

            }

            System.Web.HttpContext.Current.Session["AutoStrMode"] = strMode;

            TextBox txtItemNameNationality = ((TextBox)ddlNationality.FindControl("txtItemName"));
            txtItemNameNationality.Attributes.Add("onchange", "fnTrashSuggest('" + ddlNationality.ClientID + "');");


            TextBox txtItemNameDealer = ((TextBox)ddlDealer.FindControl("txtItemName"));
            txtItemNameDealer.Attributes.Add("onchange", "fnTrashSuggest('" + ddlDealer.ClientID + "');");

            TextBox txtItemNameBankName = ((TextBox)txtBankName.FindControl("txtItemName"));
            txtItemNameBankName.Attributes.Add("onchange", "fnTrashSuggest('" + txtBankName.ClientID + "');");



            this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);
            if (!IsPostBack)
            {
                FunBind_Effective_To();
                // FunPubBindAddsGrid();
                FunProIntializeData();
                FunproInitializeAddressData();
                FunPriLoadCommonData();
                //if (PageMode != PageModes.Query)
                //{
                //FunProLoadAddressCombos();

                Dictionary<string, string> Procparam = new Dictionary<string, string>();



                if (PageMode == PageModes.Create)
                {
                    //Procparam.Add("@Option", "1");
                    //ddlEntityType.BindDataTable(SPNames.S3G_ORG_GetEntity_List, Procparam, new string[] { "ENTITY_TYPE_ID", "ENTITY_TYPE_Name" });
                    Procparam.Add("@Company_ID", Convert.ToString(intCompanyId));
                    Procparam.Add("@User_ID", Convert.ToString(intUserId));
                    ddlEntityType.BindDataTable("S3G_ORG_GET_ENTITY_TYPE_LIST", Procparam, new string[] { "ENTITY_TYPE_ID", "ENTITY_TYPE_Name" });
                }


                Procparam.Clear();
                Procparam.Add("@Option", "2");
                ddlAccountType.BindDataTable(SPNames.S3G_ORG_GetEntity_List, Procparam, new string[] { "ID", "Name" });

                Procparam.Clear();
                Procparam.Add("@Option", "3");
                Procparam.Add("@Company_ID", intCompanyId.ToString());
                ddlGLPostingCode.BindDataTable(SPNames.S3G_ORG_GetEntity_List, Procparam, new string[] { "GL_CODE", "Description" });
                Procparam.Clear();
                //}

                tcEntityMaster.ActiveTab = tbEntity;

                // Modified by R. Manikandan dated on sep/24/11
                //Procparam.Add("@Company_id", intCompanyId.ToString());
                //Procparam.Add("@Is_Active", "1");
                //Procparam.Add("@User_Id", intUserId.ToString());
                ////Procparam.Add("@Program_ID", "31");
                ////ddlBranch.BindDataTable("S3G_Get_Branch_List", Procparam, new string[] { "Location_ID", "Location_Code", "Location_Name" });

                //btnSave.Enabled = false;

                //Based on user access Related Party Indicator Enabled/Disabled
                if (UserFunctionCheck("ENT_RPI"))
                {
                    cbRelatedParIndic.Enabled = true;
                }
                else
                {
                    cbRelatedParIndic.Enabled = false;
                }

                if (intEntityId > 0)
                {
                    bool blnTranExists;
                    FunPubProGetEntityDetails(intCompanyId, intEntityId, out blnTranExists);
                    AdditionalInforFetch();

                    if (blnTranExists)
                    {
                        ddlGLPostingCode.Enabled = false;
                        ddlEntityType.Enabled = false;
                    }
                    else
                    {
                        ddlGLPostingCode.Enabled = true;
                        ddlEntityType.Enabled = true;
                    }
                    if (strMode == "M")
                    {
                        FunPriEntityControlStatus(1);
                        revCRNumber.Enabled = RevIdentityColumn.Enabled = RevCRNUMBERVal.Enabled = false;

                    }
                    if (strMode == "Q")
                    {
                        FunPriEntityControlStatus(-1);
                        ucAddressDetailsSetup.ReadOnly(true);
                        ucEntityNamesSetup.ReadOnly(true);
                    }
                }
                else
                {
                    FunPriEntityControlStatus(0);
                    FunPubBindGridDetails();
                }

                ucAddressDetailsSetup.BindAddressSetupDetails(1);
                ucEntityNamesSetup.BindNameSetupDetails();

                //ddlSLPostingCode.Items.Clear();
                //ddlSLPostingCode.Items.Insert(0, "--Select--");
                ddlEntityType.Focus();

                txtEffectiveFrom.Attributes.Add("onChange", "fnDoDate(this,'" + txtEffectiveFrom.ClientID + "','" + strDateFormat + "',false,  true);");
                txtEffectiveTo.Attributes.Add("onChange", "fnDoDate(this,'" + txtEffectiveTo.ClientID + "','" + strDateFormat + "',false,  true);");

                txtColChargesPerMonth.SetDecimalPrefixSuffix(ObjS3GSession.ProGpsPrefixRW, ObjS3GSession.ProGpsSuffixRW, true, "Col Charges Per Month");

                ucAddressDetailsSetup.ValGroup("Add_Address");
                ucEntityNamesSetup.ValGroup("Submit");
            }
        }
        catch (Exception ex)
        {
            cvEntity.IsValid = false;
            cvEntity.ErrorMessage = "Unable to load Entity due to data problem";
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }

    }
    #endregion

    #region Protected Method

    protected void FunSetComboBoxAttributes(AjaxControlToolkit.ComboBox cmb, string Type, string maxLength)
    {
        try
        {
            TextBox textBox = cmb.FindControl("TextBox") as TextBox;

            if (textBox != null)
            {
                textBox.Attributes.Add("onkeyup", "maxlengthfortxt('" + maxLength + "');fnCheckSpecialChars('true');");
                textBox.Attributes.Add("onblur", "funCheckFirstLetterisNumeric(this, '" + Type + "');");
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }

    protected void FunSetComboBoxAttributes(TextBox textBox, string Type, string maxLength)
    {
        try
        {
            if (textBox != null)
            {
                textBox.Attributes.Add("onkeyup", "maxlengthfortxt('" + maxLength + "');fnCheckSpecialChars('true');");
                textBox.Attributes.Add("onblur", "funCheckFirstLetterisNumeric(this, '" + Type + "');");
                textBox.Attributes.Add("onpaste", "return false");
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }

    //<<Performance>>

    protected void FunProLoadAddressCombos()
    {
        try
        {
            Dictionary<string, string> Procparam = new Dictionary<string, string>();
            if (intCompanyId > 0)
            {
                Procparam.Add("@Company_ID", Convert.ToString(intCompanyId));
            }
            DataTable dtAddr = Utility.GetDefaultData("S3G_SYSAD_GetAddressLoodup", Procparam);

            DataTable dtSource = new DataTable();
            //if (dtAddr.Select("Category = 1").Length > 0)
            //{
            //    dtSource = dtAddr.Select("Category = 1").CopyToDataTable();
            //}
            //else
            //{
            //    dtSource = FunProAddAddrColumns(dtSource);
            //}
            //txtCity.FillDataTable(dtSource, "Name", "Name", false);

            dtSource = new DataTable();
            if (dtAddr.Select("Category = 2").Length > 0)
            {
                dtSource = dtAddr.Select("Category = 2").CopyToDataTable();
            }
            else
            {
                dtSource = FunProAddAddrColumns(dtSource);
            }


            dtSource = new DataTable();
            if (dtAddr.Select("Category = 3").Length > 0)
            {
                dtSource = dtAddr.Select("Category = 3").CopyToDataTable();
            }
            else
            {
                dtSource = FunProAddAddrColumns(dtSource);
            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
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
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            return dt;
        }
    }

    #endregion

    #region Page Events
    /// <summary>
    /// Save Button event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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

            if (gvAddressDetails.Rows.Count == 0)
            {
                Utility.FunShowAlertMsg(this, "Add Atleast one address");
                return;
            }

            if (ddlEntityType.SelectedItem.ToString().ToUpper() == "DEALER SALES PERSON")
            {
                if (ddlDealer_Group.SelectedText == "--Select--")
                {
                    Utility.FunShowAlertMsg(this, "Dealer Group is Mandatory");
                    return;
                }
            }

            if (ddlEntityType.SelectedItem.ToString().ToUpper() == "LIFE INSURANCE")
            {
                foreach (GridViewRow grvRow in gvInsurancePolicyDet.Rows)
                {
                    Label lblCompanyRate = (Label)grvRow.FindControl("lblCompanyRate");

                    if (string.IsNullOrEmpty(lblCompanyRate.Text))
                    {
                        Utility.FunShowAlertMsg(this, "Add Atleast one Insurance Policy Details");
                        return;
                    }
                }
            }


            ObjEntityMasterDataTable = new ORG.OrgMasterMgtServices.S3G_ORG_Entity_MasterDataTable();
            ORG.OrgMasterMgtServices.S3G_ORG_Entity_MasterRow ObjEntityRow;
            ObjEntityRow = ObjEntityMasterDataTable.NewS3G_ORG_Entity_MasterRow();
            ObjEntityRow.Company_Id = intCompanyId;
            ObjEntityRow.Entity_Master_ID = intEntityId;
            ObjEntityRow.Entity_Code = txtEntityCode.Text;
            ObjEntityRow.Customer_Type_ID = Convert.ToInt32(ddlCustomerType.SelectedValue);
            ObjEntityRow.Nationality = Convert.ToInt32(ddlNationality.SelectedValue);
            ObjEntityRow.Constitution_ID = Convert.ToInt32(ddlConstitutionName.SelectedValue);
            ObjEntityRow.Entity_Type = Convert.ToInt32(ddlEntityType.SelectedItem.Value);
            ObjEntityRow.Entity_Name = txtEntityName.Text.Trim();
            ObjEntityRow.Entity_Name1 = ucEntityNamesSetup.FirstName.Trim();
            ObjEntityRow.Entity_Name2 = ucEntityNamesSetup.SecondName.Trim();
            ObjEntityRow.Entity_Name3 = ucEntityNamesSetup.ThirdName.Trim();
            ObjEntityRow.Entity_Name4 = ucEntityNamesSetup.FourthName.Trim();
            ObjEntityRow.Entity_Name5 = ucEntityNamesSetup.FifthName.Trim();
            ObjEntityRow.Entity_Name6 = ucEntityNamesSetup.SixthName.Trim();

            ObjEntityRow.GL_Code = ddlGLPostingCode.SelectedItem.Value == "" ? "0" : ddlGLPostingCode.SelectedItem.Value;

            //ObjEntityRow.Dealer_Group = "";

            if (ddlEntityType.SelectedItem.ToString().ToUpper() == "DEALER")
            {
                if (ddlDealer_Group.SelectedText == "")
                    ObjEntityRow.Dealer_Group = "";
                else
                    ObjEntityRow.Dealer_Group = ddlDealer_Group.SelectedValue;
            }
            else if (ddlEntityType.SelectedItem.ToString().ToUpper() == "DEALER SALES PERSON")
            {
                if (ddlDealer.SelectedText == "")
                    ObjEntityRow.Dealer_Group = "";
                else
                    ObjEntityRow.Dealer_Group = ddlDealer.SelectedValue;
            }
            else if (ddlEntityType.SelectedItem.ToString().ToUpper() == "SUPPLIER")
            {
                if (ddlSupplier_Group.SelectedText == "")
                    ObjEntityRow.Dealer_Group = "";
                else
                    ObjEntityRow.Dealer_Group = ddlSupplier_Group.SelectedValue;
            }
            else
            {
                ObjEntityRow.Dealer_Group = "";
            }

            ObjEntityRow.Cust_VAT_TIN = txtVATIN.Text.Trim();
            S3GSession ObjS3GSession = new S3GSession();

            if (txtTaxNumber.Text != string.Empty)
            {
                ObjEntityRow.Tax_Account_Number = txtTaxNumber.Text.Trim();
            }
            ObjEntityRow.VAT_Number = txtVAT.Text.Trim();
            ObjEntityRow.ROC_Number = txtROC.Text.Trim();
            ObjEntityRow.IMPEXP_Code = txtIMPEXP.Text.Trim();
            ObjEntityRow.CR_Number = txtIdentityColumn.Text.Trim();
            ObjEntityRow.Service_Branch = Convert.ToInt32(ddlServiceBranch.SelectedValue);
            ObjEntityRow.Credit_Period_Allowed = txtCreditPeriodAllowed.Text == "" ? 0 : Convert.ToInt32(txtCreditPeriodAllowed.Text.Trim());
            ObjEntityRow.Col_Charges_PM = txtColChargesPerMonth.Text == "" ? 0 : Convert.ToDecimal(txtColChargesPerMonth.Text.Trim());
            //ObjEntityRow.Col_Charges_PM = txtColChargesPerMonth.Text == "" ? 0 : Convert.ToInt32(txtColChargesPerMonth.Text.Trim());
            ObjEntityRow.Related_Party_Indic = cbRelatedParIndic.Checked == true ? 1 : 0;

            ObjEntityRow.Consumer_Dealer = cbConsumerDealer.Checked == true ? 1 : 0;
            ObjEntityRow.Delivery_Loc = Convert.ToInt32(ddlDeliveryLocation.SelectedValue);
            ObjEntityRow.Effective_From = Utility.StringToDate(txtEffectiveFrom.Text);
            ObjEntityRow.Effective_To = Utility.StringToDate(txtEffectiveTo.Text);

            ObjEntityRow.Employee_ID = ddlEmployeeName.SelectedValue == "" ? 0 : Convert.ToInt32(ddlEmployeeName.SelectedValue);

            if (ddlEmployeeRating.SelectedValue == "" || ddlEmployeeRating.SelectedValue == "----Select----")
                ObjEntityRow.Employee_Rating = 0;
            else
                ObjEntityRow.Employee_Rating = Convert.ToInt32(ddlEmployeeRating.SelectedValue);

            ObjEntityRow.Entity_Abbr = txtEntity_Abbr.Text.Trim();
            ObjEntityRow.Is_Active = chkIs_Active.Checked == true ? 1 : 0;
            ObjEntityRow.Cust_VAT_TIN = txtVATIN.Text.Trim();
            //ObjEntityRow.Branch_Code = txtBranch_Code.Text.Trim();
            //ObjEntityRow.Branch_Name = txtBranchName.Text.Trim();

            ObjEntityRow.Branch_Code = "A";
            ObjEntityRow.Branch_Name = "B";


            ObjEntityRow.XMLOperationHistDet = gvOperationHistoryDetails.FunPubFormXml();

            ObjEntityRow.XMLEmployeeHistDet = gvEmployeeHistoryDet.FunPubFormXml();

            ObjEntityRow.XMLInsurancePolicyDet = gvInsurancePolicyDet.FunPubFormXml();
            //ObjEntityRow.XMLInsurancePolicyDet = ((DataTable)ViewState["InsurancePolicyData"]).FunPubFormXml();


            ObjEntityRow.XMLAddressDetails = gvAddressDetails.FunPubFormXml().Replace("nbsp;", "").Replace("&", "");//FunProBindAddressXML();
            ObjEntityRow.XMLEntityTypeDocDet = gvConstitutionalDocuments.FunPubFormXml(true);

            //Additional Parameter Insert Start

            strAdditionalParmDet.Append("<Root>");
            if (grvAdditionalInfo.Rows.Count > 0)
            {
                foreach (GridViewRow grvRow in grvAdditionalInfo.Rows)
                {
                    Label lblPARAM_ID = (Label)grvRow.FindControl("lblPARAM_ID");
                    Label lblPARAM_DET_ID = (Label)grvRow.FindControl("lblPARAM_DET_ID");
                    Label lblCONSTANT_TRAN_ID = (Label)grvRow.FindControl("lblCONSTANT_TRAN_ID");
                    Label lblLookupType = (Label)grvRow.FindControl("lblLookupType");
                    TextBox txtValues = (TextBox)grvRow.FindControl("txtValues");
                    DropDownList ddlValues = (DropDownList)grvRow.FindControl("ddlValues");

                    strAdditionalParmDet.Append("<Details  CONSTANT_TRAN_ID = '" + lblCONSTANT_TRAN_ID.Text + "'");
                    strAdditionalParmDet.Append(" PARAM_ID = '" + lblPARAM_ID.Text + "'");
                    strAdditionalParmDet.Append(" PARAM_DET_ID = '" + lblPARAM_DET_ID.Text + "'");

                    if (!string.IsNullOrEmpty(lblLookupType.Text))
                        strAdditionalParmDet.Append(" VALUES ='" + ddlValues.SelectedValue + "'/>");
                    else
                        strAdditionalParmDet.Append(" VALUES ='" + txtValues.Text + "'/>");
                }
            }

            strAdditionalParmDet.Append("</Root>");
            ObjEntityRow.XMLAdditionalParamDet = strAdditionalParmDet.ToString();

            //Additional Parameter Insert End



            ObjEntityRow.SL_Posting_Code = ddlSLPostingCode.SelectedValue == "--Select--" ? "0" : ddlSLPostingCode.SelectedValue;
            ObjEntityRow.Created_By = intUserId;
            ObjEntityRow.Modified_By = intUserId;

            //Thangam M on 15/Nov/2012 for Trade Advance
            if (chkTradeAdvance.Checked)
            {
                ObjEntityRow.Is_TradeAdvance = 1;
            }
            else
            {
                ObjEntityRow.Is_TradeAdvance = 0;
            }
            //End here

            DataTable dtSubmit = new DataTable();
            dtSubmit = (DataTable)ViewState["DetailsTable"];

            strbBnkDetails.Append("<Root>");
            if (dtSubmit.Rows.Count > 0)
            {
                foreach (DataRow dtBankDetailsRow in dtSubmit.Rows)
                {
                    //strbBnkDetails.Append(" <Details Own_Location_ID='" + dtBankDetailsRow["Own_Location_ID"].ToString() + "'");
                    strbBnkDetails.Append("<Details  Bank_Map_ID = '" + dtBankDetailsRow["BankMapping_ID"].ToString() + "'");
                    strbBnkDetails.Append(" AccountNumber = '" + dtBankDetailsRow["Account_Number"].ToString() + "'");
                    strbBnkDetails.Append(" AccountType = '" + dtBankDetailsRow["AccountType_ID"].ToString() + "'");
                    strbBnkDetails.Append(" BankAddress ='" + dtBankDetailsRow["Bank_Address"].ToString() + "'");
                    strbBnkDetails.Append(" Bank_ID = '" + dtBankDetailsRow["Bank_ID"].ToString() + "'");
                    strbBnkDetails.Append(" BankName = '" + dtBankDetailsRow["Bank_Name"].ToString() + "'");
                    strbBnkDetails.Append(" Branch_ID = '" + dtBankDetailsRow["Branch_ID"].ToString() + "'");
                    strbBnkDetails.Append(" BranchName ='" + dtBankDetailsRow["Branch_Name"].ToString() + "'");
                    strbBnkDetails.Append(" IFSCode =''");
                    strbBnkDetails.Append(" IsDefaultAccount =''");
                    strbBnkDetails.Append(" MICR_Code = '" + dtBankDetailsRow["MICR_Code"].ToString() + "'/>");
                }
            }
            //else
            //{
            //    strAlert = strAlert.Replace("__ALERT__", "Bank details cannot be empty");
            //    strRedirectPageView = "";
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
            //    return;
            //}
            strbBnkDetails.Append("</Root>");
            ObjEntityRow.XMLBank_Details = strbBnkDetails.ToString();



            ObjEntityMasterDataTable.AddS3G_ORG_Entity_MasterRow(ObjEntityRow);

            if (ObjEntityMasterDataTable.Rows.Count > 0)
            {
                //ObjOrgMasterMgtServicesClient = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
                SerializationMode SerMode = SerializationMode.Binary;
                byte[] byteobjS3G_ORG_Entity_DataTable = ClsPubSerialize.Serialize(ObjEntityMasterDataTable, SerMode);

                if (intEntityId > 0)
                {
                    intErrorCode = ObjOrgMasterMgtServicesClient.FunPubModifyEntityInt(SerMode, byteobjS3G_ORG_Entity_DataTable);
                }
                else
                {
                    intErrorCode = ObjOrgMasterMgtServicesClient.FunPubCreateEntityInt(out strEntityCode, SerMode, byteobjS3G_ORG_Entity_DataTable);
                }

                switch (intErrorCode)
                {
                    case 0:
                        //Added by Thangam M on 18/Oct/2012 to avoid double save click
                        btnSave.Enabled_False();
                        //End here

                        txtEntityCode.Text = strEntityCode;
                        if (intEntityId > 0)
                        {
                            strAlert = strAlert.Replace("__ALERT__", "Entity details updated successfully");
                        }
                        else
                        {
                            strAlert = "Entity code " + strEntityCode + " added successfully";
                            strAlert += @"\n\nWould you like to add one more Entity?";
                            strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                            strRedirectPageView = "";
                        }
                        break;
                    case 1:
                        strAlert = strAlert.Replace("__ALERT__", "Service Tax Reg. No. already exists, Enter a new Service Tax Reg. No. Account Number");
                        strRedirectPageView = "";
                        break;
                    case -1:
                        strAlert = strAlert.Replace("__ALERT__", Resources.LocalizationResources.DocNoNotDefined);
                        strRedirectPageView = "";
                        break;
                    case -2:
                        strAlert = strAlert.Replace("__ALERT__", Resources.LocalizationResources.DocNoExceeds);
                        strRedirectPageView = "";
                        break;
                    case -3://Added by Tamilselvan.S on 2/11/2011 for Adding validation for DNC number length
                        strAlert = strAlert.Replace("__ALERT__", Resources.ValidationMsgs._3.ToString());
                        strRedirectPageView = "";
                        break;
                    case -4://Added by Tamilselvan.S on 23/11/2011 for Adding validation for Duplication check
                        strAlert = strAlert.Replace("__ALERT__", "Entity Details Already Exists");
                        strRedirectPageView = "";
                        break;
                    case -5://Added by Tamilselvan.S on 23/11/2011 for Adding validation for Duplication check
                        strAlert = strAlert.Replace("__ALERT__", "SL Code already exists for the selected GL Code");
                        strRedirectPageView = "";
                        break;
                    case -6:
                        strAlert = strAlert.Replace("__ALERT__", "Please enter valid " + lblIdentityColumn.Text + " for selected GL and SL code");
                        strRedirectPageView = "";
                        break;
                    default:
                        if (intEntityId > 0)
                        {
                            strAlert = strAlert.Replace("__ALERT__", "Error in updating entity details");
                        }
                        else
                        {
                            strAlert = strAlert.Replace("__ALERT__", "Error in adding entity details");
                        }
                        strRedirectPageView = "";
                        break;
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
            }
            btnSave.Focus();//Added by Suseela
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

    protected void grvBankDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            //AddConfirmDelete(gv, e); 
            //AddConfirmDelete((GridView)sender,e);
            DataTable dtDelete;
            dtDelete = (DataTable)ViewState["DetailsTable"];
            dtDelete.Rows.RemoveAt(e.RowIndex);
            grvBankDetails.DataSource = dtDelete;
            grvBankDetails.DataBind();
            ViewState["DetailsTable"] = dtDelete;
            FunClearBankDetails();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }

    protected void DeleteBankDetails_clik(object sender, EventArgs e)
    {
        try
        {
            string Confirm = "if(confirm('Are you sure want to remove the record?')){Yes}else {No}";
            if (Confirm.ToLower() == "yes")
            {
                string strFieldAtt = ((LinkButton)sender).ClientID;
                string strVal = strFieldAtt.Substring(strFieldAtt.LastIndexOf("grvBankDetails_")).Replace("grvBankDetails_ctl", "");
                int gRowIndex = Convert.ToInt32(strVal.Substring(0, strVal.LastIndexOf("_")));
                gRowIndex = gRowIndex - 2;

                string strRefno = ((LinkButton)grvBankDetails.Rows[gRowIndex].FindControl("lnkbtnDelete")).Text;

                DataTable dtDelete;
                dtDelete = (DataTable)ViewState["DetailsTable"];
                dtDelete.Rows.RemoveAt(gRowIndex);
                grvBankDetails.DataSource = dtDelete;
                grvBankDetails.DataBind();
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            DataRow drDetails;
            DataTable dtBankDetails = (DataTable)ViewState["DetailsTable"];

            //if (txtMICRCode.Text.Trim().Length != 10)
            //{
            //    Utility.FunShowAlertMsg(this, "MICR Code should be 10 digits");
            //    return;
            //}

            DataRow[] drDefaultBranch = dtBankDetails.Select("Account_Number = '" + txtAccountNumber.Text.Trim() + "'");
            if (drDefaultBranch.Length > 0)
            {
                Utility.FunShowAlertMsg(this, "Account Number must be Unique");
                txtAccountNumber.Focus();
                return;
            }

            // To Check Same Account Number,Bank Name and Branch
            DataRow[] drDefaultCheck = dtBankDetails.Select("Account_Number = '" + txtAccountNumber.Text.Trim() + "' and Bank_ID = '" + txtBankName.SelectedValue + "' and Branch_ID = '" + ddlBankBranch.SelectedValue + "'");
            if (drDefaultCheck.Length > 0)
            {
                Utility.FunShowAlertMsg(this, "Same Combination already exists against Account Number,Branch and Branch Name");
                txtAccountNumber.Focus();
                return;
            }

            if (dtBankDetails.Rows.Count < 10)
            {
                //grvBankDetails
                drDetails = dtBankDetails.NewRow();
                string strEntBankMapId = Convert.ToString(dtBankDetails.Rows.Count + 1);
                drDetails["Entity_BankMapping_ID"] = strEntBankMapId;
                drDetails["BankMapping_ID"] = "-1";
                drDetails["Master_ID"] = "0";
                // drDetails["OwnBranch"] = ddlBranch.SelectedItem.Text;
                //drDetails["Own_Location_ID"] = ddlBranch.SelectedItem.Value;
                drDetails["AccountType"] = ddlAccountType.SelectedItem.Text;
                drDetails["AccountType_ID"] = ddlAccountType.SelectedItem.Value;
                drDetails["Account_Number"] = txtAccountNumber.Text.Trim();
                drDetails["Bank_ID"] = txtBankName.SelectedValue;
                drDetails["Bank_Name"] = txtBankName.SelectedText.Trim();
                drDetails["Branch_ID"] = ddlBankBranch.SelectedValue;
                drDetails["Branch_Name"] = ddlBankBranch.SelectedItem.Text.Trim();
                drDetails["MICR_Code"] = txtMICRCode.Text.ToUpper().Trim();
                drDetails["Bank_Address"] = txtBankAddress.Text.ToUpper().Trim();
                drDetails["IsDefaultAccount"] = 0;//chkDefaultAccount.Checked;
                dtBankDetails.Rows.Add(drDetails);
                grvBankDetails.DataSource = dtBankDetails;
                grvBankDetails.DataBind();
                //wraptext(grvBankDetails, 20);
                ViewState["DetailsTable"] = dtBankDetails;

                FunClearBankDetails();
                ddlAccountType.Focus();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Class", "alert('Cannot add more than 10 Rows');", true);
            }
            btnAdd.Focus();//Added by Suseela
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            //cvEntity.IsValid = false;
            //cvEntity.ErrorMessage = "Unable to add bank details";
        }
    }

    protected void btnNext_Click(object sender, EventArgs e)
    {
        try
        {
            tcEntityMaster.Tabs[1].Enabled = true;
            tcEntityMaster.ActiveTabIndex = 1;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }

    protected void grvBankDetails_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //if (intEntityId > 0)
            //{
            if (ddlAccountType.Visible)
            {

                Label lblEntity_BankMapping_ID = grvBankDetails.SelectedRow.FindControl("lblEntity_BankMapping_ID") as Label;
                Label lblGridBankMappingId = grvBankDetails.SelectedRow.FindControl("lblBankMapping_ID") as Label;
                Label lblGridMasterId = grvBankDetails.SelectedRow.FindControl("lblMaster_ID") as Label;
                Label lblGridAccountTypeId = grvBankDetails.SelectedRow.FindControl("lblGAccountType_ID") as Label;
                Label lblGridAccountType = grvBankDetails.SelectedRow.FindControl("lblGAccountType") as Label;
                Label lblGridAccountNumber = grvBankDetails.SelectedRow.FindControl("lblGAccountNumber") as Label;
                Label lblGridMICR_Code = grvBankDetails.SelectedRow.FindControl("lblGMICR_Code") as Label;

                Label lblGridBank_ID = grvBankDetails.SelectedRow.FindControl("lblGBank_ID") as Label;
                Label lblGridBank_Name = grvBankDetails.SelectedRow.FindControl("lblGBank_Name") as Label;

                Label lblGridBranch_ID = grvBankDetails.SelectedRow.FindControl("lblGBranch_ID") as Label;

                CheckBox chkGridDefaultAccount = grvBankDetails.SelectedRow.FindControl("chkDefaultAccount") as CheckBox;

                //Old Code Start

                //TextBox txtgvBankAddress = (TextBox)grvBankDetails.Rows[grvBankDetails.SelectedRow.RowIndex].FindControl("txtgvBankAddress");
                //Label lbgvlAccountType_ID = (Label)grvBankDetails.Rows[grvBankDetails.SelectedRow.RowIndex].FindControl("lblAccountType_ID");


                //ddlAccountType.SelectedValue = lblAccountTypeId.Text; //grvBankDetails.SelectedRow.Cells[5].Text;
                //txtAccountNumber.Text = Convert.ToString(grvBankDetails.SelectedRow.Cells[6].Text);
                //txtBankName.Text = Convert.ToString(grvBankDetails.SelectedRow.Cells[7].Text);
                //txtBranchName.Text = Convert.ToString(grvBankDetails.SelectedRow.Cells[8].Text);
                //txtMICRCode.Text = Convert.ToString(grvBankDetails.SelectedRow.Cells[9].Text);

                ////txtBankAddress.Text = Convert.ToString(grvBankDetails.SelectedRow.Cells[11].Text);
                //txtBankAddress.Text = txtgvBankAddress.Text;

                //hdnBankId.Value = Convert.ToString(grvBankDetails.SelectedRow.Cells[0].Text);
                ////ddlBranch.SelectedValue = grvBankDetails.SelectedRow.Cells[4].Text;

                //Old Code End


                ddlAccountType.SelectedValue = lblGridAccountTypeId.Text;
                txtAccountNumber.Text = lblGridAccountNumber.Text;
                txtMICRCode.Text = lblGridMICR_Code.Text;
                txtBankName.SelectedValue = lblGridBank_ID.Text.Trim();
                txtBankName.SelectedText = lblGridBank_Name.Text.Trim();

                PopulateBankBranchList(Convert.ToString(lblGridBank_ID.Text.Trim()));
                ddlBankBranch.SelectedValue = lblGridBranch_ID.Text.Trim();
                //hdnBankId.Value = lblGridBankMappingId.Text;
                hdnBankId.Value = lblEntity_BankMapping_ID.Text;



                btnModify.Visible = true;
                btnModify.Enabled_True();
                btnAdd.Enabled_False();
            }

            //}
            //else
            //{
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "NoEdit", "alert('Edit is not allowed in this Mode');", true);
            //    btnAdd.Enabled_True();
            //}
        }
        catch (Exception ex)
        {
            if (ddlAccountType.Visible)
            {
                // ddlBranch.SelectedIndex = 0;
                btnModify.Enabled_True();
                btnAdd.Enabled_False();
            }
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
        finally
        {

        }
    }

    protected void grvBankDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            //AddConfirmDelete((GridView)sender, e);
            //if (e.Row.Cells.Count >= 1)
            //    e.Row.Cells[0].Visible = false;

            //if (e.Row.Cells.Count >= 2)
            //    e.Row.Cells[1].Visible = false;

            //if (e.Row.Cells.Count >= 3)
            //    e.Row.Cells[2].Visible = false;

            //if (e.Row.Cells.Count >= 4)
            //    e.Row.Cells[4].Visible = false;

            //if (e.Row.Cells.Count >= 5)
            //    e.Row.Cells[6].Visible = false;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink
                //(this.grvBankDetails, "Select$" + e.Row.RowIndex);
                //Modified by Tamilselvan.S on 23/11/2011 for Deleting and selecting
                for (int i = 0; i < e.Row.Cells.Count - 1; i++)
                {
                    e.Row.Cells[i].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink
                    (this.grvBankDetails, "Select$" + e.Row.RowIndex);
                }

                Label lblgvContd = (Label)e.Row.FindControl("lblgvContd");
                TextBox txtgvBankAddress = (TextBox)e.Row.FindControl("txtgvBankAddress");

                if (txtgvBankAddress.Text.Length >= 30)
                {
                    lblgvContd.Visible = true;
                }
                else
                {
                    lblgvContd.Visible = false;
                }


                if (strMode == "M")
                {
                    //Added By Thangam M on 13/Feb/2012 to make the address lable as textbox

                    e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);funSetColor('" + txtgvBankAddress.ClientID + "', 1);";
                    e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);funSetColor('" + txtgvBankAddress.ClientID + "', 2);";
                }



                LinkButton LnkBtn = (LinkButton)e.Row.FindControl("lnkbtnDelete");
                LnkBtn.CommandArgument = e.Row.RowIndex.ToString();
                LnkBtn.Attributes.Add("onclick", "return confirm(\"Are you sure want to delete?\")");
            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }

    protected void btnModify_Click(object sender, EventArgs e)
    {
        try
        {

            DataTable dtBankDetails = (DataTable)ViewState["DetailsTable"];

            //if (txtMICRCode.Text.Trim().Length != 10)
            //{
            //    Utility.FunShowAlertMsg(this, "MICR Code should be 10 digits");
            //    return;
            //}

            // DataRow[] drDefaultBranch = dtBankDetails.Select("Account_Number = '" + txtAccountNumber.Text.Trim() + "' and BankMapping_ID <> '" + hdnBankId.Value + "'");
            DataRow[] drDefaultBranch = dtBankDetails.Select("Account_Number = '" + txtAccountNumber.Text.Trim() + "' and Entity_BankMapping_ID <> '" + hdnBankId.Value + "'");
            if (drDefaultBranch.Length > 0)
            {
                Utility.FunShowAlertMsg(this, "Account Number must be Unique");
                txtAccountNumber.Focus();
                return;
            }

            // To Check Same Account Number,Bank Name and Branch
            // DataRow[] drDefaultCheck = dtBankDetails.Select("Account_Number = '" + txtAccountNumber.Text.Trim() + "' and Bank_ID = '" + txtBankName.SelectedValue + "' and Branch_ID = '" + ddlBankBranch.SelectedValue.Trim() + "' and BankMapping_ID <> '" + hdnBankId.Value + "'");
            DataRow[] drDefaultCheck = dtBankDetails.Select("Account_Number = '" + txtAccountNumber.Text.Trim() + "' and Bank_ID = '" + txtBankName.SelectedValue + "' and Branch_ID = '" + ddlBankBranch.SelectedValue.Trim() + "' and Entity_BankMapping_ID <> '" + hdnBankId.Value + "'");
            if (drDefaultCheck.Length > 0)
            {
                Utility.FunShowAlertMsg(this, "Same Combination already exists aginst Account Number,Branch and Branch Name");
                txtAccountNumber.Focus();
                return;
            }




            DataView dvBankdetails = dtBankDetails.DefaultView;
            // dvBankdetails.Sort = "BankMapping_ID";
            dvBankdetails.Sort = "Entity_BankMapping_ID";
            int rowindex = dvBankdetails.Find(hdnBankId.Value);
            //dvBankdetails[rowindex].Row["OwnBranch"] = ddlBranch.SelectedItem.Text;
            //dvBankdetails[rowindex].Row["Own_Location_ID"] = ddlBranch.SelectedItem.Value;
            dvBankdetails[rowindex].Row["AccountType"] = ddlAccountType.SelectedItem.Text;
            dvBankdetails[rowindex].Row["AccountType_ID"] = ddlAccountType.SelectedItem.Value;
            dvBankdetails[rowindex].Row["Account_Number"] = txtAccountNumber.Text.Trim();
            dvBankdetails[rowindex].Row["Bank_ID"] = txtBankName.SelectedValue;
            dvBankdetails[rowindex].Row["Bank_Name"] = txtBankName.SelectedText.Trim();
            dvBankdetails[rowindex].Row["Branch_ID"] = ddlBankBranch.SelectedValue;
            dvBankdetails[rowindex].Row["Branch_Name"] = ddlBankBranch.SelectedItem.Text.Trim();
            dvBankdetails[rowindex].Row["MICR_Code"] = txtMICRCode.Text.ToUpper().Trim();
            dvBankdetails[rowindex].Row["Bank_Address"] = txtBankAddress.Text.ToUpper().Trim();
            dvBankdetails[rowindex].Row["IsDefaultAccount"] = chkDefaultAccount.Checked;
            grvBankDetails.DataSource = dvBankdetails;
            grvBankDetails.DataBind();
            ViewState["DetailsTable"] = dvBankdetails.Table;
            btnAdd.Enabled_True();
            btnModify.Enabled_False();
            //ddlBranch.SelectedIndex = 0;
            ddlAccountType.SelectedIndex = 0;
            txtAccountNumber.Text = txtMICRCode.Text =
            txtBankAddress.Text = "";
            txtBankName.SelectedText = "";
            txtBankName.SelectedValue = "0";
            ddlBankBranch.SelectedIndex = -1;
            //btnSave.Enabled = true;
            btnModify.Focus();//Added by Suseela
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            //cvEntity.IsValid = false;
            //cvEntity.ErrorMessage = "Unable to modify bank details";
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            //txtEntityCode.Text = "";
            ddlEntityType.SelectedIndex = 0;
            ddlGLPostingCode.SelectedIndex = 0;
            txtEntityName.Text = "";

            txtVAT.Text = txtROC.Text = txtIMPEXP.Text = "";
            txtTaxNumber.Text = "";

            tcEntityMaster.ActiveTabIndex = 0;
            grvBankDetails.DataSource = null;
            grvBankDetails.DataBind();
            ViewState["DetailsTable"] = null;
            FunProIntializeData();

            gvAddressDetails.DataSource = null;
            gvAddressDetails.DataBind();
            ViewState["AddressDetails"] = null;
            FunproInitializeAddressData();
            btnAddAddress.Enabled_True();
            btnModifyAddress.Visible = false;

            //ddlBranch.SelectedIndex = 0;
            ddlAccountType.SelectedIndex = 0;
            txtAccountNumber.Text = "";
            txtBankName.SelectedText = "";
            txtBankName.SelectedValue = "0";
            ddlBankBranch.SelectedIndex = -1;
            txtMICRCode.Text = "";
            txtBankAddress.Text = "";

            ddlCustomerType.SelectedIndex = 0;
            txtBranch_Code.Clear();
            txtBranch_Name.Clear();
            ucAddressDetailsSetup.ClearControls();

            pnlEntityNames.Visible = false;


            ddlConstitutionName.SelectedIndex = 0;
            txtEntity_Abbr.Text = "";
            ddlServiceBranch.SelectedIndex = 0;
            ddlDeliveryLocation.SelectedIndex = 0;
            txtIdentityColumn.Clear();
            txtCreditPeriodAllowed.Clear();
            txtColChargesPerMonth.Clear();
            cbRelatedParIndic.Checked = false;
            cbConsumerDealer.Checked = false;
            txtEffectiveFrom.Clear();
            //            txtEffectiveTo.Clear();
            chkIs_Active.Checked = true;

            gvOperationHistoryDetails.DataSource = null;
            gvOperationHistoryDetails.DataBind();
            ViewState["OperationHistoryData"] = null;

            gvEmployeeHistoryDet.DataSource = null;
            gvEmployeeHistoryDet.DataBind();
            ViewState["EmployeeHistoryData"] = null;

            gvInsurancePolicyDet.DataSource = null;
            gvInsurancePolicyDet.DataBind();
            ViewState["InsurancePolicyData"] = null;

            ddlDealer.SelectedText = "";
            ddlDealer.SelectedValue = "0";
            ddlDealer_Group.SelectedText = "";
            ddlDealer_Group.SelectedValue = "";
            ddlSupplier_Group.SelectedText = "";
            ddlSupplier_Group.SelectedValue = "";

            ddlNationality.SelectedText = "";
            ddlNationality.SelectedValue = "0";
            ddlSLPostingCode.ClearSelection();
            //btnClear.Focus();//Added by Suseela

            ddlEmployeeRating.ClearSelection();
            ddlEmployeeName.SelectedValue = "";
            ddlEmployeeName.SelectedText = "";
            pnlEmpHistoryDetails.Visible = false;
            trEmployeedetails.Visible = false;

            pnlBranchDetails.Visible = false;
            lblEntityCode.Text = "Entity Code";
            lblEntityName.Text = "Entity Name";
            ucEntityNamesSetup.ClearControls();
            pnlInsPolicyDetails.Visible = false;
            FunBind_Effective_To();
            hdnAddressId.Value = "";

            foreach (GridViewRow grRow in grvAdditionalInfo.Rows)
            {
                TextBox txtValues = (TextBox)grRow.FindControl("txtValues");
                DropDownList ddlValues = (DropDownList)grRow.FindControl("ddlValues");

                txtValues.Text = "";
                ddlValues.Items.Clear();

            }

            //bind All glcode start

            Procparam.Add("@Option", "3");
            Procparam.Add("@Company_ID", intCompanyId.ToString());
            ddlGLPostingCode.BindDataTable(SPNames.S3G_ORG_GetEntity_List, Procparam, new string[] { "GL_CODE", "Description" });
            ddlGLPostingCode.SelectedIndex = 0;
            //bind All glcode end

            ddlEntityType.Focus();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            //cvEntity.IsValid = false;
            //cvEntity.ErrorMessage = "Unable to clear data";
        }

    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Origination/S3gOrgEntityMaster_View.aspx");
            btnCancel.Focus();//Added by Suseela
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }

    protected void btnBnkClear_Click(object sender, EventArgs e)
    {
        try
        {
            btnAdd.Enabled_True();
            //ddlBranch.SelectedIndex = 0;
            ddlAccountType.SelectedIndex = 0;
            txtAccountNumber.Text = "";
            txtBankName.SelectedText = "";
            txtBankName.SelectedValue = "";
            ddlBankBranch.SelectedIndex = -1;
            txtMICRCode.Text = "";
            txtBankAddress.Text = "";
            hdnBankId.Value = "";
            btnModify.Enabled_False();
            btnBnkClear.Focus();//Added by Suseela
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }
    #endregion

    #region Page Methods
    /// <summary>
    /// Methos to get Entity details
    /// </summary>
    /// <param name="intCompanyId">Company Id to whcich entity belongs</param>
    /// <param name="intEntityId">Entity Id for which dat to be obtained</param>
    /// <param name="blnTranExists">Boolen to check trans exists for Enityt selected</param>
    public void FunPubProGetEntityDetails(int intCompanyId, int intEntityId, out bool blnTranExists)
    {
        ObjOrgMasterMgtServicesClient = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
        blnTranExists = false;
        try
        {
            DataSet dsEntityDetails;
            blnTranExists = false;
            //ObjOrgMasterMgtServicesClient = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
            byte[] byte_EntityDetails = ObjOrgMasterMgtServicesClient.FunPubQueryEntityDetails(out blnTranExists, intCompanyId, intEntityId);
            dsEntityDetails = (DataSet)S3GBusEntity.ClsPubSerialize.DeSerialize(byte_EntityDetails, S3GBusEntity.SerializationMode.Binary, typeof(DataSet));
            if (dsEntityDetails.Tables.Count > 0 && intCompanyId > 0)
            {
                DataTable dtEntityCode = dsEntityDetails.Tables[0];
                txtEntityCode.Text = dtEntityCode.Rows[0]["Entity_Code"].ToString();

                //ListItem lst;
                //if (PageMode == PageModes.Query)
                //{
                //    lst = new ListItem(dtEntityCode.Rows[0]["Entity_Type_Name"].ToString(), dtEntityCode.Rows[0]["Entity_Type"].ToString());
                //    ddlEntityType.Items.Add(lst);

                //    lst = new ListItem(dtEntityCode.Rows[0]["Description"].ToString(), dtEntityCode.Rows[0]["GL_Code"].ToString());
                //    ddlGLPostingCode.Items.Add(lst);
                //}

                ListItem lst;
                if (PageMode != PageModes.Create)
                {
                    lst = new ListItem(dtEntityCode.Rows[0]["ENTITY_TYPE_NAME"].ToString(), dtEntityCode.Rows[0]["ENTITY_TYPE_ID"].ToString());
                    ddlEntityType.Items.Add(lst);
                    ddlEntityType.SelectedIndex = 1;
                }

                //ddlEntityType.SelectedValue = dtEntityCode.Rows[0]["Entity_Type"].ToString();
                ddlCustomerType.SelectedValue = dtEntityCode.Rows[0]["Customer_Type_Id"].ToString();

                ddlNationality.SelectedValue = dtEntityCode.Rows[0]["ID"].ToString();
                ddlNationality.SelectedText = dtEntityCode.Rows[0]["Nationality"].ToString();

                NationalityBind();

                if (!string.IsNullOrEmpty(ddlNationality.SelectedText))
                    ddlNationality.IsMandatory = false;

                ddlEntityType_SelectedIndexChanged(ddlEntityType.SelectedValue, null);

                FunBindConstituition();
                ddlConstitutionName.SelectedValue = dtEntityCode.Rows[0]["Constitution_ID"].ToString();


                // ddlConstitutionName_OnSelectedIndexChanged(ddlConstitutionName.SelectedValue, null); // CR NO Format validation disabled

                ddlGLPostingCode.SelectedValue = dtEntityCode.Rows[0]["GL_Code"].ToString();



                if (ddlGLPostingCode.SelectedValue == "0")
                {
                    //string str = dtEntityCode.Rows[0]["GL_Code"].ToString() + " - " + dtEntityCode.Rows[0]["ACCOUNT_CODE_DESC"].ToString();
                    //ddlGLPostingCode.Items.Insert(1, new ListItem(str, dtEntityCode.Rows[0]["GL_Code"].ToString()));
                    //ddlGLPostingCode.SelectedValue = dtEntityCode.Rows[0]["GL_Code"].ToString();
                }



                ddlGLPostingCode_SelectedIndexChanged(ddlGLPostingCode.SelectedValue, null);

                if (dtEntityCode.Rows[0]["SL_Code"].ToString() != "")
                {

                    if (ddlSLPostingCode.Items.Count == 1)
                    {
                        ddlSLPostingCode.Items.Insert(1, new ListItem(dtEntityCode.Rows[0]["SL_Code"].ToString(), dtEntityCode.Rows[0]["SL_Code"].ToString()));
                        ddlSLPostingCode.SelectedIndex = 1;
                    }
                    else
                    {
                        ddlSLPostingCode.SelectedValue = dtEntityCode.Rows[0]["SL_Code"].ToString();
                    }
                }

                txtEntityName.Text = dtEntityCode.Rows[0]["Entity_Name"].ToString();

                if (ddlCustomerType.SelectedItem.ToString().ToLower() == "individual")
                {
                    ucEntityNamesSetup.BindNameSetupDetails();

                    ucEntityNamesSetup.FirstName = dtEntityCode.Rows[0]["Entity_Name1"].ToString();
                    ucEntityNamesSetup.SecondName = dtEntityCode.Rows[0]["Entity_Name2"].ToString();
                    ucEntityNamesSetup.ThirdName = dtEntityCode.Rows[0]["Entity_Name3"].ToString();
                    ucEntityNamesSetup.FourthName = dtEntityCode.Rows[0]["Entity_Name4"].ToString();
                    ucEntityNamesSetup.FifthName = dtEntityCode.Rows[0]["Entity_Name5"].ToString();
                    ucEntityNamesSetup.SixthName = dtEntityCode.Rows[0]["Entity_Name6"].ToString();
                    pnlEntityNames.Visible = true;
                }
                else
                {
                    pnlEntityNames.Visible = false;
                }

                txtVATIN.Text = dtEntityCode.Rows[0]["Cust_VAT_TIN"].ToString();
                txtTaxNumber.Text = dtEntityCode.Rows[0]["tax_account_Number"].ToString();
                txtVAT.Text = dtEntityCode.Rows[0]["VAT_Number"].ToString();
                txtROC.Text = dtEntityCode.Rows[0]["ROC_Number"].ToString();
                txtIMPEXP.Text = dtEntityCode.Rows[0]["IMPEXP_Code"].ToString();

                txtIdentityColumn.Text = dtEntityCode.Rows[0]["Cr_Number"].ToString();
                if (strMode == "M")
                {
                    if (UserFunctionCheck("CNIRICR_UP"))
                    {
                        txtIdentityColumn.Enabled = true;
                    }
                    else
                    {
                        txtIdentityColumn.ReadOnly = true;
                        RevIdentityColumn.Enabled = RevCRNUMBERVal.Enabled = false;
                    }

                    if (ddlEntityType.SelectedItem.ToString().ToUpper() == "INSURANCE COMPANY")
                    {
                        revCRNumber.Enabled = false;
                    }
                }

                ddlServiceBranch.SelectedValue = dtEntityCode.Rows[0]["Service_Branch_ID"].ToString();
                ddlServiceBranch.SelectedItem.Text = dtEntityCode.Rows[0]["Service_Branch_Text"].ToString();
                txtCreditPeriodAllowed.Text = dtEntityCode.Rows[0]["Credit_Period_Allowed"].ToString();
                txtColChargesPerMonth.Text = dtEntityCode.Rows[0]["Col_Charges_Pm"].ToString();
                cbRelatedParIndic.Checked = Convert.ToBoolean(dtEntityCode.Rows[0]["Related_Party_Indi"]);

                cbConsumerDealer.Checked = Convert.ToBoolean(dtEntityCode.Rows[0]["Consumer_Dealer"]);
                if (dtEntityCode.Rows[0]["DELIVERYLOC_ID"].ToString() != "0")
                {
                    ddlDeliveryLocation.SelectedValue = dtEntityCode.Rows[0]["DELIVERYLOC_ID"].ToString();
                    ddlDeliveryLocation.SelectedItem.Text = dtEntityCode.Rows[0]["DELIVERYLOC"].ToString();
                }

                txtEffectiveFrom.Text = dtEntityCode.Rows[0]["Effective_From"].ToString();
                txtEffectiveTo.Text = dtEntityCode.Rows[0]["Effective_To"].ToString();

                if (ddlEntityType.SelectedItem.ToString().ToUpper() == "DEALER")
                {
                    if (dsEntityDetails.Tables[6] != null && dsEntityDetails.Tables[6].Rows.Count > 0)
                    {
                        ddlDealer_Group.SelectedValue = dsEntityDetails.Tables[6].Rows[0]["ID"].ToString();
                        ddlDealer_Group.SelectedText = dsEntityDetails.Tables[6].Rows[0]["Name"].ToString();
                        divDealerGroup.Visible = true;
                    }
                }
                if (ddlEntityType.SelectedItem.ToString().ToUpper() == "DEALER SALES PERSON")
                {
                    if (dsEntityDetails.Tables[6] != null && dsEntityDetails.Tables[6].Rows.Count > 0)
                    {
                        ddlDealer.SelectedValue = dsEntityDetails.Tables[6].Rows[0]["ID"].ToString();
                        ddlDealer.SelectedText = dsEntityDetails.Tables[6].Rows[0]["Name"].ToString();
                        divDealerName.Visible = true;
                    }
                }
                else if (ddlEntityType.SelectedItem.ToString().ToUpper() == "SUPPLIER")
                {
                    if (dsEntityDetails.Tables[6] != null && dsEntityDetails.Tables[6].Rows.Count > 0)
                    {
                        ddlSupplier_Group.SelectedValue = dsEntityDetails.Tables[6].Rows[0]["ID"].ToString();
                        ddlSupplier_Group.SelectedText = dsEntityDetails.Tables[6].Rows[0]["Name"].ToString();
                        divSupplierGroup.Visible = true;
                    }
                }
                else
                {
                    //txtDealer_Group.Text = dtEntityCode.Rows[0]["Dealer_Group"].ToString();
                    //ddlDealer_Group.Visible = false;
                }

                if (ddlEntityType.SelectedItem.ToString().ToUpper() == "EMPLOYEE")
                {
                    ddlEmployeeName.SelectedValue = dtEntityCode.Rows[0]["Employee_ID"].ToString();
                    ddlEmployeeName.SelectedText = dtEntityCode.Rows[0]["Employee_Name"].ToString();
                    //  txtEmployeeRating.Text = dtEntityCode.Rows[0]["Employee_Rating"].ToString();
                    ddlEmployeeRating.SelectedValue = dtEntityCode.Rows[0]["Employee_Rating"].ToString();

                    if (dsEntityDetails.Tables[4] != null && dsEntityDetails.Tables[4].Rows.Count > 0)
                    {
                        DataTable dtEmpHistDet = dsEntityDetails.Tables[4];
                        ViewState["EmployeeHistoryData"] = dtEmpHistDet;
                        gvEmployeeHistoryDet.DataSource = dtEmpHistDet;
                        gvEmployeeHistoryDet.DataBind();
                    }

                    trEmployeedetails.Visible = true;
                    pnlEmpHistoryDetails.Visible = true;
                }
                else
                {
                    trEmployeedetails.Visible = false;
                    pnlEmpHistoryDetails.Visible = false;
                }

                if (ddlEntityType.SelectedItem.ToString().ToUpper() == "LIFE INSURANCE")
                {
                    if (dsEntityDetails.Tables[5] != null && dsEntityDetails.Tables[5].Rows.Count > 0)
                    {
                        DataTable dtInsuranceCmp = dsEntityDetails.Tables[5];
                        ViewState["InsurancePolicyData"] = dtInsuranceCmp;
                        gvInsurancePolicyDet.DataSource = dtInsuranceCmp;
                        gvInsurancePolicyDet.DataBind();

                        pnlInsPolicyDetails.Visible = true;
                    }


                }
                else
                {
                    pnlInsPolicyDetails.Visible = false;
                }

                txtEntity_Abbr.Text = dtEntityCode.Rows[0]["Entity_Abbr"].ToString();
                chkIs_Active.Checked = Convert.ToBoolean(dtEntityCode.Rows[0]["Is_Active"]);

                hdnID.Value = dtEntityCode.Rows[0]["Created_By"].ToString();

                //Binding Address Details Start
                //ucAddressDetailsSetup.BindAddressSetupDetails(1);

                //if (dsEntityDetails.Tables[2] != null && dsEntityDetails.Tables[2].Rows.Count > 0)
                //{
                //    ucAddressDetailsSetup.SetAddressDetails(dsEntityDetails.Tables[2].Rows[0]);
                //}

                DataTable dtAddressDetails = dsEntityDetails.Tables[2];
                ViewState["AddressDetails"] = dtAddressDetails;
                gvAddressDetails.DataSource = dtAddressDetails;
                gvAddressDetails.DataBind();
                gvAddressDetails.Visible = true;

                if (ddlEntityType.SelectedItem.ToString().ToUpper() == "INSURANCE COMPANY")
                {
                    gvAddressDetails.Columns[1].Visible = true;
                    gvAddressDetails.Columns[2].Visible = true;
                }
                else
                {
                    gvAddressDetails.Columns[1].Visible = false;
                    gvAddressDetails.Columns[2].Visible = false;
                }

                //Binding Address Details End

                DataTable dtBankDetails = dsEntityDetails.Tables[1];
                ViewState["DetailsTable"] = dtBankDetails;
                grvBankDetails.DataSource = dtBankDetails;
                grvBankDetails.DataBind();

                //Binding Operation Grid Details start

                if (dsEntityDetails.Tables[3] != null && dsEntityDetails.Tables[3].Rows.Count > 0)
                {
                    DataTable dtOperationHistDet = dsEntityDetails.Tables[3];
                    ViewState["OperationHistoryData"] = dtOperationHistDet;
                    gvOperationHistoryDetails.DataSource = dtOperationHistDet;
                    gvOperationHistoryDetails.DataBind();
                }
                //Binding Operation Grid Details end

                //Thangam M on 15/Nov/2012 for Trade Advance
                if (dtEntityCode.Rows[0]["Is_TradeAdvance"].ToString() == "1")
                {
                    chkTradeAdvance.Checked = true;
                }
                else
                {
                    chkTradeAdvance.Checked = false;

                }
                // End here

                /* ddlBranch.SelectedValue = grvBankDetails.Rows[0].Cells[4].Text;
                 ddlAccountType.SelectedValue = grvBankDetails.Rows[0].Cells[6].Text;
                 txtAccountNumber.Text = Convert.ToString(grvBankDetails.Rows[0].Cells[7].Text);
                 txtBankName.Text = Convert.ToString(grvBankDetails.Rows[0].Cells[8].Text);
                 txtBranchName.Text = Convert.ToString(grvBankDetails.Rows[0].Cells[9].Text);
                 txtMICRCode.Text = Convert.ToString(grvBankDetails.Rows[0].Cells[10].Text);
                 txtBankAddress.Text = Convert.ToString(grvBankDetails.Rows[0].Cells[11].Text);*/

                if (grvBankDetails.Rows.Count > 0)
                    hdnBankId.Value = Convert.ToString(grvBankDetails.Rows[0].Cells[0].Text);

                //hdnBankId.Value = Convert.ToString(dtBankDetails.Rows[0]["BankMapping_ID"].ToString());
                //hdnEntityId.Value = Convert.ToString(grvBankDetails.Rows[0].Cells[2].Text);
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            //throw new Exception("Unable to load Entity details");
        }
        finally
        {
            ObjOrgMasterMgtServicesClient.Close();

        }
    }
    /// <summary>
    /// Method to initialize then data datatable
    /// </summary>
    protected void FunProIntializeData()
    {
        try
        {
            //Bank Details Start
            DataTable dtBankViewDetails;
            dtBankViewDetails = new DataTable("BankDetails");
            //dtBankViewDetails.Columns.Add("OwnBranch");
            //dtBankViewDetails.Columns.Add("Own_Location_ID");

            dtBankViewDetails.Columns.Add("Entity_BankMapping_ID");
            dtBankViewDetails.Columns.Add("AccountType");
            dtBankViewDetails.Columns.Add("AccountType_ID");
            dtBankViewDetails.Columns.Add("Account_Number");
            dtBankViewDetails.Columns.Add("Bank_ID");
            dtBankViewDetails.Columns.Add("Bank_Name");
            dtBankViewDetails.Columns.Add("Branch_ID");
            dtBankViewDetails.Columns.Add("Branch_Name");
            dtBankViewDetails.Columns.Add("MICR_Code");
            dtBankViewDetails.Columns.Add("Bank_Address");
            dtBankViewDetails.Columns.Add("BankMapping_ID");
            dtBankViewDetails.Columns.Add("Master_ID");
            dtBankViewDetails.Columns.Add("IsDefaultAccount", typeof(bool));
            //dtBankViewDetails.Columns[2].Unique = true;
            //dtBankViewDetails.Columns[4].Unique = true;
            //dtBankViewDetails.Columns[7].Unique = true;
            ViewState["DetailsTable"] = dtBankViewDetails;
            //Bank Details End

            //Additional Parameter Start

            DataTable dtConstParamDetails;
            dtConstParamDetails = new DataTable("ConstantParamDetails");
            dtConstParamDetails.Columns.Add("PARAM_ID");
            dtConstParamDetails.Columns.Add("PARAM_DET_ID");
            ViewState["ConstParamDetails"] = dtConstParamDetails;

            //Additional Parameter End
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            //throw new Exception("Unable to Intialize data");
        }
    }

    protected void FunproInitializeAddressData()
    {
        try
        {
            DataTable dtAddressDetails;
            dtAddressDetails = new DataTable("AddressDetails");
            dtAddressDetails.Columns.Add("Entity_Add_Mapping_ID");
            dtAddressDetails.Columns.Add("Address_ID");
            dtAddressDetails.Columns.Add("Branch_Code");
            dtAddressDetails.Columns.Add("Branch_Name");

            dtAddressDetails.Columns.Add("Postal_Code_Value");
            dtAddressDetails.Columns.Add("Postal_Code_Text");
            dtAddressDetails.Columns.Add("Post_Box_No");
            dtAddressDetails.Columns.Add("Way_No");
            dtAddressDetails.Columns.Add("House_No");
            dtAddressDetails.Columns.Add("Block_No");
            dtAddressDetails.Columns.Add("Flat_No");
            dtAddressDetails.Columns.Add("Landmark");
            dtAddressDetails.Columns.Add("Area_Sheik");
            dtAddressDetails.Columns.Add("Residence_Phone_No");
            dtAddressDetails.Columns.Add("Residence_Fax_No");
            dtAddressDetails.Columns.Add("Mobile_No");
            dtAddressDetails.Columns.Add("Contact_Name");
            dtAddressDetails.Columns.Add("Contact_No");
            dtAddressDetails.Columns.Add("Office_Phone_No");
            dtAddressDetails.Columns.Add("Office_Fax_No");
            dtAddressDetails.Columns.Add("Email");
            ViewState["AddressDetails"] = dtAddressDetails;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            //throw new Exception("Unable to Intialize Address data");
        }
    }
    protected void FunBindConstituition()
    {
        try
        {
            Dictionary<string, string> Procparam = new Dictionary<string, string>();
            Procparam.Add("@Option", "2");
            Procparam.Add("@Param1", Convert.ToString(intCompanyId));
            Procparam.Add("@Param3", Convert.ToString(ddlCustomerType.SelectedValue));

            ddlConstitutionName.BindDataTable("S3G_ORG_GetCustomerLookUp", Procparam, true, "--Select--", new string[] { "Constitution_ID", "ConstitutionName" });

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }

    }
    private void FunPriEntityControlStatus(int intModeID)
    {
        try
        {
            switch (intModeID)
            {
                case 0: // Create Mode

                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);
                    btnModify.Visible = false;
                    btnModifyAddress.Visible = false;
                    btnAdd.Visible = true;
                    // txtEntityCode.Enabled = true;
                    txtEntityName.Enabled = true;
                    btnClear.Enabled_True();
                    chkIs_Active.Checked = true;
                    chkIs_Active.Enabled = false;
                    if (!bCreate)
                    {
                        btnSave.Enabled_False();
                    }
                    break;

                case 1: //Modify


                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                    tcEntityMaster.Tabs[1].Enabled = true;
                    txtEntityCode.Enabled = false;

                    if (ddlCustomerType.SelectedItem.ToString().ToLower() == "individual")
                        txtEntityName.Enabled = false;
                    else
                        txtEntityName.Enabled = true;

                    //txtCountry.AutoPostBack = true;

                    //Changed By Thangam M on 27/Feb/2012
                    //txtEntityName.Enabled = false;
                    //End here

                    // btnModify.Visible = true;
                    btnModify.Enabled_False();  //modified as per UAT

                    btnClear.Enabled_False();
                    //btnSave.Enabled = true;
                    // btnAdd.Enabled = false;
                    //btnModify.Enabled = false;

                    ddlCustomerType.Enabled = false;
                    ddlNationality.Enabled = false;
                    ddlGLPostingCode.Enabled = false;
                    //ddlConstitutionName.Enabled = false;
                    ddlEntityType.Enabled = false;
                    //txtIdentityColumn.Enabled = false;
                    txtEffectiveFrom.Enabled = false;
                    //ddlDealer_Group.Enabled = false;
                    ddlSupplier_Group.Enabled = false;
                    ddlServiceBranch.Enabled = false;

                    if (!bModify)
                    {
                        btnSave.Enabled_False();
                        btnModify.Enabled_False();
                    }

                    break;
                case -1://Query

                    if (!bQuery)
                    {
                        Response.Redirect(strRedirectPage, false);
                    }
                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                    tcEntityMaster.Tabs[1].Enabled = true;
                    // txtEntityCode.ReadOnly = true;
                    txtEntityName.ReadOnly = true;
                    btnModify.Visible = false;
                    btnModifyAddress.Visible = false;
                    btnClear.Enabled_False();
                    btnSave.Enabled_False();
                    pnlBankDetails.Visible = false;

                    txtEntityName.ReadOnly = true;
                    txtTaxNumber.ReadOnly = true;

                    txtAccountNumber.ReadOnly = true;
                    txtBankName.Enabled = false;
                    ddlBankBranch.Enabled = false;
                    txtMICRCode.ReadOnly = true;
                    txtBankAddress.ReadOnly = true;

                    //Added By Thangam M on 14/Feb/2012 to fix VAT Number non editable in query mode
                    txtVAT.ReadOnly = txtROC.ReadOnly = txtIMPEXP.ReadOnly = true;

                    if (bClearList)
                    {
                        ddlEntityType.ClearDropDownList();
                        ddlGLPostingCode.ClearDropDownList();
                        //ddlAccountType.ClearDropDownList();
                        // ddlBranch.ClearDropDownList();
                    }
                    //grvBankDetails.Enabled = false;
                    grvBankDetails.Columns[grvBankDetails.Columns.Count - 1].Visible = false;
                    btnAdd.Enabled_False();

                    //Thangam M on 15/Nov/2012 for Trade Finance
                    chkTradeAdvance.Enabled = false;
                    //End here

                    ddlCustomerType.Enabled = false;
                    ddlNationality.Enabled = false;
                    ddlConstitutionName.Enabled = false;
                    ddlDealer.Enabled = false;
                    ddlDealer_Group.Enabled = false;
                    ddlSupplier_Group.Enabled = false;

                    txtEntity_Abbr.ReadOnly = true;
                    ddlServiceBranch.Enabled = false;
                    ddlDeliveryLocation.Enabled = false;
                    txtCreditPeriodAllowed.ReadOnly = true;
                    txtColChargesPerMonth.ReadOnly = true;
                    cbRelatedParIndic.Enabled = false;
                    cbConsumerDealer.Enabled = false;
                    chkIs_Active.Enabled = false;
                    ddlEntityType.Enabled = false;
                    ddlGLPostingCode.Enabled = false;
                    ddlSLPostingCode.Enabled = false;
                    btnAddAddress.Enabled_False();
                    txtEffectiveFrom.Enabled = false;
                    txtEffectiveTo.Enabled = false;
                    txtIdentityColumn.Enabled = false;
                    RevCRNUMBERVal.Enabled = false;
                    RevIdentityColumn.Enabled = false;
                    ddlEmployeeName.Enabled = false;
                    ddlEmployeeRating.Enabled = false;
                    gvEmployeeHistoryDet.Enabled = false;
                    gvInsurancePolicyDet.Enabled = false;
                    txtBranch_Code.Enabled = false;
                    txtBranch_Name.Enabled = false;
                    grvAdditionalInfo.Enabled = false;

                    rfvIdentityColumn.Enabled = false;
                    RevIdentityColumn.Enabled = false;
                    RevCRNUMBERVal.Enabled = false;

                    break;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }

    private void wraptext(GridView dtWrap, int intwraplength)
    {
        try
        {
            int intcolcount = 0;
            foreach (GridViewRow grvRow in dtWrap.Rows)
            {
                for (intcolcount = 0; intcolcount < grvRow.Cells.Count; intcolcount++)
                {
                    if (grvRow.Cells[intcolcount].Text.Length > intwraplength)
                    {
                        int intLoopcount = grvRow.Cells[intcolcount].Text.Length;
                        StringBuilder str = new StringBuilder();
                        StringBuilder str_copy = new StringBuilder();
                        str_copy = str;
                        str.Append(grvRow.Cells[intcolcount].Text);

                        for (int intLenCount = intwraplength; intLenCount < intLoopcount; intLenCount += intwraplength)
                        {

                            str_copy = str.Insert(intLenCount, "\r\n");

                        }
                        grvRow.Cells[intcolcount].Text = str_copy.ToString();
                    }
                }
                intcolcount++;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }

    protected void FunClearBankDetails()
    {
        try
        {
            //ddlBranch.SelectedIndex = 0;
            ddlAccountType.SelectedIndex = 0;
            txtAccountNumber.Text = "";
            txtBankName.Clear();
            ddlBankBranch.SelectedIndex = -1;
            txtMICRCode.Text = "";
            txtBankAddress.Text = "";
            chkDefaultAccount.Checked = false;
            btnAdd.Enabled_True();
            btnModify.Enabled_False();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }
    protected void FunClearAddressDetails()
    {
        txtBranch_Code.Clear();
        txtBranch_Name.Clear();
        ucAddressDetailsSetup.ClearControls();
        btnAdd.Enabled_True();
        //btnModify.Enabled = false;
    }

    private void FunChangePANFormat(string strCountry)
    {
        try
        {
            txtTaxNumber.Attributes.Add("maxlength", "10");

            //Added by Thangam M on 24/Oct/2011 Based on OBS7-4 by Mr.S.Sudarsan

            revtxtPanNumber.Enabled = false;

            //End here

            if (strCountry.ToLower() != "india")
            {
                //mskexPanNumber.Enabled = false;
                revtxtPanNumber.ValidationExpression = @"^[a-zA-Z_0-9](\w|\W)*";
                revtxtPanNumber.ErrorMessage = "Enter a valid Income tax number";
                revtxtPanNumber_Submit.ValidationExpression = @"^[a-zA-Z_0-9](\w|\W)*";
                revtxtPanNumber_Submit.ErrorMessage = "Enter a valid Income tax number";
                ftexMICRCode.FilterType = AjaxControlToolkit.FilterTypes.Numbers | AjaxControlToolkit.FilterTypes.LowercaseLetters | AjaxControlToolkit.FilterTypes.UppercaseLetters;
                FTBEtaxnumber.FilterType = AjaxControlToolkit.FilterTypes.Numbers | AjaxControlToolkit.FilterTypes.LowercaseLetters | AjaxControlToolkit.FilterTypes.UppercaseLetters;
                txtMICRCode.MaxLength = 10;
            }
            else
            {
                //mskexPanNumber.Enabled = true;
                revtxtPanNumber.ValidationExpression = @"^([a-zA-Z]){5}([0-9]){4}([a-zA-Z]){1}$";
                revtxtPanNumber.ErrorMessage = "Service Tax Regn Number should be of format AAAAA9999A";
                revtxtPanNumber_Submit.ValidationExpression = @"^([a-zA-Z]){5}([0-9]){4}([a-zA-Z]){1}$";
                revtxtPanNumber_Submit.ErrorMessage = "Service Tax Regn Number should be of format AAAAA9999A";
                ftexMICRCode.FilterType = AjaxControlToolkit.FilterTypes.Numbers;
                FTBEtaxnumber.FilterType = AjaxControlToolkit.FilterTypes.Numbers | AjaxControlToolkit.FilterTypes.LowercaseLetters | AjaxControlToolkit.FilterTypes.UppercaseLetters;
                txtMICRCode.MaxLength = 9;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }

    public string FunWrapText(string strWarptext, int intWraplength)
    {
        string strWarppedtext = string.Empty;

        try
        {
            if (strWarptext.Length > 0)
            {
                int intcharlength = 1;
                foreach (char chr in strWarptext)
                {

                    if ((intcharlength % intWraplength) == 0)
                    {
                        if (intcharlength > 0)
                            strWarppedtext += chr.ToString() + System.Environment.NewLine;
                        else
                            strWarppedtext += chr.ToString();
                    }
                    else
                    {
                        strWarppedtext += chr.ToString();
                    }


                    intcharlength++;
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }

        return strWarppedtext;
    }
    #endregion

    #region DropDown Changed Events

    protected void ddlCustomerType_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlCustomerType.Focus();
            ucEntityNamesSetup.ClearControls();
            grvBankDetails.DataSource = null;
            grvBankDetails.DataBind();

            FunProIntializeData();
            //End
            ddlNationality.SelectedValue = "";
            ddlNationality.SelectedText = "";

            if (ddlCustomerType.SelectedIndex != 0)
            {
                FunBindConstituition();
            }
            else
            {
                ddlConstitutionName.Items.Clear();
                ddlConstitutionName.Items.Insert(0, "----Select----");
            }

            if (ddlCustomerType.SelectedItem.ToString().ToLower() == "individual")
            {
                pnlEntityNames.Visible = true;
                txtEntityName.Clear();
                txtEntityName.Enabled = false;

                ddlConstitutionName.Items.FindByText("1-INDIVIDUALS").Selected = true;

                ucEntityNamesSetup.BindNameSetupDetails();

                //ddlConstitutionName_OnSelectedIndexChanged(sender, e); // CR NO Format validation disabled

                //txtCustomerName.ReadOnly = false;
                //rvfCustomerName.Enabled = true;
                //pnlCustdtls.Visible = false;

                //IndReqFieldValidate(false);
                //NonIndReqFieldValidate(true);
                //funPriLoadNonIndividualLoad();
            }
            else
            {
                pnlEntityNames.Visible = false;
                txtEntityName.Clear();
                txtEntityName.Enabled = true;
                //txtCustomerName.ReadOnly = true;
                //rvfCustomerName.Enabled = false;
                //pnlCustdtls.Visible = true;
                //IndReqFieldValidate(true);
                //NonIndReqFieldValidate(false);
                //funPriLoadIndividualLoad();
            }
            //upAddress.Update();
            //upPersonal.Update();

            //ddlCustomerType.Focus();

            ddlNationality.Control_ID = ddlNationality.ClientID.ToString();
            TextBox txtItemName = ((TextBox)ddlNationality.FindControl("txtItemName"));
            txtItemName.Focus();
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            //cvEntity.ErrorMessage = Resources.LocalizationResources.CustomerTypeChangeError;
            //cvEntity.IsValid = false;
        }
    }

    protected void ddlConstitutionName_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        //OrgMasterMgtServicesReference.OrgMasterMgtServicesClient objCustomerMasterClient = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
        //try
        //{
        //    if (Convert.ToInt16(ddlConstitutionName.SelectedValue) > 0)
        //    {
        //        S3GBusEntity.S3G_Status_Parameters ObjStatus = new S3G_Status_Parameters();
        //        ObjStatus.Option = 7;
        //        ObjStatus.Param1 = ddlConstitutionName.SelectedValue.ToString();
        //        gvConstitutionalDocuments.DataSource = objCustomerMasterClient.FunPub_GetS3GStatusLookUp(ObjStatus);
        //        gvConstitutionalDocuments.DataBind();

        //        //Added By Thangam on 12/Mar/2012 to fix bug id - 5451
        //        FunProClearUploader();

        //        ViewState["ConstitutionDocument"] = objCustomerMasterClient.FunPub_GetS3GStatusLookUp(ObjStatus);
        //        ddlConstitutionName.Focus();
        //        ObjStatus.Option = 787;
        //        ObjStatus.Param1 = intCompanyId.ToString();
        //        DataTable dtConsDocPath = objCustomerMasterClient.FunPub_GetS3GStatusLookUp(ObjStatus);
        //        if (dtConsDocPath == null)
        //        {
        //            Utility.FunShowAlertMsg(this, "Define the spooling path in Document Path Setup for Consitution Document");
        //            return;
        //        }
        //        if (dtConsDocPath.Rows.Count == 0)
        //        {
        //            Utility.FunShowAlertMsg(this, "Define the spooling path in Document Path Setup for Consitution Document");
        //            return;
        //        }
        //        ViewState["ConsDocPath"] = dtConsDocPath.Rows[0][0].ToString();
        //    }
        //    else
        //    {
        //        tcEntityMaster.Tabs[3].Enabled = false;
        //    }
        //    ddlConstitutionName.Focus();//Added by Suseela
        //}
        //catch (Exception objException)
        //{
        //    ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
        //    cvEntity.ErrorMessage = Resources.LocalizationResources.ConstitutionChangeError;
        //    cvEntity.IsValid = false;
        //}
        //finally
        //{
        //    objCustomerMasterClient.Close();
        //    upConstitution.Update();
        //}
        try
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Const_ID", ddlConstitutionName.SelectedValue);
            Procparam.Add("@company_id", intCompanyId.ToString());
            DataSet dsCR = Utility.GetDataset("S3G_CR_Number_Exp", Procparam);
            if (dsCR.Tables[0].Rows[0]["Return_String"].ToString() != "" && dsCR.Tables[1].Rows[0][0].ToString() != "")
            {
                revCRNumber.Enabled = true;
                revCRNumber.ValidationExpression = dsCR.Tables[0].Rows[0]["Return_String"].ToString();
                revCRNumber.ErrorMessage = lblIdentityColumn.Text + " should be of the format  " + dsCR.Tables[1].Rows[0][0].ToString();
            }
            else
            {
                revCRNumber.Enabled = false;
                revCRNumber.ValidationExpression = "";
                revCRNumber.ErrorMessage = "";
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }

    protected void ddlNationality_Item_Selected(object Sender, EventArgs e)
    {
        try
        {
            //if (ddlCustomerType.SelectedItem.Text.Trim().ToUpper() == "INDIVIDUAL")
            //{
            //    if (ddlNationality.SelectedValue == "503")//FOR SULTANATE OF OMAN
            //    {
            //        lblIdentityColumn.Text = "N.ID.";
            //        lblIdentityColumn.ToolTip = "N.ID.";
            //        txtIdentityColumn.ToolTip = "N.ID.";
            //    }
            //    else//Non Oman
            //    {
            //        lblIdentityColumn.Text = "Passport Number";
            //        lblIdentityColumn.ToolTip = "Passport Number";
            //        txtIdentityColumn.ToolTip = "Passport Number";
            //    }
            //}
            //else
            //{
            //    lblIdentityColumn.Text = "CR No.";
            //    lblIdentityColumn.ToolTip = "CR No.";
            //    txtIdentityColumn.ToolTip = "CR No.";

            //}

            NationalityBind();
            //            ddlConstitutionName.SelectedIndex = -1;

            //ddlNationality.Focus();
            txtIdentityColumn.Clear();
            ddlConstitutionName.Focus();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName); ;
        }

    }

    private void NationalityBind()
    {
        try
        {
            if (ddlCustomerType.SelectedItem.Text.Trim().ToUpper() == "INDIVIDUAL")
            {
                string[] strCustomerDetails = ddlNationality.SelectedText.Trim().Split('-');
                if (strCustomerDetails[0].Trim() == "100")//Oman//ddlNationality.SelectedText.Trim().ToUpper().ToUpper().Contains("OMAN") || Bahrain,Kuwait,Qatar,ARABIA
                {
                    lblIdentityColumn.Text = "N.ID. Number";
                    lblIdentityColumn.ToolTip = "N.ID. Number";
                    txtIdentityColumn.ToolTip = "N.ID. Number";
                    rfvIdentityColumn.ErrorMessage = "Enter N.ID. Number";
                    txtIdentityColumn.MaxLength = 12;
                    RevIdentityColumn.Enabled = true;
                    RevCRNUMBERVal.Enabled = false;

                }
                else if (strCustomerDetails[0].Trim() == "419" || strCustomerDetails[0].Trim() == "443"
                    || strCustomerDetails[0].Trim() == "453" || strCustomerDetails[0].Trim() == "456")//Oman//ddlNationality.SelectedText.Trim().ToUpper().ToUpper().Contains("OMAN") || Bahrain,Kuwait,Qatar,ARABIA
                {
                    lblIdentityColumn.Text = "NRID. Number";
                    lblIdentityColumn.ToolTip = "NRID. Number";
                    txtIdentityColumn.ToolTip = "NRID. Number";
                    rfvIdentityColumn.ErrorMessage = "Enter NRID. Number";
                    txtIdentityColumn.MaxLength = 12;
                    RevIdentityColumn.Enabled = true;
                    RevCRNUMBERVal.Enabled = false;

                }
                else//Non Oman
                {
                    lblIdentityColumn.Text = "RID Number";
                    lblIdentityColumn.ToolTip = "RID Number";
                    txtIdentityColumn.ToolTip = "RID Number";
                    rfvIdentityColumn.ErrorMessage = "Enter RID Number";
                    txtIdentityColumn.MaxLength = 12;
                    RevIdentityColumn.Enabled = true;
                    RevCRNUMBERVal.Enabled = false;
                }
            }
            else
            {
                lblIdentityColumn.Text = "CR No.";
                lblIdentityColumn.ToolTip = "CR No.";
                txtIdentityColumn.ToolTip = "CR No.";
                rfvIdentityColumn.ErrorMessage = "Enter CR No.";
                revCRNumber.ErrorMessage = "Enter CR No.";
                txtIdentityColumn.MaxLength = 11;
                RevIdentityColumn.Enabled = false;
                RevCRNUMBERVal.Enabled = true;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }

    protected void ddlEntityType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlDealer.Clear();
            ddlDealer_Group.Clear();
            ddlSupplier_Group.Clear();

            divDealerName.Visible = false;
            divDealerGroup.Visible = false;
            divSupplierGroup.Visible = false;

            ddlDeliveryLocation.SelectedIndex = -1;
            pnlBranchDetails.Visible = false;

            cbConsumerDealer.Checked = false;
            cbConsumerDealer.Enabled = false;
            ddlDeliveryLocation.Enabled = false;

            rfvGlCode.Enabled = true;
            lblGLPostingCode.CssClass = "styleReqFieldLabel";

            lblEntityCode.Text = "Entity Code";
            lblEntityName.Text = "Entity Name";
            lblDealer_Group.Text = "Dealer Group";

            if (ddlEntityType.SelectedItem.ToString().ToUpper() == "EMPLOYEE")
            {
                //Binding Employee History Grid Start

                DataRow drEmployeeHist;
                dtEmployeeHisDet.Columns.Add("SNo");
                dtEmployeeHisDet.Columns.Add("FromDate");
                dtEmployeeHisDet.Columns.Add("ToDate");
                dtEmployeeHisDet.Columns.Add("Employee_Hist_ID");
                dtEmployeeHisDet.Columns.Add("RowStatus");

                drEmployeeHist = dtEmployeeHisDet.NewRow();
                drEmployeeHist["SNo"] = 0;
                dtEmployeeHisDet.Rows.Add(drEmployeeHist);

                ViewState["EmployeeHistoryData"] = dtEmployeeHisDet;
                gvEmployeeHistoryDet.DataSource = dtEmployeeHisDet;
                gvEmployeeHistoryDet.DataBind();
                gvEmployeeHistoryDet.Rows[0].Visible = false;
                ViewState["EmployeeHistoryData"] = dtEmployeeHisDet;
                //Binding Employee History Grid End

                trEmployeedetails.Visible = true;
                pnlEmpHistoryDetails.Visible = true;

                FunBindEmployerRating();

                lblEntityCode.Text = "Employee Code";
                lblEntityName.Text = "Employee Name";
            }
            else
            {
                trEmployeedetails.Visible = false;
                pnlEmpHistoryDetails.Visible = false;
            }
            if (ddlEntityType.SelectedItem.ToString().ToUpper() == "LIFE INSURANCE")
            {
                //Binding Insurance Policy Details Grid Start
                DataRow drInsurancePolicy;
                dtInsurancePolicyDet.Columns.Add("SNO");
                dtInsurancePolicyDet.Columns.Add("COMPANYRATE");
                dtInsurancePolicyDet.Columns.Add("CUSTOMERRATE");
                dtInsurancePolicyDet.Columns.Add("POLICYRATEID");
                dtInsurancePolicyDet.Columns.Add("FROMDATE");
                dtInsurancePolicyDet.Columns.Add("TODATE");
                dtInsurancePolicyDet.Columns.Add("ROWSTATUS");

                drInsurancePolicy = dtInsurancePolicyDet.NewRow();
                drInsurancePolicy["SNo"] = 0;
                dtInsurancePolicyDet.Rows.Add(drInsurancePolicy);

                ViewState["InsurancePolicyData"] = dtInsurancePolicyDet;
                gvInsurancePolicyDet.DataSource = dtInsurancePolicyDet;
                gvInsurancePolicyDet.DataBind();
                gvInsurancePolicyDet.Rows[0].Visible = false;
                ViewState["InsurancePolicyData"] = dtInsurancePolicyDet;
                //Binding Insurance Policy Details Grid  End


                pnlInsPolicyDetails.Visible = true;

                lblEntityCode.Text = "Life Insurance Code";
                lblEntityName.Text = "Life Insurance Name";
            }
            else
            {
                pnlInsPolicyDetails.Visible = false;
            }

            if (ddlEntityType.SelectedItem.ToString().ToUpper() == "DEALER SALES PERSON")
            {
                lblDealer_Group.Text = "Dealer Name";
                divDealerName.Visible = true;

                lblEntityCode.Text = "Dealer Person Code";
                lblEntityName.Text = "Dealer Person Name";
            }
            else
            {
                ddlDealer_Group.Visible = false;
            }
            if (ddlEntityType.SelectedItem.ToString().ToUpper() == "SUPPLIER")
            {
                lblDealer_Group.Text = "Supplier Group";
                divSupplierGroup.Visible = true;

                cbConsumerDealer.Enabled = true;
                ddlDeliveryLocation.Enabled = true;

                lblEntityCode.Text = "Supplier Code";
                lblEntityName.Text = "Supplier Name";
            }
            if (ddlEntityType.SelectedItem.ToString().ToUpper() == "DEALER")
            {
                lblDealer_Group.Text = "Dealer Group";
                divDealerGroup.Visible = true;
                ddlDealer_Group.Visible = true;

                cbConsumerDealer.Enabled = true;
                ddlDeliveryLocation.Enabled = true;

                lblEntityCode.Text = "Dealer Code";
                lblEntityName.Text = "Dealer Name";
            }
            if (ddlEntityType.SelectedItem.ToString().ToUpper() == "INSURANCE COMPANY")
            {
                lblEntityCode.Text = "Insurance Company Code";
                lblEntityName.Text = "Insurance Company Name";

                rfvtxtBranch_Code.Enabled = true;
                rfvtxtBranch_Name.Enabled = true;
                pnlBranchDetails.Visible = true;
            }
            if (ddlEntityType.SelectedItem.ToString().ToUpper() == "DEALER GROUP")
            {
                lblEntityCode.Text = "Dealer Group Code";
                lblEntityName.Text = "Dealer Group Name";

                lblGLPostingCode.CssClass = "";
                rfvGlCode.Enabled = false;
            }
            if (ddlEntityType.SelectedItem.ToString().ToUpper() == "SUPPLIER GROUP")
            {
                lblEntityCode.Text = "Supplier Group Code";
                lblEntityName.Text = "Supplier Group Name";

                lblGLPostingCode.CssClass = "";
                rfvGlCode.Enabled = false;
            }

            // FunBindEntityDocs();  Const Docs no need
            FunBindAdditionalParamDet();
            //ddlEntityType.Focus();
            ddlCustomerType.Focus();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }

    }

    protected void ddlServiceBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlGLPostingCode.Items.Clear();
            if (ddlServiceBranch.SelectedValue != "0")
            {
                Procparam.Clear();
                Procparam.Add("@Option", "3");
                Procparam.Add("@Company_ID", intCompanyId.ToString());
                Procparam.Add("@Location_ID", ddlServiceBranch.SelectedValue);
                ddlGLPostingCode.BindDataTable(SPNames.S3G_ORG_GetEntity_List, Procparam, new string[] { "GL_CODE", "Description" });
            }
            else
            {
                Procparam.Clear();
                Procparam.Add("@Option", "3");
                Procparam.Add("@Company_ID", intCompanyId.ToString());
                ddlGLPostingCode.BindDataTable(SPNames.S3G_ORG_GetEntity_List, Procparam, new string[] { "GL_CODE", "Description" });
            }

            ddlGLPostingCode.Focus();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }

    #endregion

    #region Private Methods
    private void FunPriClearUploader(AjaxControlToolkit.AsyncFileUpload Uploader)
    {
        try
        {
            if (Uploader != null)
            {
                HttpContext crntContext;
                if (HttpContext.Current != null && HttpContext.Current.Session != null)
                {
                    crntContext = HttpContext.Current;
                }
                else
                {
                    crntContext = null;
                }
                if (crntContext != null)
                {
                    foreach (string key in crntContext.Session.Keys)
                    {
                        if (key.Contains(Uploader.ClientID))
                        {
                            crntContext.Session.Remove(key);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }
    protected void FunProClearUploader()
    {
        try
        {
            if (gvConstitutionalDocuments.Rows.Count > 0)
            {
                for (int i = 0; i <= gvConstitutionalDocuments.Rows.Count - 1; i++)
                {
                    LinkButton hlnkView = gvConstitutionalDocuments.Rows[i].FindControl("hlnkView") as LinkButton;
                    AjaxControlToolkit.AsyncFileUpload AsyncFileUpload1 = (AjaxControlToolkit.AsyncFileUpload)gvConstitutionalDocuments.Rows[i].FindControl("asyFileUpload");
                    FunPriClearUploader(AsyncFileUpload1);
                }

                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Clear", "javascript:fnClearAsyncUploader('" + gvConstitutionalDocuments.Rows.Count.ToString() + "');", true);

            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }
    private void FunPriLoadCommonData()
    {
        try
        {
            Dictionary<string, string> Procparam = new Dictionary<string, string>();
            Procparam.Add("@Option", "1");
            Procparam.Add("@Param1", "CUSTOMER_TYPE");
            ddlCustomerType.BindDataTable("S3G_OR_GET_CUSTLOOKUP", Procparam, true, "--Select--", new string[] { "ID", "NAME" });


            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", intCompanyId.ToString());
            Procparam.Add("@User_ID", Convert.ToString(intUserId));
            Procparam.Add("@Option", "1");
            Procparam.Add("@Is_Active", "1");
            Procparam.Add("@Program_ID", Convert.ToString(intProgramID));
            ddlServiceBranch.BindDataTable("S3G_GET_LOCATION", Procparam, true, "--Select--", new string[] { "BRANCH_ID", "BRANCH" });

            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", intCompanyId.ToString());
            Procparam.Add("@Lookup_Type", "DELIVERY_LOC");
            Procparam.Add("@Table_Name", "S3G_ORG_LOOKUP");
            ddlDeliveryLocation.BindDataTable("S3G_GET_COMMON_LOOKUP_VAL", Procparam, true, "--Select--", new string[] { "ID", "DESCRIPTION" });

            ddlConstitutionName.Items.Clear();
            ddlConstitutionName.Items.Insert(0, "----Select----");
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }
    private void FunBindEmployerRating()
    {
        try
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", intCompanyId.ToString());
            Procparam.Add("@Lookup_Type", "EMPLOYER_RATING");
            Procparam.Add("@Table_Name", "S3G_ORG_LOOKUP");
            ddlEmployeeRating.BindDataTable("S3G_GET_COMMON_LOOKUP_VAL", Procparam, true, "--Select--", new string[] { "ID", "DESCRIPTION" });
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
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
                    }
                    else
                    {
                        txtEffectiveTo.Text = string.Empty;
                    }
                    return;
                }


            }

            if (Utility.StringToDate(txtEffectiveTo.Text) > Utility.StringToDate(hdnDefaultdate.Value))
            {
                Utility.FunShowAlertMsg(this, "Effective To Date cannot be greater than Default Effective To Date");
                txtEffectiveTo.Focus();
                txtEffectiveTo.Text = hdnDefaultdate.Value;
                return;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }


    private void FunBindAdditionalParamDet()
    {
        try
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", intCompanyId.ToString());
            Procparam.Add("@Program_ID", Convert.ToString(intProgramID));
            Procparam.Add("@Type", "2"); //1- customer, 2- Entity
            Procparam.Add("@Type_ID", ddlEntityType.SelectedValue);
            DataTable dtdata = Utility.GetDefaultData("S3G_GET_CONSTANT_PARAM_VAL", Procparam);
            if (dtdata.Rows.Count > 0)
            {
                grvAdditionalInfo.DataSource = dtdata;
                grvAdditionalInfo.DataBind();
                grvAdditionalInfo.Visible = true;
            }
            else
            {
                grvAdditionalInfo.DataSource = null;
                grvAdditionalInfo.Visible = false;
            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }

    }

    private void FunPriLoadLookupTypeData(DropDownList ddlLookup, string strLookupType)
    {
        try
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", intCompanyId.ToString());
            Procparam.Add("@Lookup_Type", strLookupType);
            Procparam.Add("@Table_Name", "S3G_ORG_LOOKUP");
            //ddlLookup.BindDataTable("S3G_GET_COMMON_LOOKUP_VAL", Procparam, true, "--Select--", new string[] { "ID", "DESCRIPTION" });
            ddlLookup.BindDataTable("S3G_GET_COMMON_LOOKUP_VAL", Procparam, true, "--Select--", new string[] { "LOOKUP_CODE", "DESCRIPTION" });

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }

    private void FunPubBindAddsGrid()
    {
        try
        {
            DataTable dtadds = new DataTable();
            DataRow dr;
            dtadds.Columns.Add("CUST_ADD_MAPPING_ID");
            dtadds.Columns.Add("Cust_Address_ID");
            dtadds.Columns.Add("Cust_Address_Type_ID");
            dtadds.Columns.Add("Postal_Code_Text");
            dtadds.Columns.Add("Postal_Code_Value");
            dtadds.Columns.Add("Post_Box_No");
            dtadds.Columns.Add("Way_No");
            dtadds.Columns.Add("House_No");
            dtadds.Columns.Add("Block_No");
            dtadds.Columns.Add("Flat_No");
            dtadds.Columns.Add("LandMark");
            dtadds.Columns.Add("Area_Sheik");
            dtadds.Columns.Add("Residence_Phone_No");
            dtadds.Columns.Add("Residence_Fax_No");
            dtadds.Columns.Add("Mobile_Phone_No");
            dtadds.Columns.Add("Contact_Name");
            dtadds.Columns.Add("Contact_No");
            dtadds.Columns.Add("Office_Phone_No");
            dtadds.Columns.Add("Cust_Email");
            dr = dtadds.NewRow();
            dtadds.Rows.Add(dr);
            gvAddressDetails.DataSource = dtadds;
            gvAddressDetails.DataBind();
            dtadds.Rows[0].Delete();
            ViewState["AddsDetailsTable"] = dtadds;
            gvAddressDetails.Rows[0].Visible = false;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);

        }
        finally
        {
        }
    }

    private void AdditionalInforFetch()
    {
        try
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", intCompanyId.ToString());
            Procparam.Add("@TRAN_ID", Convert.ToString(intEntityId));
            Procparam.Add("@PROGRAM_ID", Convert.ToString(intProgramID));
            Procparam.Add("@TYPE_ID", ddlEntityType.SelectedValue);
            DataTable dtdata = Utility.GetDefaultData("S3G_GET_CONSTANT_PARAMTRAN_VAL", Procparam);
            if (dtdata.Rows.Count > 0)
            {
                grvAdditionalInfo.DataSource = dtdata;
                grvAdditionalInfo.DataBind();
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
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
                txtEffectiveTo.Text = dtdata.Rows[0][0].ToString();
                hdnDefaultdate.Value = dtdata.Rows[0][0].ToString();
            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }

    }

    private void FunPriShowPRDD(object sender)
    {
        try
        {
            string strFieldAtt = ((LinkButton)sender).ClientID;
            int gRowIndex = Utility.FunPubGetGridRowID("gvConstitutionalDocuments", strFieldAtt);
            //Label lblPath = gvConstitutionalDocuments.Rows[gRowIndex].FindControl("myThrobber") as Label;
            Label lblActualPath = gvConstitutionalDocuments.Rows[gRowIndex].FindControl("lblActualPath") as Label;
            string strFileName = lblActualPath.Text.Replace("\\", "/").Trim();
            string strScipt = "window.open('../Common/S3GViewFile.aspx?qsFileName=" + strFileName + "', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strScipt, true);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            //throw new ApplicationException("Due to Data Problem, Unable to View the Document");
        }
    }

    private bool UserFunctionCheck(string strFunctionCode)
    {
        bool blnRights = false;
        try
        {
            Dictionary<string, string> ProParm = new Dictionary<string, string>();
            ProParm.Add("@Company_Id", Convert.ToString(intCompanyId));
            ProParm.Add("@USER_ID", Convert.ToString(intUserId));
            ProParm.Add("@PGM_CODE", strFunctionCode);
            DataTable dt = Utility.GetDefaultData("S3G_SA_GET_SCREEN_ACCESS", ProParm);
            if (dt.Rows.Count > 0)
            {
                if (Convert.ToInt32(dt.Rows[0]["COUNT"]) > 0)
                {
                    blnRights = true;
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
        return blnRights;
    }

    protected void txtBankName_Item_Selected(object Sender, EventArgs e)
    {
        try
        {
            if (txtBankName.SelectedValue != "0")
            {
                PopulateBankBranchList(txtBankName.SelectedValue);
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }

    private void PopulateBankBranchList(string strBankID)
    {
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            Procparam.Add("@OPTION", "3");
            Procparam.Add("@COMPANY_ID", Convert.ToString(intCompanyId));
            Procparam.Add("@PROGRAM_ID", Convert.ToString(intProgramID));
            Procparam.Add("@DESIGNATION_ID", strBankID.Trim());
            ddlBankBranch.ClearSelection();
            ddlBankBranch.BindDataTable("S3G_GET_USERLIST_AGT", Procparam, new string[] { "LOCATION_ID", "LOCATION_NAME" });
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }
    #endregion

    #region "Document Details"

    protected void btnBrowse_OnClick(object sender, EventArgs e)
    {
        try
        {
            int intRowIndex = Utility.FunPubGetGridRowID("gvConstitutionalDocuments", ((Button)sender).ClientID);

            HttpFileCollection hfc = Request.Files;
            for (int i = 0; i < hfc.Count; i++)
            {
                HttpPostedFile hpf = hfc[i];
                if (hpf.ContentLength > 0)
                {
                    CheckBox chkScanned = (CheckBox)gvConstitutionalDocuments.Rows[intRowIndex].FindControl("chkScanned");
                    HiddenField hdnSelectedPath = (HiddenField)gvConstitutionalDocuments.Rows[intRowIndex].FindControl("hdnSelectedPath");
                    FileUpload flUpload = (FileUpload)gvConstitutionalDocuments.Rows[intRowIndex].FindControl("flUpload");
                    //Label lblActualPath = (Label)gvConstitutionalDocuments.Rows[intRowIndex].FindControl("lblActualPath");
                    TextBox txtFileupld = (TextBox)gvConstitutionalDocuments.Rows[intRowIndex].FindControl("txtFileupld");

                    //chkSelect.Checked = true;
                    //chkScanned.ToolTip = flUpload.ToolTip = hdnSelectedPath.Value;
                    //lblActualPath.Text = hpf.FileName;
                    txtFileupld.Text = hpf.FileName;
                    string strViewst = "File" + intRowIndex.ToString();
                    Cache[strViewst] = hpf;
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }

    protected void chkScanned_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            int intRowIndex = Utility.FunPubGetGridRowID("gvConstitutionalDocuments", ((CheckBox)sender).ClientID);
            CheckBox chkScanned = (CheckBox)gvConstitutionalDocuments.Rows[intRowIndex].FindControl("chkScanned");
            Label lblActualPath = (Label)gvConstitutionalDocuments.Rows[intRowIndex].FindControl("lblActualPath");
            TextBox txtFileupld = (TextBox)gvConstitutionalDocuments.Rows[intRowIndex].FindControl("txtFileupld");
            //Button btnBrowse = (Button)gvConstitutionalDocuments.Rows[intRowIndex].FindControl("btnBrowse");

            if (!chkScanned.Checked)
            {
                lblActualPath.Text =
                   txtFileupld.Text = string.Empty;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }

    protected void FunProUploadFilesNew()
    {
        try
        {
            for (int i = 0; i <= gvConstitutionalDocuments.Rows.Count - 1; i++)
            {
                string strViewst = "File" + i.ToString();
                CheckBox chkScanned = (CheckBox)gvConstitutionalDocuments.Rows[i].FindControl("chkScanned");
                Label lblCurrentPath = (Label)gvConstitutionalDocuments.Rows[i].FindControl("lblActualPath");

                if (chkScanned.Checked)
                {
                    HttpPostedFile hpf = (HttpPostedFile)Cache[strViewst];
                    //string strServerPath = Server.MapPath(".").Replace("\\LoanAdmin", "");
                    //string strFilePath = "\\Data\\Invoice\\" + intCompanyId.ToString() + "_" + ddlMLA.SelectedText.Replace("/", "");

                    //if (!System.IO.Directory.Exists(strServerPath + strFilePath))
                    //{
                    //    System.IO.Directory.CreateDirectory(strServerPath + strFilePath);
                    //}

                    //if (ddlSLA.SelectedValue.ToString() != "0")
                    //{
                    //    strFilePath = strFilePath + @"\" + ddlSLA.SelectedItem.ToString().Replace("/", "") + "_" + (i + 1).ToString() + "_" + System.IO.Path.GetFileName(hpf.FileName).Replace("%20", "_").Replace(" ", "_");
                    //}
                    //else
                    //{
                    //    strFilePath = strFilePath + @"\" + (i + 1).ToString() + "_" + System.IO.Path.GetFileName(hpf.FileName).Replace("%20", "_").Replace(" ", "_");
                    //}

                    string strNewFileName = @"\COMPANY" + intCompanyId.ToString();
                    string strPath = "";
                    //if (txOD.Text != "")
                    //{
                    strPath = strNewFileName;
                    strPath = Convert.ToString(ViewState["ConsDocPath"]) + strNewFileName;

                    if (!Directory.Exists(strPath))
                    {
                        Directory.CreateDirectory(strPath);
                    }

                    if (hpf != null)
                    {
                        strPath += @"\" + System.IO.Path.GetFileName(hpf.FileName).Split('.')[0].ToString() + DateTime.Now.ToLocalTime().ToString().Replace(" ", "").Replace("/", "").Replace(":", "") + "." + System.IO.Path.GetFileName(hpf.FileName).Split('.')[1].ToString();
                        lblCurrentPath.Text = strPath;
                        hpf.SaveAs(strPath);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }

    private void FunPriUploadFiles()
    {
        try
        {

            foreach (GridViewRow grv in gvConstitutionalDocuments.Rows)
            {
                AjaxControlToolkit.AsyncFileUpload AsyncFileUpload1 = (AjaxControlToolkit.AsyncFileUpload)grv.FindControl("asyFileUpload");
                TextBox txOD = grv.FindControl("txOD") as TextBox;

                if (AsyncFileUpload1.FileName != "")
                {
                    FileInfo fileInfo = new FileInfo(AsyncFileUpload1.FileName);
                    if (AsyncFileUpload1.HasFile)
                    {

                        string strNewFileName = @"\COMPANY" + intCompanyId.ToString();
                        string strPath = "";
                        //if (txOD.Text != "")
                        //{
                        strPath = strNewFileName;
                        strPath = Convert.ToString(ViewState["ConsDocPath"]) + strNewFileName;
                        if (!Directory.Exists(strPath))
                        {
                            Directory.CreateDirectory(strPath);
                        }

                        strPath += @"\" + AsyncFileUpload1.FileName.Split('.')[0].ToString() + DateTime.Now.ToLocalTime().ToString().Replace(" ", "").Replace("/", "").Replace(":", "") + "." + AsyncFileUpload1.FileName.Split('.')[1].ToString();
                        //}
                        txOD.Text = strPath;

                        AsyncFileUpload1.SaveAs(strPath);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }

    protected void FunProClearCaches()
    {
        try
        {
            for (int i = 0; i <= gvConstitutionalDocuments.Rows.Count - 1; i++)
            {
                string strViewst = "File" + i.ToString();
                if (Cache[strViewst] != null)
                {
                    Cache.Remove(strViewst);
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }

    public void FunPubBindGridDetails()
    {
        //ObjAuthorizationRuleCardClient = new RuleCardMgtServicesReference.RuleCardMgtServicesClient();
        try
        {
            if (intEntityId == 0)
            {
                //Binding Operation History Grid Start
                DataRow drOperation;
                dtOperationHisDet.Columns.Add("SNo");
                dtOperationHisDet.Columns.Add("FromDate");
                dtOperationHisDet.Columns.Add("ToDate");
                dtOperationHisDet.Columns.Add("Operation_Hist_ID");
                dtOperationHisDet.Columns.Add("RowStatus");

                drOperation = dtOperationHisDet.NewRow();
                drOperation["SNo"] = 0;
                dtOperationHisDet.Rows.Add(drOperation);

                ViewState["OperationHistoryData"] = dtOperationHisDet;
                gvOperationHistoryDetails.DataSource = dtOperationHisDet;
                gvOperationHistoryDetails.DataBind();
                gvOperationHistoryDetails.Rows[0].Visible = false;
                ViewState["OperationHistoryData"] = dtOperationHisDet;
                //Binding Operation History Grid End


            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }

    #endregion

    #region Name Change Event
    protected void NameChange(object sender, EventArgs e)
    {
        try
        {
            string strEntityNames = "";
            bool isLenExceeded = false;

            strEntityNames = ucEntityNamesSetup.FirstName + " " + ucEntityNamesSetup.SecondName + " " + ucEntityNamesSetup.ThirdName + " " + ucEntityNamesSetup.FourthName + " " + ucEntityNamesSetup.FifthName + " " + ucEntityNamesSetup.SixthName;
            if (strEntityNames.Trim().Length > 100)
            {
                Utility.FunShowValidationMsg(this.Page, "ORG_ENTMST", 1);
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Combination of Entity Names length should be 100');", true);
                isLenExceeded = true;
            }

            TextBox txtFirst_Name = (TextBox)ucEntityNamesSetup.FindControl("txtFirst_Name");
            TextBox txtSecond_Name = (TextBox)ucEntityNamesSetup.FindControl("txtSecond_Name");
            TextBox txtThird_Name = (TextBox)ucEntityNamesSetup.FindControl("txtThird_Name");
            TextBox txtFourth_Name = (TextBox)ucEntityNamesSetup.FindControl("txtFourth_Name");
            TextBox txtFifth_Name = (TextBox)ucEntityNamesSetup.FindControl("txtFifth_Name");
            TextBox txtSixth_Name = (TextBox)ucEntityNamesSetup.FindControl("txtSixth_Name");


            TextBox txt = (TextBox)sender;
            if (txt.ID == "txtFirst_Name")
            {

                // txtFirst_Name.Focus();
                txtSecond_Name.Focus();

                if (isLenExceeded)
                {
                    txtFirst_Name.Clear();
                    txtFirst_Name.Focus();
                    return;
                }

            }
            if (txt.ID == "txtSecond_Name")
            {

                txtThird_Name.Focus();

                if (isLenExceeded)
                {
                    txtSecond_Name.Clear();
                    txtSecond_Name.Focus();
                    return;
                }
            }
            if (txt.ID == "txtThird_Name")
            {

                txtFourth_Name.Focus();

                if (isLenExceeded)
                {
                    txtThird_Name.Clear();
                    txtThird_Name.Focus();
                    return;
                }
            }
            if (txt.ID == "txtFourth_Name")
            {

                txtFifth_Name.Focus();

                if (isLenExceeded)
                {
                    txtFourth_Name.Clear();
                    txtFourth_Name.Focus();
                    return;
                }
            }
            if (txt.ID == "txtFifth_Name")
            {

                txtSixth_Name.Focus();

                if (isLenExceeded)
                {
                    txtFifth_Name.Clear();
                    txtFifth_Name.Focus();
                    return;
                }
            }
            if (txt.ID == "txtSixth_Name")
            {
                txtSixth_Name.Focus();

                if (isLenExceeded)
                {
                    txtSixth_Name.Clear();
                    txtSixth_Name.Focus();
                    return;
                }
            }

            txtEntityName.Text = ucEntityNamesSetup.FirstName + " " + ucEntityNamesSetup.SecondName + " " + ucEntityNamesSetup.ThirdName + " " + ucEntityNamesSetup.FourthName + " " + ucEntityNamesSetup.FifthName + " " + ucEntityNamesSetup.SixthName;

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }

    #endregion

    #region Grid Events

    protected void gvConstitutionalDocuments_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            e.Row.Cells[0].Visible = false;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblPath = e.Row.FindControl("myThrobber") as Label;
                LinkButton hlnkView = e.Row.FindControl("hlnkView") as LinkButton;
                CheckBox chkIsMandatory = e.Row.FindControl("chkIsMandatory") as CheckBox;
                CheckBox chkIsNeedImageCopy = e.Row.FindControl("chkIsNeedImageCopy") as CheckBox;
                //AjaxControlToolkit.AsyncFileUpload asyFileUpload = e.Row.FindControl("asyFileUpload") as AjaxControlToolkit.AsyncFileUpload;
                CheckBox chkCollected = e.Row.FindControl("chkCollected") as CheckBox;
                CheckBox chkScanned = e.Row.FindControl("chkScanned") as CheckBox;
                TextBox txtValues = e.Row.FindControl("txtValues") as TextBox;


                FileUpload flUpload = (FileUpload)e.Row.FindControl("flUpload");
                //Button btnDlg = (Button)e.Row.FindControl("btnDlg");
                //HiddenField hdnSelectedPath = (HiddenField)e.Row.FindControl("hdnSelectedPath");
                Label lblActualPath = (Label)e.Row.FindControl("lblActualPath");
                TextBox txtFileupld = e.Row.FindControl("txtFileupld") as TextBox;
                Button btnBrowse = (Button)e.Row.FindControl("btnBrowse");

                if (!chkIsNeedImageCopy.Checked)
                {
                    chkScanned.Enabled = false;
                }

                // AjaxControlToolkit.AsyncFileUpload AsyncFileUpload1 = (AjaxControlToolkit.AsyncFileUpload)e.Row.FindControl("asyFileUpload");
                if (lblActualPath != null)
                {
                    if (lblActualPath.Text.Trim() != "")
                    {
                        hlnkView.Enabled = true;
                    }
                    else
                    {
                        hlnkView.Enabled = false;
                    }
                }
                else
                {
                    hlnkView.Enabled = false;
                }

                if (!chkIsNeedImageCopy.Checked)
                {
                    txtFileupld.Visible =
                    flUpload.Visible = false;
                    hlnkView.Enabled = false;
                }



                TextBox txtVal = (TextBox)e.Row.FindControl("txtValues");
                // Commented By R. Manikandan Enable value field to enter the Remarks on 07-JAN-2013

                //if (txtVal != null)
                //{
                //    txtVal.Enabled = FunPriDisableValueField(e.Row.Cells[1].Text);
                //}
                if (!string.IsNullOrEmpty(Request.QueryString["qsMode"]))
                {
                    if (Request.QueryString["qsMode"].ToString() == "Q")
                    {
                        txtFileupld.Visible =
                    flUpload.Visible =
                        chkIsMandatory.Enabled = chkIsNeedImageCopy.Enabled = chkCollected.Enabled =
                            chkScanned.Enabled = txtValues.Enabled = false;
                    }
                }

                flUpload.Attributes.Add("onchange", "fnLoadPath('" + btnBrowse.ClientID + "'); ");

            }

        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
        }
    }

    protected void gvOperationHistoryDetails_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            DataRow dr;
            if (e.CommandName == "AddNew")
            {
                // Label lbl = (Label)gvOperationHistoryDetails.Rows[gvOperationHistoryDetails.Rows.Count - 1].FindControl("lblSNo");
                //Label lbl = (Label)gvOperationHistoryDetails.Rows[gvOperationHistoryDetails.Rows.Count].FindControl("lblSNo");
                //int intSNo = (Convert.ToInt32(lbl.Text)) + 1;
                TextBox txtOpeartionFrom = (TextBox)gvOperationHistoryDetails.FooterRow.FindControl("txtOperationFromDate");
                TextBox txtOpeartionTo = (TextBox)gvOperationHistoryDetails.FooterRow.FindControl("txtOperationToDate");

                if (txtOpeartionFrom.Text.Trim() == string.Empty)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Enter From Date');", true);
                    txtOpeartionFrom.Focus();
                    return;

                }
                if (txtOpeartionTo.Text.Trim() == string.Empty)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Enter To Date');", true);
                    txtOpeartionTo.Focus();
                    return;
                }

                dtOperationHisDet = (DataTable)ViewState["OperationHistoryData"];
                dr = dtOperationHisDet.NewRow();
                // dr["SNo"] = Convert.ToInt32(lbl.Text) + 1;
                dr["FromDate"] = txtOpeartionFrom.Text.Trim();
                dr["ToDate"] = txtOpeartionTo.Text.Trim();
                dtOperationHisDet.Rows.Add(dr);
                gvOperationHistoryDetails.DataSource = dtOperationHisDet;
                gvOperationHistoryDetails.DataBind();
                ViewState["OperationHistoryData"] = dtOperationHisDet;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            //lblErrorMessage.Text = ex.Message;
        }
    }

    protected void gvOperationHistoryDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            DataTable dtDelete;
            dtDelete = (DataTable)ViewState["OperationHistoryData"];
            dtDelete.Rows.RemoveAt(e.RowIndex);
            Label lblSNo = (Label)gvOperationHistoryDetails.Rows[e.RowIndex].FindControl("lblSNo");

            if (dtDelete.Rows.Count <= 0)
            {
                dtDelete = null;
                dtOperationHisDet.Clear();
                ViewState["OperationHistoryData"] = null;
                DataRow dr;
                dtOperationHisDet.Columns.Add("SNo");
                dtOperationHisDet.Columns.Add("FromDate");
                dtOperationHisDet.Columns.Add("ToDate");

                dtOperationHisDet.Columns.Add("Operation_Hist_ID");
                dtOperationHisDet.Columns.Add("RowStatus");

                dr = dtOperationHisDet.NewRow();
                dr["SNo"] = 0;
                dtOperationHisDet.Rows.Add(dr);
                ViewState["OperationHistoryData"] = dtOperationHisDet;
                gvOperationHistoryDetails.DataSource = dtOperationHisDet;
                gvOperationHistoryDetails.DataBind();
                gvOperationHistoryDetails.Rows[0].Visible = false;
                ViewState["OperationHistoryData"] = dtOperationHisDet;
                return;
            }

            gvOperationHistoryDetails.DataSource = dtDelete;
            gvOperationHistoryDetails.DataBind();
            ViewState["OperationHistoryData"] = dtDelete;

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            // lblErrorMessage.Text = ex.Message;
        }
    }

    protected void gvOperationHistoryDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                AjaxControlToolkit.CalendarExtender calFromDate = e.Row.FindControl("ceOperationFromDate") as AjaxControlToolkit.CalendarExtender;
                AjaxControlToolkit.CalendarExtender calToDate = e.Row.FindControl("ceOperationToDate") as AjaxControlToolkit.CalendarExtender;

                calFromDate.Format = strDateFormat;
                calToDate.Format = strDateFormat;

                TextBox txtOperationFrom = (TextBox)e.Row.FindControl("txtOperationFromDate");
                TextBox txtOperationTo = (TextBox)e.Row.FindControl("txtOperationToDate");

                if (strMode == "M" || strMode == "Q")
                    gvOperationHistoryDetails.ShowFooter = false;

            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                AjaxControlToolkit.CalendarExtender calFromDate = e.Row.FindControl("ceOperationFromDate") as AjaxControlToolkit.CalendarExtender;
                AjaxControlToolkit.CalendarExtender calToDate = e.Row.FindControl("ceOperationToDate") as AjaxControlToolkit.CalendarExtender;
                LinkButton lnkbtnRemove = (LinkButton)e.Row.FindControl("btnRemove");

                if (calFromDate != null)
                    calFromDate.Format = strDateFormat;

                if (calToDate != null)
                    calToDate.Format = strDateFormat;

                if (strMode == "M" || strMode == "Q")
                {
                    lnkbtnRemove.Visible = false;
                }

                if (dtOperationHisDet.Rows.Count != 0)
                {
                    if (dtOperationHisDet.Rows[0]["FromDate"].ToString() == "")
                    {
                        dtOperationHisDet = (DataTable)ViewState["OperationHistoryData"];
                        dtOperationHisDet.Rows[0].Delete();
                        ViewState["OperationHistoryData"] = dtOperationHisDet;
                    }
                }
            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }

    protected void gvEmployeeHistoryDet_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            DataRow dr;
            if (e.CommandName == "AddNew")
            {
                Label lbl = (Label)gvEmployeeHistoryDet.Rows[gvEmployeeHistoryDet.Rows.Count - 1].FindControl("lblSNo");
                int intSNo = (Convert.ToInt32(lbl.Text)) + 1;
                TextBox txtOpeartionFrom = (TextBox)gvEmployeeHistoryDet.FooterRow.FindControl("txtEmpHistoryFromDate");
                TextBox txtOpeartionTo = (TextBox)gvEmployeeHistoryDet.FooterRow.FindControl("txtEmpHistoryToDate");

                if (txtOpeartionFrom.Text.Trim() == string.Empty)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Enter From Date');", true);
                    txtOpeartionFrom.Focus();
                    return;

                }
                if (txtOpeartionTo.Text.Trim() == string.Empty)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Enter To Date');", true);
                    txtOpeartionTo.Focus();
                    return;
                }
                if ((Convert.ToInt32(Utility.StringToDate(txtOpeartionFrom.Text).ToString("yyyyMMdd"))) > (Convert.ToInt32(Utility.StringToDate(txtOpeartionTo.Text).ToString("yyyyMMdd"))))
                {
                    Utility.FunShowAlertMsg(this, "From Date should be greater than or equal to To Date");
                    txtOpeartionFrom.Focus();
                    return;
                }

                //Validation Starts

                if ((!string.IsNullOrEmpty(txtOpeartionFrom.Text)) && (!string.IsNullOrEmpty(txtOpeartionTo.Text)))
                {
                    if ((Convert.ToInt32(Utility.StringToDate(txtOpeartionFrom.Text).ToString("yyyyMMdd"))) > (Convert.ToInt32(Utility.StringToDate(txtOpeartionTo.Text).ToString("yyyyMMdd"))))
                    {
                        Utility.FunShowAlertMsg(this, "To Date should be greater than or equal to From Date");
                        gvInsurancePolicyDet.FooterRow.Focus();
                        return;
                    }
                }



                if (dtEmployeeHisDet.Rows.Count > 0)
                {
                    foreach (DataRow row in dtEmployeeHisDet.Rows)
                    {
                        DateTime StartDate = Utility.StringToDate(row["FromDate"].ToString());
                        DateTime EndDate = Utility.StringToDate(row["ToDate"].ToString());

                        if (((Utility.StringToDate(txtOpeartionFrom.Text) >= StartDate && Utility.StringToDate(txtOpeartionFrom.Text) <= EndDate) ||
                              (Utility.StringToDate(txtOpeartionFrom.Text) >= StartDate && Utility.StringToDate(txtOpeartionTo.Text) <= EndDate))
                            )
                        {
                            Utility.FunShowAlertMsg(this.Page, "Date Range already Exists.");
                            gvInsurancePolicyDet.FooterRow.Focus();
                            return;
                        }

                        if ((Utility.StringToDate(txtOpeartionFrom.Text) <= StartDate))
                        {
                            Utility.FunShowAlertMsg(this.Page, "Date Range already Exists.");
                            gvInsurancePolicyDet.FooterRow.Focus();
                            return;
                        }

                    }
                }
                //Validation ends



                dtEmployeeHisDet = (DataTable)ViewState["EmployeeHistoryData"];
                dr = dtEmployeeHisDet.NewRow();
                dr["SNo"] = Convert.ToInt32(lbl.Text) + 1;
                dr["FromDate"] = txtOpeartionFrom.Text.Trim();
                dr["ToDate"] = txtOpeartionTo.Text.Trim();
                dtEmployeeHisDet.Rows.Add(dr);
                gvEmployeeHistoryDet.DataSource = dtEmployeeHisDet;
                gvEmployeeHistoryDet.DataBind();
                ViewState["EmployeeHistoryData"] = dtEmployeeHisDet;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            //lblErrorMessage.Text = ex.Message;
        }
    }

    protected void gvEmployeeHistoryDet_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            DataTable dtDelete;
            dtDelete = (DataTable)ViewState["EmployeeHistoryData"];
            dtDelete.Rows.RemoveAt(e.RowIndex);
            Label lblSNo = (Label)gvEmployeeHistoryDet.Rows[e.RowIndex].FindControl("lblSNo");

            if (dtDelete.Rows.Count <= 0)
            {
                dtDelete = null;
                dtEmployeeHisDet.Clear();
                ViewState["EmployeeHistoryData"] = null;
                DataRow dr;
                dtEmployeeHisDet.Columns.Add("SNo");
                dtEmployeeHisDet.Columns.Add("FromDate");
                dtEmployeeHisDet.Columns.Add("ToDate");

                dtEmployeeHisDet.Columns.Add("Employee_Hist_ID");
                dtEmployeeHisDet.Columns.Add("RowStatus");

                dr = dtEmployeeHisDet.NewRow();
                dr["SNo"] = 0;
                dtEmployeeHisDet.Rows.Add(dr);

                ViewState["EmployeeHistoryData"] = dtEmployeeHisDet;
                gvEmployeeHistoryDet.DataSource = dtEmployeeHisDet;
                gvEmployeeHistoryDet.DataBind();
                gvEmployeeHistoryDet.Rows[0].Visible = false;
                ViewState["EmployeeHistoryData"] = dtEmployeeHisDet;
                return;
            }

            gvEmployeeHistoryDet.DataSource = dtDelete;
            gvEmployeeHistoryDet.DataBind();
            ViewState["EmployeeHistoryData"] = dtDelete;

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            // lblErrorMessage.Text = ex.Message;
        }
    }
    protected void gvEmployeeHistoryDet_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                AjaxControlToolkit.CalendarExtender calFromDate = e.Row.FindControl("ceEmpHistoryFromDate") as AjaxControlToolkit.CalendarExtender;
                AjaxControlToolkit.CalendarExtender calToDate = e.Row.FindControl("ceEmpHistoryToDate") as AjaxControlToolkit.CalendarExtender;



                calFromDate.Format = strDateFormat;
                calToDate.Format = strDateFormat;

                TextBox txtEmployeeFrom = (TextBox)e.Row.FindControl("txtEmpHistoryFromDate");
                TextBox txtEmployeeTo = (TextBox)e.Row.FindControl("txtEmpHistoryToDate");

                txtEmployeeFrom.Attributes.Add("onChange", "fnDoDate(this,'" + txtEmployeeFrom.ClientID + "','" + strDateFormat + "',false,  false);");
                txtEmployeeTo.Attributes.Add("onChange", "fnDoDate(this,'" + txtEmployeeTo.ClientID + "','" + strDateFormat + "',false,  false);");

                if (strMode == "M" || strMode == "Q")
                    gvEmployeeHistoryDet.ShowFooter = false;


            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                AjaxControlToolkit.CalendarExtender calFromDate = e.Row.FindControl("ceEmpHistoryFromDate") as AjaxControlToolkit.CalendarExtender;
                AjaxControlToolkit.CalendarExtender calToDate = e.Row.FindControl("ceEmpHistoryToDate") as AjaxControlToolkit.CalendarExtender;
                LinkButton lnkbtnRemove = (LinkButton)e.Row.FindControl("btnRemove");


                if (calFromDate != null)
                    calFromDate.Format = strDateFormat;

                if (calToDate != null)
                    calToDate.Format = strDateFormat;

                if (strMode == "M" || strMode == "Q")
                    lnkbtnRemove.Visible = false;

                if (dtEmployeeHisDet.Rows.Count != 0)
                {
                    if (dtEmployeeHisDet.Rows[0]["FromDate"].ToString() == "")
                    {
                        dtEmployeeHisDet = (DataTable)ViewState["EmployeeHistoryData"];
                        dtEmployeeHisDet.Rows[0].Delete();
                        ViewState["EmployeeHistoryData"] = dtEmployeeHisDet;
                    }
                }
            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }

    protected void gvInsurancePolicyDet_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            DataRow dr;
            DataRow[] dRow1;
            if (e.CommandName == "AddNew")
            {
                Label lbl = (Label)gvInsurancePolicyDet.Rows[gvInsurancePolicyDet.Rows.Count - 1].FindControl("lblSNo");
                int intSNo = (Convert.ToInt32(lbl.Text)) + 1;
                UserControls_S3GAutoSuggest ddlPolicyType = gvInsurancePolicyDet.FooterRow.FindControl("ddlPolicyType") as UserControls_S3GAutoSuggest;
                TextBox txtCompanyRate = (TextBox)gvInsurancePolicyDet.FooterRow.FindControl("txtCompanyRate");
                TextBox txtCustomerRate = (TextBox)gvInsurancePolicyDet.FooterRow.FindControl("txtCustomerRate");
                TextBox txtInsuranceDetFromDate = (TextBox)gvInsurancePolicyDet.FooterRow.FindControl("txtInsuranceDetFromDate");
                TextBox txtInsuranceDetToDate = (TextBox)gvInsurancePolicyDet.FooterRow.FindControl("txtInsuranceDetToDate");

                if (txtCompanyRate.Text.Trim() == string.Empty)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Enter Company Rate');", true);
                    txtCompanyRate.Focus();
                    return;

                }
                if (txtCustomerRate.Text.Trim() == string.Empty)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Enter Customer Rate');", true);
                    txtCustomerRate.Focus();
                    return;
                }
                if (txtInsuranceDetFromDate.Text.Trim() == string.Empty)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Select From Date');", true);
                    txtInsuranceDetFromDate.Focus();
                    return;
                }
                if (txtInsuranceDetToDate.Text.Trim() == string.Empty)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Select To Date');", true);
                    txtInsuranceDetToDate.Focus();
                    return;
                }

                if (Convert.ToDecimal(txtCompanyRate.Text.Trim()) > Convert.ToDecimal(txtCustomerRate.Text.Trim()))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Customer rate should be greater than company rate');", true);
                    txtCustomerRate.Focus();
                    return;
                }

                dtInsurancePolicyDet = (DataTable)ViewState["InsurancePolicyData"];

                //Validation Starts

                if ((!string.IsNullOrEmpty(txtInsuranceDetFromDate.Text)) && (!string.IsNullOrEmpty(txtInsuranceDetToDate.Text)))
                {
                    if ((Convert.ToInt32(Utility.StringToDate(txtInsuranceDetFromDate.Text).ToString("yyyyMMdd"))) > (Convert.ToInt32(Utility.StringToDate(txtInsuranceDetToDate.Text).ToString("yyyyMMdd"))))
                    {
                        Utility.FunShowAlertMsg(this, "To Date should be greater than or equal to From Date");
                        gvInsurancePolicyDet.FooterRow.Focus();
                        return;
                    }
                }



                if (dtInsurancePolicyDet.Rows.Count > 0)
                {
                    foreach (DataRow row in dtInsurancePolicyDet.Rows)
                    {
                        DateTime StartDate = Utility.StringToDate(row["FromDate"].ToString());
                        DateTime EndDate = Utility.StringToDate(row["ToDate"].ToString());

                        if (((Utility.StringToDate(txtInsuranceDetFromDate.Text) >= StartDate && Utility.StringToDate(txtInsuranceDetFromDate.Text) <= EndDate) ||
                              (Utility.StringToDate(txtInsuranceDetToDate.Text) >= StartDate && Utility.StringToDate(txtInsuranceDetToDate.Text) <= EndDate))
                            )
                        {
                            Utility.FunShowAlertMsg(this.Page, "Date Range already Exists.");
                            gvInsurancePolicyDet.FooterRow.Focus();
                            return;
                        }

                        if ((Utility.StringToDate(txtInsuranceDetFromDate.Text) <= StartDate))
                        {
                            Utility.FunShowAlertMsg(this.Page, "Date Range already Exists.");
                            gvInsurancePolicyDet.FooterRow.Focus();
                            return;
                        }

                    }
                }
                //Validation ends

                dr = dtInsurancePolicyDet.NewRow();
                dr["SNo"] = Convert.ToInt32(lbl.Text) + 1;
                dr["PolicyRateID"] = "0";
                dr["CompanyRate"] = txtCompanyRate.Text.Trim();
                dr["CustomerRate"] = txtCustomerRate.Text.Trim();
                dr["FromDate"] = txtInsuranceDetFromDate.Text.Trim();
                dr["ToDate"] = txtInsuranceDetToDate.Text.Trim();
                dtInsurancePolicyDet.Rows.Add(dr);
                gvInsurancePolicyDet.DataSource = dtInsurancePolicyDet;
                gvInsurancePolicyDet.DataBind();
                ViewState["InsurancePolicyData"] = dtInsurancePolicyDet;

                gvInsurancePolicyDet.FooterRow.Focus();

            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }

    protected void gvInsurancePolicyDet_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                AjaxControlToolkit.CalendarExtender calFromDate = e.Row.FindControl("ceInsuranceDetFromDate") as AjaxControlToolkit.CalendarExtender;
                AjaxControlToolkit.CalendarExtender calToDate = e.Row.FindControl("ceInsuranceDetToDate") as AjaxControlToolkit.CalendarExtender;

                TextBox txtgvCompanyRate = (TextBox)e.Row.FindControl("txtCompanyRate");
                TextBox txtgvCustomerRate = (TextBox)e.Row.FindControl("txtCustomerRate");
                TextBox txtgvInsuranceDetFromDate = (TextBox)e.Row.FindControl("txtInsuranceDetFromDate");
                TextBox txtgvInsuranceDetToDate = (TextBox)e.Row.FindControl("txtInsuranceDetToDate");

                calFromDate.Format = strDateFormat;
                calToDate.Format = strDateFormat;

                txtgvInsuranceDetFromDate.Attributes.Add("onChange", "fnDoDate(this,'" + txtgvInsuranceDetFromDate.ClientID + "','" + strDateFormat + "',false,  true);");
                txtgvInsuranceDetToDate.Attributes.Add("onChange", "fnDoDate(this,'" + txtgvInsuranceDetToDate.ClientID + "','" + strDateFormat + "',false,  true);");


                //txtgvCompanyRate.SetDecimalPrefixSuffix(strPrefixLength, strDecMaxLength, true, "Company Rate");
                //txtgvCustomerRate.SetDecimalPrefixSuffix(strPrefixLength, strDecMaxLength, true, "Customer Rate");

                txtgvCompanyRate.SetPercentagePrefixSuffix(2, 3, true, "Company Rate");
                txtgvCustomerRate.SetPercentagePrefixSuffix(2, 3, true, "Customer Rate");

                if (strMode == "Q")
                    gvInsurancePolicyDet.ShowFooter = false;
                else
                    gvInsurancePolicyDet.ShowFooter = true;

            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                AjaxControlToolkit.CalendarExtender calFromDate = e.Row.FindControl("ceInsuranceDetFromDate") as AjaxControlToolkit.CalendarExtender;
                AjaxControlToolkit.CalendarExtender calToDate = e.Row.FindControl("ceInsuranceDetToDate") as AjaxControlToolkit.CalendarExtender;
                LinkButton lnkbtnRemove = (LinkButton)e.Row.FindControl("btnRemove");

                if (calFromDate != null)
                    calFromDate.Format = strDateFormat;

                if (calToDate != null)
                    calToDate.Format = strDateFormat;

                if (strMode == "M" || strMode == "Q")
                    lnkbtnRemove.Visible = false;


                if (dtInsurancePolicyDet.Rows.Count != 0)
                {
                    if (dtInsurancePolicyDet.Rows[0]["CompanyRate"].ToString() == "")
                    {
                        dtInsurancePolicyDet = (DataTable)ViewState["InsurancePolicyData"];
                        dtInsurancePolicyDet.Rows[0].Delete();
                        ViewState["InsurancePolicyData"] = dtInsurancePolicyDet;
                    }
                }
            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }
    protected void gvInsurancePolicyDet_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            DataTable dtDelete;
            dtDelete = (DataTable)ViewState["InsurancePolicyData"];
            dtDelete.Rows.RemoveAt(e.RowIndex);
            Label lblSNo = (Label)gvInsurancePolicyDet.Rows[e.RowIndex].FindControl("lblSNo");

            if (dtDelete.Rows.Count <= 0)
            {
                dtDelete = null;
                dtInsurancePolicyDet.Clear();
                ViewState["InsurancePolicyData"] = null;
                DataRow dr;
                dtInsurancePolicyDet.Columns.Add("SNo");
                dtInsurancePolicyDet.Columns.Add("PolicyRateID");
                dtInsurancePolicyDet.Columns.Add("CompanyRate");
                dtInsurancePolicyDet.Columns.Add("CustomerRate");
                dtInsurancePolicyDet.Columns.Add("FromDate");
                dtInsurancePolicyDet.Columns.Add("ToDate");
                dtInsurancePolicyDet.Columns.Add("RowStatus");

                dr = dtInsurancePolicyDet.NewRow();
                dr["SNo"] = 0;
                dtInsurancePolicyDet.Rows.Add(dr);

                ViewState["InsurancePolicyData"] = dtInsurancePolicyDet;
                gvInsurancePolicyDet.DataSource = dtInsurancePolicyDet;
                gvInsurancePolicyDet.DataBind();
                gvInsurancePolicyDet.Rows[0].Visible = false;
                ViewState["InsurancePolicyData"] = dtInsurancePolicyDet;
                return;
            }

            gvInsurancePolicyDet.DataSource = dtDelete;
            gvInsurancePolicyDet.DataBind();
            ViewState["InsurancePolicyData"] = dtDelete;

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }

    protected void gvAddressDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                for (int i = 0; i < e.Row.Cells.Count - 1; i++)
                {
                    e.Row.Cells[i].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink
                    (this.gvAddressDetails, "Select$" + e.Row.RowIndex);
                }

                Label lblPostalCode = (Label)e.Row.FindControl("lblPostalCodeValue");


                //if (strMode == "M")
                //{
                e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);funSetColor('" + lblPostalCode.ClientID + "', 1);";
                e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);funSetColor('" + lblPostalCode.ClientID + "', 2);";
                //}


                LinkButton LnkBtn = (LinkButton)e.Row.FindControl("lnkbtnDelete");
                LnkBtn.CommandArgument = e.Row.RowIndex.ToString();
                LnkBtn.Attributes.Add("onclick", "return confirm(\"Are you sure want to delete?\")");
            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }

    }
    protected void gvAddressDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            DataTable dtDelete;
            dtDelete = (DataTable)ViewState["AddressDetails"];
            dtDelete.Rows.RemoveAt(e.RowIndex);
            gvAddressDetails.DataSource = dtDelete;
            gvAddressDetails.DataBind();
            ViewState["AddressDetails"] = dtDelete;
            FunClearAddressDetails();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }

    }
    protected void gvAddressDetails_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //if (intEntityId > 0)
            //{

            Label lblAddress_ID = (Label)gvAddressDetails.SelectedRow.FindControl("lblAddress_ID");
            // Label lblsno = (Label)gvAddressDetails.SelectedRow.FindControl("lblsno");
            Label lblEntity_Add_Mapping_ID = gvAddressDetails.SelectedRow.FindControl("lblEntity_Add_Mapping_ID") as Label;

            if (lblEntity_Add_Mapping_ID.Text.Trim() != string.Empty)
            {

                hdnAddressId.Value = lblEntity_Add_Mapping_ID.Text.Trim();

                Label lblPostalCodeValue = (Label)gvAddressDetails.SelectedRow.FindControl("lblPostalCodeValue");

                //txtBranch_Code.Text = Convert.ToString(gvAddressDetails.SelectedRow.Cells[1].Text.Replace("&nbsp;", ""));
                txtBranch_Code.Text = Convert.ToString(Server.HtmlDecode(gvAddressDetails.SelectedRow.Cells[1].Text));
                txtBranch_Name.Text = Convert.ToString(gvAddressDetails.SelectedRow.Cells[2].Text.Replace("&nbsp;", ""));

                ucAddressDetailsSetup.PostalCode_Value = lblPostalCodeValue.Text;//Convert.ToString(gvAddressDetails.SelectedRow.Cells[3].Text.Replace("&nbsp;", ""));
                ucAddressDetailsSetup.PostalCode_Text = Convert.ToString(gvAddressDetails.SelectedRow.Cells[4].Text.Replace("&nbsp;", ""));
                ucAddressDetailsSetup.PostBoxNo = Convert.ToString(gvAddressDetails.SelectedRow.Cells[5].Text.Replace("&nbsp;", ""));
                ucAddressDetailsSetup.WayNo = Convert.ToString(gvAddressDetails.SelectedRow.Cells[6].Text.Replace("&nbsp;", ""));
                ucAddressDetailsSetup.HouseNo = Convert.ToString(gvAddressDetails.SelectedRow.Cells[7].Text.Replace("&nbsp;", ""));
                ucAddressDetailsSetup.BlockNo = Convert.ToString(gvAddressDetails.SelectedRow.Cells[8].Text.Replace("&nbsp;", ""));
                ucAddressDetailsSetup.FlatNo = Convert.ToString(gvAddressDetails.SelectedRow.Cells[9].Text.Replace("&nbsp;", ""));
                ucAddressDetailsSetup.LandMark = Convert.ToString(gvAddressDetails.SelectedRow.Cells[10].Text.Replace("&nbsp;", ""));
                ucAddressDetailsSetup.AreaSheik = Convert.ToString(gvAddressDetails.SelectedRow.Cells[11].Text.Replace("&nbsp;", ""));
                ucAddressDetailsSetup.ResidencePhoneNo = Convert.ToString(gvAddressDetails.SelectedRow.Cells[12].Text.Replace("&nbsp;", ""));
                ucAddressDetailsSetup.ResidenceFaxNo = Convert.ToString(gvAddressDetails.SelectedRow.Cells[13].Text.Replace("&nbsp;", ""));
                ucAddressDetailsSetup.MobilePhoneNo = Convert.ToString(gvAddressDetails.SelectedRow.Cells[14].Text.Replace("&nbsp;", ""));
                ucAddressDetailsSetup.ContactName = Convert.ToString(gvAddressDetails.SelectedRow.Cells[15].Text.Replace("&nbsp;", ""));
                ucAddressDetailsSetup.ContactNo = Convert.ToString(gvAddressDetails.SelectedRow.Cells[16].Text.Replace("&nbsp;", ""));
                ucAddressDetailsSetup.OfficePhoneNo = Convert.ToString(gvAddressDetails.SelectedRow.Cells[17].Text.Replace("&nbsp;", ""));
                ucAddressDetailsSetup.OfficeFaxNo = Convert.ToString(gvAddressDetails.SelectedRow.Cells[18].Text.Replace("&nbsp;", ""));
                ucAddressDetailsSetup.CustomerEmail = Convert.ToString(gvAddressDetails.SelectedRow.Cells[19].Text.Replace("&nbsp;", ""));


                btnAddAddress.Enabled_False();

                if (strMode == "Q")
                    btnModifyAddress.Visible = false;
                else
                    btnModifyAddress.Visible = true;

                //}
                //else
                //{
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "NoEdit", "alert('Edit is not allowed in this Mode');", true);
                //    btnAdd.Enabled = true;
                //}
            }
        }
        catch (Exception ex)
        {
            // ddlBranch.SelectedIndex = 0;
            btnModifyAddress.Enabled_True();
            btnAddAddress.Enabled_False();

            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }
    protected void grvAdditionalInfo_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblParamName = (Label)e.Row.FindControl("lblParamName");
                Label lblParamType = (Label)e.Row.FindControl("lblParamType");
                Label lblLookupType = (Label)e.Row.FindControl("lblLookupType");
                Label lblParamSize = (Label)e.Row.FindControl("lblParamSize");
                TextBox txtValues = (TextBox)e.Row.FindControl("txtValues");
                DropDownList ddlValues = (DropDownList)e.Row.FindControl("ddlValues");
                AjaxControlToolkit.FilteredTextBoxExtender fteValues = (AjaxControlToolkit.FilteredTextBoxExtender)e.Row.FindControl("fteValues");
                AjaxControlToolkit.CalendarExtender calAddValues = (AjaxControlToolkit.CalendarExtender)e.Row.FindControl("calAddValues");
                if (lblParamType.Text.Trim() == "5")//Lookup
                {
                    txtValues.Visible = false;
                    fteValues.Enabled = false;
                    calAddValues.Enabled = false;
                    ddlValues.Visible = true;
                    FunPriLoadLookupTypeData(ddlValues, lblLookupType.Text.Trim().ToUpper());

                    Label lblParamValue = e.Row.FindControl("lblParamValues") as Label;
                    if (!string.IsNullOrEmpty(lblParamValue.Text))
                        ddlValues.SelectedValue = lblParamValue.Text;
                }
                else
                {
                    if (strMode == "Q")
                    {
                        txtValues.Enabled = false;
                        ddlValues.Enabled = false;
                    }
                    else
                    {
                        txtValues.Enabled = true;
                        ddlValues.Enabled = true;
                    }

                    txtValues.Visible = true;
                    ddlValues.Visible = false;
                    string[] strLength = lblParamSize.Text.Trim().Split(',');
                    txtValues.MaxLength = Convert.ToInt32(strLength[0]);
                    if (lblParamType.Text.Trim() == "4")//Date
                    {
                        calAddValues.Format = strDateFormat;
                        calAddValues.Enabled = true;
                        fteValues.ValidChars = "/-";
                        fteValues.FilterType = AjaxControlToolkit.FilterTypes.Custom | AjaxControlToolkit.FilterTypes.Numbers | AjaxControlToolkit.FilterTypes.LowercaseLetters | AjaxControlToolkit.FilterTypes.UppercaseLetters;
                    }
                    else
                    {
                        if (lblParamType.Text.Trim() == "1")//Number
                        {
                            fteValues.ValidChars = " .";
                            fteValues.FilterType = AjaxControlToolkit.FilterTypes.Numbers;
                        }
                        else
                        {
                            fteValues.ValidChars = " -.@";
                            fteValues.FilterType = AjaxControlToolkit.FilterTypes.Custom | AjaxControlToolkit.FilterTypes.Numbers | AjaxControlToolkit.FilterTypes.LowercaseLetters | AjaxControlToolkit.FilterTypes.UppercaseLetters;
                        }
                        calAddValues.Enabled = false;
                    }
                }


            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }

    }

    #endregion

    #region Bind Address XML

    protected string FunProBindAddressXML()
    {
        StringBuilder strbuXML = new StringBuilder();
        try
        {
            strbuXML.Append("<Root>");

            strbuXML.Append(" <Details Postal_Code='" + ucAddressDetailsSetup.PostalCode_Value + "' Post_Box_No='" + ucAddressDetailsSetup.PostBoxNo + "' Way_No='" + ucAddressDetailsSetup.WayNo
                            + "' House_No='" + ucAddressDetailsSetup.HouseNo + "' Block_No='" + ucAddressDetailsSetup.BlockNo + "' Flat_No='" + ucAddressDetailsSetup.FlatNo
                            + "' Land_Mark='" + ucAddressDetailsSetup.LandMark + "' Area_Sheik='" + ucAddressDetailsSetup.AreaSheik + "' Res_Phone_No='" + ucAddressDetailsSetup.ResidencePhoneNo
                            + "' Res_Fax_No='" + ucAddressDetailsSetup.ResidenceFaxNo + "' Mob_Phone_No='" + ucAddressDetailsSetup.MobilePhoneNo + "' Cont_Name='" + ucAddressDetailsSetup.ContactName
                            + "' Cont_No='" + ucAddressDetailsSetup.ContactNo + "' Off_Phone_No='" + ucAddressDetailsSetup.OfficePhoneNo + "' Off_Fax_No='" + ucAddressDetailsSetup.OfficeFaxNo
                            + "' Cust_Email='" + ucAddressDetailsSetup.CustomerEmail + "'/>");

            strbuXML.Append("</Root>");
            return strbuXML.ToString();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            return strbuXML.ToString();
        }
    }

    #endregion

    #region  Web Methods


    [System.Web.Services.WebMethod]
    public static string[] GetEmployeNameList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggestions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();
        Procparam.Clear();
        Procparam.Add("@Company_ID", System.Web.HttpContext.Current.Session["AutoSuggestCompanyID"].ToString());
        Procparam.Add("@PrefixText", prefixText);
        suggestions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_GET_EMPLOYEE_LIST", Procparam));
        return suggestions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetAddressPostalCodeList(String prefixText, int count)
    {

        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        DataTable dtCommon = new DataTable();

        Procparam.Add("@Company_ID", System.Web.HttpContext.Current.Session["AutoSuggestCompanyID"].ToString());

        Procparam.Add("@Type", "1");

        Procparam.Add("@PrefixText", prefixText.Trim());
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_SYSAD_GET_ADD_SETUP_LOOKUP", Procparam));
        return suggetions.ToArray();

    }

    [System.Web.Services.WebMethod]
    public static string[] GetDealerGroupList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggestions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();
        Procparam.Clear();
        Procparam.Add("@Company_ID", System.Web.HttpContext.Current.Session["AutoSuggestCompanyID"].ToString());
        Procparam.Add("@Type", "D");
        Procparam.Add("@PrefixText", prefixText);
        suggestions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_GET_DLR_SUPLIER_GROUP_LIST", Procparam));
        return suggestions.ToArray();

    }

    [System.Web.Services.WebMethod]
    public static string[] GetSupplierGroupList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggestions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();
        Procparam.Clear();
        Procparam.Add("@Company_ID", System.Web.HttpContext.Current.Session["AutoSuggestCompanyID"].ToString());
        Procparam.Add("@Type", "S");
        Procparam.Add("@PrefixText", prefixText);
        suggestions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_GET_DLR_SUPLIER_GROUP_LIST", Procparam));
        return suggestions.ToArray();

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
    public static string[] GetCityList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
        Procparam.Add("@Category", "1");
        Procparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_SYSAD_GetAddressLoodup_AGT", Procparam), false);

        return suggetions.ToArray();
    }

    #endregion

    #region Text Changed Events


    protected void txtEffectiveFrom_TextChanged(object sender, EventArgs e)
    {
        try
        {
            FunDateValidation(1);
            txtEffectiveTo.Focus();//Added by Suseela
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }

    protected void txtEffectiveTo_TextChanged(object sender, EventArgs e)
    {
        FunDateValidation(2);
        txtEffectiveTo.Focus();//Added by Suseela
    }

    protected void ddlGLPostingCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlGLPostingCode.SelectedIndex > 0)
            {
                Dictionary<string, string> Procparam = new Dictionary<string, string>();
                Procparam.Clear();
                Procparam.Add("@Company_Id", Convert.ToString(intCompanyId));
                Procparam.Add("@GL_Code", ddlGLPostingCode.SelectedValue);
                Procparam.Add("@Is_Active", "1");
                Procparam.Add("@Is_From_Entity", "1");
                ddlSLPostingCode.BindDataTable("S3G_GET_GLSL_CODELISTS", Procparam, new string[] { "SL_Code", "SL_Account_Code" });
                if (ddlSLPostingCode.Items.Count > 1)
                {
                    ddlSLPostingCode.Enabled = true;
                }
                else
                    ddlSLPostingCode.Enabled = false;

            }
            else
            {
                ddlSLPostingCode.Items.Clear();
                ddlSLPostingCode.Items.Insert(0, "--Select--");
            }

            ddlGLPostingCode.Focus();//Added by Suseela
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }

    }

    #endregion

    #region Button Events

    protected void btnAddAddress_Click(object sender, EventArgs e)
    {
        try
        {

            if (ucAddressDetailsSetup.IsPostalCode_Mandatory)
            {
                if (ucAddressDetailsSetup.PostalCode_Text == "" || ucAddressDetailsSetup.PostalCode_Text == string.Empty)
                {
                    //Utility.FunShowAlertMsg(this.Page, "Select the " + ucBasicDetAddressSetup.PostalCode_Label);
                    Utility.FunShowValidationMsg(this, "SA_COM", 1, "", "", "", false, ucAddressDetailsSetup.PostalCode_Label, true);
                    return;
                }
            }

            DataRow drDetails;
            DataTable dtAddressDetails = (DataTable)ViewState["AddressDetails"];

            //if (txtMICRCode.Text.Trim().Length != 9)
            //{
            //    Utility.FunShowAlertMsg(this, "MICR Code should be 9 digits");
            //    return;
            //}
            //int intRowcount;
            //if (gvAddressDetails.Rows.Count == 0)
            //{
            //    intRowcount = 1;
            //}
            //else
            //{
            //    intRowcount = gvAddressDetails.Rows.Count + 1;
            //}

            if (dtAddressDetails.Rows.Count < 10)
            {
                drDetails = dtAddressDetails.NewRow();
                string strEntAddressMapId = Convert.ToString(dtAddressDetails.Rows.Count + 1);
                drDetails["Entity_Add_Mapping_ID"] = strEntAddressMapId;
                drDetails["Address_ID"] = "0";
                drDetails["Branch_Code"] = txtBranch_Code.Text.Trim();
                drDetails["Branch_Name"] = txtBranch_Name.Text.Trim();
                drDetails["Postal_Code_Value"] = ucAddressDetailsSetup.PostalCode_Value;
                drDetails["Postal_Code_Text"] = ucAddressDetailsSetup.PostalCode_Text;
                drDetails["Post_Box_No"] = ucAddressDetailsSetup.PostBoxNo.Trim();
                drDetails["Way_No"] = ucAddressDetailsSetup.WayNo.ToUpper().Trim();
                drDetails["House_No"] = ucAddressDetailsSetup.HouseNo.ToUpper().Trim();
                drDetails["Block_No"] = ucAddressDetailsSetup.BlockNo.Trim();
                drDetails["Flat_No"] = ucAddressDetailsSetup.FlatNo.Trim();
                drDetails["Landmark"] = ucAddressDetailsSetup.LandMark.ToUpper().Trim();
                drDetails["Area_Sheik"] = ucAddressDetailsSetup.AreaSheik;
                drDetails["Residence_Phone_No"] = ucAddressDetailsSetup.ResidencePhoneNo.Trim();
                drDetails["Residence_Fax_No"] = ucAddressDetailsSetup.ResidenceFaxNo.Trim();
                drDetails["Mobile_No"] = ucAddressDetailsSetup.MobilePhoneNo.Trim();
                drDetails["Contact_Name"] = ucAddressDetailsSetup.ContactName.ToUpper().Trim();
                drDetails["Contact_No"] = ucAddressDetailsSetup.ContactNo.Trim();
                drDetails["Office_Phone_No"] = ucAddressDetailsSetup.OfficePhoneNo.Trim();
                drDetails["Office_Fax_No"] = ucAddressDetailsSetup.OfficeFaxNo.Trim();
                drDetails["Email"] = ucAddressDetailsSetup.CustomerEmail.ToUpper().Trim();

                dtAddressDetails.Rows.Add(drDetails);
                gvAddressDetails.DataSource = dtAddressDetails;
                gvAddressDetails.DataBind();
                gvAddressDetails.Visible = true;

                if (ddlEntityType.SelectedItem.ToString().ToUpper() == "INSURANCE COMPANY")
                {
                    gvAddressDetails.Columns[1].Visible = true;
                    gvAddressDetails.Columns[2].Visible = true;
                }
                else
                {
                    gvAddressDetails.Columns[1].Visible = false;
                    gvAddressDetails.Columns[2].Visible = false;
                }

                ViewState["AddressDetails"] = dtAddressDetails;
                FunClearAddressDetails();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Class", "alert('Cannot add more than 10 Rows');", true);
            }
            btnAddAddress.Focus();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }

    protected void btnModifyAddress_Click(object sender, EventArgs e)
    {
        try
        {
            if (ucAddressDetailsSetup.IsPostalCode_Mandatory)
            {
                if (ucAddressDetailsSetup.PostalCode_Text == "" || ucAddressDetailsSetup.PostalCode_Text == string.Empty)
                {

                    //Utility.FunShowAlertMsg(this.Page, "Select the " + ucBasicDetAddressSetup.PostalCode_Label);
                    Utility.FunShowValidationMsg(this, "SA_COM", 1, "", "", "", false, ucAddressDetailsSetup.PostalCode_Label, true);
                    return;
                }
            }


            DataTable dtAddressDetails = (DataTable)ViewState["AddressDetails"];
            DataView dvAddressdetails = dtAddressDetails.DefaultView;
            //dvAddressdetails.Sort = "Address_ID"; 
            dvAddressdetails.Sort = "Entity_Add_Mapping_ID";
            int rowindex = dvAddressdetails.Find(hdnAddressId.Value);
            dvAddressdetails[rowindex].Row["Branch_Code"] = txtBranch_Code.Text.Trim();
            dvAddressdetails[rowindex].Row["Branch_Name"] = txtBranch_Name.Text.Trim();

            dvAddressdetails[rowindex].Row["Postal_Code_Value"] = ucAddressDetailsSetup.PostalCode_Value;
            dvAddressdetails[rowindex].Row["Postal_Code_Text"] = ucAddressDetailsSetup.PostalCode_Text;
            dvAddressdetails[rowindex].Row["Post_Box_No"] = ucAddressDetailsSetup.PostBoxNo;
            dvAddressdetails[rowindex].Row["Way_No"] = ucAddressDetailsSetup.WayNo.ToUpper().Trim();
            dvAddressdetails[rowindex].Row["House_No"] = ucAddressDetailsSetup.HouseNo.ToUpper().Trim();
            dvAddressdetails[rowindex].Row["Block_No"] = ucAddressDetailsSetup.BlockNo.ToUpper().Trim();
            dvAddressdetails[rowindex].Row["Flat_No"] = ucAddressDetailsSetup.FlatNo.Trim();
            dvAddressdetails[rowindex].Row["Landmark"] = ucAddressDetailsSetup.LandMark.ToUpper().Trim();
            dvAddressdetails[rowindex].Row["Area_Sheik"] = ucAddressDetailsSetup.AreaSheik;
            dvAddressdetails[rowindex].Row["Residence_Phone_No"] = ucAddressDetailsSetup.ResidencePhoneNo.Trim();
            dvAddressdetails[rowindex].Row["Residence_Fax_No"] = ucAddressDetailsSetup.ResidenceFaxNo.Trim();
            dvAddressdetails[rowindex].Row["Mobile_No"] = ucAddressDetailsSetup.MobilePhoneNo.Trim();
            dvAddressdetails[rowindex].Row["Contact_Name"] = ucAddressDetailsSetup.ContactName.ToUpper().Trim();
            dvAddressdetails[rowindex].Row["Contact_No"] = ucAddressDetailsSetup.ContactNo.Trim();
            dvAddressdetails[rowindex].Row["Office_Phone_No"] = ucAddressDetailsSetup.OfficePhoneNo.Trim();
            dvAddressdetails[rowindex].Row["Office_Fax_No"] = ucAddressDetailsSetup.OfficeFaxNo.Trim();
            dvAddressdetails[rowindex].Row["Email"] = ucAddressDetailsSetup.CustomerEmail.ToUpper().Trim();

            gvAddressDetails.DataSource = dvAddressdetails;
            gvAddressDetails.DataBind();

            ViewState["AddressDetails"] = dvAddressdetails.Table;

            btnAddAddress.Enabled_True();
            btnModifyAddress.Visible = false;

            txtBranch_Code.Clear();
            txtBranch_Name.Clear();
            ucAddressDetailsSetup.ClearControls();

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }

    #endregion

    //protected void hlnkView_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        FunPriShowPRDD(sender);
    //    }
    //    catch (Exception ex)
    //    {
    //        ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
    //    }
    //}
    [System.Web.Services.WebMethod]
    public static string[] GetDealerList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggestions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();
        Procparam.Clear();
        Procparam.Add("@Company_ID", System.Web.HttpContext.Current.Session["AutoSuggestCompanyID"].ToString());
        Procparam.Add("@PrefixText", prefixText);
        suggestions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_GET_DEALER_LIST", Procparam));
        return suggestions.ToArray();

    }

    [System.Web.Services.WebMethod]
    public static string[] GetBankList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggestions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();
        Procparam.Clear();
        Procparam.Add("@Company_ID", Convert.ToString(System.Web.HttpContext.Current.Session["AutoSuggestCompanyID"]));
        Procparam.Add("@Program_ID", Convert.ToString(System.Web.HttpContext.Current.Session["AutoProgramID"]));
        Procparam.Add("@OPTION", "2");
        if (System.Web.HttpContext.Current.Session["AutoStrMode"] != null && Convert.ToString(System.Web.HttpContext.Current.Session["AutoStrMode"]) != string.Empty)
            Procparam.Add("@MODE", Convert.ToString(System.Web.HttpContext.Current.Session["AutoStrMode"]));
        Procparam.Add("@Prefix", prefixText);
        suggestions = Utility.GetSuggestions(Utility.GetDefaultData(SPNames.S3G_Get_User_List, Procparam));
        return suggestions.ToArray();
    }


}
