﻿/// Module Name     :   Origination
/// Screen Name     :   S3GOrgAuthorizationRuleCard_View
/// Created By      :   Ramesh M
/// Created Date    :   30-May-2010
/// Purpose         :   To View Authorization Rule card details
#region Namespaces
using System;
using System.Globalization;
using System.Resources;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using S3GBusEntity;
using System.ServiceModel;
using System.Text;
using System.Data;
using System.Configuration;
using System.Web.Security;
using System.Collections;
using System.Collections.Generic;
#endregion
public partial class Origination_S3GOrgAuthorizationRuleCard_View : ApplyThemeForProject
{
    #region Paging Config
    static string strPageName = "Authorization Rule Card";
    string strRedirectPage = "~/Origination/S3GOrgAuthorizationRuleCard_Add.aspx";
    int intNoofSearch = 4;
    //string[] arrSortCol = new string[] { "LOB", "Product", "Constitution", "Name", "Program", "" };


   //string[] arrSortCol = new string[] { "Upper(LOB_Code+ ' - ' + LOB_Name)", "Upper(Product_Code+ ' - ' + Product_Name)", "Upper(Constitution_Code + ' - ' + Constitution_Name)", "Upper(Program_Code + ' - ' + Program_Name)", "" };
    string[] arrSortCol = new string[] { "Upper(LOB_Name)", "Upper(Product_Name)", "Upper(Constitution_Name)", "Upper(Program_Name)", "" };
    //string[] arrSortCol = new string[] { "LOB_Code+ ' - ' + LOB_Name", "Product_Name", "Constitution_Name", "Program_Name", "" };
    string strProcName = "S3G_ORG_GetAuthorizationRuleCard_Paging";
    int intProgramID = 23;
    Dictionary<string, string> Procparam = null;
    public static DataTable dtUserRights;
    ArrayList arrSearchVal = new ArrayList(1);
    int intUserID = 0;
    int intCompanyID = 0;
    //User Authorization variable declaration
    UserInfo ObjUserInfo = null;
    bool bCreate = false;
    bool bModify = false;
    bool bQuery = false;
    bool bIsActive = false;
    bool bMakerChecker = false;
    //Declaration end
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

    #region PageLoad

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            arrSearchVal = new ArrayList(intNoofSearch);

            #region Paging Config

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

            #endregion

            //User Authorization
            ObjUserInfo = new UserInfo();
            intCompanyID = Convert.ToInt32(ObjUserInfo.ProCompanyIdRW);
            intUserID = Convert.ToInt32(ObjUserInfo.ProUserIdRW);

            bCreate = ObjUserInfo.ProCreateRW;
            bModify = ObjUserInfo.ProModifyRW;
            bQuery = ObjUserInfo.ProViewRW;
            bIsActive = ObjUserInfo.ProIsActiveRW;
            //bMakerChecker = ObjUserInfo.ProMakerCheckerRW;
            //Authorization Code end

            //UserInfo ObjUserInfo = new UserInfo();
            intCompanyID = ObjUserInfo.ProCompanyIdRW;
            intUserID = ObjUserInfo.ProUserIdRW;
            bModify = ObjUserInfo.ProModifyRW;
            bQuery = ObjUserInfo.ProViewRW;
            bCreate = ObjUserInfo.ProCreateRW;
            bIsActive = ObjUserInfo.ProIsActiveRW;

            if (!IsPostBack)
            {
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Details);
                FunPriBindGrid();

                //User Authorization
                if (!bIsActive)
                {
                    grvPaging.Columns[1].Visible = false;
                    grvPaging.Columns[0].Visible = false;
                    btnCreate.Enabled_False();
                    return;
                }
                if (!bModify)
                {
                    grvPaging.Columns[1].Visible = false;
                }
                if (!bQuery)
                {
                    grvPaging.Columns[0].Visible = false;
                }
                if (!bCreate)
                {
                    btnCreate.Enabled_False();
                }
                //Authorization Code end
                //Added by Suseela to set focus- code starts
                TextBox txtHeaderSearch1 = (TextBox)grvPaging.HeaderRow.FindControl("txtHeaderSearch1");
                txtHeaderSearch1.Focus();
                //Added by Suseela to set focus- code Ends
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            lblErrorMessage.InnerText = ex.Message;
        }
    }

    #endregion

    #region Page Methods

    /// <summary>
    /// This method is used to display LOB details
    /// </summary>
    private void FunPriBindGrid()
    {
        try
        {
            dtUserRights = Utility.UserAccess(0, 0, intUserID, intProgramID, intCompanyID);
            //Paging Properties set
            int intTotalRecords = 0;
            bool bIsNewRow = false;
            ObjPaging.ProCompany_ID = intCompanyID;
            ObjPaging.ProUser_ID = intUserID;
            ObjPaging.ProTotalRecords = intTotalRecords;
            ObjPaging.ProCurrentPage = ProPageNumRW;
            ObjPaging.ProPageSize = ProPageSizeRW;
            ObjPaging.ProSearchValue = hdnSearch.Value;
            ObjPaging.ProOrderBy = hdnOrderBy.Value;

            FunPriGetSearchValue();


            Procparam = new Dictionary<string, string>();
            //Procparam.Add("@CreditScore_ID", "0");

            grvPaging.BindGridView(strProcName, Procparam, out intTotalRecords, ObjPaging, out bIsNewRow);

            //This is to hide first row if grid is empty
            if (bIsNewRow)
            {
                grvPaging.Rows[0].Visible = false;
            }

            FunPriSetSearchValue();

            ucCustomPaging.Navigation(intTotalRecords, ProPageNumRW, ProPageSizeRW);
            ucCustomPaging.setPageSize(ProPageSizeRW);

            //Paging Config End
        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.InnerText = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.InnerText = ex.Message;
        }

    }
    #endregion

    #region Paging and Searching Methods For Grid


    private void FunPriGetSearchValue()
    {
        try
        {
            arrSearchVal = grvPaging.FunPriGetSearchValue(arrSearchVal, intNoofSearch);
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            lblErrorMessage.InnerText = ex.Message;
        }
    }

    private void FunPriClearSearchValue()
    {
        try
        {
            grvPaging.FunPriClearSearchValue(arrSearchVal);
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            lblErrorMessage.InnerText = ex.Message;
        }
    }

    private void FunPriSetSearchValue()
    {
        try
        {
            grvPaging.FunPriSetSearchValue(arrSearchVal);
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            lblErrorMessage.InnerText = ex.Message;
        }
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
                ((TextBox)grvPaging.HeaderRow.FindControl(txtboxSearch.ID)).Focus();

        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            lblErrorMessage.InnerText = ex.Message;
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
            string strSplitColumn = "";
            if (strColumn.Contains("+"))
            {
                strSplitColumn = strColumn.Substring(0, strColumn.IndexOf("+"));
            }
            else
            {
                strSplitColumn = strColumn;
            }
            strOrderBy = " " + strSplitColumn + " " + strSortDirection;
            hdnOrderBy.Value = strOrderBy;
        }
        catch (Exception ex)
        {
            lblErrorMessage.InnerText = ex.Message;
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
                ((Button)grvPaging.HeaderRow.FindControl(imgbtnSearch)).CssClass = "styleImageSortingAsc";
            }
            else
            {

                ((Button)grvPaging.HeaderRow.FindControl(imgbtnSearch)).CssClass = "styleImageSortingDesc";
            }
        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.InnerText = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.InnerText = ex.Message;
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    #endregion

    #region Page Events
    protected void grvPaging_RowDataBound(object sender, GridViewRowEventArgs e)
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
                Label lblActive = (Label)e.Row.FindControl("lblActive");
                CheckBox chkAct = (CheckBox)e.Row.FindControl("chkActive");
                if ((lblActive.Text == "True")||(lblActive.Text == "1"))
                {
                    chkAct.Checked = true;
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

                //if ((bModify) && (ObjUserInfo.IsUserLevelUpdate(Convert.ToInt32(lblUserID.Text), Convert.ToInt32(lblUserLevelID.Text))))
                //{
                //    imgbtnEdit.Enabled = true;
                //}
                //else
                //{
                //    imgbtnEdit.Enabled = false;
                //    imgbtnEdit.CssClass = "styleGridEditDisabled";
                //}
                //Authorization code end

            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.InnerText = ex.Message;
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }

    }

    protected void btnCreate_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect(strRedirectPage,false);
            btnCreate.Focus();//Added by Suseela
        }
        catch (Exception ex)
        {
            lblErrorMessage.InnerText = ex.Message;
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    protected void btnShowAll_Click(object sender, EventArgs e)
    {
        try
        {
            ProPageNumRW = 1;
            hdnSearch.Value = "";
            hdnOrderBy.Value = "";
            FunPriClearSearchValue();
            FunPriBindGrid();
            btnShowAll.Focus();//Added by Suseela
        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.InnerText = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.InnerText = ex.Message;
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    public bool FunPriCheckBool(string strActive)
    {
        return ((strActive == "True") ? true : false);

    }

    protected string FuncProConactFiledsStr(string strField1, string strField2)
    {
        return strField1 + "," + strField2;
    }

    protected void grvPaging_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            //DataKey dKey = grvPaging.DataKeys[grvPaging.SelectedIndex];
            //string lblCreditGuideTransaction_ID = (Label)grvPaging.DataKeyNames.vValues.ToString();  
            FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(e.CommandArgument.ToString(), false, 0);
            switch (e.CommandName.ToLower())
            {
                case "modify":
                    Response.Redirect(strRedirectPage + "?qsARC_ID=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=M");
                    break;
                case "query":
                    Response.Redirect(strRedirectPage + "?qsARC_ID=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=Q");
                    break;

            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.InnerText = ex.Message;
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

    #endregion
}
