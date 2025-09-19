

#region PageHeader
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name         :   Orgination
/// Screen Name         :   Transaction Lander
/// Created By          :   S.Kannan
/// Created Date        :   30-July-2010
/// Purpose             :   This is the landing screen for all the other Orgination Screens
/// Last Updated By		:   Rajendran
/// Last Updated Date   :   Dec 06 /2010
/// Reason              :   Removed the Old Data Type Check Logic
/// <Program Summary>
#endregion

#region How to use this
/*
   1)	Search for the word "Add Here" and add your code respectively  there...
   2)   Use the same stored procedure S3G_ORG_Get_TransLander, the return table should have the first column named "ID" - to use it as the RowCommandValue
            SP return table Rule
                1)  First Column should be "ID" - which will be used as a row command
                2)  Second Column should be "Created_By" - which will be the created_By column
                3)  third should be the "User_Level_ID" - which will be the createdBy user's level id.            
            The second and third should was used to check the user authorization.
            Take latest code from - App_Code\Utility.cs
   3)  Add your page Program ID as a parameter (@DocumentNumber)
   4)  If you want to send any other special parameters to your SP, then you can send it – through Dictionary – ProcParam. 
   5)  Also add it – to the Common SP – Commented with your program ID
   6)  The Query String can accept 6 Parameters, 
        a) Create – (Optional)
        b) Query – (Optional)
        c) Modify – (Optional)
        d) MultipleDNC - (Optional)  - If the user wants to select the Document Number Control dynamically.
        e) DNCOption - (Optional) - If the Enduser have more than one option for the selected DNC - eg: approved,unapproved - etc.
        f) Code – (Mandatory)
            ex: 
                S3GORGTransLander.aspx?Code=FEIR	
                S3GORGTransLander.aspx?Code=SNQ&Create=0&Query=1&Modify=0
                S3GORGTransLander.aspx?Code=SNQ&Query=0
                S3GORGTransLander.aspx?Code=SNQ& Modify=0
                S3GORGTransLander.aspx?Code=CRPT&MultipleDNC=1&DNCOption=1
   7) If you use the Query string "MultipleDNC" then you want to pass the parameter "@MultipleDNC_ID" to "[S3G_ORG_Get_TransLander]" SP
   8) If you use the Query string "DNCOption" then you want to pass the parameter "@MultipleOption_ID" to "[S3G_ORG_Get_TransLander]" SP 

  */
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
using System.Diagnostics;
using S3GBusEntity.Origination;
using S3GBusEntity;

#endregion

#region Class Origination_S3GORGTransLander
public partial class Origination_S3GORGTransLander : ApplyThemeForProject
{
    # region Programs Code
    string ProgramCodeToCompare = "";                                           // this is to hold the Program Code of your web page
    public static Origination_S3GORGTransLander obj_Page;

    // Add here - Add your Program Code Here - refer to the SQL table Program Master.
    const string strCRM = "CRM";//program code for CRM
    public string strProgramId = "0";
    // Program Code for Enquiry Customer Appraisal
    #endregion

    #region Common Variables
    int intCreate = -1;                                                         // intCreate = 1 then display the create button, else invisible
    int intQuery = -1;                                                          // intQuery = 1 then display the Query button, else invisible
    int intModify = -1;                                                         // intModify = 1 then display the Modify button, else invisible
    int intMultipleDNC = -1;                                                    // Allow the user to select the DNC dynamically.
    int intDNCOption = -1;                                                      // Allow the user to select the further option depend on the DNC - eg: approved,unapproved etc...
    Dictionary<int, string> dictMultipleDNC = null;                             // collection for Multiple DNC - DDL
    Dictionary<int, string> dictDNCOption = null;                               // Collection for DNCOption.
    string strProcName = "S3G_ORG_TransLander_CRM";                             // this is the Stored procedure to get call                     
    string ProgramCode = "";                                                         // To maintain the ProgramID
    UserInfo ObjUserInfo;                                                       // to maintain the user information      
    public string strDateFormat;                                                // to maintain the standard format
    string strRedirectPage = "";                                                // page to redirect to the page in query mode
    string strRedirectCreatePage = "~/Origination/S3G_ORG_CRM_ADD.aspx";
    string strRedirectSalesPage = "~/Origination/S3G_Org_CRM_Sales.aspx";    // page to redirect to the page in Create mode
    Dictionary<string, string> Procparam = null;                                // Dictionary to send our procedure's Parameters
    int intUserID = 0;                                                          // user who signed in
    int intCompanyID = 0;                                                       // conpany of the user who signed in   
    PagingValues ObjPaging = new PagingValues();                                // grid paging
    public delegate void PageAssignValue(int ProPageNumRW, int intPageSize);
    bool isQueryColumnVisible;                                                  // To change the Query Column visibility - depend on the user autherization 
    bool isEditColumnVisible;                                                  // To change the Edit Column visibility - depend on the user autherization 
    static string strPageName = "Origination TransLander";

    #region  User Authorization
    bool bCreate = false;
    bool bModify = false;
    bool bQuery = false;
    bool bIsActive = false;
    #endregion


    public int ProPageNumRW                                                     // to retain the current page size and number
    {
        get;
        set;
    }
    public int ProPageSizeRW
    {
        get;
        set;
    }
    #endregion

    #region Page Events

    protected void Page_Load(object sender, EventArgs e)
    {
        obj_Page = this;
        #region Application Standard Date Format

        S3GSession ObjS3GSession = new S3GSession();
        strDateFormat = ObjS3GSession.ProDateFormatRW;                              // to get the standard date format of the Application

        #endregion


        #region Grid Paging Config
        ProPageNumRW = 1;                                                           // to set the default page number
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

        # region User Information

        ObjUserInfo = new UserInfo();
        intCompanyID = ObjUserInfo.ProCompanyIdRW;                                  // current user's company ID.
        intUserID = ObjUserInfo.ProUserIdRW;                                        // current user's ID

        #region  User Authorization

        bCreate = ObjUserInfo.ProCreateRW;
        bModify = ObjUserInfo.ProModifyRW;
        bQuery = ObjUserInfo.ProViewRW;
        bIsActive = ObjUserInfo.ProIsActiveRW;

        #endregion
        #endregion

        #region Initialize page
        bool IsQueryStringChanged = false;
        if (!(string.IsNullOrEmpty(Request.QueryString["Code"])))                   // reading the query string
        {
            // to do  : want to decrypt this code in the URL
            btnCreate.Text = "Create";
            FunPriGetQueryStrings();
            InitPage();
            System.Web.HttpContext.Current.Session["ProgramId"] = strProgramId;

            if (ViewState["ProgramCode"] == null)                                   // Added viewstate for the program code - to refresh the page - when the query string of the URL varys. - It will assign the program code to the view state - for the very first time the page loads.
                ViewState["ProgramCode"] = ProgramCode;
            else                                                                    // If the program code in the URL changes then - it mean the user clicked on some other menu - so it need to refresh the page accordingly.
            {
                if (string.Compare(ViewState["ProgramCode"].ToString(), ProgramCode) != 0)
                {
                    IsQueryStringChanged = true;                                    // If the page changed from the current page.
                }
            }
        }
        #endregion

        #region !IsPostBack or QueryString changed.
        if ((!IsPostBack) || (IsQueryStringChanged))                                // refresh the page even if the query string of the URL varys - it mean the user navigates to some other page.
        {
            ViewState["ProgramCode"] = ProgramCode;
            IsQueryStringChanged = false;
            txtEndDateSearch.Attributes.Add("onblur", "fnDoDate(this,'" + txtEndDateSearch.ClientID + "','" + strDateFormat + "',false,  false);");
            txtStartDateSearch.Attributes.Add("onblur", "fnDoDate(this,'" + txtStartDateSearch.ClientID + "','" + strDateFormat + "',false,  false);");
            ceStartate.Format = ceEndDate.Format = strDateFormat;
            FunPriLoadLOV();                                                     // loading the LOV         
            //FunPriClearCtrl();
            ucCustomPaging.Visible = false;
            #region  User Authorization
            if (btnCreate.Enabled)                                                  // if the user can view the create button - depends on the query string
            {
                //User Authorization              
                if (!bCreate)
                {
                    btnCreate.Enabled = false;
                }
                //Authorization Code end
            }
            #endregion
        }
        #endregion
        ViewState["EnquiryorCustomer"] = string.Empty;
    }

    #endregion

    #region Control Events

    #region "Button Events"

    /// <summary>
    /// Will bind the grid and validate the from and to date.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            FunPriBindGrid();
        }
        catch (Exception objException)
        {
            FunPriWriteLog(objException);
        }
    }

    /// <summary>
    /// Will call create Page - depend on the Program ID passed.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCreate_Click(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty(strRedirectCreatePage))
            {
                Utility.FunShowAlertMsg(this, "Target page not found");
                return;
            }

            
            // to get the S3G_LOANAD_GetProgram_Ref_No
            #region To maintain Program_Ref_No in session
            /* MULTI COMAPNY CR BY VINODHA M ON APR 15,2016 */
            DataTable dt_Program_Ref_No = Utility.FunGetProgramDetailsByProgramCode(ProgramCode);
            /* MULTI COMAPNY CR BY VINODHA M ON APR 15,2016 */
            if (dt_Program_Ref_No != null && dt_Program_Ref_No.Rows.Count > 0)
            {
                Session["Program_Ref_No"] = dt_Program_Ref_No.Rows[0]["Program_Ref_No"].ToString();
            }
            else
            {
                Session["Program_Ref_No"] = null;
            }
            #endregion
            Session["DocumentNo"] = null;
            Session["EnqNewCustomerId"] = null;
            if (ProgramCode == "APPP")
            {
                Session["PricingAssetDetails"] = null;
                Session["AssetCustomer"] = null;
            }

            Response.Redirect(strRedirectCreatePage, false);
        }
        catch (Exception objException)
        {
            FunPriWriteLog(objException);
        }
    }

    protected void btnSalesCreate_Click(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty(strRedirectSalesPage))
            {
                Utility.FunShowAlertMsg(this, "Target page not found");
                return;
            }




            Response.Redirect(strRedirectSalesPage, false);
        }
        catch (Exception objException)
        {
            FunPriWriteLog(objException);
        }
    }

    /// <summary>
    /// Will clear the controls
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            FunPriClearGrid();
            lblErrorMessage.Text =
            txtStartDateSearch.Text =
            txtEndDateSearch.Text = "";
            ddlQueryType.Clear();
            ddlTaskStatus.Clear();
            ddlLOB.Items.Clear(); ddlItemView.Items.Clear();
            ddlLocation.Clear();
            FunPriLoadLOV();
            FunPriClearCtrl();
        }
        catch (Exception objException)
        {
            FunPriWriteLog(objException);
        }
    }

    #endregion

    #region "DropDown Events"

    protected void ddlItemView_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriClearGrid();
            FunPriClearCtrl();
        }
        catch (Exception objException)
        {
            FunPriWriteLog(objException);
        }
    }

    #endregion

    #region "Grid View Events"

    /// <summary>
    /// Will set the Grid Style and Alignment of the string dynamically depend on the data types of the cell
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grvTransLander_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
        }
        catch (Exception objException)
        {
        }
    }

    /// <summary>
    /// It will redirect the page from LAnding to Document Number Page.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grvTransLander_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(e.CommandArgument.ToString(), false, 0);

            if (strRedirectPage.Contains('?'))
                strRedirectPage += "&";
            else
                strRedirectPage += "?";
            if (ProgramCodeToCompare == strCRM)
            {
                Session["InitiateNumber"] = e.CommandArgument;
                Response.Redirect("~/Origination/S3G_ORG_CRM_ADD.aspx", false);
            }

            switch (e.CommandName.ToLower())
            {
                case "modify":
                    Response.Redirect(strRedirectPage + "qsViewId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=M", false);
                    break;
                case "query":
                    Response.Redirect(strRedirectPage + "qsViewId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=Q", false);
                    break;
            }
        }
        catch (Exception objException)
        {
            FunPriWriteLog(objException);
        }

    }

    #endregion

    #region "Link Button Events"

    protected void lnkgdTrackNo_Click(object sender, EventArgs e)
    {
        try
        {
            divShow.Style["display"] = "block";
            string strSelectID = ((LinkButton)sender).ClientID;
            int _iRowIdx = Utility.FunPubGetGridRowID("gvTrackDtl", strSelectID);

            LinkButton lnkgdTrackNo = (LinkButton)gvTrackDtl.Rows[_iRowIdx].FindControl("lnkgdTrackNo");
            Label lblTrackID = (Label)gvTrackDtl.Rows[_iRowIdx].FindControl("lblgdTrackID");
            FunPriClearDict();
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            Procparam.Add("@User_ID", Convert.ToString(intUserID));
            Procparam.Add("@Option", "4");
            Procparam.Add("@TICKET_NO", lnkgdTrackNo.Text);
            Procparam.Add("@Track_ID", Convert.ToString(lblTrackID.Text));
            DataTable dtTrackDtl = Utility.GetDefaultData("S3G_ORG_CRMCMNLST", Procparam);

            StringBuilder strHtml = new StringBuilder();
            strHtml.Append("<html>");
            strHtml.Append("<head>");
            string strStyle = string.Empty;
            strStyle = strStyle + "<style>";
            strStyle = strStyle + "table#tblMain {border: 1px solid black;border-collapse: collapse;border-spacing: 10px; align:center;}";
            strStyle = strStyle + "table#tblMain th	{background-color: #F2F2F2 ;color:#003d9e;font-family: calibri, Arial, Sans-Serif;font-size:14px;";
            strStyle = strStyle + "border: 1px solid #003d9e;border-collapse: collapse;}";
            strStyle = strStyle + "table#tblMain td	{background-color: #F0FFF0 ;color:#003d9e;font-family: calibri, Arial, Sans-Serif;font-size:15px;";
            strStyle = strStyle + "border: 1px solid #003d9e;border-collapse: collapse;}";
            //strStyle = strStyle + "table#tblMain tr:nth-child(even) {background-color: #EBD699;}";
            //strStyle = strStyle + "table#tblMain tr:nth-child(odd) {background-color : red;}";
            strStyle = strStyle + "</style>";

            strHtml.Append(strStyle);
            strHtml.Append("</head>");
            strHtml.Append("<body>");
            strHtml.Append("<table id=tblMain width=90% align = center>");
            strHtml.Append("<tr width=10% >");
            strHtml.Append("<th width=10%>Query Type</th>");
            strHtml.Append("<th width=10%>Follow Up Date</th>");
            strHtml.Append("<th width=20%>Follow Up User</th>");
            strHtml.Append("<th width=10%>Target Date</th>");
            strHtml.Append("<th width=10%>Mode</th>");
            strHtml.Append("<th width=30%>Description</th>");
            strHtml.Append("</tr>");

            for (Int32 i = 0; i < dtTrackDtl.Rows.Count; i++)
            {
                strHtml.Append("<tr width=100%>");
                strHtml.Append("<td width=10%>" + Convert.ToString(dtTrackDtl.Rows[i]["Action"]) + "</td>");
                strHtml.Append("<td width=10% align=center>" + Convert.ToString(dtTrackDtl.Rows[i]["Followup_Date"]) + "</td>");
                strHtml.Append("<td width=20%>" + Convert.ToString(dtTrackDtl.Rows[i]["FollowUp_User"]) + "</td>");
                strHtml.Append("<td width=10% align=center>" + Convert.ToString(dtTrackDtl.Rows[i]["Target_Date"]) + "</td>");
                strHtml.Append("<td width=10%>" + Convert.ToString(dtTrackDtl.Rows[i]["Comm_Mode"]) + "</td>");
                strHtml.Append("<td width=30%>" + Convert.ToString(dtTrackDtl.Rows[i]["Description"]) + "</td>");
                strHtml.Append("</tr>");
            }

            strHtml.Append("</table>");
            strHtml.Append("</body>");
            strHtml.Append("</html>");
            divhtml.InnerHtml = Convert.ToString(strHtml);
        }
        catch (Exception objException)
        {
            FunPriWriteLog(objException);
        }
    }

    protected void hyplnkView_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            string strFieldAtt = ((ImageButton)sender).ClientID;
            int gRowIndex = Utility.FunPubGetGridRowID("gvDocumentDtl", strFieldAtt);
            ImageButton lblDocPath = (ImageButton)gvDocumentDtl.Rows[gRowIndex].FindControl("hyplnkView");
            if (((ImageButton)sender).CommandArgument.Trim().ToUpper() != "")
            {
                string strFileName = ((ImageButton)sender).CommandArgument.Replace("\\", "/").Trim();
                string strScipt = "window.open('../Common/S3GViewFile.aspx?qsFileName=" + strFileName + "', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", strScipt, true);
            }
            else
            {
                Utility.FunShowAlertMsg(this, "File not to be scanned yet");
            }
        }
        catch (Exception objException)
        {
            FunPriWriteLog(objException);
        }
    }

    #endregion

    #endregion

    // Created By: Anbuvel
    // Created Date: 09-01-2012
    // Descrition: To Bind Location Value

    /// <summary>
    /// GetCompletionList
    /// </summary>
    /// <param name="prefixText">search text</param>
    /// <param name="count">no of matches to display</param>
    /// <returns>string[] of matching names</returns>

    #region "Web Methods"

    [System.Web.Services.WebMethod]
    public static string[] GetBranchList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        Procparam.Add("@Company_ID", Convert.ToString(obj_Page.intCompanyID));
        Procparam.Add("@User_ID", Convert.ToString(obj_Page.intUserID));
        Procparam.Add("@Program_Id", "241");
        Procparam.Add("@Is_Active", "1");
        Procparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_SYSAD_GET_BRANCHLIST_AGT", Procparam));
        return suggetions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetUserList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        Procparam.Add("@Company_ID", Convert.ToString(obj_Page.intCompanyID));
        Procparam.Add("@Prefix", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_Get_Userlist_AGT", Procparam));

        return suggetions.ToArray();
    }

    /// <summary>
    /// Get Customer/Prospect List
    /// </summary>
    /// <param name="prefixText"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    [System.Web.Services.WebMethod]
    public static string[] GetProspectList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        Procparam.Add("@Option", (Convert.ToInt32(obj_Page.ddlItemView.SelectedValue) == 1) ? "5" : "2");
        Procparam.Add("@Company_ID", Convert.ToString(obj_Page.intCompanyID));
        Procparam.Add("@Prefix", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_ORG_CRMCMNLST", Procparam));

        return suggetions.ToArray();
    }

    /// <summary>
    /// Get Lead/Panum List
    /// </summary>
    /// <param name="prefixText"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    [System.Web.Services.WebMethod]
    public static string[] GetLeadPanumList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        Procparam.Add("@Option", (Convert.ToInt32(obj_Page.ddlItemView.SelectedValue) == 1) ? "6" : "3");
        Procparam.Add("@Company_ID", Convert.ToString(obj_Page.intCompanyID));
        Procparam.Add("@Prefix", prefixText);
        if (Convert.ToString(obj_Page.ddlProspect.SelectedValue) != "0" && Convert.ToString(obj_Page.ddlProspect.SelectedText) != "")
        {
            string[] strSearchValue = Convert.ToString(obj_Page.ddlProspect.SelectedValue).Split('/');
            if (strSearchValue[0] == "Cust")
                Procparam.Add("@Customer_ID", strSearchValue[1]);
            else if (strSearchValue[0] == "Pros")
                Procparam.Add("@CRM_ID", strSearchValue[1]);
        }
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_ORG_CRMCMNLST", Procparam));

        return suggetions.ToArray();
    }

    #endregion

    #region "METHODS"

    private void FunPriLoadLOV()
    {
        try
        {
            FunPriClearDict();
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            Procparam.Add("@User_ID", Convert.ToString(intUserID));

            DataSet dsLOV = Utility.GetDataset("S3G_ORG_GETCRMVIEWLOOKUP", Procparam);

            FunFillGrid(ddlLOB, dsLOV.Tables[0], Convert.ToString(dsLOV.Tables[0].Columns[0].ColumnName), Convert.ToString(dsLOV.Tables[0].Columns[2].ColumnName));
            ddlTaskStatus.DataValueField = "Lookup_Code";
            ddlTaskStatus.DataTextField = "Lookup_Description";
            ddlTaskStatus.DataSource = dsLOV.Tables[1];

            ddlQueryType.DataValueField = "Lookup_Code";
            ddlQueryType.DataTextField = "Lookup_Description";
            ddlQueryType.DataSource = dsLOV.Tables[2];

            FunFillGrid(ddlItemView, dsLOV.Tables[3], "ID", "Description");
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunPriClearDict()
    {
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    protected void FunFillGrid(DropDownList ddlList, DataTable dt, string strValueField, string strDisplayFeild)
    {
        try
        {
            if (ddlList != null && ddlList.Items.Count > 0)
            {
                ddlList.Items.Clear();
            }

            ddlList.DataSource = dt;
            ddlList.DataValueField = strValueField;
            ddlList.DataTextField = strDisplayFeild;
            ddlList.DataBind();
            ddlList.Items.Insert(0, new ListItem("--Select--", "0"));
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunPriClearGrid()
    {
        try
        {
            divShow.Style["display"] = "none";
            grvTransLander.DataSource = null;
            gvTrackDtl.DataSource = null;
            gvDocumentDtl.DataSource = null;
            grvTransLander.Visible = gvTrackDtl.Visible = gvDocumentDtl.Visible = ucCustomPaging.Visible = false;
            lblErrorMessage.Text = "";
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunPriClearCtrl()
    {
        try
        {
            ddlProspect.Clear();
            ddlLead.Clear();
            ddlFromUser.Clear();
            ddlToUser.Clear();
            lblFromUser.Visible = ddlFromUser.Visible = lblToUser.Visible = ddlToUser.Visible = lblQuery.Visible =
            ddlQueryType.Visible = lblStatus.Visible = ddlTaskStatus.Visible = (Convert.ToInt32(ddlItemView.SelectedValue) == 4) ? true : false;
            lblLead.Text = (Convert.ToInt32(ddlItemView.SelectedValue) == 1) ? "Account Number" : "Lead/Account";
            lblProspect.Text = (Convert.ToInt32(ddlItemView.SelectedValue) == 1) ? "Customer Name" : "Prospect/Customer";
            ddlProspect.IsMandatory = (Convert.ToInt32(ddlItemView.SelectedValue) == 1) ? true : false;
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    /// <summary>
    /// To read the values from the querystring
    /// </summary>
    private void FunPriGetQueryStrings()
    {
        try
        {
            if (Request.QueryString.Get("Code") != null)
                ProgramCode = (Request.QueryString.Get("Code"));
            if (Request.QueryString.Get("Create") != null)
                intCreate = Convert.ToInt32(Request.QueryString.Get("Create"));
            if (Request.QueryString.Get("Query") != null)
                intQuery = Convert.ToInt32(Request.QueryString.Get("Query"));
            if (Request.QueryString.Get("Modify") != null)
                intModify = Convert.ToInt32(Request.QueryString.Get("Modify"));
            if (Request.QueryString.Get("MultipleDNC") != null)
                intMultipleDNC = Convert.ToInt32(Request.QueryString.Get("MultipleDNC"));
            if (Request.QueryString.Get("DNCOption") != null)
                intDNCOption = Convert.ToInt32(Request.QueryString.Get("DNCOption"));
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    /// <summary>
    /// This method will Initialize the page depend on the document Number Code passed.
    /// </summary>
    protected void InitPage()
    {
        // Add here - your case condition - with respect to you program Code 
        // only If you're not passing the MultipleDNC Query String.
        // if you want to pass the LOB and branch as a query string - then make a call to FunPriQueryString();
        try
        {
            switch (ProgramCode)
            {
                case strCRM:
                    strProgramId = "241";
                    this.Title = "S3G-CRM";
                    ProgramCodeToCompare = strCRM;
                    lblHeading.Text = "CRM-Details";
                    strRedirectPage = "~/Origination/S3G_ORG_CRM_ADD.aspx";
                    strRedirectCreatePage = strRedirectPage;// +"?qsMode=C";
                    strRedirectSalesPage = "~/Origination/S3G_Org_CRM_Sales.aspx";
                    break;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
        // }
    }

    /// <summary>
    /// To Bind the Landing Grid
    /// </summary>
    /// <param name="intPageNum"> Current Page Number of the grid </param>
    /// <param name="intPageSize"> Current Page size of the grid </param>
    protected void AssignValue(int intPageNum, int intPageSize)
    {
        try
        {
            ProPageNumRW = intPageNum;              // To set the page Number
            ProPageSizeRW = intPageSize;            // To set the page size    
            FunPriBindGrid();                       // Binding the Landing grid
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }

    /// <summary>
    /// Bind the Landing Grid.
    /// </summary>
    private void FunPriBindGrid()
    {
        try
        {
            int intTotalRecords = 0;
            bool bIsNewRow = false;
            FunPriClearGrid();
            FunPriClearDict();

            FunPriValidateFromEndDate();
            FunPriClearDict();

            ObjPaging.ProCompany_ID = intCompanyID;
            ObjPaging.ProUser_ID = intUserID;
            ObjPaging.ProTotalRecords = intTotalRecords;
            ObjPaging.ProCurrentPage = ProPageNumRW;
            ObjPaging.ProPageSize = ProPageSizeRW;
            ObjPaging.ProSearchValue = hdnSearch.Value;
            ObjPaging.ProOrderBy = hdnOrderBy.Value;

            if (Convert.ToInt32(ddlLOB.SelectedValue) > 0)
                Procparam.Add("@LOB_ID", Convert.ToString(ddlLOB.SelectedValue));
            if (Convert.ToString(txtStartDateSearch.Text) != "")
                Procparam.Add("@Start_Date", Utility.StringToDate(txtStartDateSearch.Text).ToString());
            if (Convert.ToString(txtEndDateSearch.Text) != "")
                Procparam.Add("@End_Date", Utility.StringToDate(txtEndDateSearch.Text).ToString());
            if (Convert.ToInt32(ddlLocation.SelectedValue) > 0 && Convert.ToString(ddlLocation.SelectedText) != "")
                Procparam.Add("@Location_ID", Convert.ToString(ddlLocation.SelectedValue));
            if (Convert.ToString(obj_Page.ddlProspect.SelectedValue) != "0" && Convert.ToString(obj_Page.ddlProspect.SelectedText) != "")
            {
                string[] strSearchValue = Convert.ToString(obj_Page.ddlProspect.SelectedValue).Split('/');
                if (strSearchValue[0] == "Cust")
                    Procparam.Add("@Customer_ID", strSearchValue[1]);
                else if (strSearchValue[0] == "Pros")
                    Procparam.Add("@CRM_ID", strSearchValue[1]);
            }
            if (Convert.ToString(obj_Page.ddlLead.SelectedValue) != "0" && Convert.ToString(obj_Page.ddlLead.SelectedText) != "")
            {
                string[] strSearchValue = Convert.ToString(obj_Page.ddlLead.SelectedValue).Split('/');
                if (strSearchValue[0] == "Lead")
                    Procparam.Add("@Lead_ID", strSearchValue[1]);
                else if (strSearchValue[0] == "ACC")
                    Procparam.Add("@Panum_ID", strSearchValue[1]);
            }
            Procparam.Add("@ItemView_ID", Convert.ToString(ddlItemView.SelectedValue));

            if (Convert.ToInt32(ddlItemView.SelectedValue) == 2)        //Documents
            {
                gvDocumentDtl.BindGridView("S3G_ORG_GETCRMVIEWDTL", Procparam, out intTotalRecords, ObjPaging, out bIsNewRow);

                if (bIsNewRow)
                {
                    gvDocumentDtl.Rows[0].Visible = false;
                }
                gvDocumentDtl.Visible = true;
            }

            if (Convert.ToInt32(ddlItemView.SelectedValue) == 3 || Convert.ToInt32(ddlItemView.SelectedValue) == 1)        //1- Account 3 - Lead
            {
                grvTransLander.BindGridView("S3G_ORG_GETCRMVIEWDTL", Procparam, out intTotalRecords, ObjPaging, out bIsNewRow);

                if (bIsNewRow)
                {
                    grvTransLander.Rows[0].Visible = false;
                }
                grvTransLander.Visible = true;
            }
            else if (Convert.ToInt32(ddlItemView.SelectedValue) == 4)    //Track
            {
                if (Convert.ToString(ddlQueryType.SelectedValue) != "0")
                {
                    Procparam.Add("@Query_Type", Convert.ToString(ddlQueryType.SelectedValue));
                    Procparam.Remove("@ItemView_ID");
                    Procparam.Add("@ItemView_ID", "5");

                    if (Convert.ToString(ddlTaskStatus.SelectedValue) != "0")
                        Procparam.Add("@Ticket_Status", Convert.ToString(ddlTaskStatus.SelectedValue));

                    if (Convert.ToInt32(ddlFromUser.SelectedValue) > 0 && Convert.ToString(ddlFromUser.SelectedText) != "")
                        Procparam.Add("@FromUser_ID", Convert.ToString(ddlFromUser.SelectedValue));

                    if (Convert.ToInt32(ddlToUser.SelectedValue) > 0 && Convert.ToString(ddlToUser.SelectedText) != "")
                        Procparam.Add("@ToUser_ID", Convert.ToString(ddlToUser.SelectedValue));

                    grvTransLander.BindGridView("S3G_ORG_GETCRMVIEWDTL", Procparam, out intTotalRecords, ObjPaging, out bIsNewRow);

                    if (bIsNewRow)
                    {
                        grvTransLander.Rows[0].Visible = false;
                    }
                    grvTransLander.Visible = true;
                }
                else
                {
                    if (Convert.ToString(ddlTaskStatus.SelectedValue) != "0")
                        Procparam.Add("@Ticket_Status", Convert.ToString(ddlTaskStatus.SelectedValue));

                    gvTrackDtl.BindGridView("S3G_ORG_GETCRMVIEWDTL", Procparam, out intTotalRecords, ObjPaging, out bIsNewRow);

                    if (bIsNewRow)
                    {
                        gvTrackDtl.Rows[0].Visible = false;
                    }
                    gvTrackDtl.Visible = true;
                }
            }

            ucCustomPaging.Visible = true;
            ucCustomPaging.Navigation(intTotalRecords, ProPageNumRW, ProPageSizeRW);
            ucCustomPaging.setPageSize(ProPageSizeRW);
        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            lblErrorMessage.Text = ex.Message;
        }
    }

    /// <summary>
    /// This method will validate the from and to date - entered by the user.
    /// </summary>
    private void FunPriValidateFromEndDate()
    {
        try
        {

            if ((!(string.IsNullOrEmpty(txtStartDateSearch.Text))) &&
               (!(string.IsNullOrEmpty(txtEndDateSearch.Text))))                                   // If start and end date is not empty
            {
                if (Utility.StringToDate(txtStartDateSearch.Text) > Utility.StringToDate(txtEndDateSearch.Text)) // start date should be less than or equal to the enddate
                {
                    if (hidDate.Value.ToUpper() == "START")
                        Utility.FunShowAlertMsg(this, "Start date should be lesser than the End Date");
                    else
                        Utility.FunShowAlertMsg(this, "End date should be greater than the Start Date");
                    //Added By Thangam M on 23/Mar/2012
                    grvTransLander.DataSource = null;
                    grvTransLander.DataBind();
                    ucCustomPaging.Visible = false;
                    return;
                }
            }
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunPriWriteLog(Exception objException)
    {
        try
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
        }
        catch (Exception ex)
        {
        }
    }

    #endregion

}
#endregion
