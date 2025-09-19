#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Orgination 
/// Screen Name			: Asset Details
/// Reason              : Asset Details For Application Processing
/// Created By  		: Thangam M
/// Created Date        : 29-Oct-2010
/// Modified By  		: Prabhu K
/// Modified on         : 30-Nov-2011
/// Reason              : For OL Line of Business, Some fields are only required. Capital & Non-Capital calculated automatically.
/// <Program Summary>
#endregion

#region Namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using S3GBusEntity;
using System.Globalization;
using System.Web.Security;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Configuration;
#endregion

public partial class Origination_S3G_OrgApplicationAssetDetails : ApplyThemeForProject
{
    #region Initialization

    /// <summary>
    /// To Initialize Objects and Variables
    /// </summary>
    /// 
    int intCompanyId = 0;
    int intUserId = 0;
    int intBranchId = 2;
    int intSerialNo = 0;
    int intErrorCode = 0;
    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    UserInfo ObjUserInfo;
    S3GSession objS3GSession;
    //User Authorization
    string strMode = string.Empty;
    string strDateFormat = "";
    bool bCreate = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bMakerChecker = false;
    bool bClearList = false;
    string[] ErrorList = new string[3];
    public static Origination_S3G_OrgApplicationAssetDetails obj_Page;
    string strNewWin = " window.showModalDialog('../Origination/S3GOrgApplicationAssetDetails.aspx?qsMaster=Yes&qsRowID=";
    string NewWinAttributes = "', 'Asset Details', 'location:no;toolbar:no;menubar:no;dialogwidth:700px;dialogHeight:400px;');";
    //Code end
    #endregion

    #region Page Load

    protected void Page_Load(object sender, EventArgs e)
    {
        ObjUserInfo = new UserInfo();
        intCompanyId = ObjUserInfo.ProCompanyIdRW;
        intUserId = ObjUserInfo.ProUserIdRW;
        objS3GSession = new S3GSession();
        obj_Page = this;
        //User Authorization
        bCreate = ObjUserInfo.ProCreateRW;
        bModify = ObjUserInfo.ProModifyRW;
        bQuery = ObjUserInfo.ProViewRW;
        strDateFormat = objS3GSession.ProDateFormatRW;
        bClearList = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ClearListValues"));
        //Code end


        if (Request.QueryString["qsMode"] != null)
        {
            //btnOK.Enabled = false;
            btnOK.Enabled_False();
            //ddlPayTo.SelectedValue=""
        }

        if (Request.QueryString["qsRowID"] != null)                               //When click Add for First Row//
        {
            intSerialNo = Convert.ToInt32(Request.QueryString["qsRowID"]);
        }
        else                                                                      //When click Add for Further Rows//  
        {
            DataTable dtAssetDetails = (DataTable)Session["PricingAssetDetails"];
            if (dtAssetDetails != null && dtAssetDetails.Rows.Count > 0)
            {
                txtSlNo.Text = Convert.ToString(dtAssetDetails.Rows.Count + 1);
            }
        }



        //txtRequiredFromDate_CalendarExtender.Format = strDateFormat;
        txtTotalAssetValue.Attributes.Add("readonly", "readonly");
        txtMarginAmountAsset.Attributes.Add("readonly", "readonly");
        //txtUnitValue.Attributes.Add("readonly", "readonly");
        txtMarginPercentage.Attributes.Add("readonly", "readonly");
        //txtRequiredFromDate.Attributes.Add("readonly", "readonly");        
        txtUnitCount.SetDecimalPrefixSuffix(3, 0, true, "Unit Count");
        txtUnitValue.SetDecimalPrefixSuffix(objS3GSession.ProGpsPrefixRW, objS3GSession.ProGpsSuffixRW, true, false, "Unit Value");
        txtMarginPercentage.SetDecimalPrefixSuffix(3, 2, false, "Margin %");
        txtMarginAmountAsset.SetDecimalPrefixSuffix(10, 2, false, "Margin Amount");
        txtFinanceAmountAsset.SetDecimalPrefixSuffix(objS3GSession.ProGpsPrefixRW, objS3GSession.ProGpsSuffixRW, true, "Finance Amount");

        txtDealerCommissionBasisRate.SetDecimalPrefixSuffix(objS3GSession.ProGpsPrefixRW, objS3GSession.ProGpsSuffixRW, true, "Dealer Comission Basis Rate");
        txtDealerCommissionAmount.SetDecimalPrefixSuffix(objS3GSession.ProGpsPrefixRW, objS3GSession.ProGpsSuffixRW, true, "Dealer Comission Amount");
        //txtCapitalPortion.SetDecimalPrefixSuffix(objS3GSession.ProGpsPrefixRW, objS3GSession.ProGpsSuffixRW, true, "Capital Portion");
        //txtNonCapitalPortion.SetDecimalPrefixSuffix(objS3GSession.ProGpsPrefixRW, objS3GSession.ProGpsSuffixRW, false, "Non Capital Portion");
        txtMargintoDealer.SetDecimalPrefixSuffix(10, 4, false, false, "Down Payment to Dealer");
        txtMargintoMFC.SetDecimalPrefixSuffix(10, 4, false, false, "Down Payment to MFC");
        txtTradeIn.SetDecimalPrefixSuffix(10, 4, false, false, "Trade In");

        txtDiscountAmount.SetDecimalPrefixSuffix(10, 2, false, "Discount Amount");
        HdnGPSDecimal.Value = objS3GSession.ProGpsSuffixRW.ToString();

        calimgtxtRegistrationExpiryDate.Format = strDateFormat;
        caltxtDateofRegistration.Format = strDateFormat;
        txtDateofRegistration.Attributes.Add("onblur", "fnDoDate(this,'" + txtDateofRegistration.ClientID + "','" + strDateFormat + "',true,  false);");
        txtRegistrationExpiryDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtRegistrationExpiryDate.ClientID + "','" + strDateFormat + "',false,  false);");

        if (Request.QueryString["qsMaster"].ToString() == "Yes")
        {
            //if (rdnlAssetType.SelectedIndex == 1)
            //{

            //    lblFinanceAmountAsset0.CssClass = "styleDisplayLabel";
            //}
            //else
            //{
            //    lblFinanceAmountAsset0.CssClass = "styleDisplayFieldLabel";
            //}
            rfvCustomerName.Enabled = rfvPayTo.Enabled = false;

            lblPayTo.CssClass = lblCustomerName.CssClass = "styleDisplayLabel";
        }
        else
        {
            rfvCustomerName.Enabled = rfvPayTo.Enabled = true;

            lblFinanceAmountAsset0.CssClass = lblPayTo.CssClass = lblCustomerName.CssClass = "styleReqFieldLabel";
        }
        if (!IsPostBack)
        {
            TextBox txtddlAssetCodeList = ((TextBox)ddlAssetCodeList.FindControl("txtItemName"));
            txtddlAssetCodeList.Attributes.Add("onchange", "fnTrashSuggest('" + ddlAssetCodeList.ClientID + "')");

            TextBox txtddlEntityNameList = ((TextBox)ddlEntityNameList.FindControl("txtItemName"));
            txtddlEntityNameList.Attributes.Add("onchange", "fnTrashSuggest('" + ddlEntityNameList.ClientID + "')");

            if (System.Web.HttpContext.Current.Session["Is_Dealer_Commission_Applicable"] != null)
            {
                if (System.Web.HttpContext.Current.Session["Is_Dealer_Commission_Applicable"].ToString() == "1")
                {
                    txtDealerCommissionBasisRate.Enabled = false;
                    txtDealerCommissionAmount.Enabled = false;

                }
                else
                {
                    txtDealerCommissionBasisRate.Enabled = false;
                    txtDealerCommissionAmount.Enabled = false;
                }
            }
            funPriLoadAssetType();
            funPriLoadManuFactYear();

            FunProLoadCombos();
            FunToggleLableVisble(false);
            intSerialNo = Convert.ToInt32(Request.QueryString["qsRowID"]);
            if (Request.QueryString["qsMaster"].ToString() == "Yes")
            {
                FunToggleLeaseAssetCode(true);
                txtUnitCount.Enabled = false;

            }
            else
            {
                FunToggleLeaseAssetCode(false);

            }
            if (string.IsNullOrEmpty(txtSlNo.Text))
            {
                DataTable dtAssetDetails = (DataTable)Session["PricingAssetDetails"];
                if (dtAssetDetails != null && dtAssetDetails.Rows.Count > 0)
                {
                    FunPriLoadAssetDetails(dtAssetDetails);
                    if (ddlAssetType.SelectedValue == "1")
                    {

                        txtChasisNumber.Enabled = false;
                        txtEngineNo.Enabled = false;
                    }
                    else
                    {

                        txtChasisNumber.Enabled = true;
                        txtEngineNo.Enabled = true;
                    }
                }
                else
                {
                    txtSlNo.Text = "1";
                }
            }
            //funPriSetUnitTestValue();
        }
        /* these code executes for .Net 3.0 and above only.*/
        Response.Expires = 0;
        Response.Cache.SetNoStore();
        Response.AppendHeader("Pragma", "no-cache");

    }
    //private void funPriGetDealerCommsissionDetails()
    //{
    //    try
    //    {
    //        DataTable dtDealerCommission;
    //        if (HttpContext.Current.Session["CONSTITUTION_ID"] == null)
    //        {
    //            Utility.FunShowAlertMsg(this,"Constitution not Available");
    //            return;
    //        }
    //        if (HttpContext.Current.Session["CONTRACT_TYPE"] == null)
    //        {
    //            Utility.FunShowAlertMsg(this, "Contract Type not Selected in Application");
    //            return;
    //        }
    //        if (HttpContext.Current.Session["TSCSMP"] == null)
    //        {
    //            Utility.FunShowAlertMsg(this, "TCSMP not Available");
    //            return;
    //        }

    //        if (ddlEntityNameList.SelectedValue == "0")
    //        {
    //            Utility.FunShowAlertMsg(this, "Select the Entity");
    //            return;
    //        }
    //        if (txtFinanceAmountAsset.Text == string.Empty)
    //        {
    //            Utility.FunShowAlertMsg(this, "Enter the Finance Amount");
    //            return;
    //        }


    //        Dictionary<string, string> strProParm = new Dictionary<string, string>();
    //        strProParm.Add("@OPTION", "8");
    //        strProParm.Add("@COMPANYID", intCompanyId.ToString());
    //        strProParm.Add("@USERID", intUserId.ToString());
    //        strProParm.Add("@PROGRAMID", "38");
    //        strProParm.Add("@PAGE_MODE", "C");
    //        strProParm.Add("@Deal_Comm_Entity_id", ddlEntityNameList.SelectedValue);
    //        strProParm.Add("@CONSTITUTION_ID",  HttpContext.Current.Session["CONSTITUTION_ID"].ToString());
    //        strProParm.Add("@CONTRACT_TYPE", HttpContext.Current.Session["CONTRACT_TYPE"].ToString());
    //        strProParm.Add("@TCSMP_TYPE", HttpContext.Current.Session["TSCSMP"].ToString());
    //        strProParm.Add("@FinanaceAmount", txtFinanceAmountAsset.Text);
    //        dtDealerCommission = Utility.GetDefaultData("S3G_OR_LOAD_LOV_APP", strProParm);
    //        if (dtDealerCommission.Rows.Count > 0)
    //        {
    //            txtDealerCommissionBasisRate.Text = dtDealerCommission.Rows[0]["RATE"].ToString();
    //            txtDealerCommissionAmount.Text = dtDealerCommission.Rows[0]["COMMISSION"].ToString();
    //            hdnDealerCommissionId.Value = dtDealerCommission.Rows[0]["DEALER_COMM_ID"].ToString();
    //        }
    //        else
    //        {
    //            Utility.FunShowAlertMsg(this,"Dealer Comission Paramaters not Matching or Commission Details not defined in Dealer Comission");
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
    //    }
    //}

    private void funPriLoadAssetType()
    {
        try
        {
            DataTable dtAssetType;
            Dictionary<string, string> strProParm = new Dictionary<string, string>();
            strProParm.Add("@OPTION", "9");//Asset Type
            strProParm.Add("@COMPANYID", intCompanyId.ToString());
            strProParm.Add("@USERID", intUserId.ToString());
            strProParm.Add("@PROGRAMID", "38");
            strProParm.Add("@PAGE_MODE", "C");
            dtAssetType = Utility.GetDefaultData("S3G_OR_LOAD_LOV_APP", strProParm);
            ddlAssetType.FillDataTable(dtAssetType, "VALUE", "NAME");
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    private void funPriLoadManuFactYear()
    {
        try
        {
            DataSet dSYearofManufact;
            Dictionary<string, string> strProParm = new Dictionary<string, string>();
            strProParm.Add("@OPTION", "19");//Asset Type
            strProParm.Add("@COMPANYID", intCompanyId.ToString());
            strProParm.Add("@USERID", intUserId.ToString());
            strProParm.Add("@PROGRAMID", "38");
            strProParm.Add("@PAGE_MODE", "C");
            dSYearofManufact = Utility.GetDataset("S3G_OR_LOAD_LOV_APP", strProParm);
            ddlYearofManufacturer.FillDataTable(dSYearofManufact.Tables[0], "VALUE", "NAME");
            ViewState["Manu_Fact_Year"] = dSYearofManufact.Tables[1].Rows[0]["MAN_YR"].ToString();

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    protected new void Page_PreInit(object sender, EventArgs e)
    {
        if (Request.QueryString["qsMaster"] != null)
        {
            UserInfo ObjUserInfo = new UserInfo();
            this.Page.Theme = ObjUserInfo.ProUserThemeRW;
        }
        else
        {
            UserInfo ObjUserInfo = new UserInfo();
            this.Page.Theme = ObjUserInfo.ProUserThemeRW;
        }
        this.Page.MasterPageFile = "~/Common/MasterPage.master";
    }

    #endregion

    #region Page Events

    protected void rdnlAssetType_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtBookDepreciationPerc.Text = "";
        //txtBlockDepreciationPerc.Text = "";
        //txtCapitalPortion.Text = "";
        //txtNonCapitalPortion.Text = "";
        txtUnitValue.Text = "";
        txtFinanceAmountAsset.Text = "";
        txtMarginAmountAsset.Text = "";
        txtMarginPercentage.Text = "";
        txtTotalAssetValue.Text = "";
        //txtRequiredFromDate.Text = "";
        //if (rdnlAssetType.SelectedValue == "1")
        //{
        //    FunProLoadAssetValue("OLD");
        //    FunToggleLableVisble(true);
        //    ddlLeaseAssetNo.SelectedIndex = 0;
        //    //lblRequiredFromDate.Enabled = true;
        //    //lblRequiredFromDate.CssClass = "styleReqFieldLabel";
        //    //rfvRequiredFromDate.Enabled = true;
        //    //txtRequiredFromDate_CalendarExtender.Enabled = true;
        //}
        //else
        //{
        //    FunProLoadAssetValue("NEW");
        //    FunToggleLableVisble(false);
        //    ddlAssetCodeList.SelectedIndex = 0;
        //    //lblRequiredFromDate.Enabled = false;
        //    //lblRequiredFromDate.CssClass = "styleDisplayLabel";
        //    //rfvRequiredFromDate.Enabled = false;
        //    //txtRequiredFromDate_CalendarExtender.Enabled = false;
        //}
    }

    protected void ddlPayTo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlPayTo.SelectedItem.ToString().ToLower() == "entity")
        {
            FunToggleEntityControls(true);
        }
        else if (ddlPayTo.SelectedItem.ToString().ToLower() == "customer")
        {
            FunToggleEntityControls(false);
        }
        else
        {
            txtCustomerName.Text = "";

        }
        ddlPayTo.Focus();
    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        try
        {

            if (Session["AssetCustomer"] == null)
            {

                if (Session["AssetCustomerValiMessege"] != null)
                {
                    Utility.FunShowAlertMsg(this, Session["AssetCustomerValiMessege"].ToString());
                    return;
                }
                else
                {
                    Utility.FunShowAlertMsg(this, Session["AssetCustomerValiMessege"].ToString());
                    return;
                }
            }

            string[] str = Session["AssetCustomer"].ToString().Split(';');

            if (str.Length != 2)
            {
                if (Session["AssetCustomer"] != null)
                {
                    if (Session["AssetCustomerValiMessege"] != null)
                    {
                        Utility.FunShowAlertMsg(this, Session["AssetCustomerValiMessege"].ToString());
                        return;
                    }
                    else
                    {
                        Utility.FunShowAlertMsg(this, Session["AssetCustomerValiMessege"].ToString());
                        return;
                    }
                }

            }

            if (str[0].ToString() == string.Empty)
            {
                Utility.FunShowAlertMsg(this, "Customer not Created");
                return;
            }
            if (ddlAssetCodeList.SelectedText == string.Empty)
            {
                Utility.FunShowAlertMsg(this, "Select the Entity");
                return;
            }

            if (Session["PricingAssetDetails"] != null)
            {
                DataTable dtAsset = (DataTable)Session["PricingAssetDetails"];

                if (dtAsset.Rows.Count > 0)
                {
                    if (intSerialNo == 0)
                    {

                        if (Convert.ToDecimal(dtAsset.Rows.Count) + Convert.ToDecimal(txtUnitCount.Text) > 500)
                        {
                            Utility.FunShowAlertMsg(this, "Asset Line Item Count should not exceed the 500");
                            return;
                        }
                    }

                }
            }

            if (txtRegistrationNo1.Text != string.Empty)
            {
                if (txtRegNo2.Text == string.Empty)
                {
                    Utility.FunShowAlertMsg(this, "Enter the Registration Number2");
                    return;
                }

                if (txtDateofRegistration.Text == string.Empty)
                {
                    Utility.FunShowAlertMsg(this, "Enter the Date of Registration");
                    return;
                }

                if (txtRegistrationExpiryDate.Text == string.Empty)
                {
                    Utility.FunShowAlertMsg(this, "Enter the Registration Expiry Date");
                    return;
                }

                if (txtRegisteredOwner.Text == string.Empty)
                {
                    Utility.FunShowAlertMsg(this, "Enter the Registered Owner");
                    return;
                }
            }

            if (txtRegistrationExpiryDate.Text != string.Empty && txtDateofRegistration.Text != string.Empty)
            {
                if (Utility.StringToDate(txtRegistrationExpiryDate.Text) <= Utility.StringToDate(txtDateofRegistration.Text))
                {
                    Utility.FunShowAlertMsg(this, "Registration Expiry Date should be greater than the Date of Registration");
                    txtRegistrationExpiryDate.Text = string.Empty;
                    return;
                }
            }

            if (ddlAssetCodeList.SelectedText.Split('-').Length == 1)
            {
                Utility.FunShowAlertMsg(this, "Invalid Asset");
                ddlAssetCodeList.Clear();
                return;
            }




            if (Convert.ToDouble(txtFinanceAmountAsset.Text) > Convert.ToDouble(txtTotalAssetValue.Text))
            {
                cvApplicationAsset.ErrorMessage = @"Correct the following validation(s): </br></br>" + "     Finance Amount should be less than or equal to Total Asset Value";
                cvApplicationAsset.IsValid = false;
                return;
            }
            if (Request.QueryString["qsMaster"].ToString() == "No")
            {
                if (txtMarginAmountAsset.Text != "")
                {
                    decimal douFinanceMarginAmount = Convert.ToDecimal(txtFinanceAmountAsset.Text) + Convert.ToDecimal(txtMarginAmountAsset.Text);
                    if (douFinanceMarginAmount > Convert.ToDecimal(txtTotalAssetValue.Text))
                    {
                        //cvApplicationAsset.ErrorMessage = @"Correct the following validation(s): </br></br>" + "    The sum of Finance Amount and Margin Amount should be less than or equal to Total Asset Value";
                        //cvApplicationAsset.IsValid = false;
                        //return;

                        Utility.FunShowAlertMsg(this, "The sum of Finance Amount and Margin Amount should be less than or equal to Total Asset Value");
                        return;
                    }
                }
            }
            //if (Convert.ToDouble(txtCapitalPortion.Text) > Convert.ToDouble(txtFinanceAmountAsset.Text))
            //{
            //    cvApplicationAsset.ErrorMessage = @"Correct the following validation(s): </br></br>" + "     Capital Amount should be less than or equal to Finance Amount";
            //    cvApplicationAsset.IsValid = false;
            //    return;
            //}

            //if (Convert.ToDouble(txtNonCapitalPortion.Text) > Convert.ToDouble(txtFinanceAmountAsset.Text))
            //{
            //    cvApplicationAsset.Text = @"Correct the following validation(s): </br></br>" + "        Non Capital Amount should be less than or equal to Finance Amount";
            //    cvApplicationAsset.IsValid = false;
            //    return;
            //}
            //if (Convert.ToDouble(txtNonCapitalPortion.Text) + Convert.ToDouble(txtCapitalPortion.Text) != Convert.ToDouble(txtFinanceAmountAsset.Text))
            //{
            //    cvApplicationAsset.Text = @"Correct the following validation(s): </br></br>" + "        The sum of Capital and Non Capital Amounts should be equal to Finance Amount.";
            //    cvApplicationAsset.IsValid = false;
            //    return;
            //}
            //if (ddlLeaseAssetNo.Visible)
            //{
            //    if (Request.QueryString["FromPricing"] == null)
            //    {
            //        DataRow[] DrRate = (((DataTable)ViewState["RateDt2"]).Select("LEASE_ASSET_NO = '" + ddlLeaseAssetNo.SelectedValue + "'"));
            //        if (DrRate.Count() > 0)
            //        {
            //            if (Convert.ToString(DrRate[0]["AVAILABLE_DATE"]) != string.Empty)
            //            {
            //                //condition =< modified as >=for bug fixing - kuppu
            //                //if (Utility.StringToDate(txtRequiredFromDate.Text) >= Utility.StringToDate(Convert.ToString(DrRate[0]["AVAILABLE_DATE"])))
            //                //{
            //                //    cvApplicationAsset.Text = @"Correct the following validation(s): </br><br/>" + "     The Asset is not available on the selected date";
            //                //    cvApplicationAsset.IsValid = false;
            //                //    return;
            //                //}
            //            }
            //        }
            //    }
            //}
            DataTable dtAssetDetails = new DataTable();
            if (Session["PricingAssetDetails"] == null)
            {
                dtAssetDetails.Columns.Add("LeaseType");
                dtAssetDetails.Columns.Add("Required_FromDate");
                dtAssetDetails.Columns.Add("SlNo");
                dtAssetDetails.Columns.Add("Asset_ID");
                dtAssetDetails.Columns.Add("Asset_Code");
                dtAssetDetails.Columns.Add("Unit_Value");
                dtAssetDetails.Columns.Add("Unit_Value_1");
                dtAssetDetails.Columns.Add("Noof_Units");
                dtAssetDetails.Columns.Add("Margin_Percentage");
                dtAssetDetails.Columns.Add("TotalAssetValue");
                dtAssetDetails.Columns.Add("TotalAssetValue_1");
                dtAssetDetails.Columns.Add("Book_depreciation_Percentage");
                dtAssetDetails.Columns.Add("Margin_Amount");
                dtAssetDetails.Columns.Add("Margin_Amount_1");
                dtAssetDetails.Columns.Add("Block_depreciation_Percentage");
                dtAssetDetails.Columns.Add("Finance_Amount", typeof(decimal));
                dtAssetDetails.Columns.Add("NonCapital_Portion");
                dtAssetDetails.Columns.Add("Capital_Portion");
                dtAssetDetails.Columns.Add("Payment_Percentage");
                dtAssetDetails.Columns.Add("Pay_To_ID");
                dtAssetDetails.Columns.Add("Entity_ID");
                dtAssetDetails.Columns.Add("Entity_Code");
                dtAssetDetails.Columns.Add("Proforma_Id");
                dtAssetDetails.Columns.Add("Finance_Amount_1", typeof(decimal));

                dtAssetDetails.Columns["Finance_Amount"].DataType = typeof(decimal);
                dtAssetDetails.Columns["Margin_Amount"].DataType = typeof(decimal);
                dtAssetDetails.Columns["Finance_Amount_1"].DataType = typeof(decimal);
                dtAssetDetails.Columns["Margin_Amount_1"].DataType = typeof(decimal);
                dtAssetDetails.Columns["TotalAssetValue"].DataType = typeof(decimal);
                dtAssetDetails.Columns["Unit_Value_1"].DataType = typeof(decimal);
                dtAssetDetails.Columns.Add("Lease_Asset_No");
                dtAssetDetails.Columns.Add("ManuFactoring_Year", typeof(int));
                //new column added 
                dtAssetDetails.Columns.Add("Discount_Absorbed");
                dtAssetDetails.Columns.Add("Discount_Amount");
                dtAssetDetails.Columns.Add("Security_Type");
                dtAssetDetails.Columns.Add("Security_Type_Identifier");
                dtAssetDetails.Columns.Add("Security_Type_Identifier_Value");
                dtAssetDetails.Columns.Add("Margin_Dealer", typeof(decimal));
                dtAssetDetails.Columns.Add("Margin_MFC", typeof(decimal));
                dtAssetDetails.Columns.Add("Trade_In", typeof(decimal));
                dtAssetDetails.Columns.Add("Margin_Dealer_1", typeof(decimal));
                dtAssetDetails.Columns.Add("Margin_MFC_1", typeof(decimal));
                dtAssetDetails.Columns.Add("Trade_In_1", typeof(decimal));


                //RC
                dtAssetDetails.Columns.Add("Engine_No");
                dtAssetDetails.Columns.Add("Chasis_No");
                dtAssetDetails.Columns.Add("Date_of_Reg");
                dtAssetDetails.Columns.Add("Reg_No");
                dtAssetDetails.Columns.Add("Reg_No2");
                dtAssetDetails.Columns.Add("Reg_Expiry_Date");
                dtAssetDetails.Columns.Add("Model_Year");

                dtAssetDetails.Columns.Add("Registered_Owner");
                dtAssetDetails.Columns.Add("Dealer_Commission_Rate", typeof(decimal));
                dtAssetDetails.Columns.Add("Dealer_Commission_Amount", typeof(decimal));
                dtAssetDetails.Columns.Add("Dealer_Commission_Amount_1", typeof(decimal));
                dtAssetDetails.Columns.Add("DEALER_COMM_ID", typeof(int));
                dtAssetDetails.Columns.Add("DEALER_COMM_Rate_ID", typeof(int));
                dtAssetDetails.Columns.Add("Asset_Type", typeof(int));

                dtAssetDetails.Columns.Add("CLASS", typeof(string));
                dtAssetDetails.Columns.Add("MAKE", typeof(string));
                dtAssetDetails.Columns.Add("MODEL", typeof(string));
                dtAssetDetails.Columns.Add("TYPE", typeof(string));
                dtAssetDetails.Columns.Add("Purpose", typeof(string));



            }
            else
            {
                dtAssetDetails = (DataTable)Session["PricingAssetDetails"];
            }

            if (intSerialNo == 0)
            {
                int Iunicount = Convert.ToInt32(txtUnitCount.Text);
                DataRow Dr;
                for (int j = 1; j <= Iunicount; j++)
                {

                    Dr = dtAssetDetails.NewRow();





                    foreach (DataRow dr in dtAssetDetails.Rows)
                    {
                        if (txtEngineNo.Text != string.Empty)
                        {

                            if (Convert.ToInt32(txtUnitCount.Text) > 1)
                            {
                                Utility.FunShowAlertMsg(this, "Engine no not allowed for more than one unit count");
                                return;
                            }

                            if (dr["Engine_No"].ToString() == txtEngineNo.Text)
                            {
                                Utility.FunShowAlertMsg(this, "Engine no Exists");
                                return;
                            }
                        }
                        if (txtChasisNumber.Text != string.Empty)
                        {
                            if (Convert.ToInt32(txtUnitCount.Text) > 1)
                            {
                                Utility.FunShowAlertMsg(this, "Chassis no not allowed for more than one unit count");
                                return;
                            }

                            if (dr["Chasis_No"].ToString() == txtChasisNumber.Text)
                            {
                                Utility.FunShowAlertMsg(this, "Chassis no Exists");
                                return;
                            }
                        }

                        if (txtRegistrationNo1.Text != string.Empty)
                        {
                            if (Convert.ToInt32(txtUnitCount.Text) > 1)
                            {
                                Utility.FunShowAlertMsg(this, "Registration Number1 not allowed for more than one unit count");
                                return;
                            }

                            if (dr["Reg_No"].ToString() == txtRegistrationNo1.Text)
                            {
                                Utility.FunShowAlertMsg(this, "Registration Number1 Exists");
                                return;
                            }
                        }
                        if (txtRegNo2.Text != string.Empty)
                        {
                            if (Convert.ToInt32(txtUnitCount.Text) > 1)
                            {
                                Utility.FunShowAlertMsg(this, "Registration Number2 not allowed for more than one unit count");
                                return;
                            }

                            if (dr["Reg_No2"].ToString() == txtRegNo2.Text)
                            {
                                Utility.FunShowAlertMsg(this, "Registration Number2 Exists");
                                return;
                            }
                        }

                    }












                    //if (dtAssetDetails.Rows.Count > 0)
                    //{

                    //}


                    //Dr["LeaseType"] = rdnlAssetType.SelectedValue.ToString();
                    Dr["SlNo"] = dtAssetDetails.Rows.Count + 1;
                    Dr["Asset_Code"] = ddlAssetCodeList.SelectedText;
                    Dr["Asset_ID"] = ddlAssetCodeList.SelectedValue;
                    //if (rdnlAssetType.SelectedValue == "0")
                    //{
                    //    Dr["Lease_Asset_No"] = "";
                    //}
                    //else
                    //{
                    //    Dr["Lease_Asset_No"] = ddlLeaseAssetNo.SelectedValue;
                    //}
                    //if (ddlAssetCodeList.Visible)
                    //{
                    //    Dr["Asset_Code"] = ddlAssetCodeList.SelectedItem.Text;
                    //    Dr["Asset_ID"] = ddlAssetCodeList.SelectedValue;
                    //}
                    //else
                    //{
                    //    DataRow[] drLAN = ((DataTable)ViewState["RateDt2"]).Select("Asset_Code = '" + ddlLeaseAssetNo.SelectedItem.Text + "'");
                    //    if (drLAN.Length > 0)
                    //    {
                    //        Dr["Asset_Code"] = ddlLeaseAssetNo.SelectedItem.Text;
                    //        Dr["Asset_ID"] = drLAN[0]["LEASE_ASSET_ID"];
                    //    }
                    //}
                    Dr["Noof_Units"] = 1;
                    Dr["Finance_Amount"] = Convert.ToDecimal(txtUnitValue.Text) - (Convert.ToDecimal(txtMargintoDealer.Text) + Convert.ToDecimal(txtMargintoMFC.Text) + Convert.ToDecimal(txtTradeIn.Text));
                    Dr["Finance_Amount_1"] = Convert.ToDecimal(txtUnitValue.Text) - (Convert.ToDecimal(txtMargintoDealer.Text) + Convert.ToDecimal(txtMargintoMFC.Text) + Convert.ToDecimal(txtTradeIn.Text));
                    //if (!string.IsNullOrEmpty(txtRequiredFromDate.Text))
                    //{
                    //    Dr["Required_FromDate"] = Utility.StringToDate(txtRequiredFromDate.Text);
                    //}
                    //else
                    //{
                    //Dr["Required_FromDate"] = DBNull;
                    //}
                    Dr["Unit_Value"] =Convert.ToDecimal( txtUnitValue.Text);
                    Dr["Unit_Value_1"] =Convert.ToDecimal( txtUnitValue.Text);
                    Dr["Margin_Percentage"] = string.IsNullOrEmpty(txtMarginPercentage.Text) ? "0" : txtMarginPercentage.Text;
                    Dr["TotalAssetValue"] = Convert.ToDecimal(txtUnitValue.Text) * Convert.ToDecimal(txtUnitCount.Text);
                    Dr["TotalAssetValue_1"] = Convert.ToDecimal(txtUnitValue.Text) * Convert.ToDecimal(txtUnitCount.Text);


                    Dr["Book_depreciation_Percentage"] = string.IsNullOrEmpty(txtBookDepreciationPerc.Text) ? "0" : txtBookDepreciationPerc.Text;
                    Dr["Margin_Amount"] = string.IsNullOrEmpty(txtMarginAmountAsset.Text) ? 0 : Convert.ToDecimal(txtMarginAmountAsset.Text);
                    Dr["Margin_Amount_1"] = string.IsNullOrEmpty(txtMarginAmountAsset.Text) ? 0 : Convert.ToDecimal(txtMarginAmountAsset.Text);
                    //Dr["Block_depreciation_Percentage"] = string.IsNullOrEmpty(txtBlockDepreciationPerc.Text) ? "0" : txtBlockDepreciationPerc.Text;
                    //Dr["NonCapital_Portion"] = txtNonCapitalPortion.Text;
                    //Dr["Capital_Portion"] = txtCapitalPortion.Text;
                    //Dr["Payment_Percentage"] = txtPaymentPercentage.Text;
                    Dr["Security_Type"] = ddlAssetTypeofSecurity.SelectedValue;
                    Dr["Security_Type_Identifier"] = ddlSecurityIdentifierType.SelectedValue;
                    Dr["Security_Type_Identifier_Value"] = txtSecurityIdentifierValue.Text;
                    /*Changed by Prabhu.K on 30-Nov-2011 - For OL , it is not required*/
                    if (ddlPayTo.SelectedIndex == 0)
                    {
                        Dr["Pay_To_ID"] = Dr["Entity_ID"] = Dr["Entity_Code"] = "";
                    }
                    else
                    {
                        Dr["Pay_To_ID"] = ddlPayTo.SelectedValue;
                        if (ddlPayTo.SelectedItem.ToString().ToUpper() == "CUSTOMER")
                        {
                            Dr["Entity_ID"] = Session["AssetCustomer"].ToString().Substring(0, Session["AssetCustomer"].ToString().Remove(Session["AssetCustomer"].ToString().IndexOf(";")).Length);
                        }
                        else
                        {
                            Dr["Entity_ID"] = ddlEntityNameList.SelectedValue;
                            Dr["Entity_Code"] = ddlEntityNameList.SelectedText;
                        }
                    }
                    if (txtDiscountAmount.Text != "")
                    {
                        Dr["Discount_Amount"] = txtDiscountAmount.Text;
                    }
                    else
                    {
                        Dr["Discount_Amount"] = "0";
                    }
                    //if (chkDiscountAbsorbed.Checked)
                    //    Dr["Discount_Absorbed"] = 1;
                    //else
                    //    Dr["Discount_Absorbed"] = 0;


                    if (!string.IsNullOrEmpty(txtMargintoDealer.Text))
                    {
                        Dr["Margin_Dealer"] =Convert.ToDecimal( txtMargintoDealer.Text);
                        Dr["Margin_Dealer_1"] =Convert.ToDecimal( txtMargintoDealer.Text);
                    }
                    else
                    {
                        Dr["Margin_Dealer"] = Funsetsuffix();
                        Dr["Margin_Dealer_1"] = Funsetsuffix();
                    }
                    if (!string.IsNullOrEmpty(txtMargintoMFC.Text))
                    {
                        Dr["Margin_MFC"] =Convert.ToDecimal( txtMargintoMFC.Text);
                        Dr["Margin_MFC_1"] =Convert.ToDecimal( txtMargintoMFC.Text);
                    }
                    else
                    {
                        Dr["Margin_MFC"] = Funsetsuffix();
                        Dr["Margin_MFC_1"] = Funsetsuffix();
                    }
                    if (!string.IsNullOrEmpty(txtTradeIn.Text))
                    {
                        Dr["Trade_In"] =Convert.ToDecimal( txtTradeIn.Text);
                        Dr["Trade_In_1"] =Convert.ToDecimal( txtTradeIn.Text);
                    }
                    else
                    {
                        Dr["Trade_In"] = Funsetsuffix();
                        Dr["Trade_In_1"] = txtTradeIn.Text;
                    }

                    //RC Deatils
                    if (!string.IsNullOrEmpty(txtEngineNo.Text))
                        Dr["Engine_No"] = txtEngineNo.Text;

                    if (!string.IsNullOrEmpty(txtChasisNumber.Text))
                        Dr["Chasis_No"] = txtChasisNumber.Text;

                    if (!string.IsNullOrEmpty(txtDateofRegistration.Text))
                        Dr["Date_of_Reg"] = txtDateofRegistration.Text;

                    if (!string.IsNullOrEmpty(txtRegistrationNo1.Text))
                        Dr["Reg_No"] = txtRegistrationNo1.Text;
                    if (!string.IsNullOrEmpty(txtRegNo2.Text))
                        Dr["Reg_No2"] = txtRegNo2.Text;

                    if (!string.IsNullOrEmpty(txtRegistrationExpiryDate.Text))
                        Dr["Reg_Expiry_Date"] = txtRegistrationExpiryDate.Text;


                    if (!string.IsNullOrEmpty(txtModelYear.Text))
                        Dr["Model_Year"] = txtModelYear.Text;


                    if (!string.IsNullOrEmpty(ddlYearofManufacturer.SelectedItem.Text))
                        Dr["ManuFactoring_Year"] = ddlYearofManufacturer.SelectedItem.Text;

                    if (!string.IsNullOrEmpty(txtRegisteredOwner.Text))
                        Dr["Registered_Owner"] = txtRegisteredOwner.Text;

                    if (!string.IsNullOrEmpty(txtDealerCommissionBasisRate.Text))
                        Dr["Dealer_Commission_Rate"] = Convert.ToDecimal(txtDealerCommissionBasisRate.Text).ToString(Funsetsuffix());

                    if (!string.IsNullOrEmpty(txtDealerCommissionAmount.Text))
                        Dr["Dealer_Commission_Amount"] = Convert.ToDecimal(txtDealerCommissionAmount.Text).ToString(Funsetsuffix());
                    if (!string.IsNullOrEmpty(txtDealerCommissionAmount.Text))
                        Dr["Dealer_Commission_Amount_1"] = Convert.ToDecimal(txtDealerCommissionAmount.Text).ToString(Funsetsuffix());
                    if (!string.IsNullOrEmpty(hdnDealerCommissionId.Value))
                        Dr["DEALER_COMM_ID"] = hdnDealerCommissionId.Value;
                    if (!string.IsNullOrEmpty(ddlAssetType.SelectedValue))
                        Dr["Asset_Type"] = ddlAssetType.SelectedValue;


                    if (!string.IsNullOrEmpty(txtAssetClass.Text))
                        Dr["CLASS"] = txtAssetClass.Text;
                    if (!string.IsNullOrEmpty(txtMake.Text))
                        Dr["MAKE"] = txtMake.Text;
                    if (!string.IsNullOrEmpty(txtAssetType.Text))
                        Dr["TYPE"] = txtAssetType.Text;
                    if (!string.IsNullOrEmpty(txtModel.Text))
                        Dr["MODEL"] = txtModel.Text;
                    if (!string.IsNullOrEmpty(txtPurpose.Text))
                        Dr["Purpose"] = txtPurpose.Text;


                    dtAssetDetails.Rows.Add(Dr);
                }
            }
            else
            {
                DataRow[] drAsset = dtAssetDetails.Select("SlNo = '" + intSerialNo.ToString() + "'");
                //drAsset[0]["LeaseType"] = rdnlAssetType.SelectedValue.ToString();
                drAsset[0]["Asset_Code"] = ddlAssetCodeList.SelectedText;
                drAsset[0]["Asset_ID"] = ddlAssetCodeList.SelectedValue;
                //if (rdnlAssetType.SelectedValue == "0")
                //{
                //    drAsset[0]["Lease_Asset_No"] = "";
                //}
                //else
                //{
                //    drAsset[0]["Lease_Asset_No"] = ddlLeaseAssetNo.SelectedItem.Text;

                //}

                drAsset[0]["Security_Type"] = ddlAssetTypeofSecurity.SelectedValue;
                drAsset[0]["Security_Type_Identifier"] = ddlSecurityIdentifierType.SelectedValue;
                drAsset[0]["Security_Type_Identifier_Value"] = txtSecurityIdentifierValue.Text;


                drAsset[0]["Noof_Units"] = txtUnitCount.Text;
                drAsset[0]["Finance_Amount"] =Convert.ToDecimal( string.IsNullOrEmpty(txtFinanceAmountAsset.Text.Trim()) ? "0" : txtFinanceAmountAsset.Text.Trim());
                drAsset[0]["Finance_Amount_1"] =Convert.ToDecimal( string.IsNullOrEmpty(txtFinanceAmountAsset.Text.Trim()) ? "0" : txtFinanceAmountAsset.Text.Trim());
                //if (!string.IsNullOrEmpty(txtRequiredFromDate.Text))
                //{
                //    drAsset[0]["Required_FromDate"] = Utility.StringToDate(txtRequiredFromDate.Text);
                //}
                //else
                //{
                //    drAsset[0]["Required_FromDate"] = DBNull.Value;
                //}
                drAsset[0]["Unit_Value"] =Convert.ToDecimal( txtUnitValue.Text);
                drAsset[0]["Unit_Value_1"] =Convert.ToDecimal( txtUnitValue.Text);
                if (txtMarginPercentage.Text != "")
                {
                    drAsset[0]["Margin_Percentage"] = txtMarginPercentage.Text;
                }
                drAsset[0]["TotalAssetValue"] = Convert.ToDecimal(txtUnitValue.Text) * Convert.ToDecimal(txtUnitCount.Text);
                drAsset[0]["TotalAssetValue_1"] = Convert.ToDecimal(txtUnitValue.Text) * Convert.ToDecimal(txtUnitCount.Text);
                //drAsset[0]["Book_depreciation_Percentage"] = string.IsNullOrEmpty(txtBlockDepreciationPerc.Text) ? "0" : txtBlockDepreciationPerc.Text;
                drAsset[0]["Margin_Amount"] =Convert.ToDecimal( string.IsNullOrEmpty(txtMarginAmountAsset.Text) ? 0 : Convert.ToDecimal(txtMarginAmountAsset.Text));
                drAsset[0]["Margin_Amount_1"] =Convert.ToDecimal( string.IsNullOrEmpty(txtMarginAmountAsset.Text) ? 0 : Convert.ToDecimal(txtMarginAmountAsset.Text));
                //drAsset[0]["Block_depreciation_Percentage"] = string.IsNullOrEmpty(txtBlockDepreciationPerc.Text) ? "0" : txtBookDepreciationPerc.Text;
                drAsset[0]["NonCapital_Portion"] = "0";
                //drAsset[0]["Capital_Portion"] = txtCapitalPortion.Text;
                //if (txtPaymentPercentage.Text != "")
                //{
                //    drAsset[0]["Payment_Percentage"] = txtPaymentPercentage.Text;
                //}
                /*Changed by Prabhu.K on 30-Nov-2011 - For OL , it is not required*/
                if (ddlPayTo.SelectedIndex == 0)
                {
                    drAsset[0]["Pay_To_ID"] = drAsset[0]["Entity_ID"] = drAsset[0]["Entity_Code"] = "";
                }
                else
                {
                    drAsset[0]["Pay_To_ID"] = ddlPayTo.SelectedValue.ToString();

                    if (ddlPayTo.SelectedItem.ToString().ToUpper() == "CUSTOMER")
                    {



                        drAsset[0]["Entity_ID"] = Session["AssetCustomer"].ToString().Substring(0, Session["AssetCustomer"].ToString().Remove(Session["AssetCustomer"].ToString().IndexOf(";")).Length);

                    }
                    else
                    {
                        drAsset[0]["Entity_ID"] = ddlEntityNameList.SelectedValue.ToString();
                        drAsset[0]["Entity_Code"] = ddlEntityNameList.SelectedText.ToString();
                    }
                }
                if (txtDiscountAmount.Text != "")
                {
                    drAsset[0]["Discount_Amount"] =Convert.ToDecimal( txtDiscountAmount.Text);
                }
                else
                {
                    drAsset[0]["Discount_Amount"] = 0;
                }

                //if (chkDiscountAbsorbed.Checked)
                //    drAsset[0]["Discount_Absorbed"] = 1;
                //else
                //    drAsset[0]["Discount_Absorbed"] = 0;


                if (!string.IsNullOrEmpty(txtMargintoDealer.Text))
                {
                    drAsset[0]["Margin_Dealer"] =Convert.ToDecimal( txtMargintoDealer.Text);
                    drAsset[0]["Margin_Dealer_1"] =Convert.ToDecimal( txtMargintoDealer.Text);
                }
                else
                {
                    drAsset[0]["Margin_Dealer"] = txtMargintoDealer.Text;
                    drAsset[0]["Margin_Dealer_1"] = txtMargintoDealer.Text;
                }

                if (!string.IsNullOrEmpty(txtMargintoMFC.Text))
                {
                    drAsset[0]["Margin_MFC"] =Convert.ToDecimal( txtMargintoMFC.Text);
                    drAsset[0]["Margin_MFC_1"] =Convert.ToDecimal( txtMargintoMFC.Text);
                }
                else
                {
                    drAsset[0]["Margin_MFC"] = txtMargintoMFC.Text;
                    drAsset[0]["Margin_MFC_1"] = txtMargintoMFC.Text;
                }

                if (!string.IsNullOrEmpty(txtTradeIn.Text))
                {
                    drAsset[0]["Trade_In"] =Convert.ToDecimal( txtTradeIn.Text);
                    drAsset[0]["Trade_In_1"] =Convert.ToDecimal( txtTradeIn.Text);
                }
                else
                {
                    drAsset[0]["Trade_In"] = txtTradeIn.Text;
                    drAsset[0]["Trade_In_1"] = txtTradeIn.Text;
                }




                //RC Deatils
                if (!string.IsNullOrEmpty(txtEngineNo.Text))
                    drAsset[0]["Engine_No"] = txtEngineNo.Text;
                if (!string.IsNullOrEmpty(txtChasisNumber.Text))
                    drAsset[0]["Chasis_No"] = txtChasisNumber.Text;
                if (!string.IsNullOrEmpty(txtDateofRegistration.Text))
                    drAsset[0]["Date_of_Reg"] = txtDateofRegistration.Text;
                if (!string.IsNullOrEmpty(txtRegistrationNo1.Text))
                    drAsset[0]["Reg_No"] = txtRegistrationNo1.Text;
                if (!string.IsNullOrEmpty(txtRegNo2.Text))
                    drAsset[0]["Reg_No2"] = txtRegNo2.Text;
                if (!string.IsNullOrEmpty(txtRegistrationExpiryDate.Text))
                    drAsset[0]["Reg_Expiry_Date"] = txtRegistrationExpiryDate.Text;
                if (!string.IsNullOrEmpty(txtModelYear.Text))
                    drAsset[0]["Model_Year"] = txtModelYear.Text;
                if (!string.IsNullOrEmpty(ddlYearofManufacturer.SelectedItem.Text))
                    drAsset[0]["ManuFactoring_Year"] = ddlYearofManufacturer.SelectedItem.Text;
                if (!string.IsNullOrEmpty(txtRegisteredOwner.Text))
                    drAsset[0]["Registered_Owner"] = txtRegisteredOwner.Text;



                if (!string.IsNullOrEmpty(txtDealerCommissionBasisRate.Text))
                    drAsset[0]["Dealer_Commission_Rate"] = Convert.ToDecimal(txtDealerCommissionBasisRate.Text).ToString(Funsetsuffix());


                if (!string.IsNullOrEmpty(txtDealerCommissionAmount.Text))
                    drAsset[0]["Dealer_Commission_Amount"] = Convert.ToDecimal(txtDealerCommissionAmount.Text).ToString(Funsetsuffix());

                if (!string.IsNullOrEmpty(txtDealerCommissionAmount.Text))
                    drAsset[0]["Dealer_Commission_Amount_1"] = Convert.ToDecimal(txtDealerCommissionAmount.Text).ToString(Funsetsuffix());


                if (!string.IsNullOrEmpty(hdnDealerCommissionId.Value))
                    drAsset[0]["DEALER_COMM_ID"] = hdnDealerCommissionId.Value;

                if (!string.IsNullOrEmpty(ddlAssetType.SelectedValue))
                    drAsset[0]["Asset_Type"] = ddlAssetType.SelectedValue;


                if (!string.IsNullOrEmpty(txtAssetClass.Text))
                    drAsset[0]["CLASS"] = txtAssetClass.Text;
                if (!string.IsNullOrEmpty(txtMake.Text))
                    drAsset[0]["MAKE"] = txtMake.Text;
                if (!string.IsNullOrEmpty(txtAssetType.Text))
                    drAsset[0]["TYPE"] = txtAssetType.Text;
                if (!string.IsNullOrEmpty(txtModel.Text))
                    drAsset[0]["MODEL"] = txtModel.Text;
                if (!string.IsNullOrEmpty(txtPurpose.Text))
                    drAsset[0]["Purpose"] = txtPurpose.Text;


                drAsset[0].AcceptChanges();
            }
            Session["PricingAssetDetails"] = dtAssetDetails;


            if (HttpContext.Current.Session["DealerId"] != null)
            {
                ddlEntityNameList.SelectedValue = HttpContext.Current.Session["DealerId"].ToString();
            }
            if (HttpContext.Current.Session["DealerName"] != null)
            {
                ddlEntityNameList.SelectedText = HttpContext.Current.Session["DealerName"].ToString();
            }


            ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "funBacktoParenWindow();", true);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, "Application Asset Ok Button Event");
            return;
        }
    }

    protected void ddlAssetCodeList_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {



            txtBookDepreciationPerc.Text = "";
            //txtBlockDepreciationPerc.Text = "";
            //txtCapitalPortion.Text = "";
            //txtNonCapitalPortion.Text = "";
            txtUnitValue.Text = "";
            txtFinanceAmountAsset.Text = "";
            txtMarginAmountAsset.Text = "";
            txtMarginPercentage.Text = "";
            txtTotalAssetValue.Text = "";
            txtMargintoDealer.Text = string.Empty;
            txtMargintoMFC.Text = string.Empty;
            txtTradeIn.Text = string.Empty;
            //txtRequiredFromDate.Text = "";
            //ViewState["AssetAvailDate"] = string.Empty;
            FunFillDepreciationRate((DataTable)ViewState["RateDt1"], ddlAssetCodeList.SelectedValue);
            //ddlAssetCodeList.ToolTip = ddlAssetCodeList.SelectedText;
            //ddlAssetCodeList.Focus();
            //if (rdnlAssetType.SelectedValue == "1")
            //{
            //    Dictionary<string, string> dictParam = new Dictionary<string, string>();
            //    dictParam.Add("@OPTION", "3");
            //    dictParam.Add("@COMPANYID", intCompanyId.ToString());
            //    dictParam.Add("@AssetID", ddlAssetCodeList.SelectedValue);
            //    DataTable DtRate = Utility.GetDataset("S3G_ORG_GETAPPLICATIONASSET", dictParam).Tables[0];
            //    ViewState["RateDt2"] = DtRate;
            //    ddlLeaseAssetNo.DataSource = DtRate;
            //    ddlLeaseAssetNo.DataTextField = "LEASE_ASSET_NO";
            //    ddlLeaseAssetNo.DataValueField = "LEASE_ASSET_NO";
            //    ddlLeaseAssetNo.DataBind();
            //    ddlLeaseAssetNo.Items.Insert(0, new ListItem("--Select--", "0"));
            //}
            //if (ddlAssetCodeList.SelectedValue == "0")
            //{
            //    DataTable DtRate = new DataTable();
            //    Dictionary<string, string> dictParam = new Dictionary<string, string>();
            //    dictParam.Add("@OPTION", "1");
            //    dictParam.Add("@COMPANYID", intCompanyId.ToString());
            //    DtRate = Utility.GetDataset("S3G_ORG_GETAPPLICATIONASSET", dictParam).Tables[0];
            //    ddlLeaseAssetNo.DataSource = DtRate;
            //    ddlLeaseAssetNo.DataTextField = "LEASE_ASSET_NO";
            //    ddlLeaseAssetNo.DataValueField = "LEASE_ASSET_NO";
            //    ddlLeaseAssetNo.DataBind();
            //    ddlLeaseAssetNo.Items.Insert(0, new ListItem("--Select--", "0"));
            //}
            //ddlAssetCodeList.Focus();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    protected void ddlLeaseAssetNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Dictionary<string, string> dictParam = new Dictionary<string, string>();
        //if (ddlLeaseAssetNo.SelectedIndex > 0)
        //{
        //    dictParam.Add("@OPTION", "4");
        //    dictParam.Add("@COMPANYID", intCompanyId.ToString());
        //    dictParam.Add("@LeaseAssetNo", ddlLeaseAssetNo.SelectedValue);
        //    DataTable DtRate = Utility.GetDataset("S3G_ORG_GETAPPLICATIONASSET", dictParam).Tables[0];
        //    txtTotalAssetValue.Text = txtUnitValue.Text = DtRate.Rows[0]["WDV"].ToString();
        //    //Changed by Thangam M on 24/Oct/2012 to round off Finance amount
        //    //if (!string.IsNullOrEmpty(DtRate.Rows[0]["WDV"].ToString()))
        //    //{
        //    //    txtCapitalPortion.Text = txtFinanceAmountAsset.Text = Math.Round(Convert.ToDouble(DtRate.Rows[0]["WDV"].ToString())).ToString();
        //    //}
        //    //else
        //    //{
        //    //    txtCapitalPortion.Text = txtFinanceAmountAsset.Text = string.Empty;
        //    //}
        //    //End here
        //    ddlAssetCodeList.SelectedValue = DtRate.Rows[0]["Asset_Code"].ToString();
        //    txtUnitValue.ReadOnly = true;
        //}
        //else
        //{
        //    txtTotalAssetValue.Text = txtUnitValue.Text = "";
        //    txtUnitValue.ReadOnly = false;
        //}
        //ddlLeaseAssetNo.Focus();
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "window.close();", true);
    }

    #endregion

    #region Page Methods

    /// <summary>
    /// Method to load DropDownList & Ajax combos
    /// </summary>
    /// 

    protected void FunProLoadCombos()
    {
        OrgMasterMgtServicesReference.OrgMasterMgtServicesClient ObjCustomerService = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
        S3GBusEntity.S3G_Status_Parameters ObjStatus = new S3G_Status_Parameters();
        ObjStatus.Option = 1;
        ObjStatus.Param1 = S3G_Statu_Lookup.PAY_TO.ToString();
        Utility.FillDLL(ddlPayTo, ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus));
        ddlPayTo.SelectedValue = "137";
        DataTable DtRate = new DataTable();
        Dictionary<string, string> dictParam = new Dictionary<string, string>();


        if (HttpContext.Current.Session["DealerId"] != null)
        {
            ddlEntityNameList.SelectedValue = HttpContext.Current.Session["DealerId"].ToString();
        }
        if (HttpContext.Current.Session["DealerName"] != null)
        {
            ddlEntityNameList.SelectedText = HttpContext.Current.Session["DealerName"].ToString();
        }


        FunProLoadAssetValue("NEW");

        //dictParam.Add("@OPTION", "2");
        //dictParam.Add("@COMPANYID", intCompanyId.ToString());
        //DtRate = Utility.GetDataset("S3G_ORG_GETAPPLICATIONASSET", dictParam).Tables[0];
        //ddlAssetCodeList.DataSource = DtRate;
        //ddlAssetCodeList.DataTextField = "Asset_Code";
        //ddlAssetCodeList.DataValueField = "Asset_ID";
        //ddlAssetCodeList.DataBind();
        //ddlAssetCodeList.Items.Insert(0, new ListItem("--Select--", "0"));

        //ViewState["RateDt1"] = DtRate;

        //dictParam = new Dictionary<string, string>();

        //dictParam.Add("@OPTION", "1");
        //dictParam.Add("@COMPANYID", intCompanyId.ToString());
        //DtRate = Utility.GetDataset("S3G_ORG_GETAPPLICATIONASSET", dictParam).Tables[0];
        //ddlLeaseAssetNo.DataSource = DtRate;
        //ddlLeaseAssetNo.DataTextField = "LEASE_ASSET_NO";
        //ddlLeaseAssetNo.DataValueField = "LEASE_ASSET_NO";
        //ddlLeaseAssetNo.DataBind();
        //ddlLeaseAssetNo.Items.Insert(0, new ListItem("--Select--", "0"));
        //ViewState["RateDt2"] = DtRate;
        //dictParam.Clear();
        //dictParam.Add("@Option", "11");
        //dictParam.Add("@Company_ID", intCompanyId.ToString());
        //dictParam.Add("@ID", "1");
        //ddlEntityNameList.BindDataTable(SPNames.S3G_ORG_GetPricing_List, dictParam, true, new string[] { "Entity_ID", "Entity_Code", "Entity_Name" });

        //ObjStatus.Option = 1;
        //ObjStatus.Param1 = intCompanyId.ToString();
        //ObjStatus.Param2 
        //Utility.FillDLL(ddlEntityNameList, ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus), true);
    }

    [System.Web.Services.WebMethod]
    public static string[] GetVendors(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        Procparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
        Procparam.Add("@Entity_Type", "8");
        Procparam.Add("@PrefixText", prefixText);

        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_OR_GET_ENTYMAST_AGT", Procparam));

        return suggetions.ToArray();

    }

    private void FunProLoadAssetValue(string StrAssetType)
    {
        DataSet DsRate = new DataSet();
        Dictionary<string, string> dictParam = new Dictionary<string, string>();

        if (StrAssetType.ToUpper() == "NEW")
            dictParam.Add("@OPTION", "2");
        else
            dictParam.Add("@OPTION", "6");

        dictParam.Add("@COMPANYID", intCompanyId.ToString());
        DsRate = Utility.GetDataset("S3G_ORG_GETAPPLICATIONASSET", dictParam);
        //ddlAssetCodeList.DataSource = DsRate.Tables[0];
        //ddlAssetCodeList.DataTextField = "Asset_Code";
        //ddlAssetCodeList.DataValueField = "Asset_ID";
        //ddlAssetCodeList.DataBind();
        //ddlAssetCodeList.Items.Insert(0, new ListItem("--Select--", "0"));

        if (DsRate.Tables[1].Rows.Count > 0)
        {
            ddlAssetTypeofSecurity.DataSource = DsRate.Tables[1];
            ddlAssetTypeofSecurity.DataTextField = "NAME";
            ddlAssetTypeofSecurity.DataValueField = "VALUE";
            ddlAssetTypeofSecurity.DataBind();
            ddlAssetTypeofSecurity.Items.Insert(0, new ListItem("--Select--", "0"));
        }

        if (DsRate.Tables[2].Rows.Count > 0)
        {
            ddlSecurityIdentifierType.DataSource = DsRate.Tables[2];
            ddlSecurityIdentifierType.DataTextField = "NAME";
            ddlSecurityIdentifierType.DataValueField = "VALUE";
            ddlSecurityIdentifierType.DataBind();
            ddlSecurityIdentifierType.Items.Insert(0, new ListItem("--Select--", "0"));


            ddlSecurityIdentifierType2.DataSource = DsRate.Tables[2];
            ddlSecurityIdentifierType2.DataTextField = "NAME";
            ddlSecurityIdentifierType2.DataValueField = "VALUE";
            ddlSecurityIdentifierType2.DataBind();
            ddlSecurityIdentifierType2.Items.Insert(0, new ListItem("--Select--", "0"));
        }

        //ViewState["RateDt1"] = DsRate.Tables[0];
    }

    public void FunFillDepreciationRate(DataTable RateDt, string strAssetId)
    {
        txtBookDepreciationPerc.Text = "";
        //DataRow[] DrRate = (RateDt.Select("Asset_ID = '" + strAssetId + "'"));

        //if (DrRate.Count() > 0)
        //{
        //txtBookDepreciationPerc.Text = DrRate[0]["Book_Depreciation_Rate"].ToString();
        //txtBlockDepreciationPerc.Text = DrRate[0]["Block_Depreciation_Rate"].ToString();
        // }
        //txtBlockDepreciationPerc.Text = "";

        //if (Request.QueryString["qsMaster"] != null)
        //{
        //    if (Request.QueryString["qsMaster"].ToString() == "Yes")
        //    {
        //        DataRow[] DrRate = (RateDt.Select("Asset_ID = '" + strAssetId + "'"));

        //        if (DrRate.Count() > 0)
        //        {
        //            txtBookDepreciationPerc.Text = DrRate[0]["Book_Depreciation_Rate"].ToString();
        //            //txtBlockDepreciationPerc.Text = DrRate[0]["Block_Depreciation_Rate"].ToString();
        //        }
        //    }
        //}


        DataTable dsGetCheckListDetails = new DataTable();
        Dictionary<string, string> strProParm = new Dictionary<string, string>();
        strProParm.Add("@OPTION", "9");
        strProParm.Add("@COMPANYID", intCompanyId.ToString());
        strProParm.Add("@USERID", intUserId.ToString());
        strProParm.Add("@PROGRAMID", "42");
        strProParm.Add("@Page_Mode", "C");
        strProParm.Add("@ASSET_CATEGORY_ID", ddlAssetCodeList.SelectedValue);
        dsGetCheckListDetails = Utility.GetDefaultData("S3G_OR_LOAD_LOV", strProParm);
        if (dsGetCheckListDetails.Rows.Count > 0)
        {
            txtBookDepreciationPerc.Text = dsGetCheckListDetails.Rows[0]["BOOK_DEPRECIATION_RATE"].ToString();
        }

        txtAssetClass.Text = string.Empty;
        txtMake.Text = string.Empty;
        txtModel.Text = string.Empty;
        txtMake.Text = string.Empty;
        txtPurpose.Text = string.Empty;

        DataTable dsGetCheckListDetails2 = new DataTable();
        Dictionary<string, string> strProParm2 = new Dictionary<string, string>();
        strProParm2.Add("@OPTION", "10");
        strProParm2.Add("@COMPANYID", intCompanyId.ToString());
        strProParm2.Add("@USERID", intUserId.ToString());
        strProParm2.Add("@PROGRAMID", "42");
        strProParm2.Add("@Page_Mode", "C");
        strProParm2.Add("@ASSET_CATEGORY_ID", ddlAssetCodeList.SelectedValue);
        dsGetCheckListDetails2 = Utility.GetDefaultData("S3G_OR_LOAD_LOV", strProParm2);
        if (dsGetCheckListDetails2.Rows.Count > 0)
        {
            txtAssetClass.Text = dsGetCheckListDetails2.Rows[0]["CLASS"].ToString();
            txtMake.Text = dsGetCheckListDetails2.Rows[0]["MAKE"].ToString();
            txtModel.Text = dsGetCheckListDetails2.Rows[0]["MODEL"].ToString();
            txtAssetType.Text = dsGetCheckListDetails2.Rows[0]["TYPE"].ToString();
            txtPurpose.Text = dsGetCheckListDetails2.Rows[0]["Purpose"].ToString();
        }

    }

    protected void FunToggleLeaseAssetCode(bool Canshow)
    {
        //rdnlAssetType.Visible = Canshow;
    }

    protected void FunFillControls()
    {

    }

    public void FunToggleLableVisble(bool CanShow)
    {
        //lblLeaseAssetNo.Visible = CanShow;
        //ddlLeaseAssetNo.Visible = CanShow;
        //rfvLeastAssetCodeNo.Enabled = CanShow;

        //lblAssetCodeList.Visible = !CanShow;
        //ddlAssetCodeList.Visible = !CanShow;
        //rfvAssetCodeList.Enabled = !CanShow;

        //rfvRequiredFromDate.Enabled = CanShow;
        //if (CanShow)
        //{
        //    //txtRequiredFromDate.ReadOnly = false;
        //}
        //else
        //{
        //    txtRequiredFromDate.ReadOnly = true;
        //    txtRequiredFromDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtRequiredFromDate.ClientID + "','" + strDateFormat + "',false,true);");
        //}
        //txtRequiredFromDate_CalendarExtender.Enabled = CanShow;
    }

    protected void FunToggleEntityControls(bool CanShow)
    {
        txtCustomerName.Text = "";
        if (Session["AssetCustomer"] != null)
        {
            txtCustomerName.Text = Session["AssetCustomer"].ToString().Substring(Session["AssetCustomer"].ToString().IndexOf(";") + 1);
        }

        ddlEntityNameList.Visible = ddlEntityNameList.IsMandatory = CanShow;
        lblEntityNameList.Visible = CanShow;
        //rfvEntityNameList.Enabled = CanShow;

        txtCustomerName.Visible = !CanShow;
        lblCustomerName.Visible = !CanShow;
        rfvCustomerName.Enabled = !CanShow;
    }
    private void FunPriLoadAssetDetails(DataTable dtAssetDetails)
    {
        txtUnitCount.Enabled = false;
        DataRow[] drAsset = dtAssetDetails.Select("SlNo = " + intSerialNo.ToString());
        if (drAsset.Length == 0)
        {
            txtSlNo.Text = Convert.ToString(dtAssetDetails.Rows.Count + 1);
            return;
        }
        if (intSerialNo == 0)
        {
            txtSlNo.Text = (intSerialNo + 1).ToString();
        }
        else
        {
            txtSlNo.Text = (intSerialNo).ToString();
        }
        //rdnlAssetType.SelectedValue = drAsset[0]["LeaseType"].ToString();
        ddlAssetCodeList.SelectedValue = drAsset[0]["Asset_ID"].ToString();
        ddlAssetCodeList.SelectedText = drAsset[0]["Asset_Code"].ToString();
        //if (rdnlAssetType.SelectedValue == "1")
        //{
        //    ddlLeaseAssetNo.SelectedValue = drAsset[0]["Lease_Asset_No"].ToString();
        //    FunToggleLableVisble(true);
        //    //txtRequiredFromDate.Text = DateTime.Parse(drAsset[0]["Required_FromDate"].ToString(), CultureInfo.CurrentCulture).ToString(objS3GSession.ProDateFormatRW);
        //}
        //else
        //{
        //    FunToggleLableVisble(false);
        //    //txtRequiredFromDate.Text = string.Empty;
        //}
        txtAssetClass.Text = string.Empty;
        txtMake.Text = string.Empty;
        txtModel.Text = string.Empty;
        txtMake.Text = string.Empty;
        txtPurpose.Text = string.Empty;

        txtAssetClass.Text = drAsset[0]["CLASS"].ToString();
        txtMake.Text = drAsset[0]["MAKE"].ToString();
        txtModel.Text = drAsset[0]["MODEL"].ToString();
        txtAssetType.Text = drAsset[0]["TYPE"].ToString();
        txtPurpose.Text = drAsset[0]["PURPOSE"].ToString();


        txtUnitCount.Text = drAsset[0]["Noof_Units"].ToString();
       
        txtFinanceAmountAsset.Text = drAsset[0]["Finance_Amount"].ToString();
       
        //if (dtAssetDetails.Columns.Contains("Capital_Portion") == true)
        //    txtCapitalPortion.Text = drAsset[0]["Capital_Portion"].ToString();
        //if (dtAssetDetails.Columns.Contains("NonCapital_Portion") == true)
        //    txtNonCapitalPortion.Text = drAsset[0]["NonCapital_Portion"].ToString();
        txtUnitValue.Text = drAsset[0]["Unit_Value"].ToString();
        txtBookDepreciationPerc.Text = Convert.ToDecimal(drAsset[0]["Book_depreciation_Percentage"].ToString()).ToString(Funsetsuffix());
        //txtBlockDepreciationPerc.Text = drAsset[0]["Block_depreciation_Percentage"].ToString();

        ddlAssetTypeofSecurity.SelectedValue = drAsset[0]["SECURITY_TYPE"].ToString();
        ddlSecurityIdentifierType.SelectedValue = drAsset[0]["SECURITY_TYPE_IDENTIFIER"].ToString();
        txtSecurityIdentifierValue.Text = drAsset[0]["SECURITY_TYPE_IDENTIFIER_VALUE"].ToString();

        txtMarginAmountAsset.Text = Convert.ToDecimal(drAsset[0]["Margin_Amount"].ToString()).ToString(Funsetsuffix());
        txtMarginPercentage.Text = Convert.ToDecimal(drAsset[0]["Margin_Percentage"].ToString()).ToString(Funsetsuffix());
        //txtPaymentPercentage.Text = Convert.ToString(drAsset[0]["Payment_Percentage"]);
        txtTotalAssetValue.Text = Convert.ToDecimal(Convert.ToInt32(txtUnitCount.Text) * Convert.ToDouble(txtUnitValue.Text)).ToString(Funsetsuffix());
        ddlPayTo.SelectedValue = drAsset[0]["Pay_To_ID"].ToString();

        txtMargintoDealer.Text = drAsset[0]["Margin_Dealer"].ToString();
        txtMargintoMFC.Text = drAsset[0]["Margin_MFC"].ToString();
        txtTradeIn.Text = drAsset[0]["Trade_In"].ToString();



        txtEngineNo.Text = drAsset[0]["Engine_No"].ToString();
        txtChasisNumber.Text = drAsset[0]["Chasis_No"].ToString();
        txtDateofRegistration.Text = drAsset[0]["Date_of_Reg"].ToString();
        txtRegistrationNo1.Text = drAsset[0]["Reg_No"].ToString();
        txtRegNo2.Text = drAsset[0]["Reg_No2"].ToString();
        txtRegistrationExpiryDate.Text = drAsset[0]["Reg_Expiry_Date"].ToString();
        txtModelYear.Text = drAsset[0]["Model_Year"].ToString();
        ddlYearofManufacturer.SelectedValue = drAsset[0]["ManuFactoring_Year"].ToString();
        txtRegisteredOwner.Text = drAsset[0]["Registered_Owner"].ToString();
        if (drAsset[0]["Dealer_Commission_Rate"].ToString() != string.Empty)
            txtDealerCommissionBasisRate.Text = Convert.ToDecimal(drAsset[0]["Dealer_Commission_Rate"].ToString()).ToString(Funsetsuffix());
        if (drAsset[0]["Dealer_Commission_Amount"].ToString() != string.Empty)
            txtDealerCommissionAmount.Text = Convert.ToDecimal(drAsset[0]["Dealer_Commission_Amount"].ToString()).ToString(Funsetsuffix());
        hdnDealerCommissionId.Value = drAsset[0]["DEALER_COMM_ID"].ToString();
        ddlAssetType.SelectedValue = drAsset[0]["Asset_Type"].ToString();
        txtDiscountAmount.Text = drAsset[0]["Discount_Amount"].ToString();


        txtAssetClass.Text = drAsset[0]["CLASS"].ToString();
        txtAssetType.Text = drAsset[0]["TYPE"].ToString();
        txtMake.Text = drAsset[0]["MAKE"].ToString();
        txtModel.Text = drAsset[0]["MODEL"].ToString();
        txtPurpose.Text = drAsset[0]["Purpose"].ToString();

        //if (drAsset[0]["Discount_Absorbed"].ToString() == "1")
        //    chkDiscountAbsorbed.Checked = true;

        if (ddlPayTo.SelectedItem.Text.ToUpper() == "ENTITY")
        {
            FunToggleEntityControls(true);
            ddlEntityNameList.SelectedValue = drAsset[0]["Entity_ID"].ToString();
            ddlEntityNameList.SelectedText = drAsset[0]["Entity_Code"].ToString();
        }
        else if (ddlPayTo.SelectedItem.Text.ToUpper() == "CUSTOMER")
        {
            FunToggleEntityControls(false);
            if (Session["AssetCustomer"] != null)
            {
                txtCustomerName.Text = Session["AssetCustomer"].ToString().Substring(Session["AssetCustomer"].ToString().IndexOf(";") + 1);
            }
        }
        if (Request.QueryString["qsMode"] != null)
        {

            //rdnlAssetType.Enabled = false;

            // txtAssetCodeList.Text = string.Empty;
            // hdnAssetCodeList.Value = "0";
            //ddlLeaseAssetNo.ClearDropDownList();
            ddlAssetCodeList.Enabled = false;
            ddlAssetCodeList.PostBackButtonEnabled = false;
            ddlPayTo.ClearDropDownList();
            //txtRequiredFromDate_CalendarExtender.Enabled = false;
            txtUnitCount.Enabled = txtUnitValue.Enabled = txtTotalAssetValue.Enabled = txtMarginAmountAsset.Enabled =
            txtMarginPercentage.Enabled = txtBookDepreciationPerc.Enabled =
            txtFinanceAmountAsset.Enabled =

            txtSlNo.Enabled = txtDiscountAmount.Enabled = false;
            ddlPayTo.Enabled = false;
            ddlSecurityIdentifierType.Enabled = ddlSecurityIdentifierType2.Enabled = false;
            ddlAssetTypeofSecurity.Enabled = false;
            //txtRequiredFromDate_CalendarExtender.Enabled = false;
            //chkDiscountAbsorbed.Enabled = false;
            ddlEntityNameList.Enabled = false;
            //if (ddlSecurityIdentifierType.Items.Count > 0)
            //    ddlSecurityIdentifierType.ClearDropDownList();
            //if (ddlAssetTypeofSecurity.Items.Count > 0)
            //    ddlAssetTypeofSecurity.ClearDropDownList();
            //txtSecurityIdentifierValue.Enabled =false;
            txtMargintoDealer.Enabled = txtMargintoMFC.Enabled = txtTradeIn.Enabled = false;
            txtEngineNo.Enabled = false;
            txtChasisNumber.Enabled = false;
            //txtDateofRegistration.Enabled = false;
            //txtRegistrationNo1.Enabled = false;
            //txtRegNo2.Enabled = false;
            //txtRegistrationExpiryDate.Enabled = false;
            //txtRegisteredOwner.Enabled = false;
            ddlAssetType.Enabled = false;
            if (Request.QueryString["qsMode"].ToString() == "Q")
            {
                ddlYearofManufacturer.Enabled = false;
            }
            else
            {
                ddlYearofManufacturer.Enabled = true;
            }



        }
        if (Request.QueryString["FromPricing"] != null)
        {
            //rdnlAssetType.Enabled = false;
            //txtAssetCodeList.Text = string.Empty;
            //hdnAssetCodeList.Value = "0";
            //ddlLeaseAssetNo.ClearDropDownList();
            ddlAssetCodeList.Enabled = false;
            txtUnitValue.Enabled = false;
            ddlPayTo.Enabled = false;
            ddlEntityNameList.Enabled = false;
            ddlAssetType.Enabled = false;
            txtMargintoMFC.Enabled = false;
            txtMargintoDealer.Enabled = false;
            txtTradeIn.Enabled = false;

        }

        txtTotalAssetValue.funPubChangeCurrencyFormat();
        txtUnitValue.funPubChangeCurrencyFormat();
        txtFinanceAmountAsset.funPubChangeCurrencyFormat();
        txtMargintoDealer.funPubChangeCurrencyFormat();
        txtMargintoMFC.funPubChangeCurrencyFormat();
        txtTradeIn.funPubChangeCurrencyFormat();
        txtDealerCommissionAmount.funPubChangeCurrencyFormat();
        txtMarginAmountAsset.funPubChangeCurrencyFormat();
    }

    protected void txtUnitValue_TextChanged(object sender, EventArgs e)
    {
        txtMarginPercentage.Focus();
        FunPriAssignMarginAmount();
        txtUnitValue.Focus();

    }

    protected void txtMarginPercentage_TextChanged(object sender, EventArgs e)
    {
        //FunPriAssignMarginAmount();
        //txtMarginPercentage.Focus();
    }

    protected void txtUnitCount_TextChanged(object sender, EventArgs e)
    {
        FunPriAssignMarginAmount();
        txtUnitCount.Focus();
    }

    private string Funsetsuffix()
    {

        int suffix = 1;

        // S3GSession ObjS3GSession = new S3GSession();
        suffix = objS3GSession.ProGpsSuffixRW;
        // suffix = 0;
        string strformat = "0.";
        for (int i = 1; i <= suffix; i++)
        {
            strformat += "0";
        }
        return strformat;
    }

    private void FunPriAssignMarginAmount()
    {
        if (!string.IsNullOrEmpty(txtMarginPercentage.Text) && !string.IsNullOrEmpty(txtTotalAssetValue.Text))
        {
            decimal dcmTotalAssetValue = Convert.ToDecimal(txtTotalAssetValue.Text);
            decimal dcmMarginPercentage = Convert.ToDecimal(txtMarginPercentage.Text) / 100;
            txtMarginAmountAsset.Text = (dcmTotalAssetValue * dcmMarginPercentage).ToString(Funsetsuffix());

        }
        else
        {
            //txtMarginAmountAsset.Text = "";
        }
    }
    [System.Web.Services.WebMethod]
    public static string[] GetAsset(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        Procparam.Clear();
        Procparam.Add("@COMPANYID", HttpContext.Current.Session["Company_Id"].ToString());
        Procparam.Add("@PrefixText", prefixText);
        Procparam.Add("@Contract_Type", "3");
        Procparam.Add("@LOB_ID", HttpContext.Current.Session["LOBID"].ToString());
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_OR_GET_PRIAST_APP", Procparam), true);
        //if (suggetions.Count > 0)
        //{
        //    obj_PageValue.FunAssetchanges();
        //}
        return suggetions.ToArray();

    }


    #endregion
    protected void ddlEntityNameList_Item_Selected(object Sender, EventArgs e)
    {
        try
        {

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    //protected void btnCheckDealerComission_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        funPriGetDealerCommsissionDetails();
    //    }
    //    catch (Exception ex)
    //    {
    //        ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
    //    }
    //}
    protected void txtEngineNo_TextChanged(object sender, EventArgs e)
    {
        try
        {
            DataTable dtDeDup;
            Dictionary<string, string> strProParm = new Dictionary<string, string>();
            strProParm.Add("@OPTION", "13");
            strProParm.Add("@COMPANYID", intCompanyId.ToString());
            strProParm.Add("@USERID", intUserId.ToString());
            strProParm.Add("@PROGRAMID", "38");
            strProParm.Add("@PAGE_MODE", "C");
            strProParm.Add("@PayTo", ddlPayTo.SelectedValue);
            strProParm.Add("@Entity_Id", ddlEntityNameList.SelectedValue);
            strProParm.Add("@Engine_Number", txtEngineNo.Text);

            if (ddlPayTo.SelectedValue == "137")//Dealer
            {
                dtDeDup = Utility.GetDefaultData("S3G_OR_LOAD_LOV_APP", strProParm);
                if (dtDeDup.Rows.Count > 0)
                {
                    Utility.FunShowAlertMsg(this, "Engine No Exists for the same Dealer");
                    txtEngineNo.Text = string.Empty;
                    return;
                }
            }



            if (ddlAssetType.SelectedValue == "1")
            {

                txtChasisNumber.Enabled = false;
                txtEngineNo.Enabled = false;
            }
            else
            {

                txtChasisNumber.Enabled = true;
                txtEngineNo.Enabled = true;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    protected void txtChasisNumber_TextChanged(object sender, EventArgs e)
    {
        try
        {
            DataTable dtDeDup;
            Dictionary<string, string> strProParm = new Dictionary<string, string>();
            strProParm.Add("@OPTION", "14");
            strProParm.Add("@COMPANYID", intCompanyId.ToString());
            strProParm.Add("@USERID", intUserId.ToString());
            strProParm.Add("@PROGRAMID", "38");
            strProParm.Add("@PAGE_MODE", "C");
            strProParm.Add("@PayTo", ddlPayTo.SelectedValue);
            strProParm.Add("@Entity_Id", ddlEntityNameList.SelectedValue);
            strProParm.Add("@CHASIS_NUMBER", txtChasisNumber.Text);

            if (ddlPayTo.SelectedValue == "137")//Dealer
            {
                dtDeDup = Utility.GetDefaultData("S3G_OR_LOAD_LOV_APP", strProParm);
                if (dtDeDup.Rows.Count > 0)
                {
                    Utility.FunShowAlertMsg(this, "Chasis No Exists for the same Dealer");
                    txtChasisNumber.Text = string.Empty;
                    return;
                }
            }


            if (ddlAssetType.SelectedValue == "1")
            {

                txtChasisNumber.Enabled = false;
                txtEngineNo.Enabled = false;
            }
            else
            {

                txtChasisNumber.Enabled = true;
                txtEngineNo.Enabled = true;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }

    }
    protected void ddlAssetType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlAssetType.SelectedValue == "1")
        {
            txtEngineNo.Enabled = false;
            txtChasisNumber.Enabled = false;
        }
        else
        {
            txtEngineNo.Enabled = true;
            txtChasisNumber.Enabled = true;
        }
    }
    protected void ddlYearofManufacturer_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["Manu_Fact_Year"] != null)
            {
                int YearParm = Convert.ToInt32(ViewState["Manu_Fact_Year"].ToString());
                DateTime dtSelectedYear = Utility.StringToDate("01/01/" + ddlYearofManufacturer.SelectedValue);
                int YearGap = new DateTime((DateTime.Now - dtSelectedYear).Ticks).Year - 1;
                if (YearGap > YearParm)
                {
                    Utility.FunShowAlertMsg(this, "Year of Manufacture should not exceed the( " + YearParm + " )Years");
                    ddlYearofManufacturer.ClearSelection();
                }

            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
}

