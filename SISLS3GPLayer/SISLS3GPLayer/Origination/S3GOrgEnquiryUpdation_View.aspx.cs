﻿#region PageHeader
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name         :   Origination
/// Screen Name         :   EnquiryUpdation_View
/// Created By          :   Prabhu.K
/// Created Date        :   04-Aug-2010
/// Purpose             :   To view Enquiry details
/// Last Updated By		:   
/// Last Updated Date   :   
/// Reason              :   
/// <Program Summary>
#endregion

#region Namespaces
using System;
using System.ServiceModel;
using System.Web.UI.WebControls;
using S3GBusEntity;
using System.Data;
using System.Collections.Generic;
using System.Collections;
using System.Configuration;
using System.Web.Security;
#endregion

public partial class Origination_S3GOrgEnquiryUpdation_View : ApplyThemeForProject
{
    #region Paging Config

    string strRedirectPage = "~/Origination/S3GOrgEnquiryUpdation_Add.aspx";
    int intNoofSearch = 4;
    string[] arrSortCol = new string[] { "Enquiry_No", "CusMas.Customer_Code +'-'+CusMas.Customer_Name", "Facility_Amount", "Enquiry_Date", "" };
    string strProcName = "S3G_ORG_GetEnquiryUpdation_Paging";
    Dictionary<string, string> Procparam = null;

    ArrayList arrSearchVal = new ArrayList(1);
    int intUserID = 0;
    int intCompanyID = 0;
    PagingValues ObjPaging = new PagingValues();
    public string strDateFormat;
    //User Authorization variable declaration
    UserInfo ObjUserInfo = null;
    bool bCreate = false;
    bool bModify = false;
    bool bQuery = false;
    bool bIsActive = false;
    bool bMakerChecker = false;
    //Declaration end

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
        // Added by S.KAnnan - Start,
        S3GSession ObjS3GSession = new S3GSession();
        strDateFormat = ObjS3GSession.ProDateFormatRW;
        // Added by S.KAnnan - End.

        ObjUserInfo = new UserInfo();
        intCompanyID = ObjUserInfo.ProCompanyIdRW;
        this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);

        #region Paging Config
        arrSearchVal = new ArrayList(intNoofSearch);
        ProPageNumRW = 1;

        //User Authorization
        ObjUserInfo = new UserInfo();
        intCompanyID = ObjUserInfo.ProCompanyIdRW;
        intUserID = ObjUserInfo.ProUserIdRW;

        bCreate = ObjUserInfo.ProCreateRW;
        bModify = ObjUserInfo.ProModifyRW;
        bQuery = ObjUserInfo.ProViewRW;
        bIsActive = ObjUserInfo.ProIsActiveRW;
        //bMakerChecker = ObjUserInfo.ProMakerCheckerRW;
        //Authorization Code end

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

        if (!IsPostBack)
        {
            lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Details);
            FunPriBindGrid();
        }
        //User Authorization
        if (!bIsActive)
        {
            grvPaging.Columns[1].Visible = false;
            grvPaging.Columns[0].Visible = false;
            btnCreate.Enabled = false;
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
            btnCreate.Enabled = false;
        }
        //Authorization Code end
        FunPubSetIndex(1);

        //Code block added for Bug Fixing-Bug_ID- 5512 - Kuppusamy.B - 21-Feb-2012
        /*Starts*/
        Procparam = new Dictionary<string, string>();
        Procparam.Add("@Option", "788");
        Procparam.Add("@Param1", intCompanyID.ToString());
        DataTable dtCreate = Utility.GetDefaultData("S3G_ORG_GetCustomerLookUp", Procparam);

        if (dtCreate != null && dtCreate.Rows.Count > 0 && dtCreate.Rows[0][0].ToString() == "0")
        {
            btnCreate.Enabled = false;
        }
        /*Ends*/

    }
    #endregion

    #region Page Events

    protected void grvPaging_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            //User Authorization
            Label lblUserID = (Label)e.Row.FindControl("lblUserID");
            Label lblUserLevelID = (Label)e.Row.FindControl("lblUserLevelID");
            ImageButton imgbtnEdit = (ImageButton)e.Row.FindControl("imgbtnEdit");

            // Added by S.Kannan - Start,
            Label lblEnquiryDate = (Label)e.Row.FindControl("lblEnquiryDate");

            if (lblEnquiryDate != null && (!(string.IsNullOrEmpty(lblEnquiryDate.Text))))
                lblEnquiryDate.Text = Convert.ToDateTime(lblEnquiryDate.Text).ToString(strDateFormat);
            // Added by S.Kannan - End.


            if ((bModify) && (ObjUserInfo.IsUserLevelUpdate(Convert.ToInt32(lblUserID.Text), Convert.ToInt32(lblUserLevelID.Text))))
            {
                imgbtnEdit.Enabled = true;
            }
            else
            {
                // Code Modify by R. Manikandan 03 - NOV - 2014 to allow modify Enq Upd
                // NCPM Customization point No 45
                //imgbtnEdit.Enabled = false;
                //imgbtnEdit.CssClass = "styleGridEditDisabled";
                imgbtnEdit.Enabled = true;
                // Modification End by R. Manikandan 03 - NOV - 2014 to allow modify Enq Upd
               
            }
            //Authorization code end

        }

        //Added by Thangam M on 13/Mar/2012 to fix bug id - 6022
        else if (e.Row.RowType == DataControlRowType.Header)
        {
            TextBox txtHeaderSearch4 = (TextBox)e.Row.FindControl("txtHeaderSearch4");
            AjaxControlToolkit.CalendarExtender clEnqDate = (AjaxControlToolkit.CalendarExtender)e.Row.FindControl("clEnqDate");

            txtHeaderSearch4.Attributes.Add("readonly", "readonly");
            clEnqDate.Format = strDateFormat;
        }
        //End Here

    }

    protected void grvPaging_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "modify")
        {
            FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(e.CommandArgument.ToString(), false, 0);
            Response.Redirect(strRedirectPage + "?qsEnquiryUpdationId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=M");
        }
        if (e.CommandName.ToLower() == "query")
        {
            FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(e.CommandArgument.ToString(), false, 0);
            Response.Redirect(strRedirectPage + "?qsEnquiryUpdationId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=Q");
        }
    }

    protected void btnCreate_Click(object sender, EventArgs e)
    {
        if (Session["EnqNewCustomerId"] != null)
            Session["EnqNewCustomerId"] = null;
        Response.Redirect(strRedirectPage,false);
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

    #region Page Methods

    private void FunPriBindGrid()
    {
        try
        {
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
            lblErrorMessage.InnerText = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.InnerText = ex.Message;
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
                // icoung=3 (searching the value based on Enquiry date so we can not put like )
                if (iCount == 3)
                {
                    if (arrSearchVal[iCount].ToString() != "")
                    {
                        strSearchVal += " and " +  arrSortCol[iCount].ToString() + " = '" + Utility.StringToDate(arrSearchVal[iCount].ToString()) + " '";
                    }
                }
                else if (arrSearchVal[iCount].ToString() != "")
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
                ((ImageButton)grvPaging.HeaderRow.FindControl(imgbtnSearch)).CssClass = "styleImageSortingAsc";
            }
            else
            {

                ((ImageButton)grvPaging.HeaderRow.FindControl(imgbtnSearch)).CssClass = "styleImageSortingDesc";
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

    #endregion

}
