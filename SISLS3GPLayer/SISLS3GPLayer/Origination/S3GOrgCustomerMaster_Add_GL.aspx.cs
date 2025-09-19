#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Orgination 
/// Screen Name			: Customer Master
/// Created By			: Prabhu K
/// Created Date		: 22-July-2010
/// Purpose	            : To Add/Edit the Customer
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
using System.Globalization;
using System.Configuration;
using System.Web;
using System.Security.Permissions;
using System.Reflection;
using System.IO;
using System.Text.RegularExpressions;
#endregion

public partial class Origination_S3GOrgCustomerMaster_Add_GL : ApplyThemeForProject
{
    #region Initialization

    #region Variables
    int intCompanyId = 0;
    int intUserId = 0;
    int intCustomerId = 0;
    string strCustomerCode;
    Dictionary<string, string> Procparam = null;
    //User Authorization
    string strMode = string.Empty;
    bool bClearList = false;
    bool bCreate = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bMakerChecker = false;
    //Code end
    public string strDateFormat;
    string strKey = "Insert";
    static string strPageName = "Customer Master";
    string strAlert = "alert('__ALERT__');";
    string strRedirectPageView = "window.location.href='../Origination/S3GOrgCustomerMaster_View.aspx?qsType=GL';";
    string strRedirectPageAdd = "window.location.href='../Origination/S3GOrgCustomerMaster_Add.aspx';";
    public static Origination_S3GOrgCustomerMaster_Add_GL obj_Page;
    #endregion

    #region Objects
    S3GSession ObjS3GSession;
    UserInfo ObjUserInfo;
    StringBuilder strbBnkDetails = new StringBuilder();
    #endregion

    #endregion

    #region PageLoad

    /// <summary>
    /// Event for Display the UI to Add/Edit the Customer.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {


            if (!IsPostBack)
            {
                FunProIntializeData();
                // FunPriLoadRelationType();
            }

            obj_Page = this;

            this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);
            ObjUserInfo = new UserInfo();
            ObjS3GSession = new S3GSession();
            strDateFormat = ObjS3GSession.ProDateFormatRW;
            intCompanyId = ObjUserInfo.ProCompanyIdRW;
            intUserId = ObjUserInfo.ProUserIdRW;
            ftxtBankAddress.ValidChars += "\n";
            ftxtBankBranch.ValidChars += "\n";
            //User Authorization
            bCreate = ObjUserInfo.ProCreateRW;
            bModify = ObjUserInfo.ProModifyRW;
            bQuery = ObjUserInfo.ProViewRW;
            //Code end

            //<<Performance>>
            FunSetComboBoxAttributes(txtComCity.TextBox, "City", "30");
            FunSetComboBoxAttributes(txtComState, "State", "60");
            FunSetComboBoxAttributes(txtComCountry, "Country", "60");

            //<<Performance>>
            FunSetComboBoxAttributes(txtPerCity.TextBox, "City", "30");
            FunSetComboBoxAttributes(txtPerState, "State", "60");
            FunSetComboBoxAttributes(txtPerCountry, "Country", "60");

            if (Request.QueryString["qsCustomerId"] != null)
            {
                FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsCustomerId"));
                if (fromTicket != null)
                {
                    intCustomerId = Convert.ToInt32(fromTicket.Name);
                    ViewState["CustomerId"] = intCustomerId;
                }
                else
                {
                    strAlert = strAlert.Replace("__ALERT__", "Invalid Customer Details");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                }
                strMode = Request.QueryString["qsMode"];
            }
            if (ViewState["CustomerId"] != null)
            {
                intCustomerId = Convert.ToInt32(ViewState["CustomerId"]);
            }
            txtPercentageStake.SetDecimalPrefixSuffix(2, 2, false, false, "Promoters Stake %");
            txtJVPartnerStake.SetDecimalPrefixSuffix(2, 2, false, false, "JV Partner Stake %");
            if (ObjUserInfo.ProCountryNameR.Trim().ToLower() == "india")
            {
                ftxtMICRCode.FilterType = AjaxControlToolkit.FilterTypes.Numbers;
                txtMICRCode.MaxLength = 9;
            }
            else
            {
                ftxtMICRCode.FilterType = AjaxControlToolkit.FilterTypes.Custom | AjaxControlToolkit.FilterTypes.Numbers | AjaxControlToolkit.FilterTypes.LowercaseLetters | AjaxControlToolkit.FilterTypes.UppercaseLetters;
                txtMICRCode.MaxLength = 10;
            }
            if (ObjS3GSession.ProPINCodeDigitsRW > 0)
            {
                txtComPincode.MaxLength = txtPerPincode.MaxLength = ObjS3GSession.ProPINCodeDigitsRW;
                if (ObjS3GSession.ProPINCodeTypeRW.ToUpper() == "ALPHA NUMERIC")
                {
                    ftxtComPincode.FilterType = AjaxControlToolkit.FilterTypes.Numbers | AjaxControlToolkit.FilterTypes.Custom | AjaxControlToolkit.FilterTypes.UppercaseLetters | AjaxControlToolkit.FilterTypes.LowercaseLetters;
                    ftxtPerPincode.FilterType = AjaxControlToolkit.FilterTypes.Numbers | AjaxControlToolkit.FilterTypes.Custom | AjaxControlToolkit.FilterTypes.UppercaseLetters | AjaxControlToolkit.FilterTypes.LowercaseLetters;
                }
                else
                {
                    ftxtComPincode.FilterType = AjaxControlToolkit.FilterTypes.Numbers;
                    ftxtPerPincode.FilterType = AjaxControlToolkit.FilterTypes.Numbers;
                }
            }
            if (!IsPostBack)
            {
                FunPriSetControlSettings();

                CalendarCEOWeddingDate.Format = ObjS3GSession.ProDateFormatRW;
                CalendarExtenderSD.Format = ObjS3GSession.ProDateFormatRW;
                CalendarValidupto.Format = ObjS3GSession.ProDateFormatRW;
                CalendarWeddingdate.Format = ObjS3GSession.ProDateFormatRW;
                if (strMode != "Q")
                {
                    FunPriLoadMasterData();
                    FunProLoadAddressCombos();
                }
                //User Authorization
                bClearList = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ClearListValues"));
                if (intCustomerId > 0)
                {

                    if (strMode == "M")
                    {
                        FunPriDisableControls(1);
                        if (ddlCustomerType.SelectedItem.ToString().ToLower() == "non individual")
                        {
                            rfvComEmail.Enabled = true;
                            rfvPerEmail.Enabled = true;
                        }
                        else
                        {
                            txtIndustryName.Text = "";
                            cmbIndustryCode.Text = "";
                            rfvComEmail.Enabled = false;
                            rfvPerEmail.Enabled = false;
                        }
                    }
                    else
                    {
                        FunPriDisableControls(-1);
                    }

                }
                else
                {
                    FunPriDisableControls(0);
                }

                if (Request.QueryString["IsFromEnquiry"] != null || Request.QueryString["IsFromApplication"] != null)
                {
                    chkCoApplicant.Enabled = chkGuarantor1.Enabled = chkGuarantor2.Enabled = false;
                }

            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException, strPageName);
            cvCustomerMaster.ErrorMessage = Resources.LocalizationResources.PageLoadError;
            cvCustomerMaster.IsValid = false;
        }
    }

    protected void FunSetComboBoxAttributes(TextBox textBox, string Type, string maxLength)
    {
        if (textBox != null)
        {
            textBox.Attributes.Add("onkeyup", "maxlengthfortxt('" + maxLength + "');fnCheckSpecialChars('true');");
            textBox.Attributes.Add("onblur", "funCheckFirstLetterisNumeric(this, '" + Type + "');");
            textBox.Attributes.Add("onpaste", "return false");
        }
    }

    protected void FunSetComboBoxAttributes(AjaxControlToolkit.ComboBox cmb, string Type, string maxLength)
    {
        TextBox textBox = cmb.FindControl("TextBox") as TextBox;

        if (textBox != null)
        {
            textBox.Attributes.Add("onkeyup", "maxlengthfortxt('" + maxLength + "');fnCheckSpecialChars('true');");
            textBox.Attributes.Add("onblur", "funCheckFirstLetterisNumeric(this, '" + Type + "');");
            textBox.Attributes.Add("onpaste", "return false");
        }
    }

    //<<Performance>>
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
            if (dtAddr.Select("Category = 1").Length > 0)
            {
                dtSource = dtAddr.Select("Category = 1").CopyToDataTable();
            }
            else
            {
                dtSource = FunProAddAddrColumns(dtSource);
            }
            //txtComCity.FillDataTable(dtSource, "Name", "Name", false);
            //txtPerCity.FillDataTable(dtSource, "Name", "Name", false);

            dtSource = new DataTable();
            if (dtAddr.Select("Category = 2").Length > 0)
            {
                dtSource = dtAddr.Select("Category = 2").CopyToDataTable();
            }
            else
            {
                dtSource = FunProAddAddrColumns(dtSource);
            }
            txtComState.FillDataTable(dtSource, "Name", "Name", false);
            txtPerState.FillDataTable(dtSource, "Name", "Name", false);

            dtSource = new DataTable();
            if (dtAddr.Select("Category = 3").Length > 0)
            {
                dtSource = dtAddr.Select("Category = 3").CopyToDataTable();
            }
            else
            {
                dtSource = FunProAddAddrColumns(dtSource);
            }
            txtComCountry.FillDataTable(dtSource, "Name", "Name", false);
            txtPerCountry.FillDataTable(dtSource, "Name", "Name", false);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            cvCustomerMaster.ErrorMessage = ex.Message;
            cvCustomerMaster.IsValid = false;
        }
    }

    protected DataTable FunProAddAddrColumns(DataTable dt)
    {
        dt.Columns.Add("ID");
        dt.Columns.Add("Name");
        dt.Columns.Add("Category");

        return dt;
    }

    /// <summary>
    /// Event for Pre-Initialize the Components  
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected new void Page_PreInit(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString["IsFromEnquiry"] != null || Request.QueryString["IsFromApplication"] != null)
            {
                this.Page.MasterPageFile = "~/Common/MasterPage.master";
                UserInfo ObjUserInfo = new UserInfo();
                this.Page.Theme = ObjUserInfo.ProUserThemeRW;
            }
            else
            {
                this.Page.MasterPageFile = "~/Common/S3GMasterPageCollapse.master";
                UserInfo ObjUserInfo = new UserInfo();
                this.Page.Theme = ObjUserInfo.ProUserThemeRW;
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException, strPageName);
            cvCustomerMaster.ErrorMessage = "Unable to Initialize the Customer Details";
            cvCustomerMaster.IsValid = false;
        }
    }


    #endregion

    #region Button Events

    /// <summary>
    /// Event for Insert/Update the CustomerDetails
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string strDocName = "";
            string strConstitutionAlert = "";
            string strDeDupCustomerCode = "";
            if (FunPriValidateDeDuplicateCustomer(out strDocName, out strDeDupCustomerCode))
            {
                strDocName = strDocName.Substring(0, strDocName.Length - 1);
                strConstitutionAlert = "alert('__ALERT__');";
                if (strDeDupCustomerCode != "")
                {
                    strDeDupCustomerCode = strDeDupCustomerCode.Substring(0, strDeDupCustomerCode.Length - 1);
                    strDeDupCustomerCode.Replace(",", @"\n");
                    strConstitutionAlert = strDocName + @" already Exist for following Customer \n" + strDeDupCustomerCode;
                }
                else
                {
                    strConstitutionAlert = strDocName + @" already Exist for another Customer";
                }
                strConstitutionAlert += @"\n\nWould you like to proceed?";
                strConstitutionAlert = "if(confirm('" + strConstitutionAlert + "')){ document.getElementById('ctl00_ContentPlaceHolder1_btnProceedSave').click();}else{;" + strRedirectPageView + "}";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Cusotmer Master", strConstitutionAlert, true);
                return;
            }
            strDocName = "";
            if (FunPriValidateDuplicateConstitutionDocs(out strDocName))
            {
                strDocName = strDocName.Substring(0, strDocName.Length - 1);
                strConstitutionAlert = "alert('__ALERT__');";
                strConstitutionAlert = strDocName + " already Exist for another Customer";
                strAlert = strAlert.Replace("__ALERT__", strConstitutionAlert);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Customer Master", strAlert, true);
                //strConstitutionAlert += @"\n\nWould you like to proceed?";
                //strConstitutionAlert = "if(confirm('" + strConstitutionAlert + "')){ document.getElementById('ctl00_ContentPlaceHolder1_btnProceedSave').click();}else{;" + strRedirectPageView + "}";
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "Cusotmer Master", strConstitutionAlert, true);
                return;
            }

            if (ObjS3GSession.ProPINCodeDigitsRW != txtComPincode.Text.Trim().Length)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Customer Details", "alert('Communication address PIN should be " + ObjS3GSession.ProPINCodeDigitsRW.ToString() + " digits');", true);
                tcCustomerMaster.ActiveTab = tbAddress;
                return;
            }
            else if (ObjS3GSession.ProPINCodeDigitsRW != txtPerPincode.Text.Trim().Length)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Customer Details", "alert('Permanent address PIN should be " + ObjS3GSession.ProPINCodeDigitsRW.ToString() + " digits');", true);
                tcCustomerMaster.ActiveTab = tbAddress;
                return;
            }

            //if (ddlCustomerType.SelectedItem.Text.ToUpper() != "INDIVIDUAL")
            //{
            //    if (ddlPublic.SelectedIndex == 0)
            //    {
            //        ScriptManager.RegisterStartupScript(this, this.GetType(), "Customer Details", "alert('Select the Public/Closely held');", true);
            //        tcCustomerMaster.ActiveTabIndex = 1;
            //        return;

            //    }
            //    if (txtDirectors.Text == "")
            //    {
            //        ScriptManager.RegisterStartupScript(this, this.GetType(), "Customer Details", "alert('Enter the No.of Directors/Partners');", true);
            //        tcCustomerMaster.ActiveTabIndex = 1;
            //        return;

            //    }
            //    if (txtBusinessProfile.Text == "")
            //    {
            //        ScriptManager.RegisterStartupScript(this, this.GetType(), "Customer Details", "alert('Enter the Business Profile');", true);
            //        tcCustomerMaster.ActiveTabIndex = 1;
            //        return;

            //    }
            //    if (txtGeographical.Text == "")
            //    {
            //        ScriptManager.RegisterStartupScript(this, this.GetType(), "Customer Details", "alert('Enter the Geographical Coverage');", true);
            //        tcCustomerMaster.ActiveTabIndex = 1;
            //        return;

            //    }

            //    if (txtnobranch.Text == "")
            //    {
            //        ScriptManager.RegisterStartupScript(this, this.GetType(), "Customer Details", "alert('Enter the No.of Branch');", true);
            //        tcCustomerMaster.ActiveTabIndex = 1;
            //        return;

            //    }
            //    if (txtCEOName.Text == "")
            //    {
            //        ScriptManager.RegisterStartupScript(this, this.GetType(), "Customer Details", "alert('Enter the CEO Name');", true);
            //        tcCustomerMaster.ActiveTabIndex = 1;
            //        return;

            //    }
            //    if (txtCEOAge.Text == "")
            //    {
            //        ScriptManager.RegisterStartupScript(this, this.GetType(), "Customer Details", "alert('Enter the Age of CEO');", true);
            //        tcCustomerMaster.ActiveTabIndex = 1;
            //        return;

            //    }
            //    if (txtCEOexperience.Text == "")
            //    {
            //        ScriptManager.RegisterStartupScript(this, this.GetType(), "Customer Details", "alert('Enter the Experience');", true);
            //        tcCustomerMaster.ActiveTabIndex = 1;
            //        return;

            //    }
            //    if (txtResidentialAddress.Text == "")
            //    {
            //        ScriptManager.RegisterStartupScript(this, this.GetType(), "Customer Details", "alert('Enter the Residential Address');", true);
            //        tcCustomerMaster.ActiveTabIndex = 1;
            //        return;

            //    }
            //}
            //if (ddlCustomerType.SelectedItem.Text.ToUpper() == "INDIVIDUAL")
            //{
            //    if (txtDOB.Text == "")
            //    {
            //        ScriptManager.RegisterStartupScript(this, this.GetType(), "Customer Details", "alert('Enter the Date of Birth');", true);
            //        tcCustomerMaster.ActiveTabIndex = 1;
            //        return;
            //    }
            //    if (txtQualification.Text == "")
            //    {
            //        ScriptManager.RegisterStartupScript(this, this.GetType(), "Customer Details", "alert('Enter the Qualification');", true);
            //        tcCustomerMaster.ActiveTabIndex = 1;
            //        return;

            //    }
            //    if (txtProfession.Text == "")
            //    {
            //        ScriptManager.RegisterStartupScript(this, this.GetType(), "Customer Details", "alert('Enter the Profession');", true);
            //        tcCustomerMaster.ActiveTabIndex = 1;
            //        return;

            //    }
            //    if (ddlHouseType.SelectedIndex == 0)
            //    {
            //        ScriptManager.RegisterStartupScript(this, this.GetType(), "Customer Details", "alert('Select the House/Flat');", true);
            //        tcCustomerMaster.ActiveTabIndex = 1;
            //        return;

            //    }

            //    if (ddlOwn.SelectedIndex == 0)
            //    {
            //        ScriptManager.RegisterStartupScript(this, this.GetType(), "Customer Details", "alert('Select the Own');", true);
            //        tcCustomerMaster.ActiveTabIndex = 1;
            //        return;

            //    }
            //    if (ddlOwn.SelectedItem.Text.ToLower() == "yes")
            //    {
            //        if (txtCurrentMarketValue.Text == "")
            //        {
            //            ScriptManager.RegisterStartupScript(this, this.GetType(), "Customer Details", "alert('Enter the Current Market Value');", true);
            //            tcCustomerMaster.ActiveTabIndex = 1;
            //            return;

            //        }
            //    }

            //}
            //if (ddlCustomerType.SelectedItem.Text.ToUpper() != "INDIVIDUAL" && txtResidentialAddress.Text != "" && txtResidentialAddress.Text.Length > 300)
            //{
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Customer Details", "alert('Residential Address Length should be less than or equal to 300');", true);
            //    tcCustomerMaster.ActiveTabIndex = 1;
            //    return;

            //}
            if (ViewState["ConsDocPath"] == null)
            {
                Utility.FunShowAlertMsg(this, "Define the spooling path in Document Path Setup for Constitution Document");
                return;
            }
            if (ViewState["ConsDocPath"] != null)
            {
                if (Convert.ToString(ViewState["ConsDocPath"]) == "")
                {
                    Utility.FunShowAlertMsg(this, "Define the spooling path in Document Path Setup for Constitution Document");
                    return;
                }
            }

            foreach (GridViewRow grv in gvConstitutionalDocuments.Rows)
            {
                AjaxControlToolkit.AsyncFileUpload AsyncFileUpload1 = (AjaxControlToolkit.AsyncFileUpload)grv.FindControl("asyFileUpload");
                TextBox txOD = grv.FindControl("txOD") as TextBox;
                LinkButton hlnkView = grv.FindControl("hlnkView") as LinkButton;
                CheckBox chkIsMandatory = (CheckBox)grv.FindControl("chkIsMandatory");
                CheckBox chkIsNeedImageCopy = (CheckBox)grv.FindControl("chkIsNeedImageCopy");
                CheckBox chkCollected = (CheckBox)grv.FindControl("chkCollected");
                CheckBox chkScanned = (CheckBox)grv.FindControl("chkScanned");
                TextBox txtValues = grv.FindControl("txtValues") as TextBox;

                string fileExtension = AsyncFileUpload1.FileName.Substring(AsyncFileUpload1.FileName.LastIndexOf('.') + 1);
                if (fileExtension != "" && fileExtension.ToLower() != "bmp" && fileExtension.ToLower() != "jpeg" && fileExtension.ToLower() != "jpg" && fileExtension.ToLower() != "gif" && fileExtension.ToLower() != "png" && fileExtension.ToLower() != "pdf")
                {
                    Utility.FunShowAlertMsg(this, "Image/PDF file only can be uploaded. Check the file format of " + AsyncFileUpload1.FileName.ToString() + "");
                    return;
                }

                if (chkIsMandatory.Checked && !chkCollected.Checked)
                {
                    Utility.FunShowAlertMsg(this, "All the mandatory documents should be collected.");
                    tcCustomerMaster.ActiveTab = TabConstitution;
                    return;
                }
                //if (chkIsNeedImageCopy.Checked && (!chkScanned.Checked || (!hlnkView.Enabled && !AsyncFileUpload1.HasFile)))
                if (chkIsNeedImageCopy.Checked && (!hlnkView.Enabled && !AsyncFileUpload1.HasFile))
                {
                    Utility.FunShowAlertMsg(this, "All the mandatory documents should be Uploaded.");
                    tcCustomerMaster.ActiveTab = TabConstitution;
                    return;
                }

                if (chkCollected.Checked && (grv.Cells[1].Text.ToString().StartsWith("CID")) && txtValues.Text.Trim() == string.Empty)
                {
                    Utility.FunShowAlertMsg(this, "All the Collected documents values should be entered.");
                    tcCustomerMaster.ActiveTab = TabConstitution;
                    return;
                }
                
                if (grv.Cells[1].Text.ToString().ToUpper() == "CID-PTN")
                {
                    if (ObjS3GSession.ProCompanyCountry.Trim().ToLower() == "india" && txtValues.Text.Trim() != "")
                    {
                        string strExp = @"^([a-zA-Z]){5}([0-9]){4}([a-zA-Z]){1}$";
                        if (!Regex.IsMatch(txtValues.Text.Trim(), strExp))
                        {
                            Utility.FunShowAlertMsg(this, "Permanent Tax Number should be in format of AAAAA9999A");
                            tcCustomerMaster.ActiveTab = TabConstitution;
                            txtValues.Focus();
                            return;
                        }
                    }
                }
            }

            FunPriUploadFiles();
            FunPriSaveCustomer();
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException, strPageName);
            if (intCustomerId > 0)
            {
                cvCustomerMaster.ErrorMessage = Resources.LocalizationResources.UpdateError;
            }
            else
            {
                cvCustomerMaster.ErrorMessage = Resources.LocalizationResources.InsertError;
            }
            cvCustomerMaster.IsValid = false;

        }
    }
    protected void btnProceedSave_Click(object sender, EventArgs e)
    {
        try
        {
            FunPriSaveCustomer();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            if (intCustomerId > 0)
            {
                cvCustomerMaster.ErrorMessage = Resources.LocalizationResources.UpdateError;
            }
            else
            {
                cvCustomerMaster.ErrorMessage = Resources.LocalizationResources.InsertError;
            }
            cvCustomerMaster.IsValid = false;
        }
    }
    private void FunPriSaveCustomer()
    {
        OrgMasterMgtServicesReference.OrgMasterMgtServicesClient objCustomerMasterClient = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
        try
        {
            if (cmbIndustryCode.Text == "")
            {
                cmbIndustryCode.Text = (0).ToString();
            }
            FunPriSetControlSettings();
            int intErrorCode = 0;
            CustomerMasterBusEntity ObjCustomerMaster = new CustomerMasterBusEntity();
            ObjCustomerMaster.ID = Convert.ToInt32(intCustomerId);
            ObjCustomerMaster.CustomerCode = txtCustomercode.Text.Trim();
            ObjCustomerMaster.CustomerType_ID = Convert.ToInt32(ddlCustomerType.SelectedValue);
            if (!string.IsNullOrEmpty(cmbGroupCode.Text.Trim())) ObjCustomerMaster.GroupCode = cmbGroupCode.Text.Trim();
            ObjCustomerMaster.Groupname = txtGroupName.Text.Trim();
            if (!string.IsNullOrEmpty(cmbIndustryCode.Text.Trim())) ObjCustomerMaster.IndustryCode = cmbIndustryCode.Text.Trim();
            ObjCustomerMaster.IndustryName = txtIndustryName.Text.Trim();
            if (!string.IsNullOrEmpty(ddlConstitutionName.Text.Trim())) ObjCustomerMaster.Constitution_ID = Convert.ToInt32(ddlConstitutionName.SelectedValue);
            ObjCustomerMaster.Title = ddlTitle.SelectedValue;
            ObjCustomerMaster.CustomerName = txtCustomerName.Text.Trim();
            if (!string.IsNullOrEmpty(ddlPostingGroup.SelectedValue)) ObjCustomerMaster.CustomerPostingGroupCode_ID = Convert.ToInt32(ddlPostingGroup.SelectedValue);
            ObjCustomerMaster.Comm_Address1 = txtComAddress1.Text.Trim();
            ObjCustomerMaster.Comm_Address2 = txtCOmAddress2.Text.Trim();
            //ObjCustomerMaster.Comm_City = txtComCity.Text.Trim();
            ObjCustomerMaster.Comm_City = txtComCity.SelectedText.Trim();
            ObjCustomerMaster.Comm_State = txtComState.Text.Trim();
            ObjCustomerMaster.Comm_Country = txtComCountry.Text.Trim();
            ObjCustomerMaster.Comm_PINCode = txtComPincode.Text.Trim();
            ObjCustomerMaster.Comm_Mobile = txtComMobile.Text.Trim();
            ObjCustomerMaster.Comm_Telephone = txtComTelephone.Text.Trim();
            ObjCustomerMaster.Comm_Email = txtComEmail.Text.Trim();
            ObjCustomerMaster.Comm_Website = txtComWebsite.Text.Trim();
            ObjCustomerMaster.Perm_Address1 = txtPerAddress1.Text.Trim();
            ObjCustomerMaster.Perm_Address2 = txtPerAddress2.Text.Trim();
            //ObjCustomerMaster.Perm_City = txtPerCity.SelectedItem.Text.Trim();
            ObjCustomerMaster.Perm_City = txtPerCity.SelectedText.Trim();
            ObjCustomerMaster.Perm_State = txtPerState.SelectedItem.Text.Trim();
            ObjCustomerMaster.Perm_Country = txtPerCountry.SelectedItem.Text.Trim();
            ObjCustomerMaster.Perm_PINCode = txtPerPincode.Text.Trim();
            ObjCustomerMaster.Perm_Mobile = txtPerMobile.Text.Trim();
            ObjCustomerMaster.Perm_Telephone = txtPerTelephone.Text.Trim();
            ObjCustomerMaster.Perm_Email = txtPerEmail.Text.Trim();
            ObjCustomerMaster.Perm_Website = txtPerWebSite.Text.Trim();
            ObjCustomerMaster.Gender = ddlGender.SelectedValue;
            if (!string.IsNullOrEmpty(txtDOB.Text.Trim()))
                ObjCustomerMaster.DateofBirth = Utility.StringToDate(txtDOB.Text);
            if (!string.IsNullOrEmpty(ddlMaritalStatus.Text.Trim())) ObjCustomerMaster.MaritalStatus_ID = Convert.ToInt32(ddlMaritalStatus.SelectedValue);
            ObjCustomerMaster.Qualification = txtQualification.Text.Trim();
            ObjCustomerMaster.Profession = txtProfession.Text.Trim();
            ObjCustomerMaster.SpouseName = txtSpouseName.Text.Trim();
            if (!string.IsNullOrEmpty(txtChildren.Text.Trim())) ObjCustomerMaster.Children = Convert.ToInt32(txtChildren.Text.Trim());
            if (!string.IsNullOrEmpty(txtTotalDependents.Text.Trim())) ObjCustomerMaster.TotalDependents = Convert.ToInt32(txtTotalDependents.Text.Trim());
            if (!string.IsNullOrEmpty(txtWeddingdate.Text.Trim())) ObjCustomerMaster.WeddingAnniversaryDate = Utility.StringToDate(txtWeddingdate.Text);
            if (!string.IsNullOrEmpty(ddlHouseType.Text.Trim())) ObjCustomerMaster.HouseORFlat_ID = Convert.ToInt32(ddlHouseType.SelectedValue);
            if (!string.IsNullOrEmpty(ddlOwn.Text.Trim())) ObjCustomerMaster.ISOwn = Convert.ToInt16(ddlOwn.SelectedValue);
            if (!string.IsNullOrEmpty(txtCurrentMarketValue.Text.Trim())) ObjCustomerMaster.CurrentMarketValue = txtCurrentMarketValue.Text.Trim();
            if (!string.IsNullOrEmpty(txtRemainingLoanValue.Text.Trim())) ObjCustomerMaster.RemainingLoanValue = txtRemainingLoanValue.Text.Trim();
            if (!string.IsNullOrEmpty(txtTotNetWorth.Text.Trim())) ObjCustomerMaster.TotalNetMorth = txtTotNetWorth.Text.Trim();
            if (!string.IsNullOrEmpty(ddlPublic.Text.Trim())) ObjCustomerMaster.PublicCloselyheld_ID = Convert.ToInt32(ddlPublic.SelectedValue);
            if (!string.IsNullOrEmpty(txtDirectors.Text.Trim())) ObjCustomerMaster.NoOfDirectors = Convert.ToInt32(txtDirectors.Text.Trim());
            ObjCustomerMaster.ListedAtStockExchange = txtSE.Text.Trim();
            if (!string.IsNullOrEmpty(txtPaidCapital.Text.Trim())) ObjCustomerMaster.PaidupCapital = Convert.ToDecimal(txtPaidCapital.Text.Trim());
            if (!string.IsNullOrEmpty(txtfacevalue.Text.Trim())) ObjCustomerMaster.FaceValueofShares = Convert.ToDecimal(txtfacevalue.Text.Trim());
            if (!string.IsNullOrEmpty(txtbookvalue.Text.Trim())) ObjCustomerMaster.BookValueofShares = Convert.ToDecimal(txtbookvalue.Text.Trim());
            ObjCustomerMaster.BusinessProfile = txtBusinessProfile.Text.Trim();
            ObjCustomerMaster.Geographicalcoverage = txtGeographical.Text.Trim();
            if (!string.IsNullOrEmpty(txtnobranch.Text.Trim())) ObjCustomerMaster.NoOfBranches = Convert.ToDecimal(txtnobranch.Text.Trim());
            if (!string.IsNullOrEmpty(ddlGovernment.Text.Trim())) ObjCustomerMaster.GovernmentInstitutionalParticipation_ID = Convert.ToInt32(ddlGovernment.SelectedValue);
            if (!string.IsNullOrEmpty(txtPercentageStake.Text.Trim())) ObjCustomerMaster.PercentageOfStake = Convert.ToDecimal(txtPercentageStake.Text.Trim());
            ObjCustomerMaster.JVPartnerName = txtJVPartnerName.Text.Trim();
            if (!string.IsNullOrEmpty(txtJVPartnerStake.Text.Trim())) ObjCustomerMaster.JVPartnerStake = Convert.ToDecimal(txtJVPartnerStake.Text.Trim());
            ObjCustomerMaster.CEOName = txtCEOName.Text.Trim();
            if (!string.IsNullOrEmpty(txtCEOAge.Text.Trim())) ObjCustomerMaster.CEOAge = Convert.ToDecimal(txtCEOAge.Text.Trim());
            if (!string.IsNullOrEmpty(txtCEOexperience.Text.Trim())) ObjCustomerMaster.CEOExperienceInYears = Convert.ToDecimal(txtCEOexperience.Text.Trim());
            if (!string.IsNullOrEmpty(txtCEOWeddingDate.Text.Trim())) ObjCustomerMaster.CEOWeddingDate = Utility.StringToDate(txtCEOWeddingDate.Text);
            ObjCustomerMaster.ResidentialAddress = txtResidentialAddress.Text.Trim();


            string strXMLBankDetails = ((DataTable)ViewState["DetailsTable"]).FunPubFormXml(true);
            strXMLBankDetails = strXMLBankDetails.Replace("OWN_BRANCH_ID='-1'", "");
            strXMLBankDetails = strXMLBankDetails.Replace("BRANCH='&nbsp;'", "");
            strXMLBankDetails = strXMLBankDetails.Replace("OWN_BRANCH_ID='&nbsp;'", "");
            ObjCustomerMaster.XmlBankDetails = strXMLBankDetails;

            if (!string.IsNullOrEmpty(txtValidupto.Text.Trim())) ObjCustomerMaster.ValidUpto = Utility.StringToDate(txtValidupto.Text);
            ObjCustomerMaster.Created_By = intUserId;
            ObjCustomerMaster.Modified_By = intUserId;
            ObjCustomerMaster.Company_ID = intCompanyId;
            ObjCustomerMaster.Customer = chkCustomer.Checked;
            ObjCustomerMaster.Guarantor1 = chkGuarantor1.Checked;
            ObjCustomerMaster.Guarantor2 = chkGuarantor2.Checked;
            ObjCustomerMaster.CoApplicant = chkCoApplicant.Checked;
            ObjCustomerMaster.XmlConstitutionalDocuments = gvConstitutionalDocuments.FunPubFormXml(true);

            //BCA Changes - Kuppu - Aug-17
            ObjCustomerMaster.Family_Name = txtFamilyName.Text.Trim();
            ObjCustomerMaster.Notes = txtNotes.Text.Trim();

            if (intCustomerId > 0)
            {
                ObjCustomerMaster.Mode = "Update";
                if (lblNoCustomerTrack.Visible)
                {
                    ObjCustomerMaster.XmlTrackDetails = "<Root></Root>";
                }
                else
                {
                    string strXML = gvTrack.FunPubFormXml(true);
                    strXML = strXML.Replace("RELEASEDBY='&nbsp;'", " ");
                    strXML = strXML.Replace("TYPE='-1'", " ");
                    strXML = strXML.Replace("RELEASEDATE=''", " ");
                    strXML = strXML.Replace("DATE=''", " ");
                    ObjCustomerMaster.XmlTrackDetails = strXML;
                }

            }
            else
            {
                ObjCustomerMaster.Mode = "Insert";
            }
            intErrorCode = objCustomerMasterClient.FunPubCreateCustomerInt(out strCustomerCode, ObjCustomerMaster);

            if (intErrorCode == 0)
            {

                if (intCustomerId > 0)
                {
                    strAlert = strAlert.Replace("__ALERT__", "Customer details updated successfully");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                }
                else
                {
                    string[] strCustomerDetails = strCustomerCode.Split('~');
                    txtCustomercode.Text = strCustomerDetails[1].ToString();
                    if (Request.QueryString["IsFromEnquiry"] != null || Request.QueryString["IsFromApplication"] != null)
                    {
                        string strCustomerAlert = "alert('Customer details added successfully');";
                        strCustomerAlert += "window.close();window.opener.location.reload();";
                        strRedirectPageView = "";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", strCustomerAlert, true);
                        Utility.store("EnqNewCustomerId", strCustomerDetails[0].ToString());
                        return;
                    }
                    else
                    {
                        strAlert = "Customer Code is " + txtCustomercode.Text;
                        strAlert += @"\n\nCustomer details added successfully";
                        strAlert += @"\n\nWould you like to add one more Customer?";
                        strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", strAlert, true);
                        strRedirectPageView = "";
                    }
                }
            }
            else if (intErrorCode == -1)
            {
                Utility.FunShowAlertMsg(this.Page, Resources.LocalizationResources.DocNoNotDefined);
            }
            else if (intErrorCode == -2)
            {
                Utility.FunShowAlertMsg(this.Page, Resources.LocalizationResources.DocNoExceeds);
            }
            else if (intErrorCode == -3)
            {
                //Added by Tamilselvan.S on 2/11/2011 for Adding validation for DNC number length
                Utility.FunShowAlertMsg(this.Page, Resources.ValidationMsgs._3.ToString());
            }
            else
            {
                if (intCustomerId > 0)
                {
                    strAlert = strAlert.Replace("__ALERT__", "Error in updating Customer details");
                }
                else
                {
                    strAlert = strAlert.Replace("__ALERT__", "Error in adding Customer details");
                }
                strRedirectPageView = "";
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            objCustomerMasterClient.Close();
        }
    }
    private bool FunPriValidateDuplicateConstitutionDocs(out string strConsDocName)
    {
        bool blnIsDuplicate = false;
        strConsDocName = "";
        try
        {
            Dictionary<string, string> Procparam = new Dictionary<string, string>();
            Procparam.Add("@Option", "309");
            Procparam.Add("@Param1", intCompanyId.ToString());
            DataSet dsConstitution = Utility.GetDataset("S3G_ORG_GetCustomerLookUp", Procparam);
            foreach (GridViewRow grDocs in gvConstitutionalDocuments.Rows)
            {
                string strID = grDocs.Cells[0].Text;
                string strDocumentFlag = grDocs.Cells[1].Text;
                string strDocName = grDocs.Cells[2].Text;
                TextBox txtValues = (TextBox)grDocs.FindControl("txtValues");
                if (txtValues != null && strDocumentFlag.ToUpper().StartsWith("CID"))
                {
                    if (txtValues.Text.Trim() != "")
                    {
                        string strCustomerCondition = "";
                        if (intCustomerId > 0)
                        {
                            strCustomerCondition = " and Customer_Id <> " + intCustomerId;
                        }
                        DataRow[] drDuplicateDoc = dsConstitution.Tables[0].Select("DocumentFlag = '" + strDocumentFlag + "' and Values = '" + txtValues.Text.ToUpper().Trim() + "'" + strCustomerCondition);
                        if (drDuplicateDoc.Length > 0)
                        {
                            strConsDocName += strDocName + ",";
                            blnIsDuplicate = true;
                        }
                    }
                }
            }
            //if (blnIsDuplicate)
            //{

            //}
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Validate Duplicate Documents");
        }
        return blnIsDuplicate;
    }

    private bool FunPriValidateDeDuplicateCustomer(out string strConsDocName, out string strDeDupCustomerCode)
    {
        bool blnIsDuplicate = false;
        strConsDocName = "";
        strDeDupCustomerCode = "";
        try
        {
            Dictionary<string, string> Procparam = new Dictionary<string, string>();
            Procparam.Add("@CONSTITUTIONID", ddlConstitutionName.SelectedValue);
            Procparam.Add("@COMPANYID", intCompanyId.ToString());
            DataSet dsConstitution = Utility.GetDataset("S3G_ORG_GETDEDUPPARAMETER", Procparam);
            DataRow[] drDeDupField = dsConstitution.Tables[0].Select("DeDup_Field <> '' and IsRequired = 1");
            bool IsDuplicateCustomerName = false;
            bool IsDuplicateAddress1 = false;
            bool IsDuplicateAddress2 = false;

            if (drDeDupField.Length > 0)
            {
                foreach (DataRow drFieldName in drDeDupField)
                {
                    string strFieldName = drFieldName["DeDup_Field"].ToString();
                    dsConstitution.Tables[1].CaseSensitive = false;
                    if (strFieldName.ToUpper() == "CUSTOMER_NAME")
                    {
                        DataRow[] drDeDup = dsConstitution.Tables[1].Select("Customer_Name = '" + txtCustomerName.Text + "'");
                        if (drDeDup.Length > 0)
                        {
                            IsDuplicateCustomerName = true;
                            strConsDocName += "Customer Name,";
                        }
                    }
                    if (strFieldName.ToUpper() == "COMM_ADDRESS1")
                    {
                        DataRow[] drDeDup = dsConstitution.Tables[1].Select("Comm_Address1 = '" + txtComAddress1.Text + "'");
                        if (drDeDup.Length > 0)
                        {
                            IsDuplicateAddress1 = true;
                            strConsDocName += "Communication Address1,";
                        }
                    }
                    if (strFieldName.ToUpper() == "COMM_ADDRESS2")
                    {
                        DataRow[] drDeDup = dsConstitution.Tables[1].Select("Comm_Address2 = '" + txtCOmAddress2.Text + "'");
                        if (drDeDup.Length > 0)
                        {
                            IsDuplicateAddress2 = true;
                            strConsDocName += "Communication Address2,";
                        }
                    }
                }
            }
            foreach (GridViewRow grDocs in gvConstitutionalDocuments.Rows)
            {
                string strCategoryID = grDocs.Cells[9].Text;
                string strDocumentFlag = grDocs.Cells[1].Text;
                string strDocName = grDocs.Cells[2].Text;
                TextBox txtValues = (TextBox)grDocs.FindControl("txtValues");
                if (txtValues != null)
                {
                    if (txtValues.Text.Trim() != "")
                    {
                        DataRow[] drDuplicateDoc = dsConstitution.Tables[0].Select("CONSDOCUMENTCATEGORY_ID = '" + strCategoryID + "' and Document_Value = '" + txtValues.Text.ToUpper().Trim() + "'");
                        if (drDuplicateDoc.Length > 0)
                        {
                            strConsDocName += strDocName + ",";
                            blnIsDuplicate = true;
                            foreach (DataRow dr in drDuplicateDoc)
                            {
                                if (!strDeDupCustomerCode.Contains(dr["CustomerCode"].ToString()))
                                {
                                    strDeDupCustomerCode += dr["CustomerCode"].ToString() + ",";
                                }

                            }
                        }
                    }
                }
            }
            if (IsDuplicateAddress1 || IsDuplicateAddress2 || IsDuplicateCustomerName || blnIsDuplicate)
            {
                blnIsDuplicate = true;
            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Validate Duplicate Documents");
        }
        return blnIsDuplicate;
    }

    /// <summary>
    /// Event for Clear the Controls
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            txtGroupName.Text = txtIndustryName.Text = txtCustomerName.Text =
           txtComAddress1.Text = txtPerAddress1.Text = txtCOmAddress2.Text = txtPerAddress2.Text = txtComPincode.Text = txtPerPincode.Text =
           txtComMobile.Text = txtPerMobile.Text = txtComTelephone.Text = txtPerTelephone.Text = txtComEmail.Text = txtPerEmail.Text =
           txtComWebsite.Text = txtPerWebSite.Text = txtDOB.Text = txtAge.Text = txtQualification.Text = txtProfession.Text =
           txtSpouseName.Text = txtChildren.Text = txtTotalDependents.Text = txtWeddingdate.Text = txtCurrentMarketValue.Text
           = txtRemainingLoanValue.Text = txtTotNetWorth.Text = txtDirectors.Text = txtSE.Text = txtPaidCapital.Text = txtfacevalue.Text
           = txtbookvalue.Text = txtBusinessProfile.Text = txtGeographical.Text = txtnobranch.Text = txtPercentageStake.Text =
           txtJVPartnerName.Text = txtJVPartnerStake.Text = txtCEOName.Text = txtCEOAge.Text = txtCEOexperience.Text =
           txtResidentialAddress.Text = txtCEOWeddingDate.Text = txtAccountNumber.Text = txtBankName.Text = txtBankBranch.Text =
           txtMICRCode.Text = txtBankAddress.Text =
           txtLOBName.Text = txtProductGroup.Text = txtSanctionamt.Text = txtValidupto.Text = txtUtilizedAmt.Text
           = txtFacilityType.Text = txtCustomercode.Text = string.Empty;

            //<<Performance>>
            //txtComCity.Items.Insert(0, "");
            txtComCity.Clear();
            //txtPerCity.Items.Insert(0, "");
            txtPerCity.Clear();
            txtComState.Items.Insert(0, "");
            txtPerState.Items.Insert(0, "");
            txtComCountry.Items.Insert(0, "");
            txtPerCountry.Items.Insert(0, "");

            //txtComCity.SelectedIndex = 
            //txtPerCity.SelectedIndex =
            txtComState.SelectedIndex = txtPerState.SelectedIndex = txtComCountry.SelectedIndex = txtPerCountry.SelectedIndex = -1;

            if (ddlCustomerType.Items.Count > 0) ddlCustomerType.SelectedIndex = 0;
            if (ddlConstitutionName.Items.Count > 0) ddlConstitutionName.SelectedIndex = 0;
            if (ddlTitle.Items.Count > 0) ddlTitle.SelectedIndex = 0;
            if (ddlPostingGroup.Items.Count > 0) ddlPostingGroup.SelectedIndex = 0;
            if (ddlGender.Items.Count > 0) ddlGender.SelectedIndex = 0;
            if (ddlMaritalStatus.Items.Count > 0) ddlMaritalStatus.SelectedIndex = 0;
            if (ddlHouseType.Items.Count > 0) ddlHouseType.SelectedIndex = 0;
            if (ddlOwn.Items.Count > 0) ddlOwn.SelectedIndex = 0;
            if (ddlPublic.Items.Count > 0) ddlPublic.SelectedIndex = 0;
            if (ddlGovernment.Items.Count > 0) ddlGovernment.SelectedIndex = 0;

            chkCoApplicant.Checked = chkCustomer.Checked = chkGuarantor1.Checked = chkGuarantor2.Checked = false;

            if (ddlAccountType.Items.Count > 0) ddlAccountType.SelectedIndex = 0;

            cmbGroupCode.Text = string.Empty;
            cmbIndustryCode.Text = string.Empty;
            gvConstitutionalDocuments.Dispose();
            gvConstitutionalDocuments.DataSource = null;
            gvConstitutionalDocuments.DataBind();

            txtCustomerName.Enabled = true;

            tcCustomerMaster.ActiveTab = tbAddress;
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException, strPageName);
            cvCustomerMaster.ErrorMessage = Resources.LocalizationResources.ClearError;
            cvCustomerMaster.IsValid = false;

        }
    }

    /// <summary>
    /// Event for Cancel the Process
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString["IsFromEnquiry"] != null)
            {
                strAlert = "window.close();";
                //window.opener.location.reload();"; for bug in Pricing Commented on 15/02/2011 by Nataraj
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", strAlert, true);
            }
            else if (Request.QueryString["IsFromApplication"] != null)
            {
                strAlert = "window.close();";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", strAlert, true);
            }
            else
            {
                Response.Redirect("~/Origination/S3gOrgCustomerMaster_View.aspx?qsType=GL");
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException, strPageName);
            cvCustomerMaster.ErrorMessage = Resources.LocalizationResources.CancelError;
            cvCustomerMaster.IsValid = false;
        }
    }
    #endregion

    #region Local Methods

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
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("PRDD Master Document Path does not exist");
        }
    }

    #region GetCompletionList
    /// <summary>
    /// GetCompletionList
    /// </summary>
    /// <param name="prefixText">search text</param>
    /// <param name="count">no of matches to display</param>
    /// <returns>string[] of matching names</returns>
    [System.Web.Services.WebMethod]
    public static string[] GetCompletionList(String prefixText, int count)
    {
        DataTable dt1 = new DataTable();

        dt1 = (DataTable)HttpContext.Current.Session["GroupDT"];

        List<String> suggetions = GetSuggestions(prefixText, count, dt1);


        return suggetions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetIndustryList(String prefixText, int count)
    {
        DataTable dt1 = new DataTable();

        dt1 = (DataTable)HttpContext.Current.Session["IndustryDT"];

        List<String> suggetions = GetSuggestions(prefixText, count, dt1);


        return suggetions.ToArray();
    }

    #endregion


    #region GetSuggestions
    /// <summary>
    /// GetSuggestions
    /// </summary>
    /// <param name="key">Country Names to search</param>
    /// <returns>Country Names Similar to key</returns>
    private static List<String> GetSuggestions(string key, int count, DataTable dt1)
    {
        List<String> suggestions = new List<string>();
        try
        {

            //DataTable dt1 = new DataTable();

            //dt1 = (DataTable)HttpContext.Current.Session["GroupDT"];// objCustomerMasterClient.FunPub_GetS3GStatusLookUp(ObjStatus);

            string filterExpression = "value like '" + key + "%'";
            DataRow[] dtSuggestions = dt1.Select(filterExpression);

            foreach (DataRow dr in dtSuggestions)
            {
                string suggestion = dr["value"].ToString();
                suggestions.Add(suggestion);
            }

        }
        catch (Exception objException)
        {
            return suggestions;
            ClsPubCommErrorLog.CustomErrorRoutine(objException, strPageName);
            //   lblErrorMessage.Text = Resources.LocalizationResources.CustomerTypeChangeError;
        }

        return suggestions;
    }
    #endregion


    /// <summary>
    /// Load the Master data in Dropdownlist
    /// </summary>
    private void FunPriLoadMasterData()
    {
        OrgMasterMgtServicesReference.OrgMasterMgtServicesClient objCustomerMasterClient = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
        try
        {

            S3GBusEntity.S3G_Status_Parameters ObjStatus = new S3G_Status_Parameters();
            ObjStatus.Option = 1;
            ObjStatus.Param1 = S3G_Statu_Lookup.CUSTOMER_TYPE.ToString();
            Utility.FillDLL(ddlCustomerType, objCustomerMasterClient.FunPub_GetS3GStatusLookUp(ObjStatus));

            ObjStatus.Param1 = S3G_Statu_Lookup.HOUSE_TYPE.ToString();
            Utility.FillDLL(ddlHouseType, objCustomerMasterClient.FunPub_GetS3GStatusLookUp(ObjStatus));

            ObjStatus.Param1 = S3G_Statu_Lookup.COMPANY_TYPE.ToString();
            Utility.FillDLL(ddlPublic, objCustomerMasterClient.FunPub_GetS3GStatusLookUp(ObjStatus));

            ObjStatus.Param1 = S3G_Statu_Lookup.GOVERNMENT.ToString();
            Utility.FillDLL(ddlGovernment, objCustomerMasterClient.FunPub_GetS3GStatusLookUp(ObjStatus));

            ObjStatus.Param1 = S3G_Statu_Lookup.MARITAL_STATUS.ToString();
            Utility.FillDLL(ddlMaritalStatus, objCustomerMasterClient.FunPub_GetS3GStatusLookUp(ObjStatus));

            ObjStatus.Param1 = S3G_Statu_Lookup.ACCOUNT_TYPE.ToString();
            Utility.FillDLL(ddlAccountType, objCustomerMasterClient.FunPub_GetS3GStatusLookUp(ObjStatus));

            ObjStatus.Param1 = "TITLE";
            Utility.FillDLL(ddlTitle, objCustomerMasterClient.FunPub_GetS3GStatusLookUp(ObjStatus));

            ObjStatus.Option = 2;
            ObjStatus.Param1 = intCompanyId.ToString();
            Utility.FillDLL(ddlConstitutionName, objCustomerMasterClient.FunPub_GetS3GStatusLookUp(ObjStatus));

            ObjStatus.Option = 3;
            ObjStatus.Param1 = intCompanyId.ToString();
            ObjStatus.Param2 = intCustomerId.ToString();
            Utility.FillDLL(ddlPostingGroup, objCustomerMasterClient.FunPub_GetS3GStatusLookUp(ObjStatus));

            //ObjStatus.Option = 4;
            //ObjStatus.Param1 = intCompanyId.ToString();
            //ObjStatus.Param2 = intUserId.ToString();
            //Utility.FillDLL(ddlBranchList, objCustomerMasterClient.FunPub_GetS3GStatusLookUp(ObjStatus));

            ObjStatus.Option = 8;
            ObjStatus.Param1 = "GROUP";
            ObjStatus.Param2 = intCompanyId.ToString();
            DataTable dt = new DataTable();
            dt = objCustomerMasterClient.FunPub_GetS3GStatusLookUp(ObjStatus);
            System.Web.HttpContext.Current.Session["GroupDT"] = dt;


            ObjStatus.Option = 8;
            ObjStatus.Param1 = "INDUSTRY";
            ObjStatus.Param2 = intCompanyId.ToString();
            DataTable Indusdt = new DataTable();
            Indusdt = objCustomerMasterClient.FunPub_GetS3GStatusLookUp(ObjStatus);
            System.Web.HttpContext.Current.Session["IndustryDT"] = Indusdt;

        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException, strPageName);
            throw objException;
        }
        finally
        {
            objCustomerMasterClient.Close();
        }
    }
    /*
    private void FunPriLoadRelationType()
    {
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();

            Procparam.Add("@Company_ID", intCompanyId.ToString());
            DataTable dt = new DataTable();
            dt = Utility.GetDefaultData("S3G_ORG_GetRelationType", Procparam);
            GrvRelation.DataSource = dt;
            GrvRelation.DataBind();
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException, strPageName);
            throw objException;
        }
    }
     */

    /// <summary>
    /// Toggle the Tabs for LoanAdmin Checking
    /// </summary>
    /// <param name="blnIsEnabled"></param>
    private void FunPriToggleLoanAdminControls(bool blnIsEnabled)
    {
        try
        {
            tcCustomerMaster.Tabs[4].Enabled = blnIsEnabled;
            tcCustomerMaster.Tabs[5].Enabled = blnIsEnabled;
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException, strPageName);
            throw new ApplicationException("Unable to Toggle Customer Track/Credit Details");
        }
    }

    /// <summary>
    /// Toggle the Controls for CustomerType(Individual/NonIndividual)
    /// </summary>
    private void FunPriToggleCustomerTypeControls()
    {
        try
        {
            if (Convert.ToInt16(ddlCustomerType.SelectedValue) > 0)
            {
                if (ddlCustomerType.SelectedItem.Text.ToUpper() == "INDIVIDUAL")
                {
                    PriFunToggleCustomerTypeControls(false);
                    ddlPublic.SelectedIndex = ddlGovernment.SelectedIndex = 0;
                    txtDirectors.Text = txtSE.Text = txtPaidCapital.Text = txtfacevalue.Text =
                    txtbookvalue.Text = txtBusinessProfile.Text = txtGeographical.Text =
                    txtnobranch.Text = txtPercentageStake.Text = txtJVPartnerName.Text =
                    txtJVPartnerStake.Text = txtCEOName.Text = txtCEOAge.Text =
                    txtCEOexperience.Text = txtResidentialAddress.Text =
                    txtCEOWeddingDate.Text = string.Empty;

                    lblComEmail.CssClass = lblPerEmail.CssClass = "styleDisplayLabel";
                    lblIndustryCode.CssClass = 
                    //lblIndustryName.CssClass =
                    lblPublic.CssClass = lblCEO.CssClass = lblDirectors.CssClass = lblGeographical.CssClass =
                    lblCEOAge.CssClass = lblCEOexperience.CssClass = lblResidentialAddress.CssClass = lblnobranch.CssClass =
                    lblBusinessProfile.CssClass = "styleDisplayLabel";
                    lblGender.CssClass = lblDOB.CssClass = lblQualification.CssClass = lblProfession.CssClass =
                    lblHouseType.CssClass = lblOwn.CssClass = "styleReqFieldLabel";
                }
                else
                {
                    PriFunToggleCustomerTypeControls(true);
                    txtDOB.Text = txtAge.Text = txtQualification.Text =
                    txtProfession.Text = txtSpouseName.Text = txtChildren.Text =
                    txtTotalDependents.Text = txtTotNetWorth.Text = txtRemainingLoanValue.Text =
                    txtWeddingdate.Text = string.Empty;
                    frvCurrentMarketValue.Enabled = false;
                    lblComEmail.CssClass = lblPerEmail.CssClass = "styleReqFieldLabel";
                    lblIndustryCode.CssClass = 
                    //lblIndustryName.CssClass =
                    lblPublic.CssClass = lblCEO.CssClass = lblDirectors.CssClass = lblGeographical.CssClass = lblnobranch.CssClass =
                    lblCEOAge.CssClass = lblCEOexperience.CssClass = lblResidentialAddress.CssClass =
                    lblBusinessProfile.CssClass = "styleReqFieldLabel";
                    lblGender.CssClass = lblDOB.CssClass = lblQualification.CssClass = lblProfession.CssClass =
                    lblHouseType.CssClass = lblOwn.CssClass = "styleDisplayLabel";
                    ddlMaritalStatus.SelectedIndex = ddlHouseType.SelectedIndex = ddlOwn.SelectedIndex = 0;
                    ddlGender.SelectedIndex = 0;

                }
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException, strPageName);
            throw new ApplicationException("Due to Data Problem,Unable to Toggle Controls for Customer Type");
        }
    }


    /// <summary>
    /// Toggle the Controls for CustomerType(Individual/NonIndividual)
    /// </summary>
    /// <param name="blnIsEnabled"></param>
    private void PriFunToggleCustomerTypeControls(bool blnIsEnabled)
    {
        try
        {

            //Toggle the Controls for IndividualCustomerType
            pnlNonIndividual.Enabled = ddlPublic.Enabled = txtDirectors.Enabled = blnIsEnabled;
            txtSE.Enabled = txtPaidCapital.Enabled = txtfacevalue.Enabled = txtbookvalue.Enabled = blnIsEnabled;
            txtBusinessProfile.Enabled = txtGeographical.Enabled = txtnobranch.Enabled = ddlGovernment.Enabled = blnIsEnabled;
            txtPercentageStake.Enabled = txtJVPartnerName.Enabled = txtJVPartnerStake.Enabled = txtCEOName.Enabled = blnIsEnabled;
            txtCEOAge.Enabled = txtCEOexperience.Enabled = txtResidentialAddress.Enabled = blnIsEnabled;
            txtCEOWeddingDate.Enabled = CalendarCEOWeddingDate.Enabled = cmbIndustryCode.Enabled = blnIsEnabled;
            rfvIndustryCode.Enabled = txtIndustryName.Enabled = rfvIndustryName.Enabled = rfvPublic.Enabled = blnIsEnabled;
            rfvDirectors.Enabled = rfvBusinessProfile.Enabled = rfvGeographical.Enabled = rfvnobranch.Enabled = blnIsEnabled;
            rfvCEOName.Enabled = rfvCEOAge.Enabled = rfvCEOexperience.Enabled = rfvResidentialAddress.Enabled = blnIsEnabled;


            //Toggle the Controls for NonIndividualCustomerType
            pnlIndividual.Enabled = ddlGender.Enabled = txtDOB.Enabled = CalendarExtenderSD.Enabled = !blnIsEnabled;
            txtAge.Enabled = ddlMaritalStatus.Enabled = txtQualification.Enabled = txtProfession.Enabled = !blnIsEnabled;
            //txtSpouseName.Enabled = txtChildren.Enabled = ddlHouseType.Enabled = ddlOwn.Enabled = txtTotalDependents.Enabled = !blnIsEnabled;
            txtSpouseName.Enabled = txtChildren.Enabled = ddlHouseType.Enabled = ddlOwn.Enabled = !blnIsEnabled;
            //txtTotNetWorth.Enabled = txtRemainingLoanValue.Enabled = txtWeddingdate.Enabled = CalendarWeddingdate.Enabled = !blnIsEnabled;
            txtWeddingdate.Enabled = CalendarWeddingdate.Enabled = !blnIsEnabled;
            rfvGender.Enabled = rfvDOB.Enabled = rfvQualification.Enabled = rfvtxtProfession.Enabled = !blnIsEnabled;
            rfvHouseType.Enabled = rfvOwn.Enabled = rfvComEmail.Enabled = rfvPerEmail.Enabled =
                !blnIsEnabled;
            //Common for BothCustomerTypes
            rfvIndustryCode.Enabled = rfvIndustryName.Enabled = revPerEmail.Enabled = revEmailId.Enabled = blnIsEnabled;

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Toggle Customer Type Controls");
        }
    }


    /// <summary>
    /// method for default settings for all controls
    /// </summary>
    private void FunPriSetControlSettings()
    {
        try
        {
            //lblCustomercode.Text = Resources.LocalizationResources.ORG_CUST_lblCustomercode;
            //lblCustomerType.Text = Resources.LocalizationResources.ORG_CUST_lblCustomerType;
            //lblgroupcode.Text = Resources.LocalizationResources.ORG_CUST_lblgroupcode;
            //lblGroupName.Text = Resources.LocalizationResources.ORG_CUST_lblGroupName;
            //lblIndustryCode.Text = Resources.LocalizationResources.ORG_CUST_lblIndustryCode;
            //lblIndustryName.Text = Resources.LocalizationResources.ORG_CUST_lblIndustryName;
            //lblConstitutionName.Text = Resources.LocalizationResources.ORG_CUST_lblConstitutionName;
            //lblTitle.Text = Resources.LocalizationResources.ORG_CUST_lblTitle;
            //lblCustomerName.Text = Resources.LocalizationResources.ORG_CUST_lblCustomerName;
            //lblCustomerPostingGroup.Text = Resources.LocalizationResources.ORG_CUST_lblCustomerPostingGroup;

            //lblComAddress1.Text = Resources.LocalizationResources.ORG_CUST_lblComAddress1;
            //lblPerAddress1.Text = Resources.LocalizationResources.ORG_CUST_lblPerAddress1;
            //lblComAddress2.Text = Resources.LocalizationResources.ORG_CUST_lblComAddress2;
            //lblPerAddress2.Text = Resources.LocalizationResources.ORG_CUST_lblPerAddress2;
            //lblComcity.Text = Resources.LocalizationResources.ORG_CUST_lblComcity;
            //lblPerCity.Text = Resources.LocalizationResources.ORG_CUST_lblPerCity;
            //lblComState.Text = Resources.LocalizationResources.ORG_CUST_lblComState;
            //lblPerState.Text = Resources.LocalizationResources.ORG_CUST_lblPerState;
            //lblComCountry.Text = Resources.LocalizationResources.ORG_CUST_lblComCountry;
            //lblPerCountry.Text = Resources.LocalizationResources.ORG_CUST_lblPerCountry;
            //lblCompincode.Text = Resources.LocalizationResources.ORG_CUST_lblCompincode;
            //lblPerpincode.Text = Resources.LocalizationResources.ORG_CUST_lblPerpincode;
            //lblComMobile.Text = Resources.LocalizationResources.ORG_CUST_lblComMobile;
            //lblPerMobile.Text = Resources.LocalizationResources.ORG_CUST_lblPerMobile;
            //lblComTelephone.Text = Resources.LocalizationResources.ORG_CUST_lblComTelephone;
            //lblPerTelephone.Text = Resources.LocalizationResources.ORG_CUST_lblPerTelephone;
            //lblComEmail.Text = Resources.LocalizationResources.ORG_CUST_lblComEmail;
            //lblPerEmail.Text = Resources.LocalizationResources.ORG_CUST_lblPerEmail;
            //lblComWebSite.Text = Resources.LocalizationResources.ORG_CUST_lblComWebSite;
            //lblPerWebSite.Text = Resources.LocalizationResources.ORG_CUST_lblPerWebSite;

            //lblGender.Text = Resources.LocalizationResources.ORG_CUST_lblGender;
            //lblDOB.Text = Resources.LocalizationResources.ORG_CUST_lblDOB;
            //lblAge.Text = Resources.LocalizationResources.ORG_CUST_lblAge;
            //lblMaritalStatus.Text = Resources.LocalizationResources.ORG_CUST_lblMaritalStatus;
            //lblQualification.Text = Resources.LocalizationResources.ORG_CUST_lblQualification;
            //lblProfession.Text = Resources.LocalizationResources.ORG_CUST_lblProfession;
            //lblSpouseName.Text = Resources.LocalizationResources.ORG_CUST_lblSpouseName;
            //lblChildren.Text = Resources.LocalizationResources.ORG_CUST_lblChildren;
            //lblTotalDependents.Text = Resources.LocalizationResources.ORG_CUST_lblTotalDependents;
            //lblWeddingdate.Text = Resources.LocalizationResources.ORG_CUST_lblWeddingdate;
            //lblHouseType.Text = Resources.LocalizationResources.ORG_CUST_lblHouseType;
            //lblOwn.Text = Resources.LocalizationResources.ORG_CUST_lblOwn;
            //lblCurrentMarketValue.Text = Resources.LocalizationResources.ORG_CUST_lblCurrentMarketValue;
            //lblRemainingLoanValue.Text = Resources.LocalizationResources.ORG_CUST_lblRemainingLoanValue;
            //lblTotNetWorth.Text = Resources.LocalizationResources.ORG_CUST_lblTotNetWorth;

            //lblPublic.Text = Resources.LocalizationResources.ORG_CUST_lblPublic;
            //lblDirectors.Text = Resources.LocalizationResources.ORG_CUST_lblDirectors;
            //lblStockExchange.Text = Resources.LocalizationResources.ORG_CUST_lblStockExchange;
            //lblPaidCapital.Text = Resources.LocalizationResources.ORG_CUST_lblPaidCapital;
            //lblfacevalue.Text = Resources.LocalizationResources.ORG_CUST_lblfacevalue;
            //lblbookvalue.Text = Resources.LocalizationResources.ORG_CUST_lblbookvalue;
            //lblBusinessProfile.Text = Resources.LocalizationResources.ORG_CUST_lblBusinessProfile;
            //lblGeographical.Text = Resources.LocalizationResources.ORG_CUST_lblGeographical;
            //lblnobranch.Text = Resources.LocalizationResources.ORG_CUST_lblnobranch;
            //lblGovernment.Text = Resources.LocalizationResources.ORG_CUST_lblGovernment;
            //lblStake.Text = Resources.LocalizationResources.ORG_CUST_lblStake;
            //lblJVPartnerName.Text = Resources.LocalizationResources.ORG_CUST_lblJVPartnerName;
            //lblJVPartnerStake.Text = Resources.LocalizationResources.ORG_CUST_lblJVPartnerStake;
            //lblCEO.Text = Resources.LocalizationResources.ORG_CUST_lblCEO;
            //lblCEOAge.Text = Resources.LocalizationResources.ORG_CUST_lblCEOAge;
            //lblCEOexperience.Text = Resources.LocalizationResources.ORG_CUST_lblCEOexperience;
            //lblResidentialAddress.Text = Resources.LocalizationResources.ORG_CUST_lblResidentialAddress;
            //lblCEOWeddingDate.Text = Resources.LocalizationResources.ORG_CUST_lblCEOWeddingDate;

            //lblAccountType.Text = Resources.LocalizationResources.ORG_CUST_lblAccountType;
            //lblAccountNumber.Text = Resources.LocalizationResources.ORG_CUST_lblAccountNumber;
            //lblBankName.Text = Resources.LocalizationResources.ORG_CUST_lblBankName;
            //lblBankBranch.Text = Resources.LocalizationResources.ORG_CUST_lblBankBranch;
            //lblMICRCode.Text = Resources.LocalizationResources.ORG_CUST_lblMICRCode;
            //lblBankAddress.Text = Resources.LocalizationResources.ORG_CUST_lblBankAddress;

            //lblLOBName.Text = Resources.LocalizationResources.ORG_CUST_lblLOBName;
            //lblProductGroup.Text = Resources.LocalizationResources.ORG_CUST_lblProductGroup;
            //lblSanctionamt.Text = Resources.LocalizationResources.ORG_CUST_lblSanctionamt;
            //lblValid.Text = Resources.LocalizationResources.ORG_CUST_lblValid;
            //lblUtilizedAmt.Text = Resources.LocalizationResources.ORG_CUST_lblUtilizedAmt;
            //lblFacilityType.Text = Resources.LocalizationResources.ORG_CUST_lblFacilityType;
            //lblHeading.Text = Resources.LocalizationResources.ORG_CUST_lblHeading;

            //btnSave.Text = Resources.LocalizationResources.ORG_CUST_btnSave;
            //btnClear.Text = Resources.LocalizationResources.ORG_CUST_btnClear;



            rfvCustomerType.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rfvCustomerType;
            rfvConstitutionName.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rfvConstitutionName;
            rfvTitle.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rfvTitle;
            rfvComAddress1.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rfvComAddress1;
            rfvtxtPerAddress1.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rfvtxtPerAddress1;
            //<<Performance>>
            //rfvComCity.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rfvComCity;
            txtComCity.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rfvComCity;
            //rfvPerCity.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rfvPerCity;
            rfvComState.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rfvComState;
            rfvPerState.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rfvPerState;
            rfvComCountry.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rfvComCountry;
            rfvPerCountry.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rfvPerCountry;
            rfvComPincode.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rfvComPincode;
            rvfPerPincode.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rvfPerPincode;
            rfvComTelephone.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rfvComTelephone;
            rfvPerTelephone.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rfvPerTelephone;
            rfvComEmail.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rfvComEmail;
            revEmailId.ErrorMessage = Resources.LocalizationResources.ORG_CUST_revEmailId;
            rfvPerEmail.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rfvPerEmail;
            revPerEmail.ErrorMessage = Resources.LocalizationResources.ORG_CUST_revPerEmail;
            revComWebsite.ErrorMessage = Resources.LocalizationResources.ORG_CUST_revComWebsite;

            revPerWebSite.ErrorMessage = Resources.LocalizationResources.ORG_CUST_revPerWebSite;
            rfvGender.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rfvGender;
            rfvDOB.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rfvDOB;
            rfvQualification.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rfvQualification;
            rfvtxtProfession.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rfvtxtProfession;
            rfvHouseType.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rfvHouseType;
            rfvOwn.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rfvOwn;
            //rfvtotalnetworth.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rfvtotalnetworth;
            rfvPublic.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rfvPublic;
            rfvDirectors.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rfvDirectors;
            rfvBusinessProfile.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rfvBusinessProfile;
            rfvGeographical.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rfvGeographical;
            rfvnobranch.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rfvnobranch;
            rfvCEOName.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rfvCEOName;
            rfvCEOAge.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rfvCEOAge;
            rfvCEOexperience.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rfvCEOexperience;
            rfvResidentialAddress.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rfvResidentialAddress;

            rfvAccountType.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rfvAccountType;
            rfvAccountNumber.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rfvAccountNumber;
            rfvBankName.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rfvBankName;
            rfvBankBranch.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rfvBankBranch;
            rfvMICRCode.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rfvMICRCode;
            rfvBankAddress.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rfvBankAddress;

            rvfCustomerName.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rvfCustomerName;

            rvfCustomerName.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rvfCustomerName;


        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Initialise the Controls");
        }
    }

    /// <summary>
    /// Method for Loading the Constitutional Documents
    /// </summary>
    /// <param name="CustomerID"></param>
    private void FunPriLoadCustomerConstitutionDocs(int CustomerID)
    {
        OrgMasterMgtServicesReference.OrgMasterMgtServicesClient objCustomerMasterClient = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
        try
        {
            S3GBusEntity.S3G_Status_Parameters ObjStatus = new S3G_Status_Parameters();
            ObjStatus.Option = 169;
            ObjStatus.Param1 = CustomerID.ToString();
            gvConstitutionalDocuments.DataSource = objCustomerMasterClient.FunPub_GetS3GStatusLookUp(ObjStatus);
            gvConstitutionalDocuments.DataBind();

            //Added By Thangam M on 12/Mar/2012 to fix bug id - 5451
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Clear", "javascript:fnClearAsyncUploader('" + gvConstitutionalDocuments.Rows.Count.ToString() + "');", true);

            ViewState["ConstitutionDocument"] = objCustomerMasterClient.FunPub_GetS3GStatusLookUp(ObjStatus);
            ObjStatus.Option = 787;
            ObjStatus.Param1 = intCompanyId.ToString();
            DataTable dtConsDocPath = objCustomerMasterClient.FunPub_GetS3GStatusLookUp(ObjStatus);
            if (dtConsDocPath == null || (dtConsDocPath != null && dtConsDocPath.Rows.Count == 0))
            {
                Utility.FunShowAlertMsg(this, "Define the spooling path in Document Path Setup for Consitution Document");
                return;
            }
            ViewState["ConsDocPath"] = dtConsDocPath.Rows[0][0].ToString();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Due to Data Problem, Unable to Load the Constitution Documents");
        }
        finally
        {
            objCustomerMasterClient.Close();
        }
    }
    protected void hlnkView_Click(object sender, EventArgs e)
    {
        try
        {
            FunPriShowPRDD(sender);
        }
        catch (Exception ex)
        {
            cvCustomerMaster.ErrorMessage = ex.Message;
            cvCustomerMaster.IsValid = false;
        }
    }
    private void FunPriShowPRDD(object sender)
    {
        try
        {
            string strFieldAtt = ((LinkButton)sender).ClientID;
            int gRowIndex = Utility.FunPubGetGridRowID("gvConstitutionalDocuments", strFieldAtt);
            Label lblPath = gvConstitutionalDocuments.Rows[gRowIndex].FindControl("myThrobber") as Label;
            string strFileName = lblPath.Text.Replace("\\", "/").Trim();
            string strScipt = "window.open('../Common/S3GViewFile.aspx?qsFileName=" + strFileName + "', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strScipt, true);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Due to Data Problem, Unable to View the Document");
        }
    }
    private void FunPriLoadCustomerBankDetails(int CustomerID)
    {
        OrgMasterMgtServicesReference.OrgMasterMgtServicesClient objCustomerMasterClient = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
        try
        {
            S3GBusEntity.S3G_Status_Parameters ObjStatus = new S3G_Status_Parameters();
            ObjStatus.Option = 200;
            ObjStatus.Param1 = CustomerID.ToString();
            ViewState["DetailsTable"] = objCustomerMasterClient.FunPub_GetS3GStatusLookUp(ObjStatus);
            if (ViewState["DetailsTable"] != null)
            {
                grvBankDetails.DataSource = (DataTable)ViewState["DetailsTable"];
                grvBankDetails.DataBind();
                FunPriHideBankColumns();

            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Due to Data Problem, Unable to Load Bank Details");
        }
        finally
        {
            objCustomerMasterClient.Close();
        }
    }
    private void FunPriHideBankColumns()
    {
        try
        {
            //grvBankDetails.Columns[0].Visible = false;
            //grvBankDetails.Columns[1].Visible = false;
            //grvBankDetails.Columns[2].Visible = false;
            //grvBankDetails.Columns[4].Visible = false;
            //grvBankDetails.Columns[6].Visible = false;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    /// <summary>
    /// Method for Disable the Text in Constitutional Document Details tab
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    private bool FunPriDisableValueField(string str)
    {
        string[] strCheck = new string[2];
        bool blnIsResult = true;
        try
        {
            strCheck = str.Split('-');
            if (strCheck.Length > 0)
            {
                if (Convert.ToString(strCheck[0]) != "CID")
                    blnIsResult = false;
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException, strPageName);
            throw objException;
        }
        return blnIsResult;

    }

    /// <summary>
    /// Method for assign the Text into Coombobox
    /// </summary>
    /// <param name="strControlText"></param>
    /// <param name="cmbControl"></param>
    private void FunPriSetDropDownText(string strControlText, AjaxControlToolkit.ComboBox cmbControl)
    {
        try
        {
            for (int i = 0; i < cmbControl.Items.Count; i++)
            {
                if (cmbControl.Items[i].Text == strControlText)
                    cmbControl.SelectedItem.Text = strControlText;
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException, strPageName);
            throw objException;
        }
    }

    /// <summary>
    /// Method for Loading the Customer Details
    /// </summary>
    /// <param name="CustomerID"></param>
    private void FunPriLoadCustomerDetails(int CustomerID)
    {
        OrgMasterMgtServicesReference.OrgMasterMgtServicesClient objCustomerMasterClient = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
        try
        {
            S3GBusEntity.S3G_Status_Parameters ObjStatus = new S3G_Status_Parameters();
            ObjStatus.Option = 10;
            ObjStatus.Param1 = CustomerID.ToString();
            DataTable ObjCustomerDetails = new DataTable();
            ObjCustomerDetails = objCustomerMasterClient.FunPub_GetS3GStatusLookUp(ObjStatus);

            if (ObjCustomerDetails.Rows.Count > 0)
            {
                txtCustomercode.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["Customer_Code"]);


                ListItem lst;

                if (strMode == "Q")
                {
                    lst = new ListItem(Convert.ToString(ObjCustomerDetails.Rows[0]["Customer_Type"]), Convert.ToString(ObjCustomerDetails.Rows[0]["Customer_Type_ID"]));
                    ddlCustomerType.Items.Add(lst);
                }

                ddlCustomerType.SelectedValue = Convert.ToString(ObjCustomerDetails.Rows[0]["Customer_Type_ID"]);
                cmbGroupCode.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["GroupCode"]);
                // FunPriSetDropDownText(Convert.ToString(ObjCustomerDetails.Rows[0]["GroupCode"]), cmbGroupCode); commented by Prakash 
                txtGroupName.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["Groupname"]);
                //if (!string.IsNullOrEmpty(txtGroupName.Text))
                //{
                //    txtGroupName.ReadOnly = true;
                //}
                //  FunPriSetDropDownText(Convert.ToString(ObjCustomerDetails.Rows[0]["IndustryCode"]), cmbIndustryCode); commented by Prakash 
                cmbIndustryCode.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["IndustryCode"]);
                txtIndustryName.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["IndustryName"]);

                txtFamilyName.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["Family_Name"]);
                txtNotes.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["Notes"]);
                //if (!string.IsNullOrEmpty(txtIndustryName.Text))
                //{
                //    txtIndustryName.Enabled = false;
                //}

                if (strMode == "Q")
                {
                    lst = new ListItem(Convert.ToString(ObjCustomerDetails.Rows[0]["Constitution_Name"]), Convert.ToString(ObjCustomerDetails.Rows[0]["Constitution_ID"]));
                    ddlConstitutionName.Items.Add(lst);
                }

                ddlConstitutionName.SelectedValue = Convert.ToString(ObjCustomerDetails.Rows[0]["Constitution_ID"]);

                if (strMode == "Q")
                {
                    lst = new ListItem(Convert.ToString(ObjCustomerDetails.Rows[0]["Title"]), Convert.ToString(ObjCustomerDetails.Rows[0]["Title_ID"]));
                    ddlTitle.Items.Add(lst);
                }
                ddlTitle.SelectedValue = Convert.ToString(ObjCustomerDetails.Rows[0]["Title_ID"]);
                //added Relation Type
                chkCustomer.Checked = Convert.ToBoolean(ObjCustomerDetails.Rows[0]["Customer"]);
                chkGuarantor1.Checked = Convert.ToBoolean(ObjCustomerDetails.Rows[0]["Guarantor1"]);
                chkGuarantor2.Checked = Convert.ToBoolean(ObjCustomerDetails.Rows[0]["Guarantor2"]);
                chkCoApplicant.Checked = Convert.ToBoolean(ObjCustomerDetails.Rows[0]["Co_Applicant"]);

                if (chkCustomer.Checked)
                    chkCustomer.Enabled = false;
                if (chkGuarantor1.Checked)
                    chkGuarantor1.Enabled = false;
                if (chkGuarantor2.Checked)
                    chkGuarantor2.Enabled = false;
                if (chkCoApplicant.Checked)
                    chkCoApplicant.Enabled = false;
                //end
                txtCustomerName.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["Customer_Name"]);

                ListItem ListPosting = new ListItem(Convert.ToString(ObjCustomerDetails.Rows[0]["Posting_Code"]), Convert.ToString(ObjCustomerDetails.Rows[0]["Customer_Posting_Group_Code_ID"]), true);
                ListPosting.Selected = false;
                if (ddlPostingGroup.Items.FindByValue(Convert.ToString(ObjCustomerDetails.Rows[0]["Customer_Posting_Group_Code_ID"])) == null)
                {
                    if (ddlPostingGroup.Items.FindByText(Convert.ToString(ObjCustomerDetails.Rows[0]["Posting_Code"])) != null)
                    {
                        ddlPostingGroup.Items.Remove(ddlPostingGroup.Items.FindByText(Convert.ToString(ObjCustomerDetails.Rows[0]["Posting_Code"])));
                    }
                    ddlPostingGroup.Items.Add(ListPosting);
                }

                ddlPostingGroup.SelectedValue = Convert.ToString(ObjCustomerDetails.Rows[0]["Customer_Posting_Group_Code_ID"]);
                txtComAddress1.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["Comm_Address1"]);
                txtCOmAddress2.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["Comm_Address2"]);
                //<<Performance>>
                //txtComCity.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["Comm_City"]);
                txtComCity.SelectedText = Convert.ToString(ObjCustomerDetails.Rows[0]["Comm_City"]);

                if (strMode == "Q")
                {
                    txtComState.Items.Add(Convert.ToString(ObjCustomerDetails.Rows[0]["Comm_State"]));
                    txtComCountry.Items.Add(Convert.ToString(ObjCustomerDetails.Rows[0]["Comm_Country"]));
                    txtPerState.Items.Add(Convert.ToString(ObjCustomerDetails.Rows[0]["Perm_State"]));
                    txtPerCountry.Items.Add(Convert.ToString(ObjCustomerDetails.Rows[0]["Perm_Country"]));
                }
                txtComState.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["Comm_State"]);
                txtComCountry.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["Comm_Country"]);
                txtComPincode.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["Comm_PINCode"]);
                txtComMobile.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["Comm_Mobile"]);
                txtComTelephone.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["Comm_Telephone"]);
                txtComEmail.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["Comm_Email"]);
                txtComWebsite.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["Comm_Website"]);
                txtPerAddress1.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["Perm_Address1"]);
                txtPerAddress2.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["Perm_Address2"]);
                //<<Performance>>
                //txtPerCity.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["Perm_City"]);
                txtPerCity.SelectedText = Convert.ToString(ObjCustomerDetails.Rows[0]["Perm_City"]);

                txtPerState.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["Perm_State"]);
                txtPerCountry.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["Perm_Country"]);
                txtPerPincode.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["Perm_PINCode"]);
                txtPerMobile.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["Perm_Mobile"]);
                txtPerTelephone.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["Perm_Telephone"]);
                txtPerEmail.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["Perm_Email"]);
                txtPerWebSite.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["Perm_Website"]);
                if (ddlCustomerType.SelectedItem.Text.ToUpper() == "INDIVIDUAL")
                {

                    #region Individual Type
                    if (ObjCustomerDetails.Rows[0]["Gender"] == DBNull.Value)
                    {
                        ddlGender.SelectedIndex = 0;
                    }
                    else
                    {
                        ddlGender.SelectedValue = Convert.ToInt16(ObjCustomerDetails.Rows[0]["Gender"]).ToString();
                    }
                    if (Convert.ToString(ObjCustomerDetails.Rows[0]["DateofBirth"]) != "") txtDOB.Text = DateTime.Parse(Convert.ToString(ObjCustomerDetails.Rows[0]["DateofBirth"]), CultureInfo.CurrentCulture.DateTimeFormat).ToString(ObjS3GSession.ProDateFormatRW);

                    //Check if not exist
                    if (txtDOB.Text == "1/1/1900")
                    {
                        txtDOB.Text = string.Empty;
                    }
                    if (!string.IsNullOrEmpty(txtDOB.Text))
                    {
                        int intDOBYear = Utility.StringToDate(txtDOB.Text).Year;
                        txtAge.Text = ((DateTime.Now.Year - intDOBYear) + 1).ToString();
                    }
                    if (ObjCustomerDetails.Rows[0]["Marital_Status_ID"] == DBNull.Value)
                    {
                        ddlMaritalStatus.SelectedIndex = -1;
                    }
                    else
                    {
                        if (strMode == "Q")
                        {
                            lst = new ListItem(Convert.ToString(ObjCustomerDetails.Rows[0]["Marital_Status"]), Convert.ToString(ObjCustomerDetails.Rows[0]["Marital_Status_ID"]));
                            ddlMaritalStatus.Items.Add(lst);
                        }

                        ddlMaritalStatus.SelectedValue = Convert.ToString(ObjCustomerDetails.Rows[0]["Marital_Status_ID"]);
                    }
                    txtQualification.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["Qualification"]);
                    txtProfession.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["Profession"]);
                    txtSpouseName.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["Spouse_Name"]);
                    txtChildren.Text = (Convert.ToString(ObjCustomerDetails.Rows[0]["Children"]) == "0") ? "" : Convert.ToString(ObjCustomerDetails.Rows[0]["Children"]);
                    txtTotalDependents.Text = (Convert.ToString(ObjCustomerDetails.Rows[0]["Total_Dependents"]) == "0") ? "" : Convert.ToString(ObjCustomerDetails.Rows[0]["Total_Dependents"]);
                    if (Convert.ToString(ObjCustomerDetails.Rows[0]["Wedding_Anniversary_Date"]) != "") txtWeddingdate.Text = DateTime.Parse(Convert.ToString(ObjCustomerDetails.Rows[0]["Wedding_Anniversary_Date"]), CultureInfo.CurrentCulture.DateTimeFormat).ToString(ObjS3GSession.ProDateFormatRW);
                    if (txtWeddingdate.Text == "1/1/1900")
                    {
                        txtWeddingdate.Text = string.Empty;
                    }

                    if (ObjCustomerDetails.Rows[0]["House_Flat_ID"] == DBNull.Value)
                    {
                        ddlHouseType.SelectedIndex = -1;
                    }
                    else
                    {
                        if (strMode == "Q")
                        {
                            lst = new ListItem(Convert.ToString(ObjCustomerDetails.Rows[0]["House_Flat"]), Convert.ToString(ObjCustomerDetails.Rows[0]["House_Flat_ID"]));
                            ddlHouseType.Items.Add(lst);
                        }

                        ddlHouseType.SelectedValue = Convert.ToString(ObjCustomerDetails.Rows[0]["House_Flat_ID"]);
                    }
                    if (ObjCustomerDetails.Rows[0]["Is_Own"] == DBNull.Value)
                    {
                        ddlOwn.SelectedIndex = -1;
                    }
                    else
                    {
                        ddlOwn.SelectedValue = (Convert.ToString(ObjCustomerDetails.Rows[0]["Is_Own"]).ToLower() == "true") ? "1" : "0";
                    }
                    //Added by ganapathy to fix the bugID 6343
                    if (ddlOwn.SelectedValue == "0")
                    {
                        txtCurrentMarketValue.Enabled = false;
                        txtRemainingLoanValue.Enabled = false;
                        txtTotNetWorth.Enabled = false;
                    }
                    else
                    {
                        txtCurrentMarketValue.Enabled = true;
                        txtRemainingLoanValue.Enabled = true;
                        txtTotNetWorth.Enabled = true;
                    }
                    //End here
                    //Commented by Thangam M on 09/Mar/2012 to fix bug id 5445
                    //txtCurrentMarketValue.Text = (Convert.ToString(ObjCustomerDetails.Rows[0]["Current_Market_Value"]) == "0") ? "" : Convert.ToString(ObjCustomerDetails.Rows[0]["Current_Market_Value"]);
                    //End here
                    //Code Modified By Ganapathy for fixing the bug 6343
                    txtCurrentMarketValue.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["Current_Market_Value"]);
                    //txtCurrentMarketValue.Text = (Convert.ToString(ObjCustomerDetails.Rows[0]["Current_Market_Value"]) == "0") ? "" : Convert.ToString(ObjCustomerDetails.Rows[0]["Current_Market_Value"]);
                    //txtRemainingLoanValue.Text = (Convert.ToString(ObjCustomerDetails.Rows[0]["Remaining_Loan_Value"]) == "0") ? "" : Convert.ToString(ObjCustomerDetails.Rows[0]["Remaining_Loan_Value"]);
                    txtRemainingLoanValue.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["Remaining_Loan_Value"]);
                    //txtTotNetWorth.Text = (Convert.ToString(ObjCustomerDetails.Rows[0]["Total_Net_Morth"]) == "0") ? "" : Convert.ToString(ObjCustomerDetails.Rows[0]["Total_Net_Morth"]);
                    txtTotNetWorth.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["Total_Net_Morth"]);
                    //End here
                    #endregion
                }
                else
                {
                    #region Non-Individual

                    if (strMode == "Q")
                    {
                        lst = new ListItem(Convert.ToString(ObjCustomerDetails.Rows[0]["Public_Closely"]), Convert.ToString(ObjCustomerDetails.Rows[0]["Public_Closely_ID"]));
                        ddlPublic.Items.Add(lst);
                    }

                    ddlPublic.SelectedValue = Convert.ToString(ObjCustomerDetails.Rows[0]["Public_Closely_ID"]);
                    txtDirectors.Text = (Convert.ToString(ObjCustomerDetails.Rows[0]["No_Of_Directors"]) == "0") ? "" : Convert.ToString(ObjCustomerDetails.Rows[0]["No_Of_Directors"]);
                    txtSE.Text = (Convert.ToString(ObjCustomerDetails.Rows[0]["Listed_At_Stock_Exchange"]) == "0") ? "" : Convert.ToString(ObjCustomerDetails.Rows[0]["Listed_At_Stock_Exchange"]);
                    txtPaidCapital.Text = (Convert.ToString(ObjCustomerDetails.Rows[0]["Paidup_Capital"]) == "0") ? "" : Convert.ToString(ObjCustomerDetails.Rows[0]["Paidup_Capital"]);
                    txtfacevalue.Text = (Convert.ToString(ObjCustomerDetails.Rows[0]["Face_Value_of_Shares"]) == "0") ? "" : Convert.ToString(ObjCustomerDetails.Rows[0]["Face_Value_of_Shares"]);
                    txtbookvalue.Text = (Convert.ToString(ObjCustomerDetails.Rows[0]["Book_Value_of_Shares"]) == "0") ? "" : Convert.ToString(ObjCustomerDetails.Rows[0]["Book_Value_of_Shares"]);
                    txtBusinessProfile.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["Business_Profile"]);
                    txtGeographical.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["Geographical_Coverage"]);
                    txtnobranch.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["No_Of_Branches"]);

                    if (strMode == "Q")
                    {
                        lst = new ListItem(Convert.ToString(ObjCustomerDetails.Rows[0]["Participation"]), Convert.ToString(ObjCustomerDetails.Rows[0]["Participation_ID"]));
                        ddlGovernment.Items.Add(lst);
                    }

                    ddlGovernment.SelectedValue = Convert.ToString(ObjCustomerDetails.Rows[0]["Participation_ID"]);

                    //Changed By Thangam M on 12/Mar/2012 to fix bug id - 5447
                    txtPercentageStake.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["Percentage_Of_Stake"]);
                    //txtPercentageStake.Text = (Convert.ToString(ObjCustomerDetails.Rows[0]["Percentage_Of_Stake"]).StartsWith("0")) ? "" : Convert.ToString(ObjCustomerDetails.Rows[0]["Percentage_Of_Stake"]);

                    txtJVPartnerName.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["JV_Partner_Name"]);

                    txtJVPartnerStake.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["JV_Partner_Stake"]);
                    //txtJVPartnerStake.Text = (Convert.ToString(ObjCustomerDetails.Rows[0]["JV_Partner_Stake"]).StartsWith("0")) ? "" : Convert.ToString(ObjCustomerDetails.Rows[0]["JV_Partner_Stake"]);
                    //End here

                    txtCEOName.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["CEO_Name"]);
                    txtCEOAge.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["CEO_Age"]);
                    if (Convert.ToString(ObjCustomerDetails.Rows[0]["CEO_Wedding_Date"]) != "") txtCEOWeddingDate.Text = DateTime.Parse(Convert.ToString(ObjCustomerDetails.Rows[0]["CEO_Wedding_Date"]), CultureInfo.CurrentCulture.DateTimeFormat).ToString(ObjS3GSession.ProDateFormatRW);
                    if (txtCEOWeddingDate.Text == "1/1/1900")
                    {
                        txtCEOWeddingDate.Text = string.Empty;
                    }

                    txtCEOexperience.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["CEO_Experience_In_Years"]);
                    txtResidentialAddress.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["Residential_Address"]);
                    #endregion
                }
                FunPriLoadCustomerBankDetails(CustomerID);
                FunPriLoadCustomerConstitutionDocs(CustomerID);
                FunPriLoadCustomerTrackDetails(CustomerID);
                FunPriToggleCustomerTypeControls();
                FunProClearUploader();
            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
        finally
        {
            objCustomerMasterClient.Close();
        }
    }

    #region Alert Tab

    private void FunPriBindCustomerTrackDLL()
    {
        try
        {
            Dictionary<string, string> ProcParam = new Dictionary<string, string>();
            ProcParam.Add("@CompanyId", intCompanyId.ToString());
            ProcParam.Add("@CustomerId", intCustomerId.ToString());
            ProcParam.Add("@UserId", intUserId.ToString());
            DataSet dsCustomerTrackDDL = Utility.GetDataset("S3G_ORG_LOADCUSTOMERTRACKDDL", ProcParam);
            ViewState["TrackLOB"] = dsCustomerTrackDDL.Tables[0];
            ViewState["TrackAccountNo"] = dsCustomerTrackDDL.Tables[1];
            ViewState["TrackType"] = dsCustomerTrackDDL.Tables[2];

            DataTable objTrack = ViewState["CustomerTrackDetails"] as DataTable;
            bool blnIsFooterOnly = false;
            if (objTrack.Rows.Count == 0)
            {
                DataRow drTrack = objTrack.NewRow();
                drTrack["LOB_ID"] = 0;
                drTrack["LOB_NAME"] = "";
                drTrack["PA_SA_REF_ID"] = 0;
                drTrack["ACCOUNTNO"] = "";
                drTrack["DATE"] = "";
                drTrack["RELEASEDATE"] = "";
                drTrack["LOGINBY"] = "";
                drTrack["RELEASED_BY"] = 0;
                drTrack["RELEASEDBY"] = "";
                drTrack["REASON"] = "";
                drTrack["TRACK_TYPE"] = "";
                drTrack["TRACK_TYPE_ID"] = 0;
                objTrack.Rows.Add(drTrack);
                blnIsFooterOnly = true;
            }
            gvTrack.DataSource = objTrack;
            gvTrack.DataBind();
            if (blnIsFooterOnly)
            {
                objTrack.Rows.Clear();
                ViewState["CustomerTrackDetails"] = objTrack;

                gvTrack.Rows[0].Cells.Clear();
                gvTrack.Rows[0].Visible = false;
            }
            FunPriGenerateNewAlertRow();


        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    private void FunPriGenerateNewAlertRow()
    {
        try
        {

            DropDownList ddlLOBTrack = gvTrack.FooterRow.FindControl("ddlLOBTrack") as DropDownList;
            DropDownList ddlAccountNo = gvTrack.FooterRow.FindControl("ddlAccountNo") as DropDownList;
            DropDownList ddlType = gvTrack.FooterRow.FindControl("ddlType") as DropDownList;

            Utility.FillDLL(ddlLOBTrack, ((DataTable)ViewState["TrackLOB"]), true);
            Utility.FillDLL(ddlAccountNo, ((DataTable)ViewState["TrackAccountNo"]), true);
            Utility.FillDLL(ddlType, ((DataTable)ViewState["TrackType"]), true);

            ddlLOBTrack.Focus();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void Track_AddRow_OnClick(object sender, EventArgs e)
    {

        DataTable objTrack = (DataTable)ViewState["CustomerTrackDetails"];

        DropDownList ddlLOBTrack = gvTrack.FooterRow.FindControl("ddlLOBTrack") as DropDownList;
        DropDownList ddlAccountNo = gvTrack.FooterRow.FindControl("ddlAccountNo") as DropDownList;
        DropDownList ddlType = gvTrack.FooterRow.FindControl("ddlType") as DropDownList;
        TextBox txtDate = gvTrack.FooterRow.FindControl("txtDate") as TextBox;
        TextBox txtReleaseDate = gvTrack.FooterRow.FindControl("txtReleaseDate") as TextBox;
        TextBox txtReason = gvTrack.FooterRow.FindControl("txtReason") as TextBox;
        DataRow drTrack = objTrack.NewRow();
        drTrack["LOB_ID"] = ddlLOBTrack.SelectedValue;
        drTrack["LOB_NAME"] = ddlLOBTrack.SelectedItem.Text;
        drTrack["PA_SA_REF_ID"] = ddlAccountNo.SelectedValue;
        drTrack["ACCOUNTNO"] = ddlAccountNo.SelectedItem.Text;
        drTrack["DATE"] = txtDate.Text;
        drTrack["RELEASEDATE"] = txtReleaseDate.Text;
        drTrack["LOGINBY"] = ObjUserInfo.ProUserNameRW;
        drTrack["RELEASED_BY"] = intUserId.ToString();
        drTrack["RELEASEDBY"] = ObjUserInfo.ProUserNameRW;
        drTrack["REASON"] = txtReason.Text;
        drTrack["TRACK_TYPE"] = ddlType.SelectedItem.Text;
        drTrack["TRACK_TYPE_ID"] = ddlType.SelectedValue;
        objTrack.Rows.Add(drTrack);

        gvTrack.DataSource = objTrack;
        gvTrack.DataBind();
        ViewState["CustomerTrackDetails"] = objTrack;
        FunPriGenerateNewAlertRow();
        //}
        //else
        //{
        //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('Please select Email or SMS');", true);

        //}
    }

    protected void gvTrack_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                TextBox txtDate = e.Row.FindControl("txtDate") as TextBox;
                txtDate.Attributes.Add("readonly", "readonly");
                AjaxControlToolkit.CalendarExtender CalendarDate = e.Row.FindControl("CalendarDate") as AjaxControlToolkit.CalendarExtender;
                CalendarDate.Format = strDateFormat;

                TextBox txtReleaseDate = e.Row.FindControl("txtReleaseDate") as TextBox;
                txtReleaseDate.Attributes.Add("readonly", "readonly");
                AjaxControlToolkit.CalendarExtender CalendarReleaseDate = e.Row.FindControl("CalendarReleaseDate") as AjaxControlToolkit.CalendarExtender;
                CalendarReleaseDate.Format = strDateFormat;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
            cvCustomerMaster.ErrorMessage = "Due to Data Problem,Unable to Set the DateFormat in Customer Track Details";
            cvCustomerMaster.IsValid = false;
        }
    }

    protected void gvTrack_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblLOBName = (Label)e.Row.FindControl("lblLOBName");
                Label lblAccountNo = (Label)e.Row.FindControl("lblAccountNo");
                Label lblTypeId = (Label)e.Row.FindControl("lblTypeId");
                Label lblDate = (Label)e.Row.FindControl("lblDate");
                Label lblReason = (Label)e.Row.FindControl("lblReason");
                Label lblReleaseDate = (Label)e.Row.FindControl("lblReleaseDate");
                Label lblReleasedBy = (Label)e.Row.FindControl("lblReleasedBy");
                if (ViewState["CustomerTrackDetails"] != null)
                {
                    DataTable dtCustomerTrack = (DataTable)ViewState["CustomerTrackDetails"];
                    lblLOBName.Text = Convert.ToString(dtCustomerTrack.Rows[e.Row.RowIndex]["LOB_NAME"]);
                    lblAccountNo.Text = Convert.ToString(dtCustomerTrack.Rows[e.Row.RowIndex]["ACCOUNTNO"]);
                    lblReason.Text = Convert.ToString(dtCustomerTrack.Rows[e.Row.RowIndex]["REASON"]);
                    lblDate.Text = Convert.ToString(dtCustomerTrack.Rows[e.Row.RowIndex]["DATE"]);
                    lblReleaseDate.Text = Convert.ToString(dtCustomerTrack.Rows[e.Row.RowIndex]["RELEASEDATE"]);
                    lblTypeId.Text = Convert.ToString(dtCustomerTrack.Rows[e.Row.RowIndex]["TRACK_TYPE"]);
                    if (dtCustomerTrack.Rows[e.Row.RowIndex]["RELEASEDBY"].ToString() == "")
                    {
                        lblReleasedBy.Text = intUserId.ToString();
                        e.Row.Cells[8].Text =
                        e.Row.Cells[11].Text = ObjUserInfo.ProUserNameRW;
                    }
                    else
                    {
                        lblReleasedBy.Text = Convert.ToString(dtCustomerTrack.Rows[e.Row.RowIndex]["RELEASED_BY"]);
                        e.Row.Cells[8].Text = Convert.ToString(dtCustomerTrack.Rows[e.Row.RowIndex]["LOGINBY"]);
                        e.Row.Cells[11].Text = Convert.ToString(dtCustomerTrack.Rows[e.Row.RowIndex]["RELEASEDBY"]);
                    }
                }

            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }
    #endregion
    private void FunPriLoadCustomerTrackDetails(int intCustomerId)
    {
        OrgMasterMgtServicesReference.OrgMasterMgtServicesClient objCustomerMasterClient = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
        try
        {

            S3GBusEntity.S3G_Status_Parameters ObjStatus = new S3G_Status_Parameters();
            ObjStatus.Option = 303;
            ObjStatus.Param1 = intCustomerId.ToString();
            ObjStatus.Param2 = intUserId.ToString();
            ObjStatus.Param3 = intCompanyId.ToString();
            DataTable ObjCustomerTrackDetails = objCustomerMasterClient.FunPub_GetS3GStatusLookUp(ObjStatus);
            ViewState["CustomerTrackDetails"] = ObjCustomerTrackDetails;
            FunPriBindCustomerTrackDLL();
            if (gvTrack.Rows.Count == 1 && gvTrack.FooterRow.Visible)
            {
                lblNoCustomerTrack.Visible = true;
                //gvTrack.Visible = false;
                //divTrack.Visible = false;
            }
            else
            {
                lblNoCustomerTrack.Visible = false;
                gvTrack.Visible = true;
                divTrack.Visible = true;
            }
            Dictionary<string, string> objProcedureParameters = new Dictionary<string, string>();
            objProcedureParameters.Add("@CustomerId", intCustomerId.ToString());
            gvCredit.DataSource = Utility.GetDefaultData("s3g_org_LoadCustomerCreditDetails", objProcedureParameters);
            gvCredit.DataBind();

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Due to Data Problem, Unable to Load the Customer Track Details");
        }
        finally
        {
            objCustomerMasterClient.Close();
        }

    }
    /// <summary>
    /// Toggle the Controls based on User's AccessPermission
    /// </summary>
    /// <param name="intModeID"></param>
    private void FunPriDisableControls(int intModeID)
    {
        try
        {
            if (!bQuery)
            {
                Response.Redirect(strRedirectPageView);
            }


            switch (intModeID)
            {
                case 0: // Create Mode
                    FunPriToggleLoanAdminControls(false);
                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);
                    txtCustomercode.Enabled = true;
                    txtCustomerName.Enabled = true;
                    btnClear.Enabled = true;
                    ddlGender.SelectedIndex = 0;
                    if ((!bCreate))
                    {
                        btnSave.Enabled = false;
                    }
                    btnModify.Enabled = false;
                    break;

                case 1: // Modify Mode
                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                    FunPriSetControlSettings();
                    FunPriLoadCustomerDetails(intCustomerId);
                    FunPriToggleCustomerTypeControls();
                    btnModify.Enabled = false;
                    tcCustomerMaster.ActiveTabIndex = 0;
                    lblCustomercode.Visible = true;
                    txtCustomercode.Visible = true;

                    //Lined by Thangam M 0n 28/Feb/2012 for bug id 5453
                    //txtCustomerName.ReadOnly = true;
                    //End here

                    txtAge.ReadOnly = true;
                    if ((!bModify))
                    {
                        btnSave.Enabled = false;
                    }
                    btnClear.Enabled = false;
                    Dictionary<string, string> ProcParam = new Dictionary<string, string>();
                    ProcParam.Add("@COMPANYID", intCompanyId.ToString());
                    ProcParam.Add("@CUSTOMERID", intCustomerId.ToString());
                    DataSet dsAccountCount = Utility.GetDataset("S3G_ORG_GETACCOUNTCOUNT", ProcParam);
                    if (Convert.ToInt32(dsAccountCount.Tables[0].Rows[0][0]) > 0)
                    {
                        txtCustomerName.ReadOnly = true;
                        if (bClearList)
                        {
                            ddlTitle.RemoveDropDownList();
                            ddlCustomerType.RemoveDropDownList();
                            ddlConstitutionName.RemoveDropDownList();
                            ddlPostingGroup.RemoveDropDownList();
                            ddlGender.ClearDropDownList();

                        }
                        CalendarExtenderSD.Enabled = false;
                        //cmbIndustryCode.ReadOnly = txtIndustryName.ReadOnly = true;
                    }
                    break;
                case -1:// Query Mode
                    FunPriSetControlSettings();
                    FunPriLoadCustomerDetails(intCustomerId);
                    FunPriToggleCustomerTypeControls();
                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                    tcCustomerMaster.ActiveTabIndex = 0;
                    lblCustomercode.Visible = true;
                    txtCustomercode.Visible = true;
                    FunPriToggleQueryModeControls();
                    cmbGroupCode.Enabled = false;
                    cmbIndustryCode.Enabled = false;
                    btnCopyAddress.Visible = false;
                    chkCustomer.Enabled = false;
                    chkGuarantor1.Enabled = false;
                    chkGuarantor2.Enabled = false;
                    chkCoApplicant.Enabled = false;
                    break;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    private void FunPriToggleQueryModeControls()
    {
        txtCustomerName.ReadOnly = true;
        tblBankDetails.Visible = false;
        if (bClearList)
        {
            ddlCustomerType.RemoveDropDownList();
            ddlConstitutionName.RemoveDropDownList();
            ddlGender.ClearDropDownList();
            ddlGovernment.RemoveDropDownList();
            ddlHouseType.RemoveDropDownList();

            ddlMaritalStatus.RemoveDropDownList();
            ddlOwn.RemoveDropDownList();
            ddlPostingGroup.RemoveDropDownList();
            ddlPublic.RemoveDropDownList();
            ddlTitle.RemoveDropDownList();
        }
        CalendarExtenderSD.Enabled = false;
        //<<Performance>>
        //txtComCity.DropDownStyle = 
        txtComCity.ReadOnly = true;
        txtComState.DropDownStyle = txtComCountry.DropDownStyle = AjaxControlToolkit.ComboBoxStyle.DropDownList;
        //txtComCity.ClearDropDownList();
        txtComState.ClearDropDownList();
        txtComCountry.ClearDropDownList();

        //<<Performance>>
        //txtPerCity.DropDownStyle = 
        txtPerCountry.DropDownStyle = txtPerState.DropDownStyle = AjaxControlToolkit.ComboBoxStyle.DropDownList;
        //txtPerCity.ClearDropDownList();
        txtPerCity.ReadOnly = true;
        txtPerState.ClearDropDownList();
        txtPerCountry.ClearDropDownList();

        //txtComCity.ReadOnly = txtComCountry.ReadOnly = txtComState.ReadOnly =
        // = txtPerCity.ReadOnly = txtPerCountry.ReadOnly = txtPerState.ReadOnly = 
        cmbGroupCode.ReadOnly = cmbIndustryCode.ReadOnly =
        txtIndustryName.ReadOnly = txtGroupName.ReadOnly =
        txtComAddress1.ReadOnly = txtCOmAddress2.ReadOnly = txtComEmail.ReadOnly =
        txtComMobile.ReadOnly = txtComPincode.ReadOnly = txtComTelephone.ReadOnly = txtComWebsite.ReadOnly =
        txtPerAddress1.ReadOnly = txtPerAddress2.ReadOnly = txtPerEmail.ReadOnly =
        txtPerMobile.ReadOnly = txtPerPincode.ReadOnly = txtPerTelephone.ReadOnly = txtPerWebSite.ReadOnly =
        txtQualification.ReadOnly = txtProfession.ReadOnly = txtSpouseName.ReadOnly = txtChildren.ReadOnly = txtTotalDependents.ReadOnly =
        txtCurrentMarketValue.ReadOnly = txtRemainingLoanValue.ReadOnly = txtTotNetWorth.ReadOnly =
        txtDirectors.ReadOnly = txtSE.ReadOnly = txtPaidCapital.ReadOnly = txtfacevalue.ReadOnly = txtbookvalue.ReadOnly =
        txtBusinessProfile.ReadOnly = txtGeographical.ReadOnly = txtnobranch.ReadOnly = txtPercentageStake.ReadOnly =
        txtJVPartnerName.ReadOnly = txtJVPartnerStake.ReadOnly = txtCEOName.ReadOnly = txtCEOAge.ReadOnly = txtCEOexperience.ReadOnly =
        txtResidentialAddress.ReadOnly = txtAccountNumber.ReadOnly = txtBankAddress.ReadOnly = txtBankBranch.ReadOnly =
        txtBankName.ReadOnly = txtMICRCode.ReadOnly = true;
        CalendarCEOWeddingDate.Enabled = false;
        CalendarWeddingdate.Enabled = false;

        btnAdd.Enabled = btnModify.Enabled = btnClear.Enabled = btnSave.Enabled = false;
        /*Modified By prabhu on 16-Nov-2011 for CR (Default Account in Bank Details)*/
        grvBankDetails.Columns[11].Visible = false;
        if (gvTrack.Rows.Count > 0)
        {
            if (gvTrack.Columns[12] != null) gvTrack.Columns[12].Visible = false;
            //gvTrack.FooterRow.Visible = false;
        }
    }
    private void FunPriToggleModifyModeControls()
    {

    }


    #endregion

    #region Dropdown Events

    protected void ddlCustomerType_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriToggleCustomerTypeControls();
            ddlCustomerType.Focus();
            grvBankDetails.DataSource = null;
            grvBankDetails.DataBind();

            //Added By Thangam M on 09/mar/2012 to fix bug id - 6031
            FunProIntializeData();
            //End

            if (ddlCustomerType.SelectedItem.ToString().ToLower() == "non individual")
            {
                rfvComEmail.Enabled = true;
                rfvPerEmail.Enabled = true;

            }
            else
            {
                txtIndustryName.Text = "";
                cmbIndustryCode.Text = "";
                rfvComEmail.Enabled = false;
                rfvPerEmail.Enabled = false;
            }
            upAddress.Update();
            upPersonal.Update();
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException, strPageName);
            cvCustomerMaster.ErrorMessage = Resources.LocalizationResources.CustomerTypeChangeError;
            cvCustomerMaster.IsValid = false;
        }
    }

    protected void ddlConstitutionName_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        OrgMasterMgtServicesReference.OrgMasterMgtServicesClient objCustomerMasterClient = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
        try
        {

            if (Convert.ToInt16(ddlConstitutionName.SelectedValue) > 0)
            {
                S3GBusEntity.S3G_Status_Parameters ObjStatus = new S3G_Status_Parameters();
                ObjStatus.Option = 7;
                ObjStatus.Param1 = ddlConstitutionName.SelectedValue.ToString();
                gvConstitutionalDocuments.DataSource = objCustomerMasterClient.FunPub_GetS3GStatusLookUp(ObjStatus);
                gvConstitutionalDocuments.DataBind();

                //Added By Thangam on 12/Mar/2012 to fix bug id - 5451
                FunProClearUploader();

                ViewState["ConstitutionDocument"] = objCustomerMasterClient.FunPub_GetS3GStatusLookUp(ObjStatus);
                ddlConstitutionName.Focus();
                ObjStatus.Option = 787;
                ObjStatus.Param1 = intCompanyId.ToString();
                DataTable dtConsDocPath = objCustomerMasterClient.FunPub_GetS3GStatusLookUp(ObjStatus);
                if (dtConsDocPath == null)
                {
                    Utility.FunShowAlertMsg(this, "Define the spooling path in Document Path Setup for Consitution Document");
                    return;
                }
                if (dtConsDocPath.Rows.Count == 0)
                {
                    Utility.FunShowAlertMsg(this, "Define the spooling path in Document Path Setup for Consitution Document");
                    return;
                }
                ViewState["ConsDocPath"] = dtConsDocPath.Rows[0][0].ToString();
            }
            else
            {
                tcCustomerMaster.Tabs[3].Enabled = false;
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException, strPageName);
            cvCustomerMaster.ErrorMessage = Resources.LocalizationResources.ConstitutionChangeError;
            cvCustomerMaster.IsValid = false;
        }
        finally
        {
            objCustomerMasterClient.Close();
            upConstitution.Update();
        }
    }

    protected void cmbGroupCode_OnTextChanged(object sender, EventArgs e)
    {
        try
        {

            txtGroupName.Text = string.Empty;
            txtGroupName.Focus();

            DataTable dt = (DataTable)HttpContext.Current.Session["GroupDT"];

            if (dt.Rows.Count > 0)
            {
                string filterExpression = "Value = '" + cmbGroupCode.Text + "'";
                DataRow[] dtSuggestions = dt.Select(filterExpression);

                if (dtSuggestions.Length > 0)
                {
                    txtGroupName.Text = dtSuggestions[0]["Description"].ToString();
                    //txtGroupName.ReadOnly = true;
                    txtGroupName.ReadOnly = false;
                }
                else
                {
                    txtGroupName.ReadOnly = false;
                }
            }
            if (cmbGroupCode.Text == string.Empty)
            {
                //txtGroupName.ReadOnly = false;
                txtGroupName.ReadOnly = true;
                txtGroupName.Text = string.Empty;
            }


        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            cvCustomerMaster.ErrorMessage = Resources.LocalizationResources.GroupChangeError;
            cvCustomerMaster.IsValid = false;
        }
    }

    protected void cmbIndustryCode_OnTextChanged(object sender, EventArgs e)
    {
        txtIndustryName.Text = string.Empty;
        try
        {
            DataTable dt = (DataTable)HttpContext.Current.Session["IndustryDT"];

            if (dt.Rows.Count > 0)
            {
                string filterExpression = "Value = '" + cmbIndustryCode.Text + "' and Company_Id = " + intCompanyId.ToString();
                DataRow[] dtSuggestions = dt.Select(filterExpression);
                if (dtSuggestions.Length > 0)
                {
                    txtIndustryName.Text = dtSuggestions[0]["Description"].ToString();
                    //txtIndustryName.ReadOnly = true;
                    txtIndustryName.ReadOnly = false;
                }
                else
                {
                    txtIndustryName.ReadOnly = false;
                }
            }
            if (cmbIndustryCode.Text == string.Empty)
            {
                //txtIndustryName.ReadOnly = false;
                txtIndustryName.ReadOnly = true;
                txtIndustryName.Text = string.Empty;
            }
            upIndustryCode.Update();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            cvCustomerMaster.ErrorMessage = "Due to Data Problem, Unable to Load the Industry Name";
            cvCustomerMaster.IsValid = false;
        }
    }

    #endregion

    #region Grid Events

    /// <summary>
    /// Disable the "Values" Control in Constitutional Document Tab
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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
                AjaxControlToolkit.AsyncFileUpload asyFileUpload = e.Row.FindControl("asyFileUpload") as AjaxControlToolkit.AsyncFileUpload;
                CheckBox chkCollected = e.Row.FindControl("chkCollected") as CheckBox;
                CheckBox chkScanned = e.Row.FindControl("chkScanned") as CheckBox;
                TextBox txtValues = e.Row.FindControl("txtValues") as TextBox;

                if (!chkIsNeedImageCopy.Checked)
                {
                    chkScanned.Enabled = false;
                }

               // AjaxControlToolkit.AsyncFileUpload AsyncFileUpload1 = (AjaxControlToolkit.AsyncFileUpload)e.Row.FindControl("asyFileUpload");
                if (lblPath != null)
                {
                    if (lblPath.Text.Trim() != "")
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
                    asyFileUpload.Enabled = false;
                    hlnkView.Enabled = false;
                }

                

                TextBox txtVal = (TextBox)e.Row.FindControl("txtValues");
                if (txtVal != null)
                {
                    txtVal.Enabled = FunPriDisableValueField(e.Row.Cells[1].Text);
                }
                if (Request.QueryString["qsMode"].ToString() == "Q")
                {
                    asyFileUpload.Enabled =
                    chkIsMandatory.Enabled = chkIsNeedImageCopy.Enabled = chkCollected.Enabled =
                        chkScanned.Enabled = txtValues.Enabled = false;
                }
            }
            e.Row.Cells[11].Visible = false;

        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException, strPageName);
            cvCustomerMaster.ErrorMessage = "Unable to Toggle the Values in Constitution Document Details";
            cvCustomerMaster.IsValid = false;
        }
    }

    //Added by Thangam M on 12/Mar/2012 to fix bug id - 5451
    private void FunPriClearUploader(AjaxControlToolkit.AsyncFileUpload Uploader)
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

    //Added by Thangam M on 12/Mar/2012 to fix bug id - 5451
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

                ScriptManager.RegisterStartupScript(this, this.GetType(), "Clear", "javascript:fnClearAsyncUploader('" + gvConstitutionalDocuments.Rows.Count.ToString() + "');", true);

            }
        }
        catch (Exception)
        {

        }
    }

    protected void gvCustomerTrack_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddlType = (DropDownList)e.Row.FindControl("ddlType");
                OrgMasterMgtServicesReference.OrgMasterMgtServicesClient objCustomerMasterClient = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
                S3GBusEntity.S3G_Status_Parameters ObjStatus = new S3G_Status_Parameters();
                ObjStatus.Option = 1;
                ObjStatus.Param1 = S3G_Statu_Lookup.CATEGORY_TYPE.ToString();
                Utility.FillDLL(ddlType, objCustomerMasterClient.FunPub_GetS3GStatusLookUp(ObjStatus));

                if (ViewState["CustomerTrackDetails"] != null)
                {
                    DataTable dtCustomerTrack = (DataTable)ViewState["CustomerTrackDetails"];
                    ddlType.SelectedValue = dtCustomerTrack.Rows[e.Row.RowIndex]["TRACK_TYPE_ID"].ToString();
                    if (dtCustomerTrack.Rows[e.Row.RowIndex]["RELEASEDBY"].ToString() == "")
                    {
                        Label lblReleasedBy = (Label)e.Row.FindControl("lblReleasedBy");
                        lblReleasedBy.Text = intUserId.ToString();
                        e.Row.Cells[10].Text = ObjUserInfo.ProUserNameRW;
                    }
                }

            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }
    protected void gvCustomerTrack_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox txtDate = e.Row.FindControl("txtDate") as TextBox;
                txtDate.Attributes.Add("readonly", "readonly");
                AjaxControlToolkit.CalendarExtender CalendarDate = e.Row.FindControl("CalendarDate") as AjaxControlToolkit.CalendarExtender;
                CalendarDate.Format = ObjS3GSession.ProDateFormatRW;

                TextBox txtReleaseDate = e.Row.FindControl("txtReleaseDate") as TextBox;
                txtReleaseDate.Attributes.Add("readonly", "readonly");
                AjaxControlToolkit.CalendarExtender CalendarReleaseDate = e.Row.FindControl("CalendarReleaseDate") as AjaxControlToolkit.CalendarExtender;
                CalendarReleaseDate.Format = ObjS3GSession.ProDateFormatRW;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
            cvCustomerMaster.ErrorMessage = "Due to Data Problem,Unable to Set the DateFormat in Customer Track Details";
            cvCustomerMaster.IsValid = false;
        }
    }

    #endregion

    #region TextBox Events

    /// <summary>
    /// Assign the Age field while DOB Changing 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void txtDOB_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            txtAge.Text = string.Empty;
            if (!string.IsNullOrEmpty(txtDOB.Text))
            {
                int intDOBYear = Utility.StringToDate(txtDOB.Text).Year;
                txtAge.Text = ((DateTime.Now.Year - intDOBYear) + 1).ToString();
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            cvCustomerMaster.ErrorMessage = Resources.LocalizationResources.AgeCalculateError;
            cvCustomerMaster.IsValid = false;
        }
    }
    #endregion

    #region Bank Details Tab
    protected void FunProIntializeData()
    {
        DataTable dtBankViewDetails = new DataTable("BankDetails");
        try
        {
            dtBankViewDetails.Columns.Add("AccountType");
            dtBankViewDetails.Columns.Add("AccountType_ID");
            dtBankViewDetails.Columns.Add("Account_Number");
            dtBankViewDetails.Columns.Add("Bank_Name");
            dtBankViewDetails.Columns.Add("Branch_Name");
            dtBankViewDetails.Columns.Add("MICR_Code");
            dtBankViewDetails.Columns.Add("Bank_Address");
            dtBankViewDetails.Columns.Add("BankMapping_ID");
            dtBankViewDetails.Columns.Add("Master_ID");
            dtBankViewDetails.Columns.Add("IsDefaultAccount", typeof(bool));
            dtBankViewDetails.Columns[4].Unique = true;
            ViewState["DetailsTable"] = dtBankViewDetails;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Initialize the Data for BankDetails");
        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtBankAddress.Text.Length > 300)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Followup Details", "alert('Bank Address Length should be less than or equal to 300');", true);
                tcCustomerMaster.ActiveTabIndex = 2;
                return;
            }
            DataRow drDetails;
            DataTable dtBankDetails = (DataTable)ViewState["DetailsTable"];
            /*Modified By prabhu on 16-Nov-2011 for CR (Default Account in Bank Details)*/
            DataRow[] drDefault = dtBankDetails.Select("IsDefaultAccount = 'True'");
            if (drDefault.Length > 0 && chkDefaultAccount.Checked)
            {
                Utility.FunShowAlertMsg(this, "Only one Default Account is applicable to particular Customer");
                return;
            }

            if (dtBankDetails.Rows.Count < 10)
            {
                drDetails = dtBankDetails.NewRow();
                string strBankMapId = Convert.ToString(dtBankDetails.Rows.Count + 1);
                drDetails["BankMapping_ID"] = strBankMapId;
                drDetails["Master_ID"] = "0";
                drDetails["AccountType"] = ddlAccountType.SelectedItem.Text;
                drDetails["AccountType_ID"] = ddlAccountType.SelectedItem.Value;
                drDetails["Account_Number"] = txtAccountNumber.Text.Trim();
                drDetails["Bank_Name"] = txtBankName.Text.Trim();
                drDetails["Branch_Name"] = txtBankBranch.Text.Trim();
                drDetails["MICR_Code"] = txtMICRCode.Text.Trim();
                drDetails["Bank_Address"] = txtBankAddress.Text.Trim();
                /*Modified By prabhu on 16-Nov-2011 for CR (Default Account in Bank Details)*/
                drDetails["IsDefaultAccount"] = chkDefaultAccount.Checked;
                dtBankDetails.Rows.Add(drDetails);
                grvBankDetails.DataSource = dtBankDetails;
                grvBankDetails.DataBind();
                FunPriHideBankColumns();
                ViewState["DetailsTable"] = dtBankDetails;
                ClearBankDetailsControls();

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Bank Details", "alert('Customer Bank Details should be less than or equal to 9 Banks');", true);
            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            cvCustomerMaster.ErrorMessage = "Due to Data Problem, Unable to Add Bank Details";
            cvCustomerMaster.IsValid = false;
        }
    }

    protected void btnModify_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtBankAddress.Text.Length > 300)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Followup Details", "alert('Bank Address Length should be less than or equal to 300');", true);
                tcCustomerMaster.ActiveTabIndex = 2;
                return;
            }
            DataTable dtBankDetails = (DataTable)ViewState["DetailsTable"];
            /*Modified By prabhu on 16-Nov-2011 for CR (Default Account in Bank Details)*/
            DataRow[] drDefault = dtBankDetails.Select("IsDefaultAccount = 'True' and BankMapping_ID <> '" + hdnBankId.Value + "'");
            if (drDefault.Length > 0 && chkDefaultAccount.Checked)
            {
                Utility.FunShowAlertMsg(this, "Only one Default Account is applicable to particular Customer");
                return;
            }
            DataView dvBankdetails = dtBankDetails.DefaultView;
            dvBankdetails.Sort = "BankMapping_ID";
            int rowindex = dvBankdetails.Find(hdnBankId.Value);
            dvBankdetails[rowindex].Row["AccountType"] = ddlAccountType.SelectedItem.Text;
            dvBankdetails[rowindex].Row["AccountType_ID"] = ddlAccountType.SelectedItem.Value;
            dvBankdetails[rowindex].Row["Account_Number"] = txtAccountNumber.Text.Trim();
            dvBankdetails[rowindex].Row["Bank_Name"] = txtBankName.Text.Trim();
            dvBankdetails[rowindex].Row["Branch_Name"] = txtBankBranch.Text.Trim();
            dvBankdetails[rowindex].Row["MICR_Code"] = txtMICRCode.Text.Trim();
            dvBankdetails[rowindex].Row["Bank_Address"] = txtBankAddress.Text.Trim();
            /*Modified By prabhu on 16-Nov-2011 for CR (Default Account in Bank Details)*/
            dvBankdetails[rowindex].Row["IsDefaultAccount"] = chkDefaultAccount.Checked;
            grvBankDetails.DataSource = dvBankdetails;
            grvBankDetails.DataBind();
            FunPriHideBankColumns();
            ViewState["DetailsTable"] = dvBankdetails.Table;
            ClearBankDetailsControls();
            btnAdd.Enabled = true;
            btnModify.Enabled = false;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            cvCustomerMaster.ErrorMessage = "Due to Data Problem, Unable to Modify the Bank Details";
            cvCustomerMaster.IsValid = false;

        }
    }

    protected void ClearBankDetailsControls()
    {
        try
        {
            ddlAccountType.SelectedIndex = 0;
            txtAccountNumber.Text = "";
            txtBankName.Text = "";
            txtBankBranch.Text = "";
            txtMICRCode.Text = "";
            txtBankAddress.Text = "";
            /*Modified By prabhu on 16-Nov-2011 for CR (Default Account in Bank Details)*/
            chkDefaultAccount.Checked = false;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    protected void btnBnkClear_Click(object sender, EventArgs e)
    {
        try
        {

            ddlAccountType.SelectedIndex = 0;
            txtAccountNumber.Text = "";
            txtBankName.Text = "";
            txtBankBranch.Text = "";
            txtMICRCode.Text = "";
            txtBankAddress.Text = "";
            hdnBankId.Value = "";
            btnAdd.Enabled = true;
            btnModify.Enabled = false;
            /*Modified By prabhu on 16-Nov-2011 for CR (Default Account in Bank Details)*/
            chkDefaultAccount.Checked = false;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            cvCustomerMaster.ErrorMessage = "Unable to Clear the Data in Bank Details";
            cvCustomerMaster.IsValid = false;
        }
    }

    protected void grvBankDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (strMode != "Q")
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    /* For Deleting purpose, Restrcit to add attribute to the Cell Remove linkbutton*/
                    for (int intCellIndex = 0; intCellIndex < e.Row.Cells.Count - 2; intCellIndex++)
                    {
                        e.Row.Cells[intCellIndex].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink
                        (this.grvBankDetails, "Select$" + e.Row.RowIndex);
                    }
                    e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
                    e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";

                }
            }
            if (strMode == "Q" && e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox lblBankAddress = e.Row.FindControl("lblBankAddress") as TextBox;
                lblBankAddress.Enabled = true;
                lblBankAddress.ReadOnly = true;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Load the Bank Details");
        }
    }

    protected void grvBankDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            DataTable dtDelete = (DataTable)ViewState["DetailsTable"];
            dtDelete.Rows.RemoveAt(e.RowIndex);
            grvBankDetails.DataSource = dtDelete;
            grvBankDetails.DataBind();
            FunPriHideBankColumns();
            ViewState["DetailsTable"] = dtDelete;
            ClearBankDetailsControls();
            if (grvBankDetails.Rows.Count == 0)
            {

                btnAdd.Enabled = true;
                btnModify.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            cvCustomerMaster.ErrorMessage = "Due to Data Problem, Unable to Remove Bank Details";
            cvCustomerMaster.IsValid = false;
        }
    }

    protected void grvBankDetails_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            Label lblBankMappingId = grvBankDetails.SelectedRow.FindControl("lblBankMappingId") as Label;
            Label lblMasterId = grvBankDetails.SelectedRow.FindControl("lblMasterId") as Label;
            Label lblAccountType = grvBankDetails.SelectedRow.FindControl("lblAccountType") as Label;
            Label lblAccountTypeId = grvBankDetails.SelectedRow.FindControl("lblAccountTypeId") as Label;
            TextBox lblBankAddress = grvBankDetails.SelectedRow.FindControl("lblBankAddress") as TextBox;
            /*Modified By prabhu on 16-Nov-2011 for CR (Default Account in Bank Details)*/
            CheckBox chkGridDefaultAccount = grvBankDetails.SelectedRow.FindControl("chkDefaultAccount") as CheckBox;

            ddlAccountType.SelectedValue = lblAccountTypeId.Text;
            txtAccountNumber.Text = Convert.ToString(grvBankDetails.SelectedRow.Cells[5].Text);
            txtMICRCode.Text = Convert.ToString(grvBankDetails.SelectedRow.Cells[6].Text);
            txtBankName.Text = Convert.ToString(grvBankDetails.SelectedRow.Cells[7].Text);
            txtBankBranch.Text = Convert.ToString(grvBankDetails.SelectedRow.Cells[8].Text);
            txtBankAddress.Text = lblBankAddress.Text;
            hdnBankId.Value = lblBankMappingId.Text;
            /*Modified By prabhu on 16-Nov-2011 for CR (Default Account in Bank Details)*/
            chkDefaultAccount.Checked = chkGridDefaultAccount.Checked;
            btnAdd.Enabled = false;
            btnModify.Enabled = true;

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            cvCustomerMaster.ErrorMessage = "Due to Data Problem,Unable to view the Bank Details";
            cvCustomerMaster.IsValid = false;
        }
    }

    #endregion


    protected void asyncFileUpload_UploadedComplete(object sender, AjaxControlToolkit.AsyncFileUploadEventArgs e)
    {

    }

    private void FunPriCopyCommuncationAddress()
    {
        //<<Performance>>
        //if (txtPerCity.Items.Contains(new ListItem((txtComCity.SelectedItem.Text), (txtComCity.SelectedItem.Text))))// && txtComCity.SelectedItem.Text.Trim() != "")
        //{
        //    txtPerCity.Items.Add(new ListItem((txtComCity.SelectedItem.Text), (txtComCity.SelectedItem.Text)));
        //}
        if (txtPerState.Items.Contains(new ListItem((txtComState.SelectedItem.Text), (txtComState.SelectedItem.Text))))// && txtComState.SelectedItem.Text.Trim() != "")
        {
            txtPerState.Items.Add(new ListItem((txtComState.SelectedItem.Text), (txtComState.SelectedItem.Text)));
        }
        if (txtPerCountry.Items.Contains(new ListItem((txtComCountry.SelectedItem.Text), (txtComCountry.SelectedItem.Text))))// && txtComCountry.SelectedItem.Text.Trim() != "")
        {
            txtPerCountry.Items.Add(new ListItem((txtComCountry.SelectedItem.Text), (txtComCountry.SelectedItem.Text)));
        }

        txtPerAddress1.Text = txtComAddress1.Text;
        txtPerAddress2.Text = txtCOmAddress2.Text;
        //<<Performance>>
        txtPerCity.SelectedText = txtComCity.SelectedText;
        //if (txtComCity.SelectedItem.Value == "0")
        //{
        //    txtPerCity.SelectedIndex = 0;
        //}
        //else
        //{
        //    txtPerCity.SelectedItem.Text = txtComCity.SelectedItem.Text;
        //}
        if (txtComState.SelectedItem.Value == "0")
        {
            txtPerState.SelectedIndex = 0;
        }
        else
        {
            txtPerState.SelectedItem.Text = txtComState.SelectedItem.Text;
        }
        if (txtComCountry.SelectedItem.Value == "0")
        {
            txtPerCountry.SelectedIndex = 0;
        }
        else
        {
            txtPerCountry.SelectedItem.Text = txtComCountry.SelectedItem.Text;
        }
        txtPerTelephone.Text = txtComTelephone.Text;
        txtPerPincode.Text = txtComPincode.Text;
        txtPerMobile.Text = txtComMobile.Text;
        txtPerEmail.Text = txtComEmail.Text;
        txtPerWebSite.Text = txtComWebsite.Text;
    }
    protected void btnCopyAddress_Click(object sender, EventArgs e)
    {
        FunPriCopyCommuncationAddress();
    }
    protected void gvCredit_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (gvCredit.DataSource != null && ((DataTable)gvCredit.DataSource).Rows.Count > 0)
        {
            if (e.Row.Cells[2] != null && e.Row.Cells[2].Text != string.Empty)
            {
                e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Right;
            }
        }

    }
}
