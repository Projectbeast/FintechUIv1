﻿#region PageHeader
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name               : Common control
/// Screen Name               : LOV User Control
/// Created By                : Tamilselvan.S
/// Created Date              : 09-Mar-2011
/// Purpose                   : 
/// Last Updated By           : 
/// Last Updated Date         : 
/// Reason                    :

/// <Program Summary>
#endregion

#region [Namespace]
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;
using S3GBusEntity;
#endregion [Namespace]

public partial class UserControls_S3GDynamicLOV : System.Web.UI.UserControl
{
    #region [Local Variables & Paging Properties]

    PagingValues ObjPaging = new PagingValues();                                // grid paging
    public delegate void LOVPageAssignValue(int ProPageNumRW, int intPageSize);
    Dictionary<string, string> Procparam = new Dictionary<string, string>();

    UserInfo ObjUserInfo;                                                       // to maintain the user information      
    ArrayList arrSearchVal = new ArrayList(1);
    int intNoofSearch = 2;

    public delegate void LoadCustomerHandler(object Sender, EventArgs e);
    public event LoadCustomerHandler LoadCusotmer;

    static string[] arrSortCol = new string[] { };
    public int _ProPageNumRW = 1;
    public int ProPageNumRW                                                     // to retain the current page size and number
    {
        get { return _ProPageNumRW; }
        set { _ProPageNumRW = value; }
    }

    public int ProPageSizeRW
    {
        get;
        set;
    }

    public Unit Width
    {
        set
        {
            pnlLoadLOV.Width = gvList.Width = value;
        }
    }

    public static Dictionary<string, string> _Procparam = new Dictionary<string, string>();
    public Dictionary<string, string> ucProcparam
    {
        get { return _Procparam; }
        set { _Procparam = value; }
    }

    private static string _gvDisplay;
    public string gvDisplay
    {
        get { return _gvDisplay; }
        set { _gvDisplay = value; }
    }

    private static string _LOVCode;
    public string LOVCode
    {
        get { return _LOVCode; }
        set { _LOVCode = value; }
    }

    public string SelectedValue
    {
        get { return hdnID.Value; }
        set { hdnID.Value = value; }
    }

    public string SelectedText
    {
        get { return hdnText.Value; }
        set { hdnText.Value = value; }
    }

    #region [Dispaly Column Details]

    public int intNoofColumn = 2;
    public string[] arrColumns = new string[2];

    #endregion [Dispaly Column Details]

    #endregion [Local Variables & Paging Properties]

    #region [Event's]

    #region [Page Event's]

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            FunPubPageLoad();
        }
        catch (Exception ex)
        { }
    }

    #endregion [Page Event's]

    #region [Grid Event's]

    public void Show()
    {
        pnlLoadLOV.Visible = true;
        btnGetLOV_Click(null, null);
    }

    public void Hide()
    {
        pnlLoadLOV.Visible = false;
    }

    public void Clear()
    {
        hdnText.Value = hdnID.Value = "";
        pnlLoadLOV.Visible = false;
    }

    protected void gvList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        //if (e.CommandName == "Select")
        //{
        if (e.CommandArgument.ToString().Trim() != "")
        {
            hdnText.Value = e.CommandArgument.ToString();//((LinkButton)e.CommandSource).Text;           
            int intRowIndex = Utility.FunPubGetGridRowID("gvList", ((LinkButton)e.CommandSource).ClientID);
            hdnID.Value = ((LinkButton)gvList.Rows[intRowIndex].FindControl("lnk0")).Text;
            pnlLoadLOV.Visible = false;

            if (LoadCusotmer != null)
            {
                LoadCusotmer(null, null);
            }
            //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Script", "javascript:fnLoadCustomer();", true);
        }
        else
        {
            //Utility.FunShowAlertMsg((Control)sender, "Selected row does not have " + gvDisplay.Replace("_", " ") + " ");
            //ucMPE.Show();
        }

        //}
    }

    #endregion [Grid Event's]

    #region [Button Event's]

    public void btnGetLOV_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["Is_Valid_Call"] = "1";
            if (arrSearchVal.Count > 0)
            {
                arrSearchVal.Clear();
            }
            ProPageNumRW = 1;

            hdnSearch.Value = "";
            hdnOrderBy.Value = "";
            FunPriClearSearchValue();
            FunPubOnPopupGridLoad();
            pnlLoadLOV.Visible = true;
        }
        catch (Exception ex)
        { }
    }

    #endregion [Button Event's]

    #region [Text Box Event's]

    protected void FunProHeaderSearch(object sender, EventArgs e)
    {
        intNoofSearch = arrColumns.Length;
        arrSearchVal = new ArrayList(intNoofSearch);
        string strSearchVal = string.Empty;
        TextBox txtboxSearch;
        try
        {
            txtboxSearch = ((TextBox)sender);
            FunPriGetSearchValue();
            //Replace the corresponding fields needs to search in sqlserver

            for (int iCount = 0; iCount < arrSearchVal.Count; iCount++)
            {
                if (arrSearchVal[iCount].ToString() != "")
                {
                    ViewState["Is_Valid_Call"] = 1;
                    if (strSearchVal == "")
                        strSearchVal = " and UPPER(" + arrColumns[iCount + 1].ToString() + ") like '%" + arrSearchVal[iCount].ToString().ToUpper() + "%'";
                    else
                        strSearchVal += " and UPPER(" + arrColumns[iCount + 1].ToString() + ") like '%" + arrSearchVal[iCount].ToString().ToUpper() + "%'";
                }
            }

            hdnSearch.Value = strSearchVal;
            FunPubOnPopupGridLoad();
            if (txtboxSearch.Text != "")
                ((TextBox)gvList.HeaderRow.FindControl(txtboxSearch.ID)).Focus();
            pnlLoadLOV.Visible = true;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    protected void FunProSortingColumn(object sender, EventArgs e)
    {
        intNoofSearch = arrColumns.Length;
        arrSearchVal = new ArrayList(intNoofSearch);
        var imgbtnSearch = string.Empty;
        try
        {
            LinkButton lnkbtnSearch = (LinkButton)sender;
            string strSortColName = string.Empty;
            //To identify image button which needs to get chnanged
            imgbtnSearch = lnkbtnSearch.ID.Replace("lnkbtn", "imgbtn");

            int ii = 0;
            foreach (string strclm in arrColumns)
            {
                if (lnkbtnSearch.ID == "lnkHeader" + ii.ToString())//strclm.Replace("_", ""))                
                {
                    strSortColName = strclm.ToString();
                    break;
                }
                ii++;
            }
            string strDirection = FunPriGetSortDirectionStr(strSortColName);
            FunPubOnPopupGridLoad();

            if (strDirection == "ASC")
            {
                ((ImageButton)gvList.HeaderRow.FindControl("imgbtnSearch" + ii)).CssClass = "styleImageSortingAsc";
            }
            else
            {
                ((ImageButton)gvList.HeaderRow.FindControl("imgbtnSearch" + ii)).CssClass = "styleImageSortingDesc";
            }

            pnlLoadLOV.Visible = true;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    #endregion [Text Box Event's]

    #endregion [Event's]

    #region [Function's]

    public void FunPubPageLoad()
    {
        try
        {
            TextBox txtPageSize = (TextBox)ucLOVPageNavigater.FindControl("txtPageSize");
            TextBox txtGotoPage = (TextBox)ucLOVPageNavigater.FindControl("txtGotoPage");
            Label lblCurrentPAge = (Label)ucLOVPageNavigater.FindControl("lblCurrentPage");

            #region Grid Paging Config
            arrSearchVal = new ArrayList(intNoofSearch);
            //lblCurrentPAge.Text = txtGotoPage.Text=  lblCurrentPAge.Text = "";

            if (txtPageSize.Text != "")
                ProPageSizeRW = Convert.ToInt32(txtPageSize.Text.Split(',')[txtPageSize.Text.Split(',').Length - 1]);
            else
                ProPageSizeRW = Convert.ToInt32(ConfigurationManager.AppSettings.Get("GridPageSize"));

            if (lblCurrentPAge.Text != "")
                ProPageNumRW = Convert.ToInt32(lblCurrentPAge.Text);

            if (txtGotoPage.Text != "")
                ProPageNumRW = Convert.ToInt32(txtGotoPage.Text.Split(',')[txtGotoPage.Text.Split(',').Length - 1]);
            if (ProPageNumRW == 0)
                ProPageNumRW = 1;

            LOVPageAssignValue obj = new LOVPageAssignValue(this.AssignValue);
            ucLOVPageNavigater.callback = obj;
            ucLOVPageNavigater.ProPageNumRW = ProPageNumRW;
            ucLOVPageNavigater.ProPageSizeRW = ProPageSizeRW;

            if (!IsPostBack)
            {
                pnlLoadLOV.Visible = false;
                ViewState["Is_Valid_Call"] = "0";
            }
            else
            {
                hdnSearch.Value = "";
                FunPubOnPopupGridLoad();
            }
            #endregion
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    public void FunPubOnPopupGridLoad()
    {
        try
        {
            FunPubSetColumnToDispaly();
            FunPriSetHeaderText();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    private void FunPriSetHeaderText()
    {
        string strHeaderText = "";
        switch (LOVCode)
        {
            case "LOB":
                strHeaderText = "Line Of Business";
                break;
            case "CMD":
                strHeaderText = "Customer Master";
                break;
            case "CMDA":
                strHeaderText = "Customer Search";
                break;
            case "COAP":
                strHeaderText = "Customer Search";
                break;
            case "GCMD":
                strHeaderText = "Customer Search";
                break;
            case "PCMD":
                strHeaderText = "Customer Search";
                break;
            case "CCMD":
                strHeaderText = "Customer Search";
                break;
            case "PDT":
                strHeaderText = "Customer Search";
                break;
            case "REPAY":
                strHeaderText = "Customer Search";
                break;
            case "CMB":
                strHeaderText = "Customer Search";
                break;
            case "CCP":
                strHeaderText = "Customer Search";
                break;
            case "USRM":
                strHeaderText = "User Master";
                break;
            case "RGN":
                strHeaderText = "Region Master";
                break;
            case "BRAH":
                strHeaderText = "Branch Master";
                break;
            case "PROS":
                strHeaderText = "Prospect";
                break;
            case "ENDSA":
                strHeaderText = "Entity Search";
                break;
            case "ENFIA":
                strHeaderText = "Entity Search";
                break;
            case "ENDMA":
                strHeaderText = "Entity Search";
                break;
            case "ENDBTCOLL":
                strHeaderText = "Entity Search";
                break;
            case "ENVENDOR":
                strHeaderText = "Entity Search";
                break;
            case "ENSUNDRY":
                strHeaderText = "Entity Search";
                break;
            case "ENDEALER":
                strHeaderText = "Entity Search";
                break;
            case "ENBROK":
                strHeaderText = "Entity Search";
                break;
            case "ENEMP":
                strHeaderText = "Entity Search";
                break;
            case "ENINS":
                strHeaderText = "Entity Search";
                break;
            case "TADLR":
                strHeaderText = "Entity Search";
                break;
            default:
                strHeaderText = "Filter";
                break;
        }
        lblHeader.Text = strHeaderText;
    }

    /// <summary>
    /// Will add the common parameters to the Dictionary - to pass it to the Common SP.
    /// </summary>
    private void FunPriAddCommonParameters()
    {
        //Paging Properties set  
        int intTotalRecords = 0;
        ObjPaging.ProCompany_ID = ObjUserInfo.ProCompanyIdRW;
        ObjPaging.ProUser_ID = ObjUserInfo.ProUserIdRW;
        ObjPaging.ProTotalRecords = intTotalRecords;
        ObjPaging.ProCurrentPage = ProPageNumRW;
        ObjPaging.ProPageSize = ProPageSizeRW;
        ObjPaging.ProSearchValue = hdnSearch.Value;
        ObjPaging.ProOrderBy = hdnOrderBy.Value;

        Procparam = new Dictionary<string, string>();
        if (Procparam != null)
        {
            Procparam.Clear();
        }
    }

    /// <summary>
    /// To Bind the Landing Grid
    /// </summary>
    /// <param name="intPageNum"> Current Page Number of the grid </param>
    /// <param name="intPageSize"> Current Page size of the grid </param>
    protected void AssignValue(int intPageNum, int intPageSize)
    {
        ProPageNumRW = intPageNum;              // To set the page Number
        ProPageSizeRW = intPageSize;           // To set the page size    
        ViewState["Is_Valid_Call"] = "1";
        FunPubOnPopupGridLoad();
    }

    private void FunPriGetSearchValue()
    {
        arrSearchVal = gvList.FunPriGetSearchValue(arrSearchVal, intNoofSearch);
    }

    private void FunPriClearSearchValue()
    {
        gvList.FunPriClearSearchValue(arrSearchVal);
    }

    private void FunPriSetSearchValue()
    {
        gvList.FunPriSetSearchValue(arrSearchVal);
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

    public void FunPubSetColumnToDispaly()
    {
        DataTable dt1 = new DataTable();
        gvList.Columns.Clear();
        gvList.DataSource = null;
        gvList.DataBind();
        int intTotalRecords = 0;
        /*Added by vinodha m on march 26,2016 to display error message based on flag */
        string IsDynamicSearchFlag = String.Empty;
        /*Added by vinodha m on march 26,2016 to display error message based on flag */
        ObjUserInfo = new UserInfo();
        try
        {
            FunPriGetSearchValue();
            FunPriAddCommonParameters(); // Adding the Common parameters to the dictionary   
            ViewState["Is_Valid_Call"] = "1";
            if ((ViewState["Is_Valid_Call"]).ToString() == "1")
            {

                if (LOVCode != null)
                {
                    if (!string.IsNullOrEmpty(LOVCode.Trim()))
                    {
                        var LOVCode_Entity_List = new List<string> {"CMD", "CMDA", "ENDSA", "ENFIA", "ENDMA", "ENDBTCOLL", "ENVENDOR",
                        "ENSUNDRY", "ENDEALER", "ENBROK", "ENEMP", "ENINS", "GCMD", "PCMD", "COAP","CCP","CCMD","PDT","REPAY",
                        "CMB" , "TADLR" };
                        string x = LOVCode;
                        bool contains = LOVCode_Entity_List.Contains(x, StringComparer.OrdinalIgnoreCase);
                        if (contains)
                        {
                            if (ucProcparam != null)
                            {
                                foreach (KeyValuePair<string, string> ProcPair in ucProcparam)
                                {
                                    if (!string.IsNullOrEmpty(ProcPair.Value))
                                    {
                                        Procparam.Add(ProcPair.Key, ProcPair.Value);
                                    }
                                }
                            }
                            Procparam.Add("@IsActive", "1");
                            Procparam.Add("@User_Code", ObjUserInfo.ProUserLoginRW);
                            Procparam.Add("@LOV_Code", LOVCode);
                            dt1 = Utility.GetGridData("S3G_CMN_GetLOV_Dynamic", Procparam, out intTotalRecords, ObjPaging, out IsDynamicSearchFlag);
                            x = String.Empty;
                        }
                        else if (LOVCode == "FWUPD")
                        {
                            Procparam.Add("@Option", "2");
                            Procparam.Add("@IsActive", "1");
                            if (Session["FU_TYPE"] != null)
                            {
                                Procparam.Add("@FU_Type", Session["FU_TYPE"].ToString());
                            }

                            //dt1 = Utility.GetGridData("S3G_CLN_GetFollowUpTypeDetails", Procparam, out intTotalRecords, ObjPaging);

                            //Condition Included by Sathish.G
                            if (ObjPaging.ProPageSize != 1)
                            {
                                dt1 = Utility.GetGridData("S3G_CLN_GetCRMProspect", Procparam, out intTotalRecords, ObjPaging);
                                //ObjPaging.ProPageSize = 1;
                            }
                        }
                        else if (LOVCode == "PROS")
                        {
                            Procparam.Add("@Option", "1");
                            Procparam.Add("@IsActive", "1");

                            //Condition Included by Sathish.G
                            if (ObjPaging.ProPageSize != 1)
                            {
                                dt1 = Utility.GetGridData("S3G_CLN_GetCRMProspect", Procparam, out intTotalRecords, ObjPaging);
                                //ObjPaging.ProPageSize = 1;
                            }
                        }
                        else //if (LOVCode == "CRMENT")
                        {
                            if (ucProcparam != null)
                            {
                                foreach (KeyValuePair<string, string> ProcPair in ucProcparam)
                                {
                                    if (!string.IsNullOrEmpty(ProcPair.Value))
                                    {
                                        Procparam.Add(ProcPair.Key, ProcPair.Value);
                                    }
                                }
                            }
                            Procparam.Add("@LOV_Code", LOVCode);
                            dt1 = Utility.GetGridData("S3G_CMN_GetLOV", Procparam, out intTotalRecords, ObjPaging);
                        }
                    }
                }
                ViewState["Is_Valid_Call"] = 0;
                ViewState["dt1"] = dt1;
            }
            dt1 = (DataTable)ViewState["dt1"];
            if (dt1 != null)
            {
                if (dt1.Columns.Count > 0)
                {
                    /*Added by vinodha m on march 26,2016 to remove the flag columns before forming grid view */

                    string Column1 = "IsPageFlag";
                    string Column2 = "IsSearchFlag";
                    DataColumnCollection Column = dt1.Columns;
                    if (Column.Contains(Column1))
                    {
                        dt1.Columns.Remove(Column1);
                        dt1.AcceptChanges();
                    }
                    else if (Column.Contains(Column2))
                    {
                        dt1.Columns.Remove(Column2);
                        dt1.AcceptChanges();
                    }

                    /*Added by vinodha m on march 26,2016 to remove the flag columns before forming grid view */

                    int intRowCnt = dt1.Rows.Count;
                    arrColumns = new string[dt1.Columns.Count];
                    int ii = 0;
                    DataRow dr = null;

                    foreach (DataColumn dc in dt1.Columns)
                    {
                        arrColumns[ii] = dc.ColumnName;
                        ii++;
                        if (intRowCnt == 0)
                        {
                            dr = dt1.NewRow();
                            if (dc.DataType.FullName.ToString() == "System.Int32")
                                dr[dc.ColumnName] = 0;
                            else if (dc.DataType.FullName.ToString() == "System.DateTime")
                                dr[dc.ColumnName] = DateTime.Now;
                            else if (dc.DataType.FullName.ToString() == "System.Decimal")
                                dr[dc.ColumnName] = "0.0";
                            else
                                dr[dc.ColumnName] = "";
                        }
                    }
                    if (intRowCnt == 0) dt1.Rows.Add(dr);

                    ii = 0;
                    foreach (string strclm in arrColumns)
                    {
                        BoundField bf = new BoundField();
                        bf.DataField = strclm;
                        TemplateField tf = new TemplateField();
                        tf.HeaderTemplate = new GridViewTemplate(ListItemType.Header, strclm.Replace("_", " "), ii.ToString(), gvDisplay);
                        tf.ItemTemplate = new GridViewTemplate(ListItemType.Item, strclm, ii.ToString(), gvDisplay);
                        gvList.Columns.Add(tf);
                        ii++;
                    }

                    ucLOVPageNavigater.Visible = true;
                    int intModevalue = intTotalRecords % ProPageSizeRW == 0 ? 0 : 1;
                    if (Convert.ToInt32(intTotalRecords / ProPageSizeRW + intModevalue) >= ProPageNumRW)
                    {
                        if (ProPageNumRW == 0)
                            ProPageNumRW = 1;
                        /*Added by vinodha m on march 26,2016 - sending flag value to display error msg based on flag */
                        ucLOVPageNavigater.Navigation(intTotalRecords, ProPageNumRW, ProPageSizeRW, true, IsDynamicSearchFlag);
                        /*Added by vinodha m on march 26,2016 - sending flag value to display error msg based on flag */
                    }
                    else
                    {
                        //ProPageNumRW = Convert.ToInt32(intTotalRecords / ProPageSizeRW + intModevalue);           

                        /*Added by vinodha m on march 26,2016 - sending flag value to display error msg based on flag */
                        ucLOVPageNavigater.Navigation(intTotalRecords, ProPageNumRW, ProPageSizeRW, true, IsDynamicSearchFlag);
                        /*Added by vinodha m on march 26,2016 - sending flag value to display error msg based on flag */
                    }
                    ucLOVPageNavigater.setPageSize(ProPageSizeRW);

                    gvList.DataSource = dt1;
                    gvList.DataBind();
                    if (intRowCnt == 0) gvList.Rows[0].Visible = false;

                    ii = 0;
                    if (gvList.HeaderRow != null)
                    {
                        foreach (string strclm in arrColumns)
                        {
                            LinkButton lnkHeader = (LinkButton)gvList.HeaderRow.FindControl("lnkHeader" + ii.ToString());//strclm.Replace("_", ""));
                            TextBox txtHead = (TextBox)gvList.HeaderRow.FindControl("txtHeaderSearch" + ii.ToString());// strclm.Replace("_", ""));
                            if (txtHead != null)
                            {
                                lnkHeader.Click += new EventHandler(FunProSortingColumn);
                                txtHead.TextChanged += new EventHandler(FunProHeaderSearch);
                                txtHead.AutoPostBack = true;
                            }
                            ii++;
                        }
                    }
                    FunPriSetSearchValue();
                    dt1.Dispose();
                }
            }
            else
            {
                FunPriSetSearchValue();
            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, "Common User Control");
            throw;
        }
    }
    #endregion [Function's]

    protected void btnClose_Click(object sender, EventArgs e)
    {
        pnlLoadLOV.Visible = false;
    }
}
