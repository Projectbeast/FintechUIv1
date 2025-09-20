﻿#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Orgination 
/// Screen Name			: Entity Master
/// Created By			: Nataraj Y
/// Created Date		: 24-June-2010
/// Purpose	            : 
#endregion

#region Namespaces
using System;
using System.Data;
using System.Web.Security;
using System.Web.UI.WebControls;
using S3GBusEntity;
using System.Collections;
using System.Configuration;
using System.ServiceModel;
using System.Collections.Generic;
#endregion

public partial class Origination_S3GOrgEntityMaster_View : ApplyThemeForProject
{
    #region Initialization
    // OrgMasterMgtServicesReference.OrgMasterMgtServicesClient ObjOrgMasterMgtServicesClient;
    #endregion

    #region Paging Config

    string strRedirectPage = "~/Origination/S3GOrgEntityMaster_Add.aspx";
    int intNoofSearch = 4;
    string[] arrSortCol = new string[] { "Entity_Type_Name", "Entity_Code", "Entity_Name", "CR_Number", "", "" };
    string strProcName = SPNames.S3G_ORG_GetEntityDetails_Paging;
    Dictionary<string, string> Procparam = null;
    public static DataTable dtUserRights;
    static string strPageName = "Entity Master View";

    ArrayList arrSearchVal = new ArrayList(1);
    int intUserID = 0;
    int intCompanyID = 0;
    int intProgramID = 32;
    //User Authorization variable declaration
    UserInfo ObjUserInfo = null;
    bool bCreate = false;
    bool bModify = false;
    bool bQuery = false;
    bool bIsActive = false;
    bool bMakerChecker = false;
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

            if (IsUserAccessChecker(1))
            {
                btnCreate.Enabled_True();
            }
            else
            {
                btnCreate.Enabled_False();
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }
    #endregion

    #region Page Events

    protected void grvEntityMaster_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(e.CommandArgument.ToString(), false, 0);
            switch (e.CommandName.ToLower())
            {
                case "modify":
                    Response.Redirect(strRedirectPage + "?qsEntityId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=M");
                    break;
                case "query":
                    Response.Redirect(strRedirectPage + "?qsEntityId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=Q");
                    break;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }

    protected void btnCreate_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect(strRedirectPage, false);
            btnCreate.Focus();//Added by Suseela
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
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
            //lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
            ClsPubCommErrorLogDB.CustomErrorRoutine(objFaultExp);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
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

            FunPriGetSearchValue();


            Procparam = new Dictionary<string, string>();
            //Procparam.Add("@Workflow_Sequence_ID", "0");

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
            //lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
            ClsPubCommErrorLogDB.CustomErrorRoutine(objFaultExp, strPageName);
        }
        catch (Exception ex)
        {
            //lblErrorMessage.Text = ex.Message;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }

    }

    #region Paging and Searching Methods For Grid


    private void FunPriGetSearchValue()
    {
        arrSearchVal = grvPaging.FunPriGetSearchValue(arrSearchVal, intNoofSearch);
    }

    private void FunPriClearSearchValue()
    {
        grvPaging.FunPriClearSearchValue(arrSearchVal);
    }

    private void FunPriSetSearchValue()
    {
        grvPaging.FunPriSetSearchValue(arrSearchVal);
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
                ((Button)grvPaging.HeaderRow.FindControl(imgbtnSearch)).CssClass = "styleImageSortingAsc";
            }
            else
            {

                ((Button)grvPaging.HeaderRow.FindControl(imgbtnSearch)).CssClass = "styleImageSortingDesc";
            }
        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            //lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
            ClsPubCommErrorLogDB.CustomErrorRoutine(objFaultExp);
        }
        catch (Exception ex)
        {
            //lblErrorMessage.Text = ex.Message;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    protected void grvPaging_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                //User Authorization
                Label lblUserID = (Label)e.Row.FindControl("lblUserID");
                Label lblUserLevelID = (Label)e.Row.FindControl("lblUserLevelID");
                LinkButton imgbtnEdit = (LinkButton)e.Row.FindControl("imgbtnEdit");
                LinkButton imgbtnQuery = (LinkButton)e.Row.FindControl("imgbtnQuery");

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
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }

    #endregion

    #endregion
    
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
