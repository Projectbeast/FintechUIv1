using S3GBusEntity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Origination_S3GOrgDeclineList_HotlistEntry_View : ApplyThemeForProject
{
    #region Initialization

    static string strPageName = "Decline List Hotlist Entry View";
    public string strDateFormat;
    int intCompanyId = 0;
    int intUserId = 0;
    #endregion

    #region Paging Config

    string strRedirectPage = "~/Origination/S3GOrgDeclineList_HotlistEntry_Add.aspx";
    int intNoofSearch = 2;
    string[] arrSortCol = new string[] { "NegativeList_Type", "Prospect_Mobile" };
    string strProcName = "S3g_Org_Get_DeclinHotlist_Pagi";
    Dictionary<string, string> Procparam = null;

    public static DataTable dtUserRights;
    ArrayList arrSearchVal = new ArrayList(2);
    int intUserID = 0;
    int intCompanyID = 0;
    int intProgramID = 537;
    //User Authorization variable declaration
    UserInfo ObjUserInfo = null;
    bool bCreate = false;
    bool bModify = false;
    bool bQuery = false;
    bool bIsActive = false;
    bool bMakerChecker = false;
    PagingValues ObjPaging = new PagingValues();
    S3GSession ObjS3GSession;

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

    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            ObjUserInfo = new UserInfo();
            intCompanyID = ObjUserInfo.ProCompanyIdRW;
            intUserID = ObjUserInfo.ProUserIdRW;

            bCreate = ObjUserInfo.ProCreateRW;
            bModify = ObjUserInfo.ProModifyRW;
            bQuery = ObjUserInfo.ProViewRW;
            bIsActive = ObjUserInfo.ProIsActiveRW;


            intCompanyId = ObjUserInfo.ProCompanyIdRW;//Convert.ToInt32(Request.QueryString["qsCmpnyId"]);
            intUserId = ObjUserInfo.ProUserIdRW;

            System.Web.HttpContext.Current.Session["AutoSuggestCompanyID"] = intCompanyId.ToString();

            ObjS3GSession = new S3GSession();
            strDateFormat = ObjS3GSession.ProDateFormatRW;


            ceDeclineFromDate.Format = strDateFormat;
            ceDeclineToDate.Format = strDateFormat;


            this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);

            #region Paging Config
            arrSearchVal = new ArrayList(intNoofSearch);
            ProPageNumRW = 1;
            TextBox txtPageSize = (TextBox)ucCustomPaging.FindControl("txtPageSize");
            if (txtPageSize.Text != "")
                ProPageSizeRW = Convert.ToInt32(txtPageSize.Text);
            else
                ProPageSizeRW = Convert.ToInt32(ConfigurationManager.AppSettings.Get("GridPageSize"));

            PageAssignValue obj = new PageAssignValue(this.AssignValue);
            ucCustomPaging.callback = obj;
            ucCustomPaging.ProPageNumRW = ProPageNumRW;
            ucCustomPaging.ProPageSizeRW = ProPageSizeRW;
            bCreate = ObjUserInfo.ProCreateRW;
            bModify = ObjUserInfo.ProModifyRW;
            bQuery = ObjUserInfo.ProViewRW;
            bIsActive = ObjUserInfo.ProIsActiveRW;
            bMakerChecker = ObjUserInfo.ProMakerCheckerRW;
            #endregion

            if (!IsPostBack)
            {
                FunPriLoadPage();
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Details);
                //FunPriBindGrid();
                //User Authorization
                if (!bIsActive)
                {
                    gvDeclineListHotlistEntry.Columns[1].Visible = false;
                    gvDeclineListHotlistEntry.Columns[0].Visible = false;
                    btnCreate.Enabled_False();
                    return;
                }
                if (!bModify)
                {
                    gvDeclineListHotlistEntry.Columns[1].Visible = false;
                }
                if (!bQuery)
                {
                    gvDeclineListHotlistEntry.Columns[0].Visible = false;
                }
                if (!bCreate)
                {
                    btnCreate.Enabled_False();
                }
                //Authorization Code end


                //TextBox txtname = ((TextBox)ddlNationality.FindControl("txtItemName"));
                //txtname.Focus();
                txtIdentityValue.Focus();
            }

            if (Session["RemoveLock"] != null)
            {
                Utility.FunPriRemoveLockedRow(intUserId, "0", "0");
                Session.Remove("RemoveLock");
            }

            txtDeclineFrom.Attributes.Add("onChange", "fnDoDate(this,'" + txtDeclineFrom.ClientID + "','" + strDateFormat + "',true,  false);");
            txtDeclineTo.Attributes.Add("onChange", "fnDoDate(this,'" + txtDeclineTo.ClientID + "','" + strDateFormat + "',false,  false);");


        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName); ;
        }

    }
    #endregion

    #region Page Events



    protected void btnCreate_Click(object sender, EventArgs e)
    {
        Response.Redirect(strRedirectPage, false);
    }


    #endregion

    #region Page Methods

    private void FunPriBindGrid()
    {
        try
        {
            //Paging Properties set
            dtUserRights = Utility.UserAccess(0, 0, intUserID, intProgramID, intCompanyID);
            int intTotalRecords = 0;
            bool bIsNewRow = false;
            ObjPaging.ProCompany_ID = intCompanyID;
            ObjPaging.ProUser_ID = intUserID;
            ObjPaging.ProTotalRecords = intTotalRecords;
            ObjPaging.ProCurrentPage = ProPageNumRW;
            ObjPaging.ProPageSize = ProPageSizeRW;
            ObjPaging.ProSearchValue = hdnSearch.Value;
            ObjPaging.ProOrderBy = hdnOrderBy.Value;

            //FunPriGetSearchValue();

            if ((!(string.IsNullOrEmpty(txtDeclineFrom.Text))) && ((string.IsNullOrEmpty(txtDeclineTo.Text))))
            {
                txtDeclineTo.Text = DateTime.Parse(DateTime.Now.ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);

            }

            Procparam = new Dictionary<string, string>();
            if (Procparam != null)
            {
                Procparam.Clear();
            }
            //if (ddlNationality.SelectedValue != "")
            //{
            //    Procparam.Add("@Nationality", ddlNationality.SelectedValue);
            //}
            if (!(string.IsNullOrEmpty(txtIdentityValue.Text)))
                Procparam.Add("@Nationality", txtIdentityValue.Text);

            if (!(string.IsNullOrEmpty(txtProspectName.Text)))
                Procparam.Add("@Prospect_Name", txtProspectName.Text);

            if (!(string.IsNullOrEmpty(txtNegativelistRemarks.Text)))
                Procparam.Add("@Remarks", txtNegativelistRemarks.Text);

            if (ddlNegativeList_Reason.SelectedIndex > 0)
            {
                Procparam.Add("@Reason", ddlNegativeList_Reason.SelectedValue);
            }

            if (!(string.IsNullOrEmpty(txtDeclineFrom.Text)))
                Procparam.Add("@Decline_From", Utility.StringToDate(txtDeclineFrom.Text).ToString());


            if (!(string.IsNullOrEmpty(txtDeclineTo.Text)))
                Procparam.Add("@Decline_To", Utility.StringToDate(txtDeclineTo.Text).ToString());

            gvDeclineListHotlistEntry.BindGridView(strProcName, Procparam, out intTotalRecords, ObjPaging, out bIsNewRow);

            //This is to hide first row if grid is empty
            if (bIsNewRow)
            {
                gvDeclineListHotlistEntry.Rows[0].Visible = false;
            }

            FunPriSetSearchValue();

            ucCustomPaging.Navigation(intTotalRecords, ProPageNumRW, ProPageSizeRW);
            ucCustomPaging.setPageSize(ProPageSizeRW);
            gvDeclineListHotlistEntry.Visible = true;
            ucCustomPaging.Visible = true;
            //Paging Config End
        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objFaultExp, strPageName);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }

    }

    #region Paging and Searching Methods For Grid


    private void FunPriGetSearchValue()
    {
        arrSearchVal = gvDeclineListHotlistEntry.FunPriGetSearchValue(arrSearchVal, intNoofSearch);
    }

    private void FunPriClearSearchValue()
    {
        gvDeclineListHotlistEntry.FunPriClearSearchValue(arrSearchVal);
    }

    private void FunPriSetSearchValue()
    {
        gvDeclineListHotlistEntry.FunPriSetSearchValue(arrSearchVal);
    }

    protected void FunProHeaderSearch(object sender, EventArgs e)
    {

        string strSearchVal = string.Empty;
        TextBox txtboxSearch;
        try
        {
            txtboxSearch = ((TextBox)sender);

            FunPriGetSearchValue();
            //Replace the corresponding fields needs to search in sqlserver

            for (int iCount = 0; iCount < arrSearchVal.Capacity; iCount++)
            {
                if (arrSearchVal[iCount].ToString() != "")
                {
                    strSearchVal += " and " + arrSortCol[iCount].ToString() + " like '%" + arrSearchVal[iCount].ToString() + "%'";
                }
            }

            if (strSearchVal.StartsWith(" and "))
            {
                strSearchVal = strSearchVal.Remove(0, 5);
            }

            hdnSearch.Value = strSearchVal;
            FunPriBindGrid();
            FunPriSetSearchValue();
            if (txtboxSearch.Text != "")
                ((TextBox)gvDeclineListHotlistEntry.HeaderRow.FindControl(txtboxSearch.ID)).Focus();

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    private string FunPriGetSortDirectionStr(string strColumn)
    {
        string strSortDirection = string.Empty;
        string strSortExpression = string.Empty;
        // By default, set the sort direction to ascending.
        string strOrderBy = string.Empty;
        strSortDirection = "DESC";
        try
        {
            // Retrieve the last strColumn that was sorted.
            // Check if the same strColumn is being sorted.
            // Otherwise, the default value can be returned.
            strSortExpression = hdnSortExpression.Value;
            if ((strSortExpression != "") && (strSortExpression == strColumn) && (hdnSortDirection.Value != null) && (hdnSortDirection.Value == "DESC"))
            {
                strSortDirection = "ASC";
            }
            // Save new values in hidden control.
            hdnSortDirection.Value = strSortDirection;
            hdnSortExpression.Value = strColumn;
            strOrderBy = " " + strColumn + " " + strSortDirection;
            hdnOrderBy.Value = strOrderBy;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
        return strSortDirection;
    }

    protected void FunProSortingColumn(object sender, EventArgs e)
    {
        arrSearchVal = new ArrayList(intNoofSearch);
        var imgbtnSearch = string.Empty;
        try
        {
            LinkButton lnkbtnSearch = (LinkButton)sender;
            string strSortColName = string.Empty;
            //To identify image button which needs to get chnanged
            imgbtnSearch = lnkbtnSearch.ID.Replace("lnkbtn", "imgbtn");

            for (int iCount = 0; iCount < arrSearchVal.Capacity; iCount++)
            {
                if (lnkbtnSearch.ID == "lnkbtnSort" + (iCount + 1).ToString())
                {
                    strSortColName = arrSortCol[iCount].ToString();
                    break;
                }
            }

            string strDirection = FunPriGetSortDirectionStr(strSortColName);
            FunPriBindGrid();

            if (strDirection == "ASC")
            {
                ((ImageButton)gvDeclineListHotlistEntry.HeaderRow.FindControl(imgbtnSearch)).CssClass = "styleImageSortingAsc";
            }
            else
            {

                ((ImageButton)gvDeclineListHotlistEntry.HeaderRow.FindControl(imgbtnSearch)).CssClass = "styleImageSortingDesc";
            }
        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objFaultExp, strPageName);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }

    #endregion


    #endregion

    #region Text Changed Events
    protected void txtDeclineFrom_TextChanged(object sender, EventArgs e)
    {
        FunDateValidation(1);
        txtDeclineFrom.Focus();
    }

    protected void txtDeclineTo_TextChanged(object sender, EventArgs e)
    {
        FunDateValidation(2);
        txtDeclineTo.Focus();
    }

    #endregion

    private void FunDateValidation(int No)
    {
        try
        {
            if ((!string.IsNullOrEmpty(txtDeclineFrom.Text)) && (!string.IsNullOrEmpty(txtDeclineTo.Text)))
            {
                if ((Convert.ToInt32(Utility.StringToDate(txtDeclineFrom.Text).ToString("yyyyMMdd"))) > (Convert.ToInt32(Utility.StringToDate(txtDeclineTo.Text).ToString("yyyyMMdd"))))
                {
                    Utility.FunShowAlertMsg(this, "Decline To should be greater than or equal to Decline From");
                    if (No == 1)
                    {
                        txtDeclineFrom.Text = string.Empty;
                        txtDeclineFrom.Focus();
                    }
                    else
                    {
                        txtDeclineTo.Text = string.Empty;
                        txtDeclineTo.Focus();
                    }
                    return;
                }
            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        ddlNationality.SelectedValue = "";
        ddlNationality.SelectedText = "";
        txtIdentityValue.Clear();
        txtProspectName.Clear();
        txtNegativelistRemarks.Clear();
        ddlNegativeList_Reason.SelectedIndex = 0;
        txtDeclineFrom.Clear();
        txtDeclineTo.Clear();
        gvDeclineListHotlistEntry.Visible = false;
        ucCustomPaging.Visible = false;

        //TextBox txtname = ((TextBox)ddlNationality.FindControl("txtItemName"));
        //txtname.Focus();
        txtIdentityValue.Focus();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        FunPriBindGrid();
    }
    private void FunPriLoadPage()
    {
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();

            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyId));
            Procparam.Add("@Lookup_Type", "NEGATIVELIST_REASON");
            Procparam.Add("@Table_Name", "S3G_ORG_Lookup");
            ddlNegativeList_Reason.BindDataTable("S3G_GET_COMMON_LOOKUP_VAL", Procparam, new string[] { "LOOKUP_CODE", "DESCRIPTION" });

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
        }
    }

    //[System.Web.Services.WebMethod]
    //public static string[] GetNationalityList(String prefixText, int count)
    //{
    //    Dictionary<string, string> Procparam;
    //    Procparam = new Dictionary<string, string>();
    //    List<String> suggestions = new List<String>();
    //    DataTable dtCommon = new DataTable();
    //    DataSet Ds = new DataSet();
    //    Procparam.Clear();
    //    Procparam.Add("@Company_ID", System.Web.HttpContext.Current.Session["AutoSuggestCompanyID"].ToString());
    //    Procparam.Add("@PrefixText", prefixText);
    //    suggestions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_GET_NATIONALITY_LIST", Procparam));
    //    return suggestions.ToArray();
    //}

    protected void gvDeclineListHotlistEntry_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                //User Authorization
                Label lblUserID = (Label)e.Row.FindControl("lblUserID");
                Label lblUserLevelID = (Label)e.Row.FindControl("lblUserLevelID");
                ImageButton imgbtnEdit = (ImageButton)e.Row.FindControl("imgbtnEdit");
                ImageButton imgbtnQuery = (ImageButton)e.Row.FindControl("imgbtnQuery");

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
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }

    protected void gvDeclineListHotlistEntry_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            string ProgramCodeToCompare = "DECHLE";
            FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(e.CommandArgument.ToString(), false, 0);
            switch (e.CommandName.ToLower())
            {
                case "modify":

                    string strUserRowLocked = Utility.FunPriCheckRowConcurrency(intUserID, ProgramCodeToCompare, e.CommandArgument.ToString(), Session.SessionID);
                    if (strUserRowLocked != "0")
                    {
                        Utility.FunShowAlertMsg(this, strUserRowLocked);
                        return;
                    }

                    Session["RemoveLock"] = "1";

                    Response.Redirect(strRedirectPage + "?qsNegativeListId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=M");
                    break;
                case "query":
                    Response.Redirect(strRedirectPage + "?qsNegativeListId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=Q");
                    break;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);

        }
    }
    #region User Access

    public bool IsUserAccessChecker(int Option)
    {
        try
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
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }

        return false;
    }

    #endregion
}