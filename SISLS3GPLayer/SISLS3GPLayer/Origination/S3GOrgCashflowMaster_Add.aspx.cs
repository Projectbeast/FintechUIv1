#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Origination
/// Screen Name			: Cash Flow Details Creation
/// Created By			: Kannan RC
/// Created Date		: 17-July-2010
/// Purpose	            : 
/// Last Updated By		: Kannan RC
/// Last Updated Date   : 10-Aug-2010  
/// 
/// Last Updated By	    : Thangam M, Manikandan R, Saran M
/// Last Updated Date   : 10-09-2010
/// Reason              : Bug fixing for the follwing Test cases: 
///                        TC-086,TC-049,STD-10,14,18,20,22,27 ,TC_025
/// Last Updated By	    : Thalaiselvam N
/// Last Updated Date   : 23-09-2010
/// Reason              : User Mgmt Conversion

/// <Program Summary> 
#endregion


using System;
using System.Globalization;
using System.Resources;
using System.Collections.Generic;
using System.Web.UI;
using System.ServiceModel;
using System.Web.Security;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Text;
using S3GBusEntity.Origination;
using S3GBusEntity;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Web.Security;
using System.Configuration;
using System.util;

public partial class Origination_S3GOrgCashflowMaster_Add : ApplyThemeForProject
{
    #region Intialization
    Dictionary<string, string> dictParam = null;
    CashflowMgtServicesReference.CashflowMgtServicesClient objCashflow_MasterClient;
    int intErrCode = 0;
    int intCashfloId = 0;
    int intCashDebitId = 0;
    int intCashCreditId = 0;
    int intCashflowProgramID = 0;
    int CFSlNo = 0;
    string strMode;
    int intCompanyId;
    int intUserId;
    bool bCreate = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bClearList = false;
    bool bMakerChecker = false;
    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    string strRedirectPageView = "window.location.href='../Origination/S3GOrgCashflowMaster_View.aspx';";
    string strRedirectPageAdd = "window.location.href='../Origination/S3GOrgCashflowMaster_Add.aspx';";
    CompanyMgtServices.S3G_SYSAD_Branch_ListDataTable ObjS3G_BranchList = new CompanyMgtServices.S3G_SYSAD_Branch_ListDataTable();
    //CashflowMgtServices.S3G_ORG_CashflowFlagListDataTable ObjS3G_CashflowFlagList = new CashflowMgtServices.S3G_ORG_CashflowFlagListDataTable();
    CashflowMgtServices.S3G_ORG_GLSLCodeListDataTable ObjS3G_GLSLList = new CashflowMgtServices.S3G_ORG_GLSLCodeListDataTable();
    CashflowMgtServices.S3G_Status_LookUPDataTable ObjS3G_LookUpList = new CashflowMgtServices.S3G_Status_LookUPDataTable();
    CashflowMgtServices.S3G_ORG_GetProgramDataTable ObjS3G_GetProgramMaster = new CashflowMgtServices.S3G_ORG_GetProgramDataTable();
    CashflowMgtServices.S3G_ORG_GetCashflowDetailsDataTable ObjS3G_GetProgramDetails = new CashflowMgtServices.S3G_ORG_GetCashflowDetailsDataTable();

    CashflowMgtServices.S3G_ORG_GetCashflowMappingDataTable ObjS3G_GetProgramMapping = new CashflowMgtServices.S3G_ORG_GetCashflowMappingDataTable();
    CashflowMgtServices.S3G_ORG_GetCashflowMappingRow ObjCashflowMappingsRow;

    CashflowMgtServices.S3G_ORG_CashflowMasterDataTable ObjS3G_CashflowMaster = new CashflowMgtServices.S3G_ORG_CashflowMasterDataTable();
    CashflowMgtServices.S3G_ORG_CashflowMasterRow ObjCashflowMasterRow;

    CashflowMgtServices.S3G_ORG_CashFlowMasterProgramMappingDataTable ObjS3G_CashflowMapping = new CashflowMgtServices.S3G_ORG_CashFlowMasterProgramMappingDataTable();
    CashflowMgtServices.S3G_ORG_CashFlowMasterProgramMappingRow ObjCashflowMappingRow;

    CashflowMgtServices.S3G_ORG_DeleteCashFlowMasterDataTable ObjS3G_DeleteCashflow = new CashflowMgtServices.S3G_ORG_DeleteCashFlowMasterDataTable();
    CashflowMgtServices.S3G_ORG_DeleteCashFlowMasterRow ObjDeleteCashflowRow;

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        //ddlLOB.Focus();
        //if (Request.QueryString["cid"] != null)
        //    intCashfloId = Convert.ToInt32(Request.QueryString["cid"]);
        //if (Request.QueryString["did"] != null)
        //    intCashDebitId = Convert.ToInt32(Request.QueryString["did"]);
        //if (Request.QueryString["crid"] != null)
        //    intCashCreditId = Convert.ToInt32(Request.QueryString["crid"]);

        if (Request.QueryString["qsCashflowIds"] != null)
        {

            FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsCashflowIds"));

            strMode = Request.QueryString.Get("qsMode");
            if (fromTicket != null)
            {
                //string[] arrayIds = fromTicket.Name.Split(',');
                intCashfloId = Convert.ToInt32(fromTicket.Name);
                //intCashDebitId = Convert.ToInt32(arrayIds[1].ToString());
                //intCashCreditId = Convert.ToInt32(arrayIds[2].ToString());

            }
            else
            {
                strAlert = strAlert.Replace("__ALERT__", "Invalid URL");
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
            }
        }




        UserInfo ObjUserInfo = new UserInfo();
        intCompanyId = ObjUserInfo.ProCompanyIdRW;
        intUserId = ObjUserInfo.ProUserIdRW;
        bCreate = ObjUserInfo.ProCreateRW;
        bModify = ObjUserInfo.ProModifyRW;
        bQuery = ObjUserInfo.ProViewRW;
        FunProSetRowAttribute();
        if (!IsPostBack)
        {
            txtCashflowDesc.Attributes.Add("onblur", "verifyString('" + txtCashflowDesc.ClientID.ToString() + "')");
            bClearList = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ClearListValues"));
            if ((intCashfloId > 0) && (strMode == "M"))
            {
                FunPriDisableControls(1);
            }
            else if ((intCashfloId > 0) && (strMode == "Q"))
            {
                FunPriDisableControls(-1);
            }
            else
            {
                FunPriGetLOBList();
                FunPriGetCashflowflagList();
                FunPriGetCashflowList();
                FunPriGetRecoveryType();
                FunPriGetCashflowTypeList();
                FunPriGetProgramMaster();
                FunPriDisableControls(0);
            }
            ddlLOB.Focus();//Added by Suseela
        }
    }

    #region DDL
    private void FunPriGetLOBList()
    {
        dictParam = new Dictionary<string, string>();
        if (intCashfloId == 0)
        {
            dictParam.Add("@Is_Active", "1");
        }
        dictParam.Add("@User_Id", intUserId.ToString());
        dictParam.Add("@Company_ID", intCompanyId.ToString());
        dictParam.Add("@Program_ID", "30");

        ddlLOB.BindDataTable(SPNames.LOBMaster, dictParam, new string[] { "LOB_ID", "LOB_Name" });

    }
    protected void ddlLOB_SelectedIndexChanged(object sender, EventArgs e)
    {

        FunPriGetProgramMaster();

        if (Convert.ToInt32(ddlLOB.SelectedValue) > 0)
        {
            //FunPriGetAccountFlagList(Convert.ToInt32(ddlLOB.SelectedValue));
            FunPriSetGLCodeList(Convert.ToInt32(ddlLOB.SelectedValue));
            // Added By R. Manikandan
            // CR 32 for NCPM to Add Product in Cash flow Master
            //ddlProduct.SelectedValue = "0";
            FunPriLoadProduct();
            ddlCGLCode.Enabled = true;
            ddlCSLCode.Enabled = true;
            ddlCType.Enabled = true;
            ddlDGLCode.Enabled = true;
            ddlDSLCode.Enabled = true;
            ddlDType.Enabled = true;
            //rfvCGlcode.Visible = true;
            //rfvCSlcode.Visible = true;
            //rfvDGlcode.Visible = true;
            //rfvDSlcode.Visible = true;
            //rfvCType.Visible = true;
            //rfvDType.Visible = true;
            //if (ddlDAccountFlag.SelectedIndex != 0)
            //{
            //    ddlDGLCode.SelectedItem.Text = "--Select--";
            //    ddlDSLCode.SelectedItem.Text = "--Select--";

            //}
            ddlDType.SelectedIndex = 0;
            ddlCType.SelectedIndex = 0;
            ddlDSLCode.Items.Clear();
            ddlCSLCode.Items.Clear();
        }
        else
        {
            //ddlDAccountFlag.SelectedIndex = 0;
            //ddlCAccountFlag.SelectedIndex = 0;
            txtCashflowDesc.Text = "";
            ddlCashflow.SelectedIndex = 0;
            ddlCashflowFlag.SelectedIndex = 0;
            //if (Convert.ToInt32(ddlDAccountFlag.SelectedValue) == 0)
            //{
            //    //ddlDGLCode.SelectedIndex = -1;
            //    //ddlDSLCode.SelectedIndex = -1;                

            //    //ddlDGLCode.SelectedItem.Text = "--Select--";
            //    //ddlDSLCode.SelectedItem.Text = "--Select--";
            //    //ddlDSLCode.SelectedIndex = -1;    
            //    if (ddlCSLCode.Items.Count > 0)
            //        ddlCSLCode.SelectedItem.Text = "--Select--";
            //    if (ddlDSLCode.Items.Count > 0)
            //        ddlDSLCode.SelectedItem.Text = "--Select--";
            //    ddlDSLCode.Enabled = true;

            //    //ddlDGLCode.SelectedIndex = 0;
            //    //ddlDSLCode.SelectedIndex = 0;



            //}
            //if (Convert.ToInt32(ddlCAccountFlag.SelectedValue) == 0)
            //{
            //    //ddlCGLCode.SelectedIndex = -1;
            //    //ddlCSLCode.SelectedIndex = -1;
            //    if (Convert.ToInt32(ddlLOB.SelectedValue) == 0)
            //    {
            //        //ddlCGLCode.SelectedItem.Text = "--Select--";
            //        //ddlCSLCode.SelectedItem.Text = "--Select--";
            //        //ddlCGLCode.SelectedIndex = 0;
            //        //ddlCSLCode.SelectedIndex = 0;
            //        ddlCSLCode.SelectedIndex = -1;
            //    }
            //}
            ddlDType.SelectedIndex = 0;
            ddlCType.SelectedIndex = 0;


            CbxAccIRR.Checked = false;
            CbxAll.Checked = false;
            CbxBoth.Checked = false;
            CbxBusIRR.Checked = false;

            for (int i = 0; i < gvCashflowProgram.Rows.Count; i++)
            {
                if (gvCashflowProgram.Rows[i].RowType == DataControlRowType.DataRow)
                {
                    CheckBox Cbx1 = (CheckBox)gvCashflowProgram.Rows[i].FindControl("CbxPost");
                    Cbx1.Checked = false;
                }
            }
        }
        ddlLOB.Focus();//Added by Suseela
    }

    protected void FunProSetRowAttribute()
    {
        for (int i = 0; i < gvCashflowProgram.Rows.Count; i++)
        {
            if (gvCashflowProgram.Rows[i].RowType == DataControlRowType.DataRow)
            {
                CheckBox Cbx1 = (CheckBox)gvCashflowProgram.Rows[i].FindControl("CbxPost");
                Cbx1.Attributes.Add("onclick", "fnUnselectAllExpectSelected(this,'" + gvCashflowProgram.Rows[i].ClientID + "')");
                if (Cbx1.Checked)
                {
                    gvCashflowProgram.Rows[i].BackColor = System.Drawing.Color.FromName("#f5f8ff");
                }
            }
        }
    }

    //Debit : Bind DEBIT GLCODE
    private void FunPriGetGLList(int intlobID, int intAccountID)
    {
        //dictParam = new Dictionary<string, string>();
        //dictParam.Add("@LOB_ID", ddlLOB.SelectedValue);
        //dictParam.Add("@Company_ID", intCompanyId.ToString());
        ////dictParam.Add("@Account_Setup_ID", ddlDAccountFlag.SelectedValue);
        //ddlDGLCode.BindDataTable("S3G_ORG_GetGLSLCODEList", dictParam, new string[] { "Account_Setup_ID", "GL_Code" });
        //if (ddlDGLCode.Items.Count == 2)
        //{
        //    ddlDGLCode.SelectedValue = ddlDGLCode.Items[1].Value;
        //    ddlDGLCode.ClearDropDownList();
        //}

    }
    //CREDIT : Bind CREDIT GLCODE
    private void FunPriGetSLList(int intlobID, int intAccountID)
    {
        //dictParam = new Dictionary<string, string>();
        //dictParam.Add("@LOB_ID", ddlLOB.SelectedValue);
        //dictParam.Add("@Company_ID", intCompanyId.ToString());
        ////dictParam.Add("@Account_Setup_ID", ddlCAccountFlag.SelectedValue);
        //ddlCGLCode.BindDataTable("S3G_ORG_GetGLSLCODEList", dictParam, new string[] { "Account_Setup_ID", "GL_Code" });
        //if (ddlCGLCode.Items.Count == 2)
        //{
        //    ddlCGLCode.SelectedValue = ddlCGLCode.Items[1].Value;
        //    ddlCGLCode.ClearDropDownList();
        //}
    }

    //Debit : Bind DEBIT SLCODE
    private void FunPriGetGLCodeList(int intlobid, int intcompanyid, int intglid)
    {
        //dictParam = new Dictionary<string, string>();

        //DataRow DRow = FunProGetFilterDataRow(Convert.ToString(ddlDAccountFlag.SelectedValue));

        ////dictParam.Add("@LOB_ID", ddlLOB.SelectedValue);
        ////dictParam.Add("@Account_Setup_ID", ddlDGLCode.SelectedValue);

        //dictParam.Add("@Company_ID", intCompanyId.ToString());
        //dictParam.Add("@Account_Code_Desc_ID", Convert.ToString(DRow["Account_Code_Desc_ID"]));
        //dictParam.Add("@GL_Code", Convert.ToString(DRow["GL_Code"]));
        //dictParam.Add("@Account_Tab_ID", Convert.ToString(DRow["Account_Tab_ID"]));
        //if (PageMode != PageModes.Query)
        //{
        //    dictParam.Add("@Is_Active", "1");
        //}
        //ddlDSLCode.BindDataTable("S3G_ORG_GetGLCODEList", dictParam, new string[] { "Account_Setup_ID", "SL_Code" });
        //if (ddlDSLCode.Items.Count == 2 && ddlDType.SelectedValue == "1")
        //{
        //    ddlDSLCode.SelectedValue = ddlDSLCode.Items[1].Value;
        //    ddlDSLCode.ClearDropDownList();
        //    rfvDSlcode.Enabled = false;
        //}
        //else if (ddlDSLCode.Items.Count == 1 && ddlDType.SelectedValue == "1")
        //{
        //    rfvDSlcode.Enabled = false;
        //}
        //else if (ddlDSLCode.Items.Count != 1 && ddlDType.SelectedValue == "1")
        //{
        //    rfvDSlcode.Enabled = true;
        //}
    }
    //CREDIT : Bind CREDIT SLCODE
    private void FunPriGetSLCodeList(int intlobid, int intcompanyid, int intglid)
    {
        //dictParam = new Dictionary<string, string>();
        //dictParam.Add("@LOB_ID", ddlLOB.SelectedValue);
        //dictParam.Add("@Account_Setup_ID", ddlCGLCode.SelectedValue);

        //DataRow DRow = FunProGetFilterDataRow(Convert.ToString(ddlCAccountFlag.SelectedValue));

        //dictParam.Add("@Company_ID", intCompanyId.ToString());
        //if (DRow != null)
        //{
        //    dictParam.Add("@Account_Code_Desc_ID", Convert.ToString(DRow["Account_Code_Desc_ID"]));
        //    dictParam.Add("@GL_Code", Convert.ToString(DRow["GL_Code"]));
        //    dictParam.Add("@Account_Tab_ID", Convert.ToString(DRow["Account_Tab_ID"]));
        //}
        //if (PageMode != PageModes.Query)
        //{
        //    dictParam.Add("@Is_Active", "1");
        //}
        //ddlCSLCode.BindDataTable("S3G_ORG_GetGLCODEList", dictParam, new string[] { "Account_Setup_ID", "SL_Code" });
        ////if (ddlCSLCode.Items.Count == 2)
        ////{
        ////    ddlCSLCode.SelectedValue = ddlCSLCode.Items[1].Value;
        ////    ddlCSLCode.ClearDropDownList();
        ////}

        //if (ddlCSLCode.Items.Count == 2 && ddlCType.SelectedValue == "1")
        //{
        //    ddlCSLCode.SelectedValue = ddlCSLCode.Items[1].Value;
        //    ddlCSLCode.ClearDropDownList();
        //    rfvCSlcode.Enabled = false;
        //}
        //else if (ddlCSLCode.Items.Count == 1 && ddlCType.SelectedValue == "1")
        //{
        //    rfvCSlcode.Enabled = false;
        //}
        //else if (ddlCSLCode.Items.Count != 1 && ddlCType.SelectedValue == "1")
        //{
        //    rfvCSlcode.Enabled = true;
        //}
    }

    protected DataRow FunProGetFilterDataRow(string ddlValue)
    {
        try
        {
            DataTable DtAccountFlags = new DataTable();
            DtAccountFlags = (DataTable)ViewState["DtAccountFlags"];
            string filterExpression = "Account_Setup_ID = " + ddlValue;
            DataRow[] dtSuggestions = DtAccountFlags.Select(filterExpression);

            if (dtSuggestions.Length > 0)
            {
                return (DataRow)dtSuggestions.GetValue(0);
            }

            return null;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    private void FunPriSetGLCodeList(int intlobID)
    {
        try
        {
            dictParam = new Dictionary<string, string>();
            if (PageMode != PageModes.Query)
            {
                dictParam.Add("@Is_Active", "1");
            }
            dictParam.Add("@LOB_ID", ddlLOB.SelectedValue);
            dictParam.Add("@Company_ID", intCompanyId.ToString());
            dictParam.Add("@User_ID", intUserId.ToString());
            DataTable dtAccountCard = Utility.GetDefaultData("S3G_Get_GLSL_CodeLists", dictParam);
            ddlDGLCode.FillDataTable(dtAccountCard, "GL_Code", "Account_Code");
            ddlCGLCode.FillDataTable(dtAccountCard, "GL_Code", "Account_Code");
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, "Cashflow Master");
            throw ex;
        }
    }

    private void FunPriSetSLCodeList(DropDownList ddlGLCode, DropDownList ddlSLCode)
    {
        try
        {
            dictParam = new Dictionary<string, string>();
            if (PageMode != PageModes.Query)
            {
                dictParam.Add("@Is_Active", "1");
            }
            dictParam.Add("@LOB_ID", ddlLOB.SelectedValue);
            dictParam.Add("@Company_ID", intCompanyId.ToString());
            dictParam.Add("@User_ID", intUserId.ToString());
            dictParam.Add("@GL_Code", ddlGLCode.SelectedValue);
            ddlSLCode.BindDataTable("S3G_Get_GLSL_CodeLists", dictParam, new string[] { "SL_Code", "SL_Account_Code" });

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, "Cashflow Master");
            throw ex;
        }
    }

    private void FunPriGetAccountFlagListView(DropDownList ddl)
    {
        //dictParam = new Dictionary<string, string>();
        //if (PageMode != PageModes.Query)
        //{
        //    dictParam.Add("@Is_Active", "1");
        //}
        //dictParam.Add("@LOB_ID", ddlLOB.SelectedValue);
        //dictParam.Add("@Company_ID", intCompanyId.ToString());
        //dictParam.Add("@User_ID", intUserId.ToString());
        //ViewState["DtAccountFlags"] = Utility.GetDataset("S3G_ORG_GetAccountFlagListView", dictParam).Tables[0];
        //ddl.BindDataTable((DataTable)ViewState["DtAccountFlags"], new string[] { "Account_Setup_ID", "Account_Code" });
    }

    private void FunPriGetCreditAccountFlagList(int intlobID)
    {
        //dictParam = new Dictionary<string, string>();
        //if (PageMode != PageModes.Query)
        //{
        //    dictParam.Add("@Is_Active", "1");
        //}
        //dictParam.Add("@LOB_ID", ddlLOB.SelectedValue);
        //dictParam.Add("@Company_ID", intCompanyId.ToString());
        //dictParam.Add("@User_ID", intUserId.ToString());
        //ddlCAccountFlag.BindDataTable("S3G_ORG_GetCreditAccountFlagSLList", dictParam, new string[] { "Account_Setup_ID", "Account_Code" });
    }


    //modified based on the test case id(Tc_086)
    private void SetDDLFirstitem(DropDownList DDl, string seltext, string selvalue)
    {
        try
        {
            //for (int i = 0; i < DDl.Items.Count; i++)
            //{
            //    if (DDl.Items[i].Text == "Account")
            //    {
            //        DDl.Items.RemoveAt(i);
            //    }
            //    else if (DDl.Items[i].Text == "None")
            //    {
            //        DDl.Items.RemoveAt(i);
            //    }
            //}

            dictParam = new Dictionary<string, string>();
            dictParam.Add("@Option", "1");
            dictParam.Add("@Param1", "ORG_Cashflow");
            DDl.BindDataTable("S3G_ORG_GetCashflowLookUp", dictParam, new string[] { "value", "Name" });

            for (int i = 0; i < DDl.Items.Count; i++)
            {
                if (DDl.Items[i].Text == seltext)
                {
                    DDl.Items.RemoveAt(i);
                }
                //else if (DDl.Items[i].Text == "None")
                //{
                //    DDl.Items.RemoveAt(i);
                //}
            }
            DDl.SelectedValue = selvalue.ToString();

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    //protected void ddlDType_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    //ddlDAccountFlag.SelectedIndex = 0;
    //    try
    //    {
    //        //ddlDAccountFlag.SelectedIndex = -1;
    //        FunSetDSLEnable();
    //        ddlDType.Focus();//Added by Suseela
    //    }
    //    catch (Exception ex)
    //    {
    //        ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
    //    }
    //}

    protected void ddlDType_SelectedIndexChanged(object sender, EventArgs e)
    {
        //ddlDAccountFlag.SelectedIndex = 0;

        try
        {
            ddlDGLCode.ClearSelection();
            ddlDSLCode.Items.Clear();
            ddlDSLCode.Items.Insert(0, "-Select-");
            //ddlDGLCode.SelectedValue = "0";
            //ddlDGLCode.SelectedItem.Text = "--Select--";
            //ddlDSLCode.SelectedValue = "0";
            //ddlDSLCode.SelectedItem.Text = "--Select--";
            //ddlDSLCode.IsMandatory = false;
            if (Convert.ToInt32(ddlCashflowFlag.SelectedValue) <= 0)
            {
                Utility.FunShowAlertMsg(this, "Select the Cashflow");
                ddlDType.SelectedValue = "0";
                return;
            }

            if (Convert.ToInt32(ddlLOB.SelectedValue) <= 0)
            {
                Utility.FunShowAlertMsg(this, "Select the Line of Business");
                ddlDType.SelectedValue = "0";
                return;
            }
            if (ddlDType.SelectedValue == "3")
            {
                dictParam = new Dictionary<string, string>();
                DataSet dsflag = new DataSet();
                DataTable dtflag = new DataTable();
                DataTable dtflag_slcode = new DataTable();
                dictParam.Add("@Cashflow", ddlCashflowFlag.SelectedValue.ToString());
                dictParam.Add("@company_id", intCompanyId.ToString());
                dictParam.Add("@lob_id", ddlLOB.SelectedValue);
                dsflag = Utility.GetDataset("s3g_Org_GetGPS_GLSLCode", dictParam);
                dtflag = dsflag.Tables[0];
                dtflag_slcode = dsflag.Tables[1];
                if (dtflag.Rows.Count > 0)
                {
                    if (dtflag.Rows[0]["gl_code"].ToString() != null)
                    {
                        //ListItem lstitem = new ListItem(dtflag.Rows[0]["gl_desc"].ToString(), dtflag.Rows[0]["gl_code"].ToString());
                        //ddlDGLCode.Items.Add(lstitem);

                      

                        ddlDGLCode.SelectedValue = dtflag.Rows[0]["gl_code"].ToString();
                        ddlDGLCode.SelectedItem.Text = dtflag.Rows[0]["gl_desc"].ToString();

                        ddlDGLCode_SelectedIndexChanged(sender, e);

                    }
                    if (dtflag_slcode.Rows.Count > 0)
                    {
                        if (dtflag_slcode.Rows[0]["sl_code"].ToString() != null && dtflag_slcode.Rows[0]["sl_code"].ToString()!="")
                        {


                            ddlDSLCode.SelectedValue = dtflag_slcode.Rows[0]["sl_code"].ToString();
                            ddlDSLCode.SelectedItem.Text = dtflag_slcode.Rows[0]["sl_code"].ToString();
                        }
                    }
                    ddlDSLCode.Enabled = false;
                    ddlDGLCode.Enabled = false;
                    ddlDType.Focus();
                    // ddlDGLCode.Ismandatory = false;
                    //ddlDGLCode.IsMandatory = false;
                    //ddlDSLCode.IsMandatory = false;
                    //ddlCGLCode.IsMandatory = false;
                    //ddlCSLCode.IsMandatory = false;
                    //ddlDGLCode.IsMandatory = false;
                    //ddlDGLCode.IsMandatory = false;
                    //ddlDGLCode.ReadOnly = true;
                    //ddlDSLCode.ReadOnly = true;
                }
                else
                {
                    //ddlDGLCode.ReadOnly = true;
                    //ddlDSLCode.ReadOnly = true;
                    //ddlDGLCode.IsMandatory = true;


                    // ddlDSLCode.IsMandatory = true;

                    Utility.FunShowAlertMsg(this.Page, "Define GL Code in GPS");
                    ddlDType.SelectedValue = "0";
                    return;
                }

            }

            else
            {
                //ddlDGLCode.ReadOnly = false;
                //ddlDSLCode.ReadOnly = false;
                //ListItem liSelect = new ListItem("--Select--", "0");
                //ddlDSLCode.Items.Insert(0, liSelect);
                
                ddlDSLCode.Enabled = true;
                ddlDGLCode.Enabled = true;
                //ddlDSLCode.ClearSelection();
                //ListItem liSelect = new ListItem("-Select-", "0");
                //ddlDSLCode.Items.Insert(0, liSelect);
            }
            //ddlDAccountFlag.SelectedIndex = -1;
            FunSetDSLEnable();
        }

        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    protected void FunSetDSLEnable()
    {
        //if (ddlDType.SelectedItem.Text.ToLower() == "account")
        //{
        //    //ddlDSLCode.SelectedValue = "0";
        //    //rfvDSlcode.Enabled = true;            
        //    //rfvDSlcode.Visible = true;
        //    //rfvDSlcode.Enabled = true;
        //    ddlDSLCode.Enabled = true;
        //}
        //else
        //{
        //    //rfvDSlcode.Enabled = false;
        //    //rfvDSlcode.Visible = false;
        //    ddlDSLCode.Enabled = false;
        //    ddlDSLCode.Items.Clear();
        //    ListItem liSelect = new ListItem("--Select--", "0");
        //    ddlDSLCode.Items.Insert(0, liSelect);
        //}
        ////SetDDLFirstitem(ddlCType, ddlDType.SelectedItem.Text, ddlCType.SelectedValue);
        //ddlDType.Focus();
    }

    //protected void ddlCType_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    //ddlCAccountFlag.SelectedIndex = 0;
    //    try
    //    {
    //        //ddlCAccountFlag.SelectedIndex = -1;
    //        FunSetCSLEnable();
    //    }
    //    catch (Exception ex)
    //    {
    //        ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
    //    }

    //}

    protected void ddlCType_SelectedIndexChanged(object sender, EventArgs e)
    {

        //ddlCAccountFlag.SelectedIndex = 0;
        try
        {
            ddlCGLCode.ClearSelection();
            ddlCSLCode.Items.Clear();
            ddlCSLCode.Items.Insert(0, "-Select-");
            //ListItem liSelect = new ListItem("--Select--", "0");
            //ddlCSLCode.Items.Insert(0, liSelect);
            //ddlCGLCode.SelectedValue = "0";
            //ddlCGLCode.SelectedItem.Text = "--Select--";
            //ddlCSLCode.SelectedValue = "0";
            //ddlCSLCode.SelectedItem.Text = "--Select--";
            //ddlCSLCode.IsMandatory = false;

            if (Convert.ToInt32(ddlCashflowFlag.SelectedValue) <= 0)
            {
                Utility.FunShowAlertMsg(this, "Select the Cashflow");
                ddlDType.SelectedValue = "0";
                return;
            }
            if (Convert.ToInt32(ddlLOB.SelectedValue) <= 0)
            {
                Utility.FunShowAlertMsg(this, "Select the Line of Business");
                ddlDType.SelectedValue = "0";
                return;
            }

            if (ddlCType.SelectedValue == "3")
            {
                dictParam = new Dictionary<string, string>();
                DataSet dsflag = new DataSet();
                DataTable dtflag = new DataTable();
                DataTable dtflag_slcode = new DataTable();
                dictParam.Add("@Cashflow", ddlCashflowFlag.SelectedValue.ToString());
                dictParam.Add("@company_id", intCompanyId.ToString());
                dictParam.Add("@lob_id", ddlLOB.SelectedValue);
                dsflag = Utility.GetDataset("s3g_Org_GetGPS_GLSLCode", dictParam);
                dtflag = dsflag.Tables[0];
                dtflag_slcode = dsflag.Tables[1];
                if (dtflag.Rows.Count > 0)
                {
                    if (dtflag.Rows[0]["gl_code"].ToString() != null)
                    {
                        ddlCGLCode.SelectedValue = dtflag.Rows[0]["gl_code"].ToString();
                        ddlCGLCode.SelectedItem.Text = dtflag.Rows[0]["gl_desc"].ToString();
                        ddlCGLCode_SelectedIndexChanged(sender, e);
                    }

                    if (dtflag_slcode.Rows.Count > 0)
                    {
                        if (dtflag_slcode.Rows[0]["sl_code"].ToString() != null && dtflag_slcode.Rows[0]["sl_code"].ToString()!="")
                        {
                            ddlCSLCode.SelectedValue = dtflag_slcode.Rows[0]["sl_code"].ToString();
                            ddlCSLCode.SelectedItem.Text = dtflag_slcode.Rows[0]["sl_code"].ToString();
                          
                            //ListItem lstitemddlCSLCode = new ListItem(dtflag_slcode.Rows[0]["sl_desc"].ToString(), dtflag_slcode.Rows[0]["sl_code"].ToString());
                            //ddlCSLCode.Items.Add(lstitemddlCSLCode);

                        }
                    }
                    //ddlCGLCode.IsMandatory = false;

                    //ddlCSLCode.IsMandatory = false;

                    //ddlCGLCode.ReadOnly = true;
                    //ddlCSLCode.ReadOnly = true;
                    ddlCType.Focus();
                    ddlCGLCode.Enabled = false;
                    ddlCSLCode.Enabled = false;


                }
                else
                {
                    //ddlCGLCode.ReadOnly = true;
                    //ddlCSLCode.ReadOnly = true;
                    //ddlCGLCode.IsMandatory = true;
                    // ddlCSLCode.IsMandatory = true;

                    Utility.FunShowAlertMsg(this.Page, "Define GL Code in GPS");
                    ddlDType.SelectedValue = "0";
                    return;
                }
            }
            else
            {
                //ddlCGLCode.IsMandatory = true;

                //ddlCSLCode.IsMandatory = true;

                //ddlCGLCode.ReadOnly = false;
                //ddlCSLCode.ReadOnly = false;

                ddlCGLCode.Enabled = true;
                ddlCSLCode.Enabled = true;
                ddlCSLCode.ClearSelection();
            }
            //ddlCAccountFlag.SelectedIndex = -1;
            FunSetCSLEnable();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }

    }

    protected void FunSetCSLEnable()
    {
        //if (ddlCType.SelectedItem.Text.ToLower() == "account")
        //{
        //    //ddlCSLCode.SelectedValue = "0";        
        //    //rfvCSlcode.Enabled = true;
        //    //rfvCSlcode.Visible = true;
        //    //ddlCSLCode.Enabled = false;
        //    ddlCSLCode.Enabled = true;

        //}
        //else
        //{
        //    //rfvCSlcode.Enabled = false;
        //    //rfvCSlcode.Visible = false;
        //    ddlCSLCode.Enabled = false;
        //    ddlCSLCode.Items.Clear();
        //    ListItem liSelect = new ListItem("--Select--", "0");
        //    ddlCSLCode.Items.Insert(0, liSelect);
        //}

        //// SetDDLFirstitem(ddlDType, ddlCType.SelectedItem.Text, ddlDType.SelectedValue);
        //ddlCType.Focus();
    }

    private void FunPriGetCashflowList()
    {
        dictParam = new Dictionary<string, string>();
        dictParam.Add("@Option", "2");
        dictParam.Add("@Param1", "ORG_CashflowType");
        ddlCashflow.BindDataTable("S3G_ORG_GetCashflowLookUp", dictParam, new string[] { "value", "Name" });//For Lookup reference
    }

    //addecd by saran on 30-Jun-2014 start
    private void FunPriGetRecoveryType()
    {
        dictParam = new Dictionary<string, string>();
        dictParam.Add("@Option", "2");
        dictParam.Add("@Param1", "Stagger Method");
        ddlRecoveryType.BindDataTable("S3G_ORG_GetCashflowLookUp", dictParam, false, new string[] { "value", "Name" });//For Lookup reference
        ddlRecoveryType.SelectedValue = "1";//default flat
    }
    //addecd by saran on 30-Jun-2014 end
    private void FunPriGetCashflowflagList()
    {
        dictParam = new Dictionary<string, string>();
        //dictParam.Add("@Company_ID", intCompanyId);
        ddlCashflowFlag.BindDataTable("S3G_ORG_GetCashFlowFlagList", dictParam, new string[] { "CashFlow_Flag_ID", "CashFlowFlag_Desc" });
    }
    private void FunPriGetCashflowTypeList()
    {
        dictParam = new Dictionary<string, string>();
        dictParam.Add("@Option", "1");
        dictParam.Add("@Param1", "ORG_Cashflow");

        //Changed By Thangam M on 18/Feb/2012 to Remove None option from DDL
        ddlCType.BindDataTable("S3G_ORG_GetCashflowLookUp", dictParam, new string[] { "value", "Name" });
        ddlCType.Items.Remove(ddlCType.Items.FindByValue("5"));

        ddlDType.BindDataTable("S3G_ORG_GetCashflowLookUp", dictParam, new string[] { "value", "Name" });
        ddlDType.Items.Remove(ddlDType.Items.FindByValue("5"));
    }
    //protected void CbxBoth_CheckedChanged(object sender, EventArgs e)
    //{

    //    if (CbxBoth.Checked)
    //    {
    //        CbxBusIRR.Checked = false;
    //        CbxAccIRR.Checked = false;
    //        if (strMode == "M")
    //        {
    //            CbxBusIRR.Checked = false;
    //            CbxAccIRR.Checked = false;
    //            CbxBusIRR.Enabled = true;
    //            CbxAccIRR.Enabled = true;
    //        }
    //    }
    //    else
    //    {
    //        CbxBusIRR.Enabled = true;
    //        CbxAccIRR.Enabled = true;
    //    }

    //}
    //protected void CbxBusIRR_CheckedChanged(object sender, EventArgs e)
    //{
    //    if (CbxBusIRR.Checked)
    //    {
    //        CbxBoth.Checked = false;
    //        CbxAccIRR.Checked = false;
    //        if (strMode == "M")
    //        {
    //            CbxBoth.Checked = false;
    //            CbxAccIRR.Checked = false;
    //            CbxBoth.Enabled = true;
    //            CbxAccIRR.Enabled = true;
    //        }
    //    }
    //    else
    //    {

    //        CbxBoth.Enabled = true;
    //        CbxAccIRR.Enabled = true;
    //    }
    //}
    //protected void CbxAccIRR_CheckedChanged(object sender, EventArgs e)
    //{
    //    if (CbxAccIRR.Checked)
    //    {
    //        CbxBoth.Checked = false;
    //        CbxBusIRR.Checked = false;
    //        if (strMode == "M")
    //        {
    //            CbxBoth.Checked = false;
    //            CbxBusIRR.Checked = false;
    //            CbxBoth.Enabled = true;
    //            CbxBusIRR.Enabled = true;
    //        }
    //    }
    //    else
    //    {
    //        CbxBoth.Enabled = true;
    //        CbxBusIRR.Enabled = true;
    //    }
    //}

    #endregion

    /// <summary>
    /// This methode used in validation in create and modify mode
    /// </summary>
    #region validation
    private void validation()
    {
        ddlCGLCode.Enabled = true;
        ddlCSLCode.Enabled = true;
        ddlCType.Enabled = true;
        ddlDGLCode.Enabled = true;
        ddlDSLCode.Enabled = true;
        ddlDType.Enabled = true;
        //rfvCGlcode.Visible = false;
        //rfvCSlcode.Visible = false;
        //rfvDGlcode.Visible = false;
        //rfvDSlcode.Visible = false;
        //rfvCType.Visible = true;
        //rfvDType.Visible = true;
        //btnClear.Enabled = true;
        btnClear.Enabled_True();



    }
    private void Mvalidation()
    {
        ddlLOB.Enabled = false;
        ddlCashflow.Enabled = false;
        ddlCashflowFlag.Enabled = false;
        txtCashflowNo.Enabled = false;
        // Changed by bhuvan
        txtCashflowDesc.Enabled = true;
        // end her
        //rfvCGlcode.Visible = true;
        //rfvCSlcode.Visible = true;
        //rfvDGlcode.Visible = true;
        //rfvDSlcode.Visible = true;
        //rfvCType.Visible = true;
        //rfvDType.Visible = true;
        ddlDSLCode.Enabled = true;
        //ddlCSLCode.Enabled = true;

    }
    #endregion

    /// <summary>
    /// This methode used in get Program in cashflow  details
    /// </summary>

    #region GetProgramNumber
    private void FunPriGetProgramMaster()
    {
        //objCashflow_MasterClient = new CashflowMgtServicesReference.CashflowMgtServicesClient();
        try
        {
            //Commanded and Added by Thangam M on 02/Nov/2011 to make filteration on Program List

            //CashflowMgtServices.S3G_ORG_GetProgramRow ObjProgramMasterRow;
            //ObjProgramMasterRow = ObjS3G_GetProgramMaster.NewS3G_ORG_GetProgramRow();
            ////ObjProgramMasterRow.Company_ID = intCompanyId;
            //ObjS3G_GetProgramMaster.AddS3G_ORG_GetProgramRow(ObjProgramMasterRow);



            //SerializationMode SerMode = SerializationMode.Binary;
            //byte[] byteProgramMasterDetails = objCashflow_MasterClient.FunPubGetProgramNumber(SerMode, ClsPubSerialize.Serialize(ObjS3G_GetProgramMaster, SerMode));
            //CashflowMgtServices.S3G_ORG_GetProgramDataTable dtProgramMaster = (CashflowMgtServices.S3G_ORG_GetProgramDataTable)ClsPubSerialize.DeSerialize(byteProgramMasterDetails, SerializationMode.Binary, typeof(CashflowMgtServices.S3G_ORG_GetProgramDataTable));

            if (ddlLOB.SelectedValue != "0" && ddlCashflowFlag.SelectedValue != "0" && ddlCashflow.SelectedValue != "0")
            {
                dictParam = new Dictionary<string, string>();
                dictParam.Add("@Company_ID", intCompanyId.ToString());
                dictParam.Add("@LOB_ID", ddlLOB.SelectedValue.ToString());
                dictParam.Add("@CashFlow_Flag_ID", ddlCashflowFlag.SelectedValue.ToString());
                dictParam.Add("@Flow_Type", ddlCashflow.SelectedItem.Text);
                if (ddlProduct.Items.Count > 0)
                {
                    if (Convert.ToInt32(ddlProduct.SelectedValue) > 0)
                    {
                        dictParam.Add("@Product_ID", ddlProduct.SelectedValue.ToString());
                    }
                }

                DataTable dtGrid = Utility.GetDefaultData("S3G_ORG_GetCashflowPrograms", dictParam);

                if (dtGrid.Rows.Count == 0)
                {
                    Utility.FunShowAlertMsg(this, "No Records Found");
                    //cvCashflow.ErrorMessage = "No Records Found";
                    //cvCashflow.IsValid = false;
                    return;
                }
                else
                {
                    gvCashflowProgram.DataSource = dtGrid;
                    gvCashflowProgram.DataBind();
                }
                //objCashflow_MasterClient.Close();
            }
            else
            {
                FunProResetGrid();
            }

            //End here
        }
        catch (FaultException<CompanyMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;

        }
        finally
        {
            //objCashflow_MasterClient.Close();
        }
    }
    #endregion


    /// <summary>
    /// This methode used in Create and Modify cashflow details
    /// </summary>
    #region Create Cashflow
    protected void btnSave_Click(object sender, EventArgs e)
    {
        objCashflow_MasterClient = new CashflowMgtServicesReference.CashflowMgtServicesClient();
        try
        {
            if (ddlDSLCode.Enabled && ddlDSLCode.Items.Count > 1)
            {
                rfvDSlcode.Enabled = true;
            }
            else
            {
                rfvDSlcode.Enabled = false;
            }

            if (ddlCSLCode.Enabled && ddlCSLCode.Items.Count > 1)
            {
                rfvCSlcode.Enabled = true;
            }
            else
            {
                rfvCSlcode.Enabled = false;
            }

            string strCash_No = string.Empty;
            string strKey = "Insert";
            string strAlert = "alert('__ALERT__');";
            string strRedirectPageView = "window.location.href='../Origination/S3GOrgCashflowMaster_View.aspx';";
            string strRedirectPageAdd = "window.location.href='../Origination/S3GOrgCashflowMaster_Add.aspx';";


            int counts = 0;
            foreach (GridViewRow grv in gvCashflowProgram.Rows)
            {
                if (
                (((CheckBox)grv.FindControl("CbxCapture")).Checked) ||
                (((CheckBox)grv.FindControl("CbxDisplay")).Checked) ||
                (((CheckBox)grv.FindControl("CbxPost")).Checked)
                )
                {
                    counts++;
                }


            }
            if (gvCashflowProgram.Rows.Count <= 1)
            {
                if (counts == 0)
                {
                    Utility.FunShowAlertMsg(this, "Select the program");
                    //cvCashflow.ErrorMessage = "Select the program";
                    //cvCashflow.IsValid = false;
                    return;
                }
            }
            else
            {
                if (counts == 0)
                {
                    Utility.FunShowAlertMsg(this, "Select at least one program");
                    //cvCashflow.ErrorMessage = "Select at least one program";
                    //cvCashflow.IsValid = false;
                    return;
                }
            }



            ObjCashflowMasterRow = ObjS3G_CashflowMaster.NewS3G_ORG_CashflowMasterRow();
            ObjCashflowMasterRow.CashFlow_ID = intCashfloId;
            ObjCashflowMasterRow.CashflowDebit_ID = intCashDebitId;
            ObjCashflowMasterRow.CashflowCredit_ID = intCashCreditId;
            if (intCashfloId == 0)
            {
                ObjCashflowMasterRow.CFSl_No = "0";
            }
            else
            {
                ObjCashflowMasterRow.CFSl_No = txtCashflowNo.Text;
            }
            ObjCashflowMasterRow.COMPANY_ID = Convert.ToInt32(intCompanyId);
            ObjCashflowMasterRow.LOB_ID = Convert.ToInt32(ddlLOB.SelectedValue);
            ObjCashflowMasterRow.CashFlow_Description = txtCashflowDesc.Text;
            ObjCashflowMasterRow.Flow_Type = ddlCashflow.SelectedItem.Text;
            ObjCashflowMasterRow.CASHFLOW_FLAG_ID = Convert.ToInt32(ddlCashflowFlag.SelectedValue);
            ObjCashflowMasterRow.Business_IRR = CbxBusIRR.Checked;
            ObjCashflowMasterRow.Accounting_IRR = CbxAccIRR.Checked;
            ObjCashflowMasterRow.Company_IRR = CbxBoth.Checked;
            //code added by saran on 30-Jun-2014 start
            ObjCashflowMasterRow.RecoveryType = ddlRecoveryType.SelectedValue;
            //code added by saran on 30-Jun-2014 end
            ObjCashflowMasterRow.Is_Active = CbxActive.Checked;
            if (CbxAll.Checked)
            {
                ObjCashflowMasterRow.Business_IRR = CbxBusIRR.Checked = true;
                ObjCashflowMasterRow.Accounting_IRR = CbxAccIRR.Checked = true;
                ObjCashflowMasterRow.Company_IRR = CbxBoth.Checked = true;

            }


            //Debit
            //if (ddlDSLCode.SelectedValue == "0")
            //{
            //    ObjCashflowMasterRow.Debit_Account_Setup_ID = Convert.ToInt32(ddlDAccountFlag.SelectedValue);
            //}
            //else
            //{
            //    ObjCashflowMasterRow.Debit_Account_Setup_ID = Convert.ToInt32(ddlDSLCode.SelectedValue);
            //}



            /*if (ddlDGLCode.SelectedIndex > 0)
                ObjCashflowMasterRow.DGL_ACCOUNT_CODE = ddlDGLCode.SelectedValue;
            if (ddlDSLCode.Items.Count > 0)
            {
                if (ddlDSLCode.SelectedIndex > 0)
                    ObjCashflowMasterRow.DSL_ACCOUNT_CODE = ddlDSLCode.SelectedValue;
            }
            if (ddlDType.SelectedIndex > 0)
                ObjCashflowMasterRow.Debit_Type_ID = Convert.ToInt32(ddlDType.SelectedValue);*/
            if (ddlDGLCode.SelectedValue != "0" && ddlDGLCode.SelectedValue.Trim() != "")
                ObjCashflowMasterRow.DGL_ACCOUNT_CODE = ddlDGLCode.SelectedValue;

            if (ddlDSLCode.SelectedValue != "0" && ddlDSLCode.SelectedValue.Trim() != "")
                ObjCashflowMasterRow.DSL_ACCOUNT_CODE = ddlDSLCode.SelectedValue;

            if (ddlDType.SelectedValue != "0" && ddlDType.SelectedValue.Trim() != "")
                ObjCashflowMasterRow.Debit_Type_ID = Convert.ToInt32(ddlDType.SelectedValue);


            //Credit
            //if (ddlCSLCode.SelectedValue == "0")
            //{
            //    ObjCashflowMasterRow.Credit_Account_Setup_ID = Convert.ToInt32(ddlCAccountFlag.SelectedValue);
            //}
            //else
            //{
            //    ObjCashflowMasterRow.Credit_Account_Setup_ID = Convert.ToInt32(ddlCSLCode.SelectedValue);
            //}



            /*if (ddlCGLCode.SelectedIndex > 0)
                ObjCashflowMasterRow.CGL_ACCOUNT_CODE = ddlCGLCode.SelectedValue;
            if (ddlCSLCode.Items.Count > 0)
            {
                if (ddlCSLCode.SelectedIndex > 0)
                    ObjCashflowMasterRow.CSL_ACCOUNT_CODE = ddlCSLCode.SelectedValue;
            }
            if (ddlCType.SelectedIndex > 0)
                ObjCashflowMasterRow.Credit_Type_ID = Convert.ToInt32(ddlCType.SelectedValue);*/

            if (ddlCGLCode.SelectedValue != "0" && ddlCGLCode.SelectedValue.Trim() != "")
                ObjCashflowMasterRow.CGL_ACCOUNT_CODE = ddlCGLCode.SelectedValue;
            if (ddlCSLCode.SelectedValue != "0" && ddlCSLCode.SelectedValue.Trim() != "")
                ObjCashflowMasterRow.CSL_ACCOUNT_CODE = ddlCSLCode.SelectedValue;
            if (ddlCType.SelectedValue != "0" && ddlCType.SelectedValue.Trim() != "")
                ObjCashflowMasterRow.Credit_Type_ID = Convert.ToInt32(ddlCType.SelectedValue);

            //if (ddlCType.SelectedItem.Text == "Account")
            //{
            //    //ddlCSLCode.SelectedValue = "0";
            //    //rfvCSlcode.Visible = true;
            //}
            //else
            //{
            //    ObjCashflowMasterRow.CSL_ACCOUNT_CODE = Convert.ToInt32(ddlCSLCode.SelectedValue);
            //}


            //if (ddlDType.SelectedItem.Text == "Account")
            //{
            //    //ddlDSLCode.SelectedValue = "0";
            //    //rfvDSlcode.Visible = true;
            //}
            //else
            //{
            //    ObjCashflowMasterRow.DSL_ACCOUNT_CODE = Convert.ToInt32(ddlDSLCode.SelectedValue);
            //}


            if (ddlDGLCode.SelectedValue == ddlCGLCode.SelectedValue)
            {
                Utility.FunShowAlertMsg(this, "Debit GL Account Code should not be same as Credit GL Account Code");
                //cvCashflow.ErrorMessage = "Debit GL Account Code should not be same as Credit GL Account Code";
                //cvCashflow.IsValid = false;
                return;
            }

            if (Convert.ToInt32(ddlCashflow.SelectedValue) < 4)//Other than NDE
            {
                if (ddlDType.SelectedItem.Text != "Account")
                {
                    if (ddlDType.SelectedValue == ddlCType.SelectedValue)
                    {
                        Utility.FunShowAlertMsg(this, "Debit Type should not be same as Credit Type");
                        //cvCashflow.ErrorMessage = "Debit Type should not be same as Credit Type";
                        //cvCashflow.IsValid = false;
                        return;
                    }
                }
                if (ddlCType.SelectedItem.Text != "Account")
                {
                    if (ddlDType.SelectedValue == ddlCType.SelectedValue)
                    {
                        Utility.FunShowAlertMsg(this, "Debit Type should not be same as Credit Type");
                        //cvCashflow.ErrorMessage = "Debit Type should not be same as Credit Type";
                        //cvCashflow.IsValid = false;
                        return;
                    }
                }
            }

            ObjCashflowMasterRow.CREATED_BY = Convert.ToInt32(intUserId);
            ObjCashflowMasterRow.CREATED_ON = DateTime.Now;
            ObjCashflowMasterRow.MODIFIED_BY = Convert.ToInt32(intUserId);
            ObjCashflowMasterRow.MODIFIED_ON = DateTime.Now;
            StringBuilder strProgramMap = new StringBuilder();
            strProgramMap.Append(FunProFormMLAXML());
            if (strProgramMap.Length == 0)
            {
                Utility.FunShowAlertMsg(this, "Scheme Cannot Mapped to Other than Income Recognition");
                return;
            }
            ObjCashflowMasterRow.XML_CASHDeltails = FunProFormMLAXML();
            //Code Added by saran 17-Oct-2013 for NDE implentation end
            // Added By R. Manikandan
            // CR for NCPM 
            // Added on 06 - JUL - 2015
            if (ddlProduct.Items.Count > 0)
            {
                ObjCashflowMasterRow.Product_Id = ddlProduct.SelectedValue;
            }



            ObjS3G_CashflowMaster.AddS3G_ORG_CashflowMasterRow(ObjCashflowMasterRow);

            SerializationMode SerMode = SerializationMode.Binary;
            byte[] ObjCashDataTable = ClsPubSerialize.Serialize(ObjS3G_CashflowMaster, SerMode);


            if (intCashfloId > 0)
            {
                intErrCode = objCashflow_MasterClient.FunPubModifyCashflowMaster(SerMode, ObjCashDataTable);
            }
            else
            {
                //intErrCode = objCashflow_MasterClient.FunPubCreateCashflowMaster(SerMode, ClsPubSerialize.Serialize(ObjS3G_CashflowMaster, SerMode, out strOffer_No));
                intErrCode = objCashflow_MasterClient.FunPubCreateCashflowMaster(out strCash_No, SerMode, ObjCashDataTable);

            }

            if (intErrCode == 0)
            {
                //Added by Thangam M on 18/Oct/2012 to avoid double save click
                //btnSave.Enabled = false;
                btnSave.Enabled_False();
                //End here

                if (intCashfloId > 0)
                {
                    strKey = "Modify";
                    strAlert = strAlert.Replace("__ALERT__", "Cash Flow details updated successfully");
                }
                else
                {
                    strAlert = "Cash Flow " + strCash_No + " details added successfully";
                    strAlert += @"\n\nWould you like to add one more record?";
                    strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                    strRedirectPageView = "";
                    ddlLOB.Focus();

                }


            }
            else if (intErrCode == -1)
            {
                if (intCashfloId == 0)
                {
                    strAlert = strAlert.Replace("__ALERT__", Resources.LocalizationResources.DocNoNotDefined);
                }
                strRedirectPageView = "";
            }
            else if (intErrCode == -2)
            {
                if (intCashfloId == 0)
                {
                    strAlert = strAlert.Replace("__ALERT__", Resources.LocalizationResources.DocNoExceeds);


                }
                strRedirectPageView = "";
            }
            else if (intErrCode == 1)
            {
                strAlert = strAlert.Replace("__ALERT__", "Cash Flow details already exists");
                strRedirectPageView = "";
            }

            else if (intErrCode == 2)
            {
                strAlert = strAlert.Replace("__ALERT__", "Cash Flow details already exists");
                strRedirectPageView = "";
            }

                //Added by Thangam M on 14/Feb/2012 to check the availability of Description
            else if (intErrCode == 5)
            {
                strAlert = strAlert.Replace("__ALERT__", "Cash Flow Description already exists for the selected Line of Business.");
                strRedirectPageView = "";
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
            lblErrorMessage.Text = string.Empty;
            //objCashflow_MasterClient.Close();
            btnSave.Focus();//Added by suseela
        }
        catch (FaultException<CompanyMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            objCashflow_MasterClient.Close();
        }
    }

    /// <summary>
    /// This methode used in XML Form in cashflow details
    /// </summary>
    protected string FunProFormMLAXML()
    {
        StringBuilder strbuXML = new StringBuilder();
        strbuXML.Append("<Root>");
        foreach (GridViewRow grvData in gvCashflowProgram.Rows)
        {

            string strlblPRTID = ((Label)grvData.FindControl("lblProgramid")).Text;
            string strCapture = Convert.ToString(((CheckBox)grvData.FindControl("CbxCapture")).Checked);
            string strDisplay = Convert.ToString(((CheckBox)grvData.FindControl("CbxDisplay")).Checked);
            string strPost = Convert.ToString(((CheckBox)grvData.FindControl("CbxPost")).Checked);
            //string strCreatdBy = Convert.ToString(intUserId);
            //string strCreatdOn = DateTime.Now;
            strbuXML.Append(" <Details  PROGRAM_ID='" + strlblPRTID + "' IS_CAPTURE='" + strCapture.ToString() + "' IS_DISPLAY='" + strDisplay.ToString() +
             "' IS_POST='" + strPost.ToString() + "'/>");
            //strCreatdBy='" + CREATED_BY.ToString() + "' strCreatdOn='" + CREATED_ON.ToString()

            if (strPost.ToUpper() == "TRUE")
            {
                if (strlblPRTID != "114" && Convert.ToInt32(ddlProduct.SelectedValue) > 0)
                {
                    strbuXML.Replace(strbuXML.ToString(), "");
                    return strbuXML.ToString();
                }
            }

        }
        strbuXML.Append("</Root>");
        return strbuXML.ToString();
    }

    /// <summary>
    /// This methode used in Clear the all enteralbe field
    /// </summary>
    protected void btnClear_Click(object sender, EventArgs e)
    {
        ddlCSLCode.Enabled = true;
        ddlDSLCode.Enabled = true;
        if (ddlLOB.Items.Count > 0)
        { ddlLOB.SelectedIndex = 0; }
        if (ddlCashflow.Items.Count > 0)
        { ddlCashflow.SelectedIndex = 0; }
        txtCashflowDesc.Text = "";
        if (ddlCashflowFlag.Items.Count > 0)
        { ddlCashflowFlag.SelectedIndex = 0; }
        //if (Convert.ToInt32(ddlLOB.SelectedValue) > 0)
        //{
        //    ddlCAccountFlag.SelectedIndex = 0;
        //    ddlDAccountFlag.SelectedIndex = 0;
        //}
        CbxAccIRR.Checked = false;
        CbxBoth.Checked = false;
        CbxBusIRR.Checked = false;
        CbxAll.Checked = false;

        //if (ddlLOB.SelectedValue == "0")
        //{
        //    //ddlCGLCode.SelectedIndex = -1;
        //    //ddlCSLCode.SelectedIndex = -1;
        //    //ddlDGLCode.SelectedIndex = -1;
        //    //ddlDSLCode.SelectedIndex = -1;
        //    if (ddlCAccountFlag.SelectedIndex != -1)
        //    {
        //        //ddlCGLCode.SelectedItem.Text = "--Select--";
        //        if (ddlCSLCode.Items.Count > 0)
        //            ddlCSLCode.SelectedItem.Text = "--Select--";
        //    }
        //    if (ddlDAccountFlag.SelectedIndex != -1)
        //    {
        //        //ddlDGLCode.SelectedItem.Text = "--Select--";
        //        if (ddlDSLCode.Items.Count > 0)
        //            ddlDSLCode.SelectedItem.Text = "--Select--";
        //    }
        //    ddlCAccountFlag.SelectedIndex = -1;
        //    ddlDAccountFlag.SelectedIndex = -1;
        //}

        if (ddlCType.Items.Count > 0)
            ddlCType.SelectedIndex = 0;
        if (ddlDType.Items.Count > 0)
            ddlDType.SelectedIndex = 0;
        if (ddlDGLCode.Items.Count > 0)
            ddlDGLCode.SelectedIndex = 0;
        if (ddlCGLCode.Items.Count > 0)
            ddlCGLCode.SelectedIndex = 0;
        if (ddlDSLCode.Items.Count > 0)
            ddlDSLCode.SelectedIndex = 0;
        if (ddlCSLCode.Items.Count > 0)
            ddlCSLCode.SelectedIndex = 0;

        ddlProduct.Items.Clear();

        FunProResetGrid();

        //for (int i = 0; i < gvCashflowProgram.Rows.Count; i++)
        //{
        //    if (gvCashflowProgram.Rows[i].RowType == DataControlRowType.DataRow)
        //    {
        //        CheckBox Cbx1 = (CheckBox)gvCashflowProgram.Rows[i].FindControl("CbxCapture");
        //        Cbx1.Checked = false;
        //        CheckBox Cbx2 = (CheckBox)gvCashflowProgram.Rows[i].FindControl("CbxDisplay");
        //        Cbx2.Checked = false;
        //        CheckBox Cbx3 = (CheckBox)gvCashflowProgram.Rows[i].FindControl("CbxPost");
        //        Cbx3.Checked = false;
        //    }
        //}
        ddlLOB.Focus();//Added by suseela
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Origination/S3GOrgCashflowMaster_View.aspx");
        btnCancel.Focus();//Added by suseela
    }
    #endregion

    /// <summary>
    /// This methode used in Get Cashflow details in modify mode
    /// </summary>
    #region Get CFM
    private void FunGetCashflowDetatils()
    {
        try
        {
            //CashflowMgtServices.S3G_ORG_GetCashflowDetailsRow ObjCashflowDetailsRow;
            //ObjCashflowDetailsRow = ObjS3G_GetProgramDetails.NewS3G_ORG_GetCashflowDetailsRow();
            //ObjCashflowDetailsRow.CashFlow_ID = intCashfloId;
            //ObjCashflowDetailsRow.Company_ID = Convert.ToInt32(intCompanyId);
            //ObjS3G_GetProgramDetails.AddS3G_ORG_GetCashflowDetailsRow(ObjCashflowDetailsRow);
            //objCashflow_MasterClient = new CashflowMgtServicesReference.CashflowMgtServicesClient();
            //SerializationMode SerMode = SerializationMode.Binary;
            //byte[] byteCashflowDetails = objCashflow_MasterClient.FunPubGetCashflowDetails(SerMode, ClsPubSerialize.Serialize(ObjS3G_GetProgramDetails, SerMode));
            //ObjS3G_GetProgramDetails = (CashflowMgtServices.S3G_ORG_GetCashflowDetailsDataTable)ClsPubSerialize.DeSerialize(byteCashflowDetails, SerializationMode.Binary, typeof(CashflowMgtServices.S3G_ORG_GetCashflowDetailsDataTable));

            dictParam = new Dictionary<string, string>();
            dictParam.Add("@CashFlow_ID", Convert.ToString(intCashfloId));
            dictParam.Add("@Company_ID", Convert.ToString(intCompanyId));

            DataSet DSet = Utility.GetDataset("S3G_ORG_GetCashflowDetails", dictParam);

            DataTable ObjS3G_GetProgramDetails = DSet.Tables[0];

            txtCashflowNo.Text = ObjS3G_GetProgramDetails.Rows[0]["CFSl_No"].ToString();
            if (ObjS3G_GetProgramDetails.Rows[0]["IS_ACTIVESTATUS"].ToString()!="0")
            {
                CbxActive.Enabled=false;
            }

            ListItem lst;

            lst = new ListItem(ObjS3G_GetProgramDetails.Rows[0]["LOB_Name"].ToString(), ObjS3G_GetProgramDetails.Rows[0]["LOB_ID"].ToString());
            ddlLOB.Items.Add(lst);
            ddlLOB.SelectedValue = ObjS3G_GetProgramDetails.Rows[0]["LOB_ID"].ToString();
            //FunPriGetAccountFlagList(Convert.ToInt32(ddlLOB.SelectedValue));
            //FunPriGetGLList(Convert.ToInt32(ddlLOB.SelectedValue), Convert.ToInt32(ddlDAccountFlag.SelectedValue));
            //FunPriGetSLList(Convert.ToInt32(ddlLOB.SelectedValue), Convert.ToInt32(ddlCAccountFlag.SelectedValue));

            //Code Added by saran 17-Oct-2013 for NDE implentation end
            // Added By R. Manikandan
            // CR for NCPM 
            // Added on 06 - JUL - 2015
            //FunPriLoadProduct();
            FunPriLoadModifyQueryProduct();
            ddlProduct.SelectedValue = ObjS3G_GetProgramDetails.Rows[0]["PRODUCT_ID"].ToString();

            // Added By R. Manikandan End

            lst = new ListItem(ObjS3G_GetProgramDetails.Rows[0]["Flow_Type"].ToString(), ObjS3G_GetProgramDetails.Rows[0]["Flow_Type_Id"].ToString());
            ddlCashflow.Items.Add(lst);
            ddlCashflow.SelectedValue = ObjS3G_GetProgramDetails.Rows[0]["Flow_Type_Id"].ToString();

            txtCashflowDesc.Text = ObjS3G_GetProgramDetails.Rows[0]["Cashflow_Description"].ToString();

            lst = new ListItem(ObjS3G_GetProgramDetails.Rows[0]["CashFlowFlag_Desc"].ToString(), ObjS3G_GetProgramDetails.Rows[0]["Cashflow_flag_ID"].ToString());
            ddlCashflowFlag.Items.Add(lst);
            ddlCashflowFlag.SelectedValue = ObjS3G_GetProgramDetails.Rows[0]["Cashflow_flag_ID"].ToString();

            lst = new ListItem(ObjS3G_GetProgramDetails.Rows[0]["Recovery_type"].ToString(), ObjS3G_GetProgramDetails.Rows[0]["Recovery_type_Id"].ToString());
            ddlRecoveryType.Items.Add(lst);
            ddlRecoveryType.SelectedValue = ObjS3G_GetProgramDetails.Rows[0]["Recovery_type_Id"].ToString();

            //Check for deactive LOB
            if (ddlLOB.SelectedValue == "0")
            {
                Utility.FunShowAlertMsg(this.Page, "The selected LOB is deactivated");
                return;
            }


            if (ObjS3G_GetProgramDetails.Rows[0]["Business_IRR"].ToString() == "True")
                CbxBusIRR.Checked = true;
            else
                CbxBusIRR.Checked = false;
            if (ObjS3G_GetProgramDetails.Rows[0]["Accounting_IRR"].ToString() == "True")
                CbxAccIRR.Checked = true;
            else
                CbxAccIRR.Checked = false;
            if (ObjS3G_GetProgramDetails.Rows[0]["Company_IRR"].ToString() == "True")
                CbxBoth.Checked = true;
            else
                CbxBoth.Checked = false;


            if (CbxBusIRR.Checked == true && CbxAccIRR.Checked == true && CbxBoth.Checked == true)
            {
                CbxAll.Checked = true;
                CbxBusIRR.Checked = true;
                CbxBoth.Checked = true;
                CbxAccIRR.Checked = true;
            }
            if (ObjS3G_GetProgramDetails.Rows[0]["Is_Active"].ToString() == "True")
                CbxActive.Checked = true;
            else
                CbxActive.Checked = false;




            gvCashflowProgram.DataSource = DSet.Tables[2];
            gvCashflowProgram.DataBind();
            FunProSetGridStyle(DSet.Tables[2]);
            //ddlProduct.Enabled = false;
            //ddlDType.Enabled = false;
            //ddlDGLCode.Enabled = false;
            //ddlDSLCode.Enabled = false;
            //ddlCType.Enabled = false;
            //ddlCGLCode.Enabled = false;
            //ddlCSLCode.Enabled = false;

            //Debit
            if (DSet.Tables[3].Rows.Count > 0)
            {
                DataTable dtDebit = DSet.Tables[3].Copy();
                if (PageMode != PageModes.Query)
                {
                    //ddlDType.BindDataTable(DSet.Tables[1].Copy(), new string[] { "value", "Name" });
                    //ddlDType.Items.Remove(ddlDType.Items.FindByValue("5"));

                    //ddlDType.SelectedValue = dtDebit.Rows[0]["Debit_Type_ID"].ToString();

                    //FunPriSetGLCodeList(Convert.ToInt32(ddlLOB.SelectedValue));
                    //ddlDGLCode.SelectedValue = dtDebit.Rows[0]["GL_Code"].ToString();
                    //ddlDGLCode.SelectedItem.Text = dtDebit.Rows[0]["DGLAccount_Name"].ToString();
                    //FunPriSetSLCodeList(ddlDGLCode, ddlDSLCode); //commented by sangeetha
                    //ddlDSLCode.SelectedValue = dtDebit.Rows[0]["SL_Code"].ToString();
                    //ddlDSLCode.SelectedItem.Text = dtDebit.Rows[0]["DSLAccount_Name"].ToString();
                    ddlDType.BindDataTable(DSet.Tables[1].Copy(), new string[] { "value", "Name" });
                    ddlDType.Items.Remove(ddlDType.Items.FindByValue("5"));

                    ddlDType.SelectedValue = dtDebit.Rows[0]["Debit_Type_ID"].ToString();

                    FunPriSetGLCodeList(Convert.ToInt32(ddlLOB.SelectedValue));
                    ddlDGLCode.SelectedValue = dtDebit.Rows[0]["GL_Code"].ToString();
                    FunPriSetSLCodeList(ddlDGLCode, ddlDSLCode);
                    ddlDSLCode.SelectedValue = dtDebit.Rows[0]["SL_Code"].ToString();
                    //if (ddlDType.SelectedValue == "3")
                    //{
                    //    ddlDGLCode.ClearDropDownList();
                    //    ddlDSLCode.ClearDropDownList();
                    //    ddlDGLCode.Enabled = false;
                    //    ddlDSLCode.Enabled = false;

                    //}
                    //else
                    //{
                    //    ddlDGLCode.Enabled = true;
                    //    ddlDSLCode.Enabled = true;
                    //}
                    

                }
                else
                {
                    ListItem lstitem = new ListItem(dtDebit.Rows[0]["Debit_Type_ID_Desc"].ToString(), dtDebit.Rows[0]["Debit_Type_ID"].ToString());
                    ddlDType.Items.Add(lstitem);
                    ListItem lstitemCG = new ListItem(dtDebit.Rows[0]["DGLAccount_Name"].ToString(), dtDebit.Rows[0]["GL_Code"].ToString());
                    ddlDGLCode.Items.Add(lstitemCG);
                    ListItem lstitemCS = new ListItem(dtDebit.Rows[0]["DSLAccount_Name"].ToString(), dtDebit.Rows[0]["SL_Code"].ToString());
                    ddlDSLCode.Items.Add(lstitemCS);
                }



            }
            FunPriSetRFVctrls(ddlCashflow.SelectedValue);
            //Credit
            if (DSet.Tables[4].Rows.Count > 0)
            {
                DataTable dtCredit = DSet.Tables[4].Copy();
                if (PageMode != PageModes.Query)
                {
                    ddlCType.BindDataTable(DSet.Tables[1].Copy(), new string[] { "value", "Name" });
                    ddlCType.Items.Remove(ddlCType.Items.FindByValue("5"));
                    ddlCType.SelectedValue = dtCredit.Rows[0]["Credit_Type_ID"].ToString();

                    FunPriSetGLCodeList(Convert.ToInt32(ddlLOB.SelectedValue));
                    ddlCGLCode.SelectedValue = dtCredit.Rows[0]["GL_Code"].ToString();
                    FunPriSetSLCodeList(ddlCGLCode, ddlCSLCode);
                    ddlCSLCode.SelectedValue = dtCredit.Rows[0]["SL_Code"].ToString();
                    //if (ddlCType.SelectedValue == "3")
                    //{
                    //    ddlCGLCode.ClearDropDownList();
                    //    ddlCSLCode.ClearDropDownList();
                    //    ddlCGLCode.Enabled = false;
                    //    ddlCSLCode.Enabled = false;

                    //}
                    //else
                    //{
                    //    ddlCGLCode.Enabled = true;
                    //    ddlCSLCode.Enabled = true;
                    //}
                }
                else
                {
                    ListItem lstitem = new ListItem(dtCredit.Rows[0]["Credit_Type_ID_Desc"].ToString(), dtCredit.Rows[0]["Credit_Type_ID"].ToString());
                    ddlCType.Items.Add(lstitem);
                    ListItem lstitemCG = new ListItem(dtCredit.Rows[0]["CGLAccount_Name"].ToString(), dtCredit.Rows[0]["GL_Code"].ToString());
                    ddlCGLCode.Items.Add(lstitemCG);
                    ListItem lstitemCS = new ListItem(dtCredit.Rows[0]["CSLAccount_Name"].ToString(), dtCredit.Rows[0]["SL_Code"].ToString());
                    ddlCSLCode.Items.Add(lstitemCS);
                }
            }

           







        }
        catch (FaultException<CompanyMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
    }

    /// <summary>
    /// This methode used in Get Cashflow Mapping details in modify mode
    /// </summary>
    private void FunGetCashflowMapping()
    {
        objCashflow_MasterClient = new CashflowMgtServicesReference.CashflowMgtServicesClient();
        try
        {

            //ObjCashflowMappingsRow = ObjS3G_GetProgramMapping.NewS3G_ORG_GetCashflowMappingRow();
            //ObjCashflowMappingsRow.CashFlow_ID = intCashfloId;
            //ObjCashflowMappingsRow.Company_ID = intCompanyId;
            //ObjS3G_GetProgramMapping.AddS3G_ORG_GetCashflowMappingRow(ObjCashflowMappingsRow);

            //SerializationMode SerMode = SerializationMode.Binary;
            //byte[] byteCashflowMapping = objCashflow_MasterClient.FunPubGetCashflowMapping(SerMode, ClsPubSerialize.Serialize(ObjS3G_GetProgramMapping, SerMode));
            //ObjS3G_GetProgramMapping = (CashflowMgtServices.S3G_ORG_GetCashflowMappingDataTable)ClsPubSerialize.DeSerialize(byteCashflowMapping, SerializationMode.Binary, typeof(CashflowMgtServices.S3G_ORG_GetCashflowMappingDataTable));

            dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", intCompanyId.ToString());
            dictParam.Add("@LOB_ID", ddlLOB.SelectedValue.ToString());
            dictParam.Add("@CashFlow_Flag_ID", ddlCashflowFlag.SelectedValue.ToString());
            dictParam.Add("@Flow_Type", ddlCashflow.SelectedItem.Text);
            dictParam.Add("@Cashflow_ID", intCashfloId.ToString());
            dictParam.Add("@ModeCode", strMode);
            DataTable dtGrid = Utility.GetDefaultData("S3G_ORG_GetCashflowPrograms", dictParam);

            gvCashflowProgram.DataSource = dtGrid;
            gvCashflowProgram.DataBind();
            FunProSetGridStyle(dtGrid);
            //objCashflow_MasterClient.Close();
        }
        catch (FaultException<CompanyMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            //objCashflow_MasterClient.Close();
        }

    }

    protected void FunProSetGridStyle(DataTable dtGrid)
    {
        for (int i = 0; i < gvCashflowProgram.Rows.Count; i++)
        {

            if (gvCashflowProgram.Rows[i].RowType == DataControlRowType.DataRow)
            {

                int Program_ID = Convert.ToInt32(gvCashflowProgram.DataKeys[i]["Program_ID"].ToString());
                CheckBox Cbx1 = (CheckBox)gvCashflowProgram.Rows[i].FindControl("CbxCapture");
                CheckBox Cbx2 = (CheckBox)gvCashflowProgram.Rows[i].FindControl("CbxDisplay");
                CheckBox Cbx3 = (CheckBox)gvCashflowProgram.Rows[i].FindControl("CbxPost");
                Cbx1.Checked = false;
                Cbx2.Checked = false;
                Cbx3.Checked = false;
                if (Program_ID == Convert.ToInt32(dtGrid.Rows[i]["Program_ID"].ToString()))
                {
                    if (dtGrid.Rows[i]["Program_ID"].ToString() == "0")
                    {
                        Cbx1.Checked = false;
                        Cbx2.Checked = false;
                        Cbx3.Checked = false;
                    }
                    else

                        Cbx1.Checked = Convert.ToBoolean(dtGrid.Rows[i]["IS_Capture"]);
                    Cbx2.Checked = Convert.ToBoolean(dtGrid.Rows[i]["IS_Display"]);
                    Cbx3.Checked = Convert.ToBoolean(dtGrid.Rows[i]["IS_Post"]);

                    if (Cbx3.Checked)
                    {
                        gvCashflowProgram.Rows[i].BackColor = System.Drawing.Color.FromName("#f5f8ff");
                    }
                }
            }
        }
    }

    #endregion

    /// <summary>
    /// This methode used in Delete(not using transaction) Cashflow details
    /// </summary>
    #region Delete Cashflow
    private void FunPriDeleteCashflow()
    {
        objCashflow_MasterClient = new CashflowMgtServicesReference.CashflowMgtServicesClient();
        try
        {
            ObjDeleteCashflowRow = ObjS3G_DeleteCashflow.NewS3G_ORG_DeleteCashFlowMasterRow();
            ObjDeleteCashflowRow.Company_ID = intCompanyId;
            ObjDeleteCashflowRow.CashFlow_ID = intCashfloId;
            ObjS3G_DeleteCashflow.AddS3G_ORG_DeleteCashFlowMasterRow(ObjDeleteCashflowRow);
            SerializationMode SerMode = SerializationMode.Binary;

            intErrCode = objCashflow_MasterClient.FunPubDeleteCashflowMaster(SerMode, ClsPubSerialize.Serialize(ObjS3G_DeleteCashflow, SerMode));
            if (intErrCode > 0)
            {
                deleteCashflow();
                //strAlert = strAlert.Replace("__ALERT__", "Cashflow details used in transaction");
                //ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageAdd, true);
                Utility.FunShowAlertMsg(this, "Cash Flow details used in transaction");


            }
            //else
            //{
            //    strRedirectPageAdd = "window.location.href='../Origination/S3GOrgCashflowMaster_Add.aspx';";
            //}
            //lblErrorMessage.Text = string.Empty;
            objCashflow_MasterClient.Close();
        }
        catch (FaultException<CompanyMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            objCashflow_MasterClient.Close();
        }

    }


    #endregion


    /// <summary>
    /// This methode used in Role Access Setup
    /// </summary>
    #region FieldDisable
    private void FunPriDisableControls(int intModeID)
    {

        if (!bQuery)
        {
            Response.Redirect(strRedirectPageView);
        }

        //if ((!bCreate) || (!bModify))
        //{
        //    btnSave.Enabled = false;
        //}

        switch (intModeID)
        {
            case 0: // Create Mode

                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);
                if (!bCreate)
                {
                    //btnSave.Enabled = false;
                    btnSave.Enabled_False();
                }
                // FunPriGetProgramMaster();
                validation();
                CbxActive.Checked = true;
                CbxActive.Enabled = false;
                ddlLOB.Focus();

                break;

            case 1: // Modify Mode

                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                if (!bModify)
                {
                    //btnSave.Enabled = false;
                    btnSave.Enabled_False();
                }


                FunGetCashflowDetatils();
                FunGetCashflowMapping();
                Mvalidation();
                ddlCSLCode.Enabled = true;
                //btnClear.Enabled = false;
                btnClear.Enabled_False();
                FunSetCSLEnable();//ddlCType_SelectedIndexChanged(null, null);//modified based on the test case id(Tc_086)
                FunSetDSLEnable();//ddlDType_SelectedIndexChanged(null, null);//modified based on the test case id(Tc_086)
                CbxActive.Focus();
                //CbxActive.Checked = true;

                if (ddlDSLCode.Enabled && ddlDSLCode.Items.Count > 1)
                {
                    //rfvDSlcode.Enabled = true;
                }
                else
                {
                    //rfvDSlcode.Enabled = false;
                }

                if (ddlCSLCode.Enabled && ddlCSLCode.Items.Count > 1)
                {
                    //rfvCSlcode.Enabled = true;
                }
                else
                {
                    //rfvCSlcode.Enabled = false;
                }

                FunPriDeleteCashflow();

                //Code Added by saran 17-Oct-2013 for NDE implentation start
                FunPriSetRFVctrls(ddlCashflow.SelectedValue);


                break;

            case -1:// Query Mode              
                if (!bQuery)
                {
                    Response.Redirect(strRedirectPageView);
                }
                FunGetCashflowDetatils();
                //FunGetCashflowMapping();
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                txtCashflowNo.ReadOnly = true;
                txtCashflowNo.Enabled = true;
                txtCashflowDesc.ReadOnly = true;

                if (bClearList)
                {
                    ddlLOB.ClearDropDownList();
                    ddlCashflow.ClearDropDownList();
                    ddlCashflowFlag.ClearDropDownList();

                    if (ddlDGLCode.Items.Count > 1)
                        ddlDGLCode.ClearDropDownList();
                    if (ddlDType.Items.Count > 1)
                        ddlDType.ClearDropDownList();

                    if (ddlDSLCode.Items.Count > 1)
                        ddlDSLCode.ClearDropDownList();

                    if (ddlCGLCode.Items.Count > 1)
                        ddlCGLCode.ClearDropDownList();

                    if (ddlCType.Items.Count > 1)
                        ddlCType.ClearDropDownList();

                    if (ddlCSLCode.Items.Count > 1)
                        ddlCSLCode.ClearDropDownList();

                    //ddlDAccountFlag.ClearDropDownList();
                    //ddlCAccountFlag.ClearDropDownList();
                }
                CbxAccIRR.Enabled = false;
                CbxBoth.Enabled = false;
                CbxBusIRR.Enabled = false;
                CbxActive.Enabled = false;
                CbxAll.Enabled = false;

                ddlProduct.Enabled = false;
                //btnClear.Enabled = false;
                btnClear.Enabled_False();
                //btnSave.Enabled = false;
                btnSave.Enabled_False();


                break;
        }

    }



    private void deleteCashflow()
    {
        txtCashflowNo.ReadOnly = true;
        txtCashflowNo.Enabled = true;
        txtCashflowDesc.ReadOnly = true;

        ddlLOB.ClearDropDownList();
        ddlCashflow.ClearDropDownList();
        ddlCashflowFlag.ClearDropDownList();
        ddlDGLCode.ClearDropDownList();
        ddlDType.ClearDropDownList();
        ddlDSLCode.ClearDropDownList();
        ddlCGLCode.ClearDropDownList();
        ddlCType.ClearDropDownList();
        ddlCSLCode.ClearDropDownList();

        //ddlDAccountFlag.ClearDropDownList();
        //ddlCAccountFlag.ClearDropDownList();

        CbxAccIRR.Enabled = false;
        CbxBoth.Enabled = false;
        CbxBusIRR.Enabled = false;
        CbxAll.Enabled = false;
        //btnClear.Enabled = false;
        btnClear.Enabled_False();
        foreach (GridViewRow GRow in gvCashflowProgram.Rows)
        {
            CheckBox Chebox1 = (CheckBox)GRow.FindControl("CbxCapture");
            CheckBox Chebox2 = (CheckBox)GRow.FindControl("CbxDisplay");
            CheckBox Chebox3 = (CheckBox)GRow.FindControl("CbxPost");

            Chebox1.Enabled = false;
            Chebox2.Enabled = false;
            Chebox3.Enabled = false;
        }
    }
    #endregion
    protected void gvCashflowProgram_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CheckBox Chebox1 = (CheckBox)e.Row.FindControl("CbxCapture");
            CheckBox Chebox2 = (CheckBox)e.Row.FindControl("CbxDisplay");
            CheckBox Chebox3 = (CheckBox)e.Row.FindControl("CbxPost");

            if (PageMode != PageModes.Query)
            {
                Chebox3.Attributes.Add("onclick", "fnUnselectAllExpectSelected(this,'" + e.Row.ClientID + "')");
            }

            if ((bQuery && strMode == "Q") || ((ViewState["GridCbx"] != null) && Convert.ToString(ViewState["GridCbx"]) == "0"))
            {
                Chebox1.Enabled = false;
                Chebox2.Enabled = false;
                Chebox3.Enabled = false;
            }

        }
    }
    protected void CbxAll_CheckedChanged(object sender, EventArgs e)
    {
        if (CbxAll.Checked)
        {
            CbxBoth.Checked = true;
            CbxBusIRR.Checked = true;
            CbxAccIRR.Checked = true;
        }
        else
        {
            CbxBoth.Checked = false;
            CbxBusIRR.Checked = false;
            CbxAccIRR.Checked = false;
        }
    }
    protected void CbxBusIRR_CheckedChanged(object sender, EventArgs e)
    {
        if (CbxBusIRR.Checked)
        {
            CbxAll.Checked = false;
        }
        else
        {
            CbxAll.Checked = false;
        }

    }
    protected void CbxAccIRR_CheckedChanged(object sender, EventArgs e)
    {
        if (CbxAccIRR.Checked)
        {
            CbxAll.Checked = false;
        }
        {

            CbxAll.Checked = false;
        }
        CbxAccIRR.Focus();//Added by suseela
    }
    protected void CbxBoth_CheckedChanged(object sender, EventArgs e)
    {
        if (CbxBoth.Checked)
        {
            CbxAll.Checked = false;
        }
        {
            CbxAll.Checked = false;
        }
        CbxBoth.Focus();//Added by suseela

    }

    protected void ddlDGLCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        

        //if (Convert.ToInt32(ddlDGLCode.SelectedValue) > 0)
        //{

        //    FunPriGetGLCodeList(Convert.ToInt32(ddlLOB.SelectedValue), intCompanyId, Convert.ToInt32(ddlDGLCode.SelectedValue));
        //}
        //else
        //{
        //    ddlDSLCode.SelectedIndex = 0;
        //    ddlDType.SelectedIndex = 0;
        //}
        FunPriSetSLCodeList(ddlDGLCode, ddlDSLCode);
        ddlDGLCode.Focus();//Added by suseela
        if (ddlDSLCode.Enabled && ddlDSLCode.Items.Count > 1)
        {
            rfvDSlcode.Enabled = true;
        }
        else
        {
            rfvDSlcode.Enabled = false;
        }

    }
    protected void ddlCGLCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        //if (Convert.ToInt32(ddlCGLCode.SelectedValue) > 0)
        //{
        //    FunPriGetSLCodeList(Convert.ToInt32(ddlLOB.SelectedValue), intCompanyId, Convert.ToInt32(ddlCGLCode.SelectedValue));

        //}
        //else
        //{
        //    ddlCSLCode.SelectedIndex = 0;
        //    ddlCType.SelectedIndex = 0;
        //}
        FunPriSetSLCodeList(ddlCGLCode, ddlCSLCode);
        ddlCGLCode.Focus();//Added by suseela
        if (ddlCSLCode.Enabled && ddlCSLCode.Items.Count > 1)
        {
            rfvCSlcode.Enabled = true;
        }
        else
        {
            rfvCSlcode.Enabled = false;
        }
    }


    protected void ddlDAccountFlag_SelectedIndexChanged(object sender, EventArgs e)
    {
        //if (Convert.ToInt32(ddlDAccountFlag.SelectedValue) > 0)
        //{

        //    FunPriGetGLList(Convert.ToInt32(ddlLOB.SelectedValue), Convert.ToInt32(ddlDAccountFlag.SelectedValue));
        //    if (Convert.ToInt32(ddlDType.SelectedValue) == 1)
        //    {
        //        FunPriGetGLCodeList(Convert.ToInt32(ddlLOB.SelectedValue), intCompanyId, Convert.ToInt32(ddlDGLCode.SelectedValue));
        //        ddlDSLCode.Enabled = true;
        //    }
        //    else
        //    {
        //        ddlDSLCode.Items.Clear();
        //        ListItem liSelect = new ListItem("--Select--", "0");
        //        ddlDSLCode.Items.Insert(0, liSelect);
        //        ddlDSLCode.Enabled = false;
        //    }


        //}
        //else
        //{
        //    //ddlDGLCode.SelectedIndex = 0;
        //    //ddlDType.SelectedIndex = 0;
        //    //ddlDSLCode.SelectedIndex = 0;
        //    ddlDGLCode.SelectedItem.Text = "--Select--";
        //    ddlDSLCode.SelectedItem.Text = "--Select--";

        //}
    }
    protected void ddlCAccountFlag_SelectedIndexChanged(object sender, EventArgs e)
    {
        //if (Convert.ToInt32(ddlCAccountFlag.SelectedValue) > 0)
        //{

        //    FunPriGetSLList(Convert.ToInt32(ddlLOB.SelectedValue), Convert.ToInt32(ddlCAccountFlag.SelectedValue));
        //    if (Convert.ToInt32(ddlCType.SelectedValue) == 1)
        //    {

        //        FunPriGetSLCodeList(Convert.ToInt32(ddlLOB.SelectedValue), intCompanyId, Convert.ToInt32(ddlCGLCode.SelectedValue));
        //    }
        //    else
        //    {
        //        ddlCSLCode.Items.Clear();
        //        ListItem liSelect = new ListItem("--Select--", "0");
        //        ddlCSLCode.Items.Insert(0, liSelect);
        //        ddlCSLCode.Enabled = false;
        //    }

        //}
        //else
        //{

        //    //ddlCType.SelectedIndex = 0;
        //    //ddlCGLCode.SelectedIndex = 0;
        //    //ddlCSLCode.SelectedIndex = 0;
        //    //ddlCGLCode.SelectedItem.Text = "--Select--";
        //    ddlCSLCode.SelectedItem.Text = "--Select--";

        //}


    }
    protected void ddlDSLCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlDType.SelectedItem.Text == "Account")
        {
            //ddlDSLCode.SelectedIndex = 0;
            //rfvDSlcode.Visible = true;
        }
    }
    protected void ddlCSLCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlCType.SelectedItem.Text == "Account")
        {
            //ddlCSLCode.SelectedIndex = 0;
            //rfvCSlcode.Visible = true;

        }
    }

    //Added by Thangam M on 02/Nov/2011 to make filteration on Program List

    protected void ddlCashflow_SelectedIndexChanged(object sender, EventArgs e)
    {
        FunPriGetProgramMaster();
        //Code Added by saran 17-Oct-2013 for NDE implentation start
        FunPriSetRFVctrls(ddlCashflow.SelectedValue);
        //Code Added by saran 17-Oct-2013 for NDE implentation end
    }

    protected void ddlCashflowFlag_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlDType.ClearSelection();
        ddlDGLCode.ClearSelection();
        ddlDSLCode.ClearSelection();
        ddlCType.ClearSelection();
        ddlCGLCode.ClearSelection();
        ddlCSLCode.ClearSelection();
        FunPriGetProgramMaster();
        ddlCashflowFlag.Focus();//Added by Suseela
    }
    protected void ddlProductSelectedIndexChanged(object sender, EventArgs e)
    {
        FunPriGetProgramMaster();
        ddlProduct.Focus();//Added by Suseela
    }



    private void FunPriSetRFVctrls(string strFlowType)
    {
        //Both	3
        //Inflow	1
        //NDE Inflow	4
        //NDE Outflow	5
        //Outflow	2
        rfvDGlcode.Enabled = rfvDSlcode.Enabled = rfvDType.Enabled =
            rfvCGlcode.Enabled = rfvCSlcode.Enabled = rfvCType.Enabled = false;
        tblDebit.Disabled = tblCredit.Disabled = true;
        lblDebit.CssClass = "styleDisplayLabel"; lblCredit.CssClass = "styleDisplayLabel";
        //ddlDType.SelectedIndex = 0;
        //ddlCType.SelectedIndex = 0;
        //ddlDGLCode.SelectedIndex = 0;
        //ddlDSLCode.SelectedIndex = 0;
        //ddlCGLCode.SelectedIndex = 0;
        //ddlCSLCode.SelectedIndex=0;
        switch (strFlowType)
        {

            case "1"://Inflow
            case "2"://Outflow
            case "3"://Both
                rfvDType.Enabled = rfvDGlcode.Enabled = rfvCType.Enabled = rfvCGlcode.Enabled = true;
                tblDebit.Disabled = tblCredit.Disabled = false;
                lblDebit.CssClass = "styleReqFieldLabel"; lblCredit.CssClass = "styleReqFieldLabel";
                pnlDebit.Enabled = pnlCredit.Enabled = true;
                break;
            case "4"://NDE Inflow
                rfvCType.Enabled = rfvCGlcode.Enabled = true;
                tblCredit.Disabled = false;
                lblCredit.CssClass = "styleReqFieldLabel";
                pnlDebit.Enabled = false;
                pnlCredit.Enabled = true;
                break;
            case "5"://NDE Outflow
                rfvDType.Enabled = rfvDGlcode.Enabled = true;
                tblDebit.Disabled = false;
                lblDebit.CssClass = "styleReqFieldLabel";
                pnlCredit.Enabled = false;
                pnlDebit.Enabled = true;
                break;

        }
        
    }

    protected void FunProResetGrid()
    {

        //ddlProduct.SelectedValue = "0";

        DataTable dtDummy = new DataTable();
        dtDummy.Columns.Add("Program_ID");
        dtDummy.Columns.Add("Program_Ref_No");
        dtDummy.Columns.Add("Program_Name");

        DataRow DRow = dtDummy.NewRow();
        DRow["Program_ID"] = "0";
        DRow["Program_Ref_No"] = "0";
        DRow["Program_Name"] = "0";

        dtDummy.Rows.Add(DRow);
        gvCashflowProgram.DataSource = dtDummy;
        gvCashflowProgram.DataBind();

        gvCashflowProgram.Rows[0].Visible = false;
    }

    // Added By R. Manikandan
    // CR for NCPM 
    // Added on 06 - JUL - 2015

    protected void FunPriLoadProduct()
    {
        try
        {
            dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", intCompanyId.ToString());
            if (strMode == "C")
                dictParam.Add("@Is_Active", "1");
            dictParam.Add("@LOB_ID", ddlLOB.SelectedValue.ToString());
            ddlProduct.BindDataTable(SPNames.SYS_ProductMaster, dictParam, new string[] { "Product_ID", "Product_Code", "Product_Name" });
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    protected void FunPriLoadModifyQueryProduct()
    {
        try
        {
            dictParam = new Dictionary<string, string>();
            dictParam.Add("@CASHFLOW_ID", intCashfloId.ToString());
            dictParam.Add("@Company_ID", intCompanyId.ToString());
            if (strMode == "C")
                dictParam.Add("@Is_Active", "1");
            dictParam.Add("@LOB_ID", ddlLOB.SelectedValue.ToString());
            ddlProduct.BindDataTable("S3G_SA_GET_PRODLSTMOD", dictParam, new string[] { "Product_ID", "Product_Code", "Product_Name" });
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    // End here
    protected void CbxPost_CheckedChanged(object sender, EventArgs e)
    {
        string strChkId = ((CheckBox)sender).ClientID;
        CheckBox chkCurSelect = (CheckBox)sender;
        if (chkCurSelect.Checked == true)
        {
            var isChecked = chkCurSelect.Checked;
            var tempCheckBox = new CheckBox();
            foreach (GridViewRow gvRow in gvCashflowProgram.Rows)
            {
                tempCheckBox = gvRow.FindControl("CbxPost") as CheckBox;
                if (tempCheckBox != null)
                {
                    tempCheckBox.Checked = !isChecked;
                }
            }
            if (isChecked)
            {
                chkCurSelect.Checked = true;
            }
        }
        else
        {
            GridViewRow gvRow = null;
            if (chkCurSelect != null) gvRow = (GridViewRow)chkCurSelect.Parent.Parent;
        }
    }
}
