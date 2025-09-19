#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Business
/// Screen Name			: Compliance Master
/// Created By			: U.Suseela Devi
/// Created Date		: 05-16-2018
/// Purpose	            : 
/// <Program Summary>
#endregion
# region NameSpaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using S3GBusEntity;
using System.Globalization;
using System.Collections;
using System.Resources;
using System.ServiceModel;
using System.Text;
using System.Configuration;
using System.Web.Security;
using S3GBusEntity.Origination;
using Resources;
#endregion

public partial class Origination_S3GOrgComplainceMaster_View : ApplyThemeForProject
{
    public static Origination_S3GOrgComplainceMaster_View Obj_Page;
    Dictionary<string, string> Procparam = null;
    // UserInfo ObjUserInfo = new UserInfo();
    public static DataTable dtUserRights;
    string strRedirectPage = "~/Origination/S3GOrgComplainceMaster_Add.aspx";
    int intCompanyId = 1;
    int intProgramID = 529;
    int intTotalRecords;
    int intOption;
    int intUserID;
    string prefixText;
    string strDateFormat = "";         // to maintain the standard format
    static string strPageName = "Compliance Master";
    //User Authorization variable declaration
    UserInfo ObjUserInfo = null;
    bool bCreate = false;
    bool bModify = false;
    bool bQuery = false;
    bool bIsActive = false;
    bool bMakerChecker = false;
    //Declaration end
    #region Paging Config

    PagingValues ObjPaging = new PagingValues();

    public delegate void PageAssignValue(int ProPageNumRW, int intPageSize);

    public int ProPageNumRW
    {
        get;
        set;
    }

    public int ProPageSizeRW
    {
        get;
        set;

    }

    protected void AssignValue(int intPageNum, int intPageSize)
    {
        ProPageNumRW = intPageNum;
        ProPageSizeRW = intPageSize;

        FunPriBindGrid();

    }

    #endregion
    #region Common Variables
    int intCreate = 1;                                                         // intCreate = 1 then display the create button, else invisible
    int intQuery = 0;                                                          // intQuery = 1 then display the Query button, else invisible
    int intModify = 0;
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        ObjUserInfo = new UserInfo();
        lblErrorMessage.Text = "";
        lblHeading.Text = "Compliance Master - Details";

        Obj_Page = this;
        try
        {
            #region Application Standard Date Format
            S3GSession ObjS3GSession = new S3GSession();
            strDateFormat = ObjS3GSession.ProDateFormatRW;                              // to get the standard date format of the Application
            CalendarExtenderEffectiveFromDate.Format = strDateFormat;                       // assigning the first textbox with the End date
            CalendarExtenderEffectiveToDate.Format = strDateFormat;                     // assigning the first textbox with the start date
            #endregion
            #region Paging Config

            ProPageNumRW = 1;
            TextBox txtPageSize = (TextBox)ucCustomPaging.FindControl("txtPageSize");
            if (txtPageSize.Text != "")
                ProPageSizeRW = Convert.ToInt32(txtPageSize.Text);
            else
                ProPageSizeRW = Convert.ToInt32(ConfigurationManager.AppSettings.Get("GridPageSize"));

            //TextBox txtPageNum = (TextBox)ucCustomPaging.FindControl("txtGotoPage");

            //if (Convert.ToString(txtPageNum.Text) != "" && txtPageNum.Text != "0")
            //    ProPageNumRW = Convert.ToInt32(txtPageNum.Text);
            //else
            //    ProPageNumRW = 1;

            PageAssignValue obj = new PageAssignValue(this.AssignValue);
            ucCustomPaging.callback = obj;
            ucCustomPaging.ProPageNumRW = ProPageNumRW;
            ucCustomPaging.ProPageSizeRW = ProPageSizeRW;
            #endregion
            //User Authorization
            ObjUserInfo = new UserInfo();
            intCompanyId = ObjUserInfo.ProCompanyIdRW;
            intUserID = ObjUserInfo.ProUserIdRW;

            bCreate = ObjUserInfo.ProCreateRW;
            bModify = ObjUserInfo.ProModifyRW;
            bQuery = ObjUserInfo.ProViewRW;
            bIsActive = ObjUserInfo.ProIsActiveRW;
            //bMakerChecker = ObjUserInfo.ProMakerCheckerRW;
            //Authorization Code end
            System.Web.HttpContext.Current.Session["AutoSuggestCompanyID"] = ObjUserInfo.ProCompanyIdRW.ToString();

            intUserID = ObjUserInfo.ProUserIdRW;

            txtEffectiveFromDate.Attributes.Add("onChange", "fnDoDate(this,'" + txtEffectiveFromDate.ClientID + "','" + strDateFormat + "',false,  false);");
            txtEffectiveToDate.Attributes.Add("onChange", "fnDoDate(this,'" + txtEffectiveToDate.ClientID + "','" + strDateFormat + "',false,  false);");

            if (!string.IsNullOrEmpty(strDateFormat))
            {
                CalendarExtenderEffectiveFromDate.Format = CalendarExtenderEffectiveFromDate.Format = strDateFormat;
            }

            if (!IsPostBack)
            {
                FunLoadComplainceType();
                ucCustomPaging.Visible = false;
                txtEffectiveFromDate.Focus();
                dtUserRights = Utility.UserAccess(0, 0, intUserID, intProgramID, intCompanyId);
            }
            FunPubSetIndex(1);
            //Create or Delete(Cancel) 1- Create, 3-Delete
            if (IsUserAccessChecker(1))
            {
                //btnCreate.Enabled = true;
                btnCreate.Enabled_True();
            }
            else
            {
                //btnCreate.Enabled = false;
                btnCreate.Enabled_False();
            }

        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
        }
    }
    private void FunLoadComplainceType()
    {
        Procparam = new Dictionary<string, string>();
        DataTable dtComplaince = new DataTable();
        if (dtComplaince != null)
        {
            Procparam.Add("@Company_ID", ObjUserInfo.ProCompanyIdRW.ToString());
            Procparam.Add("@User_ID", ObjUserInfo.ProUserIdRW.ToString());
            dtComplaince = Utility.GetDefaultData("S3G_ORG_LOADCOMPLAINCETYPE", Procparam);
            ddlComplainceType.FillDataTable(dtComplaince, "Value", "Name");
        }
        ddlComplainceType.Focus();
    }
    protected void grvTransLander_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {

            //  Label lblComplainceTypeID = (Label)e.Row.FindControl("lblComplainceTypeID");
            //if (e.CommandName.ToLower() == "modify")
            //{

            //    FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(e.CommandArgument.ToString(), false, 0);
            //   // FormsAuthenticationTicket Ticket1 = new FormsAuthenticationTicket(lblComplainceTypeID.Text.ToString(), false, 0);

            //    Response.Redirect(strRedirectPage + "?qsComplainceId=" + FormsAuthentication.Encrypt(Ticket)
            //        + "?qsComplainceTypeId=" + FormsAuthentication.Encrypt(Ticket1) + "&qsMode=M");
            //}
            //if (e.CommandName.ToLower() == "query")
            //{

            //    FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(e.CommandArgument.ToString(), false, 0);
            //    //FormsAuthenticationTicket Ticket1 = new FormsAuthenticationTicket(lblComplainceTypeID.Text.ToString(), false, 0);

            //    Response.Redirect(strRedirectPage + "?qsComplainceId=" + FormsAuthentication.Encrypt(Ticket)
            //        + "?qsComplainceTypeId=" + FormsAuthentication.Encrypt(Ticket1) + "&qsMode=Q");
            //}
            if (e.CommandName.ToLower() == "modify")
            {
                FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(e.CommandArgument.ToString(), false, 0);
                Response.Redirect(strRedirectPage + "?qsComplainceId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=M");
            }
            if (e.CommandName.ToLower() == "query")
            {
                FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(e.CommandArgument.ToString(), false, 0);
                Response.Redirect(strRedirectPage + "?qsComplainceId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=Q");
            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    public bool IsUserAccessChecker(int Option)
    {
        if (dtUserRights.Rows.Count > 0)
        {
            if (Option == 1)//Create
            {
                if (Convert.ToBoolean(dtUserRights.Rows[0]["ADDACCESS"]))
                {
                    return true;
                }
            }
            else if (Option == 2)//Modify
            {
                if (Convert.ToBoolean(dtUserRights.Rows[0]["MODIFYACESS"]))
                {
                    return true;
                }
            }
            if (Option == 3)//Delete
            {
                if (Convert.ToBoolean(dtUserRights.Rows[0]["DELETEACCESS"]))
                {
                    return true;
                }
            }
            else if (Option == 4)//View/Querry
            {
                if (Convert.ToBoolean(dtUserRights.Rows[0]["VIEWACCESS"]))
                {
                    return true;
                }
            }
        }
        return false;
    }
    protected void grvTransLander_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblUserID = (Label)e.Row.FindControl("lblUserID");
            Label lblUserLevelID = (Label)e.Row.FindControl("lblUserLevelID");
            Label lblCreatedOn = (Label)e.Row.FindControl("lblCreatedOn");
            ImageButton imgbtnEdit = (ImageButton)e.Row.FindControl("imgbtnEdit");
            ImageButton imgbtnQuery = (ImageButton)e.Row.FindControl("imgbtnQuery");
            CheckBox chkIsActive = (CheckBox)e.Row.FindControl("chkIsActive");
            Label lblStatus = (Label)e.Row.FindControl("lblStatus");
            if (lblStatus.Text == "True")
            {
                chkIsActive.Checked = true;
            }
            //Modify Access
            if (IsUserAccessChecker(2))// || (ObjUserInfo.IsUserMakerChecker(Convert.ToInt32(lblUserID.Text)))
            {
                imgbtnEdit.Enabled = true;
            }
            else
            {
                imgbtnEdit.Enabled = false;
                imgbtnEdit.CssClass = "styleGridEditDisabled";
            }
            //Create Access
            if (IsUserAccessChecker(4))// || (ObjUserInfo.IsUserMakerChecker(Convert.ToInt32(lblUserID.Text)))
            {
                imgbtnQuery.Enabled = true;
            }
            else
            {
                imgbtnQuery.Enabled = false;
                imgbtnQuery.CssClass = "styleGridEditDisabled";
            }
        }
    }
    private void FunPriBindGrid()
    {
        try
        {
            //Paging Properties set
            dtUserRights = Utility.UserAccess(0, 0, intUserID, intProgramID, intCompanyId);
            int intTotalRecords = 0;
            bool bIsNewRow = false;
            ObjPaging.ProCompany_ID = intCompanyId;
            ObjPaging.ProUser_ID = intUserID;
            ObjPaging.ProTotalRecords = intTotalRecords;

            ObjPaging.ProCurrentPage = ProPageNumRW;
            ObjPaging.ProPageSize = ProPageSizeRW;
            ObjPaging.ProSearchValue = hdnSearch.Value;
            ObjPaging.ProOrderBy = hdnOrderBy.Value;
            ObjPaging.ProProgram_ID = intProgramID;

            Procparam = new Dictionary<string, string>();
            if (Procparam != null)
            {
                Procparam.Clear();
            }
            Procparam.Add("@START_DATE", string.IsNullOrEmpty(txtEffectiveFromDate.Text) == false ? Utility.StringToDate(txtEffectiveFromDate.Text).ToString() : null);
            Procparam.Add("@END_DATE", string.IsNullOrEmpty(txtEffectiveToDate.Text) == false ? Utility.StringToDate(txtEffectiveToDate.Text).ToString() : null);
            Procparam.Add("@ComplainceType", ddlComplainceType.SelectedValue.ToString());

            grvTransLander.BindGridView("S3G_ORG_COMPLAINCE_PAGING", Procparam, out intTotalRecords, ObjPaging, out bIsNewRow);


            if (bIsNewRow)
            {
                grvTransLander.Rows[0].Visible = false;
            }
            lblErrorMessage.Text = "";
            ucCustomPaging.Visible = true;
            ucCustomPaging.Navigation(intTotalRecords, ProPageNumRW, ProPageSizeRW);
            ucCustomPaging.setPageSize(ProPageSizeRW);
            //Paging Config End
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        #region  User Authorization
        if (!bIsActive)
        {
            //isEditColumnVisible =
            //isQueryColumnVisible = false;
        }
        if ((!bModify) || (intModify == 0))
        {
            // isEditColumnVisible = false;

        }
        if ((!bQuery) || (intQuery == 0))
        {
            // isQueryColumnVisible = false;
        }
        #endregion
        if (txtEffectiveFromDate.Text != string.Empty && txtEffectiveToDate.Text != string.Empty)
        {
            if (Utility.StringToDate(txtEffectiveFromDate.Text) > Utility.StringToDate(txtEffectiveToDate.Text))
            {
                Utility.FunShowAlertMsg(this, "Effective To date cannot be less than Effective From date");
                txtEffectiveToDate.Focus();
                txtEffectiveToDate.Text = string.Empty;
                grvTransLander.DataSource = null;
                return;
            }
        }
        FunPriBindGrid();
        grvTransLander.Visible = true;
        btnSearch.Focus();
    }
    protected void btnCreate_Click(object sender, EventArgs e)
    {
        Response.Redirect(strRedirectPage, false);
        btnCreate.Focus();
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        if (ddlComplainceType != null && ddlComplainceType.Visible && ddlComplainceType.Items.Count > 0)
            ddlComplainceType.SelectedIndex = -1;
        grvTransLander.DataSource = null;
        grvTransLander.Visible = false;
        ucCustomPaging.Visible = false;
        txtEffectiveFromDate.Text = "";
        txtEffectiveToDate.Text = "";
        btnClear.Focus();
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        //base.VerifyRenderingInServerForm(control);
    }
}










