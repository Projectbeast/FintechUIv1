﻿/// Module Name     :   Origination
/// Screen Name     :   S3G_ORG_SanctionNumberQuery
/// Created By      :   Narayanan
/// Created Date    :   05-June-2010
/// Purpose         :   To Query 

using System;
using System.Globalization;
using System.Web.UI.WebControls;
using S3GBusEntity;
using System.Data;
using System.Text;
using System.Web.Security;
using System.Collections.Generic;
using S3GBusEntity.Origination;
using System.Configuration;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.IO;
using System.Diagnostics;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

public partial class Origination_S3G_ORG_SactionNumberQuery : ApplyThemeForProject
{
    int _CompanyID, _UserID;
    string _DateFormat = "dd/MM/yyyy";
    string strDateFormat;

    static double _PercentageOfImportance;
    static double _Guidelinescore;
    static double _ActualScore;
    static double _Negative_Deviation;
    static double _Negative_Deviation_Percent;
    static double _GlobalParameterNegativeDeviationPercent;
    static double _GlobalParameterExposureVariancePercent;
    static double _Exposure_Percent;
    static double _Exposure_Variance;
    static double _ExposureVarianceAmount;
    static double _IndicativeExposure;
    static double _RevisedLimitForTheCustomer;
    static string _Remark;
    static string _Mode = string.Empty;

    string strKey = "Insert";

    static int intCreditParamAppID = 0;
    static int intCreditParamTransID = 0;
    Dictionary<string, string> Procparam;
    UserInfo ObjUserInfo = new UserInfo();
    S3GSession ObjS3GSession = new S3GSession();
    #region Page Load

    protected void Page_Load(object sender, EventArgs e)
    {

        strDateFormat = ObjS3GSession.ProDateFormatRW;
        _CompanyID = ObjUserInfo.ProCompanyIdRW;
        _UserID = ObjUserInfo.ProUserIdRW;
        _DateFormat = ObjS3GSession.ProDateFormatRW;

        try
        {
            if (!IsPostBack)
            {
                txtStartDate.Attributes.Add("readonly", "readonly");
                txtEndDate.Attributes.Add("readonly", "readonly");

                //SetResourceSettings();
                //FillDDL();

                if (Request.QueryString.Get("qsViewId") != null)
                {
                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                    FormsAuthenticationTicket TicketViewID = FormsAuthentication.Decrypt(Request.QueryString.Get("qsViewId"));
                    if (!(string.IsNullOrEmpty(TicketViewID.Name)))
                    {
                        if (Request.QueryString.Get("qsMode") != null)
                        {
                            if (string.Compare("Q", Request.QueryString.Get("qsMode")) == 0)
                            {
                                intCreditParamAppID = Convert.ToInt32(TicketViewID.Name);
                                GetSanctionNumberDetails(intCreditParamAppID);
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            cvQuery.ErrorMessage = ex.Message;
            cvQuery.IsValid = false;
        }
    }

    #endregion

    #region Page Event

    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlOptions.SelectedValue.ToString() == "2")
            {
                if (!CheckSystemDate(txtStartDate.Text))
                    throw new ArgumentException(Resources.LocalizationResources.ORG_Enq_StartDate);

                if (!CheckSystemDate(txtEndDate.Text))
                    throw new ArgumentException(Resources.LocalizationResources.ORG_Enq_EndDate);

                if (!CompareDates(txtStartDate.Text, txtEndDate.Text))
                    throw new ArgumentException(Resources.LocalizationResources.ORG_Enq_LessEndDate);
            }
            LoadSanctionNumberQry();

        }
        //catch (FaultException<CompanyMgtServicesReference.ClsPubFaultException> objFaultExp)
        //{            
        //    cvCreditParameterTrans.ErrorMessage = objFaultExp.Detail.ProReasonRW;
        //    cvCreditParameterTrans.IsValid = false;
        //}
        catch (Exception ex)
        {
            cvQuery.ErrorMessage = ex.Message;
            cvQuery.IsValid = false;
        }
    }

    protected void gvSanctionNumberQuery_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Header)  // if header - then set the style dynamically.
            {
                for (int i_cellVal = 0; i_cellVal < e.Row.Cells.Count; i_cellVal++)
                {
                    e.Row.Cells[i_cellVal].CssClass = "styleGridHeader";
                }
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink
                (this.gvSanctionNumberQuery, "Select$" + e.Row.RowIndex);

                e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
                e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            }
        }
        //catch (FaultException<CompanyMgtServicesReference.ClsPubFaultException> objFaultExp)
        //{            
        //    cvCreditParameterTrans.ErrorMessage = objFaultExp.Detail.ProReasonRW;
        //    cvCreditParameterTrans.IsValid = false;
        //}
        catch (Exception ex)
        {
            cvQuery.ErrorMessage = ex.Message;
            cvQuery.IsValid = false;
        }
    }

    protected void gvSanctionNumberQuery_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            GetSanctionNumberDetails(Convert.ToInt32(gvSanctionNumberQuery.SelectedRow.Cells[0].Text));
        }
        //catch (FaultException<CompanyMgtServicesReference.ClsPubFaultException> objFaultExp)
        //{            
        //    cvCreditParameterTrans.ErrorMessage = objFaultExp.Detail.ProReasonRW;
        //    cvCreditParameterTrans.IsValid = false;
        //}
        catch (Exception ex)
        {
            cvQuery.ErrorMessage = ex.Message;
            cvQuery.IsValid = false;
        }
    }

    protected void ddlOptions_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlOptions.SelectedValue.ToString() == "1")
            {
                trSanctionNumber.Visible = true;
                txtSanctionNumber.Text = string.Empty;

                trEndDate.Visible = false;
                trStartdate.Visible = false;
                txtStartDate.Text = DateTime.Now.ToString(_DateFormat);
                txtEndDate.Text = DateTime.Now.ToString(_DateFormat);

            }
            else if (ddlOptions.SelectedValue.ToString() == "2")
            {
                txtSanctionNumber.Text = "0";
                trSanctionNumber.Visible = false;

                trEndDate.Visible = true;
                trStartdate.Visible = true;
                txtStartDate.Text = string.Empty;
                txtEndDate.Text = string.Empty;
            }

        }
        //catch (FaultException<CompanyMgtServicesReference.ClsPubFaultException> objFaultExp)
        //{            
        //    cvCreditParameterTrans.ErrorMessage = objFaultExp.Detail.ProReasonRW;
        //    cvCreditParameterTrans.IsValid = false;
        //}
        catch (Exception ex)
        {
            throw ex;
        }

    }

    #endregion

    #region Page method

    private string ConvertToSQLFormat(string strDate)
    {
        try
        {
            CultureInfo myDTFI = new CultureInfo("en-GB", true);
            DateTimeFormatInfo DTF = myDTFI.DateTimeFormat;
            DTF.ShortDatePattern = _DateFormat;
            DateTime _Date = new DateTime();
            if (strDate != "")
            {
                _Date = System.Convert.ToDateTime(strDate, DTF);
                return _Date.ToString("yyyy/MM/dd");
            }
            else
                return string.Empty;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private bool CompareDates(string startDate, string endDate)
    {
        try
        {
            CultureInfo myDTFI = new CultureInfo("en-GB", true);
            DateTimeFormatInfo DTF = myDTFI.DateTimeFormat;
            DTF.ShortDatePattern = _DateFormat;
            DateTime strDT = new DateTime();
            DateTime endDT = new DateTime();
            if (startDate != "")
            {
                strDT = System.Convert.ToDateTime(startDate, DTF);
            }
            if (endDate != "")
            {
                endDT = System.Convert.ToDateTime(endDate, DTF);
            }
            if (strDT > endDT)
                return false;
            else
                return true;

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private bool CheckSystemDate(string strDate)
    {
        try
        {
            CultureInfo myDTFI = new CultureInfo("en-GB", true);
            DateTimeFormatInfo DTF = myDTFI.DateTimeFormat;
            DTF.ShortDatePattern = _DateFormat;
            DateTime _date = new DateTime();

            if (strDate != "")
                _date = System.Convert.ToDateTime(strDate, DTF);

            if (_date > DateTime.Now.Date)
                return false;
            else
                return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void FillDDL()
    {
        OrgMasterMgtServicesReference.OrgMasterMgtServicesClient ObjCustomerService = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
        try
        {
            S3GBusEntity.S3G_Status_Parameters ObjStatus = new S3G_Status_Parameters();
            ObjStatus.Option = 15;
            ObjStatus.Param1 = _CompanyID.ToString();
            Utility.FillDLL(ddlSanctionNumberbatch, ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus), false);

            if (ddlSanctionNumberbatch.Items.Count > 0)
                ddlSanctionNumberbatch.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            ObjCustomerService.Close();
        }
    }

    private void LoadSanctionNumberQry()
    {
        OrgMasterMgtServicesReference.OrgMasterMgtServicesClient ObjCustomerService = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
        try
        {
            S3GBusEntity.S3G_Status_Parameters ObjStatus = new S3G_Status_Parameters();
            ObjStatus.Option = 14;

            ObjStatus.Param1 = _CompanyID.ToString();

            if (ddlOptions.SelectedValue.ToString() == "1")
            {
                if (ddlSanctionNumberbatch.Items.Count == 0)
                    throw new ApplicationException("Not defined Batch for sanction No");

                ObjStatus.Param2 = txtSanctionNumber.Text.Trim();
            }
            else
            {
                ObjStatus.Param3 = ConvertToSQLFormat(txtStartDate.Text.Trim());
                ObjStatus.Param4 = ConvertToSQLFormat(txtEndDate.Text.Trim());
            }

            gvSanctionNumberQuery.DataSource = ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus);
            gvSanctionNumberQuery.DataBind();

            if (gvSanctionNumberQuery.Rows.Count == 0)
                cvQuery.ErrorMessage = Resources.LocalizationResources.NoRecordFound;
            cvQuery.IsValid = false;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            ObjCustomerService.Close();
        }
    }

    private void GetSanctionNumberDetails(Int32 iID)
    {
        DataTable ObjDtbl = new DataTable();
        try
        {
            TCEnqu.ActiveTabIndex = 1;
            DataSet DetailsDS = new DataSet();
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Option", "306");
            Procparam.Add("@Param1", intCreditParamAppID.ToString());
            DetailsDS = Utility.GetDataset("S3G_ORG_GetCustomerLookUp", Procparam);
            if (DetailsDS.Tables[0].Rows.Count > 0)
            {
                DataTable dtHeader = DetailsDS.Tables[0];
                dtHeader.Columns.Add("Company_Name", typeof(string));
                foreach (DataRow dRow in dtHeader.Rows)
                {
                    dRow["SanctionDate"] =(dRow["SanctionDate"].ToString());
                    dRow["EnquiryDate"] = (dRow["EnquiryDate"].ToString());
                    dRow["Approval_Date"] = (dRow["Approval_Date"].ToString());
                    dRow["Company_Name"] = ObjUserInfo.ProCompanyNameRW;
                }
                ViewState["Header"] = dtHeader;

                string QueryType = DetailsDS.Tables[0].Rows[0]["QueryType"].ToString();
                if (QueryType == "1")
                {
                    pnlEnquirylevel.Visible = true;
                    FunPriInitDetailsForEnquiry(DetailsDS.Tables[0]);
                }
                else
                {
                    pnlCustomerLevel.Visible = true;
                    FunPriInitDetailsForCusotmer(DetailsDS.Tables[0]);
                    grvLOBDetails.DataSource = DetailsDS.Tables[1];
                    grvLOBDetails.DataBind();
                }
            }

            //<<Performace>>

            //Procparam = new Dictionary<string, string>();
            //DataTable Dtbl = new DataTable();
            //Procparam.Add("@Option", "305");
            //Procparam.Add("@Param1", intCreditParamAppID.ToString());
            //Dtbl = Utility.GetDefaultData("S3G_ORG_GetCustomerLookUp", Procparam);
            //if (Dtbl.Rows.Count > 0)
            //{
            //intCreditParamTransID = Convert.ToInt32(Dtbl.Rows[0]["CreditParamTrans_ID"].ToString());


            if (DetailsDS.Tables[2].Rows.Count > 0)
                DisplayScoreBoardGrid(DetailsDS.Tables[2]);
            //}
        }
        catch (Exception ex)
        {
            //if (ObjDtbl == null || ObjDtbl.Rows.Count == 0)
            //{
            //    Utility.FunShowAlertMsg(this, "There is no releated record found in Credit Parameter Approval Details");
            //}
            throw ex;
        }

    }

    private void FunPriEnableButtons(bool nextBtn, bool prevBtn)
    {
        //btnNext.Enabled = nextBtn;
        //btnPrevious.Enabled = prevBtn;
    }

    /// <summary>
    /// Will initalize the Sanction Number details - UI
    /// </summary>
    private void FunPriInitDetailsForEnquiry(DataTable ObjDtbl)
    {
        try
        {
            txtApprovalFor.Text = Convert.ToString(ObjDtbl.Rows[0]["ApproveFor"]);
            txtSanctionNumberFld.Text = Convert.ToString(ObjDtbl.Rows[0]["SantionNumber"]);
            if (!(string.IsNullOrEmpty(ObjDtbl.Rows[0]["SanctionDate"].ToString())))
            {
                txtSanctionDate.Text = (ObjDtbl.Rows[0]["SanctionDate"].ToString());
            }
            txtEnquiryno.Text = Convert.ToString(ObjDtbl.Rows[0]["EnquiryNumber"]);
            txtEnquiryDate.Text = (ObjDtbl.Rows[0]["EnquiryDate"].ToString());
            txtLOB.Text = Convert.ToString(ObjDtbl.Rows[0]["LOB"]);
            txtBranch.Text = Convert.ToString(ObjDtbl.Rows[0]["Location_Code"]);
            txtProduct.Text = Convert.ToString(ObjDtbl.Rows[0]["Product_Code"]);
            txtCustomerCode.Text = Convert.ToString(ObjDtbl.Rows[0]["CustomerCode"]);
            txtCustomerName.Text = Convert.ToString(ObjDtbl.Rows[0]["CustomerName"]);
            txtAssetName.Text = Convert.ToString(ObjDtbl.Rows[0]["AssetName"]);
            txtAssetValue.Text = Convert.ToString(ObjDtbl.Rows[0]["AssetValue"]);
            txtRequiredFacility.Text = Convert.ToString(ObjDtbl.Rows[0]["Requiredfacility"]);
            txtSanctionedLimit.Text = Convert.ToString(ObjDtbl.Rows[0]["SanctionedAmount"]);
            txtOfferCard.Text = Convert.ToString(ObjDtbl.Rows[0]["OfferCard"]);
            txtApprovalSerialNo.Text = Convert.ToString(ObjDtbl.Rows[0]["ApprovalSerialNo"]);
            txtEmployeeName.Text = Convert.ToString(ObjDtbl.Rows[0]["UserName"]);
            txtApprovalStatus.Text = Convert.ToString(ObjDtbl.Rows[0]["ApprovalStatus"]);
            txtApprovalDate.Text = (ObjDtbl.Rows[0]["Approval_Date"].ToString());// Convert.ToDateTime(ObjDtbl.Rows[0]["ApprovalDate"]).ToString(strDateFormat);
            txtRemark.Text = Convert.ToString(ObjDtbl.Rows[0]["Remarks"]);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void FunPriInitDetailsForCusotmer(DataTable ObjDtbl)
    {
        try
        {
            txtApprovalFor1.Text = Convert.ToString(ObjDtbl.Rows[0]["ApproveFor"]);
            txtSanctionNumber1.Text = Convert.ToString(ObjDtbl.Rows[0]["SantionNumber"]);
            if (!(string.IsNullOrEmpty(ObjDtbl.Rows[0]["SanctionDate"].ToString())))
            {
                txtSanctionDate1.Text = Convert.ToDateTime(ObjDtbl.Rows[0]["SanctionDate"].ToString()).ToString(strDateFormat);
            }
            txtCustomerCode1.Text = Convert.ToString(ObjDtbl.Rows[0]["CustomerCode"]);
            txtCustomerName1.Text = Convert.ToString(ObjDtbl.Rows[0]["CustomerName"]);
            txtLOB1.Text = Convert.ToString(ObjDtbl.Rows[0]["LOB"]);
            txtRequiredFacility1.Text = Convert.ToString(ObjDtbl.Rows[0]["Requiredfacility"]);
            txtRequiredFacility.Text = Convert.ToString(ObjDtbl.Rows[0]["Requiredfacility"]);
            txtSanctionedLimit1.Text = Convert.ToString(ObjDtbl.Rows[0]["Sanctionedlimit"]);
            txtOfferCard1.Text = Convert.ToString(ObjDtbl.Rows[0]["OfferCard"]);
            txtOverRide1.Text = Convert.ToString(ObjDtbl.Rows[0]["Override"]);
            txtEmployee1.Text = Convert.ToString(ObjDtbl.Rows[0]["UserName"]);
            txtFinalSanctionedLimit1.Text = Convert.ToString(ObjDtbl.Rows[0]["Final_Sanctioned_Limit"]);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void FunPriVisibleControls(TextBox txt1, Label lbl1)
    {
        try
        {
            if (lbl1 != null && txt1 != null)
            {
                if (string.IsNullOrEmpty(txt1.Text))
                {
                    lbl1.Enabled =
                    txt1.Enabled = false;

                }
                else
                {
                    lbl1.Enabled =
                    txt1.Enabled = true;
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void DisplayScoreBoardGrid(DataTable dtScoreBoard)
    {
        //CreditMgtServicesReference.CreditMgtServicesClient ObjMgtCreditMgtClient = null;
        try
        {
            //DataTable dtScoreBoard = new DataTable();

            //Procparam = new Dictionary<string, string>();
            //Procparam.Add("@CreditParamTrans_ID", intCreditParamTransID.ToString());
            //dtScoreBoard = Utility.GetDefaultData("S3G_ORG_GetCreditParameterApproval_ScoreBoard", Procparam);

            ViewState["GridDetails"] = dtScoreBoard = FunPriGetSoreBoardSum(dtScoreBoard);
            gvCreditScore.DataSource = dtScoreBoard;
            gvCreditScore.DataBind();

            gvCreditScore.Rows[dtScoreBoard.Rows.Count - 1].Cells[0].Font.Bold = true;
            gvCreditScore.Rows[dtScoreBoard.Rows.Count - 1].Cells[1].Font.Bold = true;
            gvCreditScore.Rows[dtScoreBoard.Rows.Count - 1].Cells[2].Font.Bold = true;
            gvCreditScore.Rows[dtScoreBoard.Rows.Count - 1].Cells[3].Font.Bold = true;

            gvCreditScore.Rows[dtScoreBoard.Rows.Count - 1].Cells[0].CssClass = "styleGridHeader";
            gvCreditScore.Rows[dtScoreBoard.Rows.Count - 1].Cells[1].CssClass = "styleGridHeader";
            gvCreditScore.Rows[dtScoreBoard.Rows.Count - 1].Cells[2].CssClass = "styleGridHeader";
            gvCreditScore.Rows[dtScoreBoard.Rows.Count - 1].Cells[3].CssClass = "styleGridHeader";
        }
        finally
        {
            //ObjMgtCreditMgtClient.Close();  // closing the WCF connection
        }

    }

    private DataTable FunPriGetSoreBoardSum(DataTable dt_ScoreBoard)
    {
        try
        {
            //NumberStyles style;
            //CultureInfo culture;

            //style = NumberStyles.Number | NumberStyles.AllowCurrencySymbol;
            //culture = CultureInfo.CreateSpecificCulture("en-US");
            //decimal number;



            if (dt_ScoreBoard != null && dt_ScoreBoard.Rows.Count > 0)
            {
                int count = 0;
                DataRow dr_ScoreBoardTotal = dt_ScoreBoard.NewRow();
                dr_ScoreBoardTotal["CreditScore"] = "Total % / Total Score";

                dr_ScoreBoardTotal["PercentageOfImportance"] = Convert.ToDouble("0.00");
                dr_ScoreBoardTotal["RequiredScore"] = Convert.ToDouble("0.00");
                dr_ScoreBoardTotal["ActualScore"] = Convert.ToDouble("0.00");

                double PercentageOfImportance = 0.00;
                double RequiredScore = 0.00;
                double ActualScore = 0.00;
                int intPerofImpLength = 0;
                int intActualScrLength = 0;
                for (int i_ScoreBoardRow = 0; i_ScoreBoardRow < dt_ScoreBoard.Rows.Count; i_ScoreBoardRow++)
                {
                    if (!dt_ScoreBoard.Rows[i_ScoreBoardRow]["CreditScore"].ToString().ToUpper().Contains("HYGIENE"))
                    {
                        ++count;
                        PercentageOfImportance =
                            PercentageOfImportance +
                            Convert.ToDouble(dt_ScoreBoard.Rows[i_ScoreBoardRow]["PercentageOfImportance"]);

                        RequiredScore =
                            RequiredScore +
                            Convert.ToDouble(dt_ScoreBoard.Rows[i_ScoreBoardRow]["RequiredScore"]);


                    }
                    ActualScore =
                                            ActualScore +
                                            Convert.ToDouble(dt_ScoreBoard.Rows[i_ScoreBoardRow]["ActualScore"]);
                }

                //intPerofImpLength = dt_ScoreBoard.Rows[0]["PercentageOfImportance"].ToString().Substring(dt_ScoreBoard.Rows[0]["PercentageOfImportance"].ToString().IndexOf(".") + 1).Length;
                //intActualScrLength = dt_ScoreBoard.Rows[0]["ActualScore"].ToString().Substring(dt_ScoreBoard.Rows[0]["ActualScore"].ToString().IndexOf(".") + 1).Length;

                dr_ScoreBoardTotal["PercentageOfImportance"] = PercentageOfImportance;//CheckForDecimalCount(PercentageOfImportance, intPerofImpLength);
                dr_ScoreBoardTotal["RequiredScore"] = RequiredScore;
                dr_ScoreBoardTotal["ActualScore"] = ActualScore;//CheckForDecimalCount(ActualScore, intPerofImpLength);

                // initializing for calculation
                //if ((string.Compare(_Mode, "Cus")) != 0)    // Enquiry Mode
                //{

                _IndicativeExposure = Convert.ToDouble(txtRequiredFacility.Text);

                _PercentageOfImportance = Convert.ToDouble(Convert.ToDouble(dr_ScoreBoardTotal["PercentageOfImportance"].ToString()));
                _Guidelinescore = Convert.ToDouble(Convert.ToDouble(dr_ScoreBoardTotal["RequiredScore"].ToString()));
                _ActualScore = Convert.ToDouble(Convert.ToDouble(dr_ScoreBoardTotal["ActualScore"].ToString()));
                _Negative_Deviation = (_Guidelinescore - _ActualScore);
                _Negative_Deviation_Percent = ((_Negative_Deviation / _Guidelinescore) * 100.00);
                _GlobalParameterNegativeDeviationPercent = Convert.ToDouble(Convert.ToDouble(dt_ScoreBoard.Rows[0]["Negative_Deviation"]));
                _GlobalParameterExposureVariancePercent = Convert.ToDouble(Convert.ToDouble(dt_ScoreBoard.Rows[0]["Exposure_Variance"]));
                _Exposure_Percent = ((_GlobalParameterExposureVariancePercent * _Negative_Deviation_Percent) / (_GlobalParameterNegativeDeviationPercent));
                _ExposureVarianceAmount = _IndicativeExposure * _Exposure_Percent;
                _RevisedLimitForTheCustomer = (_IndicativeExposure - _ExposureVarianceAmount);

                //txtSanctionedLimit.Text = _RevisedLimitForTheCustomer.ToString();
                if ((_Negative_Deviation > _GlobalParameterNegativeDeviationPercent)
                    &&
                    (_Exposure_Percent > _GlobalParameterExposureVariancePercent))
                {
                    txtOfferCard.Text = "Disable";
                    txtOfferCard1.Text = "Disable";
                }
                else
                {
                    txtOfferCard.Text = "Enable";
                    txtOfferCard1.Text = "Enable";
                }

                //}

                dt_ScoreBoard.Rows.Add(dr_ScoreBoardTotal);
            }

            if (dt_ScoreBoard != null && dt_ScoreBoard.Rows.Count == 0) // if no records found
            {
                DataRow dr = dt_ScoreBoard.NewRow();
                dt_ScoreBoard.Rows.Add(dr);
            }

            //}
            return dt_ScoreBoard;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private string CheckForDecimalCount(double Value, int SuffixLen)
    {
        try
        {
            string RetValue = Convert.ToString(Value);
            if (RetValue.Contains("."))
            {
                string Per = RetValue.Substring(RetValue.IndexOf(".") + 1);
                int NoOfDec = Per.Length;
                int RemDec = SuffixLen - NoOfDec;
                if (RemDec > 0)
                {
                    for (int i = 0; i <= RemDec - 1; i++)
                    {
                        RetValue = RetValue + "0";
                    }
                }
            }
            else
            {
                RetValue = RetValue + ".";
                for (int i = 0; i <= SuffixLen - 1; i++)
                {
                    RetValue = RetValue + "0";
                }
            }

            return RetValue;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void Load_CreditScoreDetailsToGrid(string CustomerID)
    {
        OrgMasterMgtServicesReference.OrgMasterMgtServicesClient ObjCustomerService = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
        int exceptionno = 0;
        try
        {
            //CreditMgtServicesReference.CreditMgtServicesClient ObjMgtCreditMgtClient;
            //ObjCreditParameterApprovalScoreBoardDataTable.AddS3G_ORG_GetCreditParameterApproval_ScoreBoardRow(ObjCreditParameterApprovalScoreBoardRow);
            //ObjMgtCreditMgtClient = new CreditMgtServicesReference.CreditMgtServicesClient();
            //SerializationMode SerMode = new SerializationMode();

            //byte[] bytesCreditParameterTransaction = ObjMgtCreditMgtClient.FunPubQueryCreditParameterApproval_ScoreBoard(SerMode, ClsPubSerialize.Serialize(ObjCreditParameterApprovalScoreBoardDataTable, SerMode));
            //dtScoreBoard = (CreditMgtServices.S3G_ORG_GetCreditParameterApproval_ScoreBoardDataTable)ClsPubSerialize.DeSerialize(bytesCreditParameterTransaction, SerializationMode.Binary, typeof(CreditMgtServices.S3G_ORG_CreditParamTransactionDataTable));
            S3GBusEntity.S3G_Status_Parameters ObjStatus = new S3G_Status_Parameters();
            ObjStatus.Option = 25;
            ObjStatus.Param1 = CustomerID;
            DataTable dtCreditScore = ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus);
            if (dtCreditScore == null || dtCreditScore.Rows.Count == 0)
            {
                exceptionno = 1;
                DataRow dr = dtCreditScore.NewRow();
                dtCreditScore.Rows.Add(dr);

            }
            gvCreditScore.DataSource = dtCreditScore;
            //gvCreditScore.Rows[0].Visible = false;
            gvCreditScore.DataBind();
        }
        catch (Exception ex)
        {
            if (exceptionno == 1)
            {
                Utility.FunShowAlertMsg(this, "There is no Credit Score Item was found for this Sanction Number");
            }
            throw ex;
        }
        finally
        {
            ObjCustomerService.Close();
        }
    }

    #endregion

    #region Assign Resource settings

    private void SetResourceSettings()
    {
        try
        {

            //lblHeading.Text = Resources.LocalizationResources.SanctionNoQry_lblHeading;
            lblOption.Text = Resources.LocalizationResources.SanctionNoQry_lblOption;
            lblSanctionNumbers.Text = Resources.LocalizationResources.SanctionNoQry_lblSanctionNumbers;
            lblStartDate.Text = Resources.LocalizationResources.SanctionNoQry_lblStartDate;
            lblEndDate.Text = Resources.LocalizationResources.SanctionNoQry_lblEndDate;
            btnShowDetails1.Text = Resources.LocalizationResources.SanctionNoQry_btnShowDetails1;
            lblApprovalFor.Text = Resources.LocalizationResources.SanctionNoQry_lblApprovalFor;
            lblSanctionNumber.Text = Resources.LocalizationResources.SanctionNoQry_lblSanctionNumber;
            lblSanctionDate.Text = Resources.LocalizationResources.SanctionNoQry_lblSanctionDate;
            lblCustomerCode.Text = Resources.LocalizationResources.SanctionNoQry_lblCustomerCode;
            lblCustomerName.Text = Resources.LocalizationResources.SanctionNoQry_lblCustomerName;
            lblLOB.Text = Resources.LocalizationResources.SanctionNoQry_lblLOB;
            lblRequiredFacility.Text = Resources.LocalizationResources.SanctionNoQry_lblRequiredFacility;
            lblSanctionedLimit1.Text = Resources.LocalizationResources.SanctionNoQry_lblSanctionedLimit;
            lblOverRide1.Text = Resources.LocalizationResources.SanctionNoQry_lblOverride;
            lblEmployeeName.Text = Resources.LocalizationResources.SanctionNoQry_lblEmployeeName;
            lblFinalSanctionedLimit1.Text = Resources.LocalizationResources.SanctionNoQry_lblFinalSanctionedLimit;
            //lblCreditScore.Text = Resources.LocalizationResources.SanctionNoQry_lblCreditScore;
            //lblPercentage.Text = Resources.LocalizationResources.SanctionNoQry_lblPercentage;
            //lblRequiredScore.Text = Resources.LocalizationResources.SanctionNoQry_lblRequiredScore;
            //lblActualScore.Text = Resources.LocalizationResources.SanctionNoQry_lblActualScore;
            lblOfferCard.Text = Resources.LocalizationResources.SanctionNoQry_lblOfferCard;

            CalendarExtenderSD.Format = _DateFormat;
            CalendarExtenderED.Format = _DateFormat;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    #endregion


    private void FunPriSanctionNavigation(bool isPrevious)
    {
        try
        {
            if ((ViewState["SanctionNumberDetails"] != null) && (ViewState["SanctionDetailsPageNumber"] != null))
            {

                int currentPage = (int)ViewState["SanctionDetailsPageNumber"];
                DataTable ObjDtbl = new DataTable();


                ObjDtbl = (DataTable)ViewState["SanctionNumberDetails"];


                if (isPrevious)
                {
                    if (currentPage > 0)
                    {
                        ViewState["SanctionDetailsPageNumber"] = --currentPage;
                        //FunPriInitDetailsForEnquiry();
                    }
                }
                else
                {
                    if (currentPage < (ObjDtbl.Rows.Count - 1))
                    {
                        ViewState["SanctionDetailsPageNumber"] = ++currentPage;
                        //FunPriInitDetailsForEnquiry();
                    }
                }

                FunPriEnableButtons(((ObjDtbl.Rows.Count - 1) > currentPage) ? true : false, (currentPage > 0) ? true : false);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void btnPrevious_Click(object sender, EventArgs e)
    {
        try
        {
            FunPriSanctionNavigation(true);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void btnNext_Click(object sender, EventArgs e)
    {
        try
        {
            FunPriSanctionNavigation(false);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Origination/S3GORGTransLander.aspx?Code=SNQ&Create=0&Modify=0");
            //TCEnqu.ActiveTabIndex = 0;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {

        StringBuilder strHTML = new StringBuilder();
        string strnewFile = (Server.MapPath(".") + "\\PDF Files\\" + "SanctionNumberQuery");
        string strFileName = "/Origination/PDF Files/" + "SanctionNumberQuery";
        DataTable dtHeader = (DataTable)ViewState["Header"];
        //dtHeader.Columns.Add("Company_Name");
        //dtHeader.Columns[0]["Company_Name"] = ObjUserInfo.ProCompanyNameRW;
        //dtHeader.AcceptChanges();
        DataTable dtDetails = (DataTable)ViewState["GridDetails"];
        if (dtHeader.Rows.Count > 0)
        {
            //dtgridDate.Rows[dtgridDate.Rows.Count - 1].Delete();

            ReportDocument rptd = new ReportDocument();
            rptd.Load(Server.MapPath("SanctionNumberQuery.rpt"));
            rptd.SetDataSource(dtHeader);
            rptd.Subreports["details.rpt"].SetDataSource(dtDetails);
            DirectoryInfo df = new DirectoryInfo(Convert.ToString(Server.MapPath(".") + "\\PDF Files"));
            if (!df.Exists)
            {
                df.Create();
            }
            if (File.Exists(strnewFile) == true)
            {
                File.Delete(strnewFile);
            }

            rptd.ExportToDisk(ExportFormatType.PortableDocFormat, Server.MapPath(".") + "\\PDF Files\\" + "SanctionNumberQuery");
        }

        string strScipt = "window.open('../Common/S3GDownloadPage.aspx?qsFileName=" + strFileName + "', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
        ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strScipt, true);

    }
}
