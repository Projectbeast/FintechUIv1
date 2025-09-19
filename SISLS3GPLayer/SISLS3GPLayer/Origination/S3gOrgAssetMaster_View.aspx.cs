#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Orgination 
/// Screen Name			: Asset Master
/// Created By			: Nataraj Y
/// Created Date		: 29-May-2010
/// Purpose	            : 
/// Last Updated By		: Nataraj Y
/// Last Updated Date   : 31-May-2010
/// Reason              : Asset Master
/// Last Updated By		: Nataraj Y
/// Last Updated Date   : 01-June-2010
/// Reason              : Asset Master
/// Last Updated By		: Nataraj Y
/// Last Updated Date   : 21-June-2010
/// Reason              : Asset Master 
/// Last Updated By		: Nataraj Y
/// Last Updated Date   : 02-July-2010
/// Reason              : Table changes incorparation
/// Last Updated By		: Nataraj Y
/// Last Updated Date   : 03-July-2010
/// Reason              : New Grid paging
/// Last Updated By		: Nataraj Y
/// Last Updated Date   : 26-July-2010
/// Reason              : New Grid paging
/// <Program Summary>
#endregion

#region NameSpaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using S3GBusEntity;
using System.Data;
using System.Security;
using System.Web.Security;
using System.Configuration;
using System.ServiceModel;
#endregion

public partial class Origination_S3g_OrgAssetMaster_View : ApplyThemeForProject
{
    #region Intialization
    // OrgMasterMgtServicesReference.OrgMasterMgtServicesClient ObjOrgMasterMgtServicesClient;
    string strRedirectPageAdd = "~/Origination/S3gOrgAssetMaster_Add.aspx";
    #endregion

    #region Paging Config

    string strSearchVal1 = string.Empty;
    string strSearchVal2 = string.Empty;
    string strSearchVal3 = string.Empty;
    string strSearchVal4 = string.Empty;
    string strSearchVal5 = string.Empty;
    int intUserID = 0;
    int intCompanyID = 0;
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
        if (tcAsset.ActiveTabIndex == 0)
        {
            FunPriBindGrid(tccategoryCodes.ActiveTab.HeaderText.ToUpper());
        }
        else
        {
            FunPriBindGrid("ASSET CODE");
        }
    }
    #endregion

    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {
        /*
        //_btnCreate.Attributes.Add("disabled", "disabled");  // enable false
        //_btnCreate.Attributes.Add("class", "css_btn_disabled");  // enable false css

        _btnCreate.Attributes.Remove("disabled");// enable true
        _btnCreate.Attributes.Add("class", "css_btn_enabled");  // enable true css


        _btnShowAll.Attributes.Add("disabled", "disabled");  // enable false
        _btnShowAll.Attributes.Add("class", "css_btn_disabled");  // enable false css
        */
        ObjUserInfo = new UserInfo();
        intCompanyID = ObjUserInfo.ProCompanyIdRW;
        intUserID = ObjUserInfo.ProUserIdRW;

        bCreate = ObjUserInfo.ProCreateRW;
        bModify = ObjUserInfo.ProModifyRW;
        bQuery = ObjUserInfo.ProViewRW;
        bIsActive = ObjUserInfo.ProIsActiveRW;
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
        bCreate = ObjUserInfo.ProCreateRW;
        bModify = ObjUserInfo.ProModifyRW;
        bQuery = ObjUserInfo.ProViewRW;
        bIsActive = ObjUserInfo.ProIsActiveRW;
        bMakerChecker = ObjUserInfo.ProMakerCheckerRW;
        #endregion
        if (!IsPostBack)
        {
            lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Details);
            FunPriBindGrid(tccategoryCodes.ActiveTab.HeaderText.ToUpper());
            //User Authorization
            if (!bIsActive)
            {
                grvAssetCategoryCodes.Columns[1].Visible = false;
                grvAssetCategoryCodes.Columns[0].Visible = false;
                //btnCreate.Enabled = false;
                btnCreate.Attributes.Add("disabled", "disabled");  // enable false
                btnCreate.Attributes.Add("class", "css_btn_disabled");  // enable false css
                return;
            }

            if (!bModify)
            {
                grvAssetCategoryCodes.Columns[1].Visible = false;
                grvAssetMaster.Columns[1].Visible = false;
            }
            if (!bQuery)
            {
                grvAssetCategoryCodes.Columns[0].Visible = false;
                grvAssetMaster.Columns[0].Visible = false;
            }
            if (!bCreate)
            {
                //btnCreate.Enabled = false;
                btnCreate.Attributes.Add("disabled", "disabled");  // enable false
                btnCreate.Attributes.Add("class", "css_btn_disabled");  // enable false css
            }
            //Authorization Code end
            btnCreate.Focus();
        }


    }
    #endregion

    #region Page Events

    protected void btnCreate_Click(object sender, EventArgs e)
    {
        // edited by s.kannan - Bug ID:  1765
        if (tcAsset.ActiveTabIndex == 0)  // Create mode by Category Code
        {
            Response.Redirect("~/Origination/S3GOrgAssetMaster_Add.aspx?Type=Category&qsMode=C");
        }
        else     // Create mode by Asset Code
        {
            Response.Redirect("~/Origination/S3GOrgAssetMaster_Add.aspx?Type=Code&qsMode=C");
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
            if (tcAsset.ActiveTabIndex == 0)
            {
                FunPriBindGrid(tccategoryCodes.ActiveTab.HeaderText.ToUpper());
            }
            else
            {
                FunPriBindGrid("ASSET CODE");
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    protected void grvAssetMaster_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(e.CommandArgument.ToString(), false, 0);
        switch (e.CommandName.ToLower())
        {
            case "modify":
                Response.Redirect(strRedirectPageAdd + "?qsAssetId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=M&Type=Code");
                break;

            case "query":
                Response.Redirect(strRedirectPageAdd + "?qsAssetId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=Q&Type=Code");
                break;
        }
    }

    protected void grvAssetClassCodes_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(e.CommandArgument.ToString(), false, 0);
        switch (e.CommandName.ToLower())
        {
            case "modify":
                Response.Redirect(strRedirectPageAdd + "?qsAssetCatId=" + FormsAuthentication.Encrypt(Ticket) + "&qsCatType=" + tccategoryCodes.ActiveTab.HeaderText.ToString() + "&qsMode=M&Type=Category");
                break;
            case "query":
                Response.Redirect(strRedirectPageAdd + "?qsAssetCatId=" + FormsAuthentication.Encrypt(Ticket) + "&qsCatType=" + tccategoryCodes.ActiveTab.HeaderText.ToString() + "&qsMode=Q&Type=Category");
                break;
        }
    }

    protected void tccategoryCodes_ActiveTabChanged(object sender, EventArgs e)
    {
        FunPriClearSearchValue();
        hdnSearch.Value = "";
        hdnOrderBy.Value = "";
        FunPriBindGrid(tccategoryCodes.ActiveTab.HeaderText.ToUpper());
    }

    protected void tcAsset_ActiveTabChanged(object sender, EventArgs e)
    {
        FunPriClearSearchValue();
        hdnSearch.Value = "";
        hdnOrderBy.Value = "";
        if (tcAsset.ActiveTab.HeaderText.ToUpper() == "ASSET CODE")
        {

            FunPriBindGrid(tcAsset.ActiveTab.HeaderText.ToUpper());
            if (!bIsActive)
            {
                grvAssetMaster.Columns[1].Visible = false;
                grvAssetMaster.Columns[0].Visible = false;

                //btnCreate.Enabled = false;
                btnCreate.Attributes.Add("disabled", "disabled");  // enable false
                btnCreate.Attributes.Add("class", "css_btn_disabled");  // enable false css
            }
            else
            {
                if (!bModify)
                {
                    grvAssetMaster.Columns[1].Visible = false;
                }
                if (!bQuery)
                {
                    grvAssetMaster.Columns[0].Visible = false;
                }
                if (!bCreate)
                {
                    //btnCreate.Enabled = false;
                    btnCreate.Attributes.Add("disabled", "disabled");  // enable false
                    btnCreate.Attributes.Add("class", "css_btn_disabled");  // enable false css
                }
            }
        }
        else
        {
            tccategoryCodes.ActiveTabIndex = 0;
            FunPriBindGrid("CLASS");
            if (!bIsActive)
            {
                grvAssetCategoryCodes.Columns[1].Visible = false;
                grvAssetCategoryCodes.Columns[0].Visible = false;
                //btnCreate.Enabled = false;
                btnCreate.Attributes.Add("disabled", "disabled");  // enable false
                btnCreate.Attributes.Add("class", "css_btn_disabled");  // enable false css
            }
            else
            {
                if (!bModify)
                {
                    grvAssetCategoryCodes.Columns[1].Visible = false;
                }
                if (!bQuery)
                {
                    grvAssetCategoryCodes.Columns[0].Visible = false;
                }
                if (!bCreate)
                {
                    //btnCreate.Enabled = false;
                    btnCreate.Attributes.Add("disabled", "disabled");  // enable false
                    btnCreate.Attributes.Add("class", "css_btn_disabled");  // enable false css
                }
            }
        }
    }

    #endregion

    #region Page Methods

    protected string FuncProConactFiledsStr(string strField1, string strField2)
    {
        return strField1 + "," + strField2;
    }

    /// <summary>
    /// This method is used to display Company details
    /// </summary>
    private void FunPriBindGrid(string strCategoryType)
    {
        try
        {
            int intTotalRecords = 0;
            ObjPaging.ProCompany_ID = intCompanyID;
            ObjPaging.ProUser_ID = intUserID;
            ObjPaging.ProTotalRecords = intTotalRecords;
            ObjPaging.ProCurrentPage = ProPageNumRW;
            ObjPaging.ProPageSize = ProPageSizeRW;
            ObjPaging.ProSearchValue = hdnSearch.Value;
            ObjPaging.ProOrderBy = hdnOrderBy.Value;

            bool blnIsNewRow = false;
            FunPriGetSearchValue();
            Dictionary<string, string> Procparam = new Dictionary<string, string>();

            if (strCategoryType.ToUpper() == "ASSET CODE")
            {
                grvAssetMaster.BindGridView(SPNames.S3G_ORG_GetAssetDetails_Paging, Procparam, out intTotalRecords, ObjPaging, out blnIsNewRow);
            }
            else
            {
                try
                {
                    Procparam.Add("@Category_Type", strCategoryType);
                    grvAssetCategoryCodes.BindGridView(SPNames.S3G_ORG_GetAssetCategoryDetails_Paging, Procparam, out intTotalRecords, ObjPaging, out blnIsNewRow);
                }
                catch (Exception ex)
                {
                    ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
                }
            }
            //Paging Config


            lblErrorMessage.InnerText = string.Empty;
            //This is to show grid header

            //This is to hide first row if grid is empty
            if (blnIsNewRow)
            {
                if (grvAssetCategoryCodes.Rows.Count >= 1)
                {
                    grvAssetCategoryCodes.Rows[0].Visible = false;
                }
                if (grvAssetMaster.Rows.Count >= 1)
                {
                    grvAssetMaster.Rows[0].Visible = false;
                }
            }

            FunPriSetSearchValue();

            ucCustomPaging.Navigation(intTotalRecords, ProPageNumRW, ProPageSizeRW);


            ucCustomPaging.setPageSize(ProPageSizeRW);

            //Paging Config End

        }

        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            lblErrorMessage.InnerText = ex.Message;
        }

    }

    /// <summary>
    /// To Get search value to display value after sorting or paging changed
    /// </summary>

    private void FunPriGetSearchValue()
    {
        if (tcAsset.ActiveTabIndex == 0)
        {
            if (grvAssetCategoryCodes.HeaderRow != null)
            {
                strSearchVal1 = ((TextBox)grvAssetCategoryCodes.HeaderRow.FindControl("txtHeaderSearch1")).Text.Trim();
                strSearchVal2 = ((TextBox)grvAssetCategoryCodes.HeaderRow.FindControl("txtHeaderSearch2")).Text.Trim();

            }
        }
        else
        {
            if (grvAssetMaster.HeaderRow != null)
            {
                strSearchVal1 = ((TextBox)grvAssetMaster.HeaderRow.FindControl("txtHeaderSearch1")).Text.Trim();
                strSearchVal2 = ((TextBox)grvAssetMaster.HeaderRow.FindControl("txtHeaderSearch2")).Text.Trim();
                strSearchVal3 = ((TextBox)grvAssetMaster.HeaderRow.FindControl("txtHeaderSearch3")).Text.Trim();
            }
        }
    }

    /// <summary>
    /// To clear value after show all is clicked
    /// </summary>
    private void FunPriClearSearchValue()
    {
        if (tcAsset.ActiveTabIndex == 0)
        {
            if (grvAssetCategoryCodes.HeaderRow != null)
            {
                ((TextBox)grvAssetCategoryCodes.HeaderRow.FindControl("txtHeaderSearch1")).Text = "";
                ((TextBox)grvAssetCategoryCodes.HeaderRow.FindControl("txtHeaderSearch2")).Text = "";

               


            }
        }
        else
        {
            if (grvAssetMaster.HeaderRow != null)
            {
                ((TextBox)grvAssetMaster.HeaderRow.FindControl("txtHeaderSearch1")).Text = "";
                ((TextBox)grvAssetMaster.HeaderRow.FindControl("txtHeaderSearch2")).Text = "";
                if (((TextBox)grvAssetMaster.HeaderRow.FindControl("txtHeaderSearch3")) != null)
                ((TextBox)grvAssetMaster.HeaderRow.FindControl("txtHeaderSearch3")).Text = "";



            }
        }
    }
    /// <summary>
    /// Tos et search value after sorting or paging changed
    /// </summary>
    private void FunPriSetSearchValue()
    {
        if (tcAsset.ActiveTabIndex == 0)
        {
            if (grvAssetCategoryCodes.HeaderRow != null)
            {
                ((TextBox)grvAssetCategoryCodes.HeaderRow.FindControl("txtHeaderSearch1")).Text = strSearchVal1;
                ((TextBox)grvAssetCategoryCodes.HeaderRow.FindControl("txtHeaderSearch2")).Text = strSearchVal2;

            }
        }
        else
        {
            if (grvAssetMaster.HeaderRow != null)
            {
                ((TextBox)grvAssetMaster.HeaderRow.FindControl("txtHeaderSearch1")).Text = strSearchVal1;
                ((TextBox)grvAssetMaster.HeaderRow.FindControl("txtHeaderSearch2")).Text = strSearchVal2;
                ((TextBox)grvAssetMaster.HeaderRow.FindControl("txtHeaderSearch3")).Text = strSearchVal3;
            }
        }
    }


    /// <summary>
    /// To Search in Grid view Gets the text box as sender and gets its text
    /// </summary>
    /// <param name="sender">Text box in gridview</param>
    /// <param name="e"></param>
    protected void FunProHeaderSearch(object sender, EventArgs e)
    {
        string strSearchVal = String.Empty;
        TextBox txtboxSearch;
        try
        {
            txtboxSearch = ((TextBox)sender);
            FunPriGetSearchValue();
            if (strSearchVal1 != "")
            {
                if (tcAsset.ActiveTabIndex == 0)
                {
                    strSearchVal += grvAssetCategoryCodes.Columns[2].SortExpression + " like '%" + strSearchVal1 + "%'";
                }
                else
                {
                    strSearchVal += grvAssetMaster.Columns[2].SortExpression + " like '%" + strSearchVal1 + "%'";
                }
            }
            if (strSearchVal2 != "")
            {
                if (tcAsset.ActiveTabIndex == 0)
                {
                    strSearchVal += " and " + grvAssetCategoryCodes.Columns[3].SortExpression + " like '%" + strSearchVal2 + "%'";
                }
                else
                {
                    strSearchVal += " and " + grvAssetMaster.Columns[3].SortExpression + " like '%" + strSearchVal2 + "%'";
                }
            }
            if (strSearchVal3 != "")
            {
                if (tcAsset.ActiveTabIndex == 0)
                {
                    strSearchVal += " and " + grvAssetCategoryCodes.Columns[4].SortExpression + " like '%" + strSearchVal3 + "%'";
                }
                else
                {
                    strSearchVal += " and " + grvAssetMaster.Columns[4].SortExpression + " like '%" + strSearchVal3 + "%'";
                }
            }

            if (strSearchVal.StartsWith(" and "))
            {
                strSearchVal = strSearchVal.Remove(0, 5);
            }



            hdnSearch.Value = strSearchVal;
            if (tcAsset.ActiveTabIndex == 0)
            {
                FunPriBindGrid(tccategoryCodes.ActiveTab.HeaderText.ToUpper());
            }
            else
            {
                FunPriBindGrid("ASSET CODE");
            }
            FunPriSetSearchValue();
            if (txtboxSearch.Text != "")
                //((TextBox)grvAssetCategoryCodes.HeaderRow.FindControl(txtboxSearch.ID)).Focus();
                txtboxSearch.Focus();

        }
        catch (Exception ex)
        {
            lblErrorMessage.InnerText = "No Records Found";
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    /// <summary>
    /// Gets the Sort Direction of the Column in the Grid View Using View State
    /// </summary>
    /// <param name="column"> Colunm Name is Passed</param>
    /// <returns>Sort Direction as a String </returns>
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

    /// <summary>
    /// Will Perform Sorting On Colunm base upon the image id calling the function
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void FunProSortingColumn(object sender, EventArgs e)
    {

        var imgbtnSearch = string.Empty;
        try
        {
            LinkButton lnkbtnSearch = (LinkButton)sender;
            string strSortColName = string.Empty;
            //To identify image button which needs to get chnanged
            imgbtnSearch = lnkbtnSearch.ID.Replace("lnkbtn", "imgbtn");
            switch (lnkbtnSearch.ID)
            {
                case "lnkbtnSort1":
                    if (tcAsset.ActiveTabIndex == 0)
                    {
                        strSortColName = "Category_Code";
                    }
                    else
                        strSortColName = "Asset_Code";
                    break;
                case "lnkbtnSort2":
                    if (tcAsset.ActiveTabIndex == 0)
                    {
                        strSortColName = "Category_Description";
                    }
                    else
                        strSortColName = "Asset_Description";
                    break;
                case "lnkbtnSort3":
                    strSortColName = "Name";
                    break;
            }

            string strDirection = FunPriGetSortDirectionStr(strSortColName);
            if (tcAsset.ActiveTabIndex == 0)
            {
                FunPriBindGrid(tccategoryCodes.ActiveTab.HeaderText.ToUpper());
            }
            else
            {
                FunPriBindGrid(tcAsset.ActiveTab.HeaderText.ToUpper());
            }
            if (strDirection == "ASC")
            {
                if (tcAsset.ActiveTabIndex == 0)
                {
                    ((Button)grvAssetCategoryCodes.HeaderRow.FindControl(imgbtnSearch)).CssClass = "styleImageSortingAsc";
                }
                else
                {
                    ((Button)grvAssetMaster.HeaderRow.FindControl(imgbtnSearch)).CssClass = "styleImageSortingAsc";
                }
            }
            else
            {
                if (tcAsset.ActiveTabIndex == 0)
                {
                    ((Button)grvAssetCategoryCodes.HeaderRow.FindControl(imgbtnSearch)).CssClass = "styleImageSortingDesc";
                }
                else
                {
                    ((Button)grvAssetMaster.HeaderRow.FindControl(imgbtnSearch)).CssClass = "styleImageSortingDesc";
                }
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


    protected void grvAssetCategoryCodes_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            //User Authorization
            Label lblUserID = (Label)e.Row.FindControl("lblUserID");
            Label lblUserLevelID = (Label)e.Row.FindControl("lblUserLevelID");
            ImageButton imgbtnEdit = (ImageButton)e.Row.FindControl("imgbtnEdit");

            if ((bModify) && (ObjUserInfo.IsUserLevelUpdate(Convert.ToInt32(lblUserID.Text), Convert.ToInt32(lblUserLevelID.Text))))
            {
                imgbtnEdit.Enabled = true;
            }
            else
            {
                imgbtnEdit.Enabled = false;
                imgbtnEdit.CssClass = "styleGridEditDisabled";
            }
            //Authorization code end

        }
    }
}
