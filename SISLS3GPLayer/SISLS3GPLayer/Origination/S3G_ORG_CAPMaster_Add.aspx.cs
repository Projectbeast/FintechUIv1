/// Module Name     :   Origination
/// Screen Name     :   CAP Master
/// Created By      :   Ganapathy Subramanian.G
/// Created Date    :   05-Jul-2012
/// Purpose         :   To insert and update CAP Master details


using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using S3GBusEntity;
using S3GBusEntity.Origination;
using System.ServiceModel;
using System.Data;
using System.IO;
using System.Globalization;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.xml;
using iTextSharp.text.html;
using System.Web.Security;
using System.Text;
using S3GBusEntity.LoanAdmin;
using System.Configuration;
using LoanAdminMgtServicesReference;
using ApprovalMgtServicesReference;

public partial class Origination_S3G_ORG_CAPMaster_Add :ApplyThemeForProject
{
    #region [Intialization]

    Dictionary<string, string> dictParam = null;
    int intCAPDetailID;
    int intDEVDetailID;
    int intUserId;
    int intUserLevelID;
    int intCompanyId;
    int intErrCode;
    string strErrorMsg;
    string strMode;           
    string strKey = "Insert";
    string strPageName = "Origination - CAP Master";
    string strAlert = "alert('__ALERT__');";
    string strRedirectPageAdd = "window.location.href='../Common/HomePage.aspx';";
    string strRedirectPageView = "window.location.href='../Common/HomePage.aspx';";

    DataSet dsCAPDetails = new DataSet();        
    UserInfo ObjUserInfo = new UserInfo();    
    ApprovalMgtServicesClient objApprovalMgtServicesClient;
    //ApprovalMgtServices.S3G_ORG_CAPDataTable ObjS3G_CAPMasterListDataTable;        
    #endregion [Intialization]
    protected void Page_Load(object sender, EventArgs e)
    {
        FunProPageLoad();
    }

    protected void FunProPageLoad()
    {        
        intCompanyId = ObjUserInfo.ProCompanyIdRW;
        intUserId = ObjUserInfo.ProUserIdRW;
        intUserLevelID = ObjUserInfo.ProUserLevelIdRW;
        intCAPDetailID = Convert.ToInt32(hdnCAPApp.Value);
        intDEVDetailID = Convert.ToInt32(hdnDEV.Value);        
        if (!IsPostBack)
        {
            tcCAPMaster.ActiveTabIndex = 1;
            if (intUserLevelID == 5)
            {
                ViewState["Mode"] = strMode = "M";
            }
            else
            {
                ViewState["Mode"] = strMode = "Q";
            }
            FunPubGetCAPDetails();
            FunPubGetDesignationDetails();
            if (strMode == "M")
            {      
                FunPriDisableControls(0);
                FunPriLoadRepayCulture();
            }            
            else
            {            
                FunPriDisableControls(-1);
            }
        }
    }

    public void FunPubGetCAPDetails()
    {
        dictParam = new Dictionary<string, string>();        
        dictParam.Add("@COMPANY_ID", Convert.ToString(intCompanyId));
        //dsCAPDetails = Utility.GetDataset("S3G_ORG_GET_CAP", dictParam);
        if (dsCAPDetails.Tables.Count > 0)
        {
            if (dsCAPDetails.Tables[0].Rows.Count > 0)
            {
                ViewState["currenttable"] = dsCAPDetails.Tables[0];
                ViewState["dtTempCAPApprover"] = dsCAPDetails.Tables[1];
                hdnCAPApp.Value = Convert.ToString(dsCAPDetails.Tables[0].Rows[0]["CAP_APPROVAL_DET_ID"]);
                hdnDEV.Value = Convert.ToString(dsCAPDetails.Tables[2].Rows[0]["CAP_DEVIATION_DET_ID"]);
                grvCAPApproval.DataSource = dsCAPDetails.Tables[0];
                grvCAPApproval.DataBind();
                //lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
            }
            else
            {
                FunPriInitializeCAPApproval();
                //lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);
                //strMode = "C";
            }
            if (dsCAPDetails.Tables[2].Rows.Count > 0)
            {
                ViewState["currenttable1"] = dsCAPDetails.Tables[2];
                ViewState["dtTempDEVApprover"] = dsCAPDetails.Tables[3];
                grvCAPDeviation.DataSource = dsCAPDetails.Tables[2];
                grvCAPDeviation.DataBind();
            }
            else
            {
                FunPriInitializeDeviationApproval();
            }
        }
        else
        {
            FunPriInitializeCAPApproval();
            FunPriInitializeDeviationApproval();
        }
    }


    private void FunPriInitializeCAPApproval()
    {
        try
        {
            DataTable dt = new DataTable();
            DataRow dr;
            dt.Columns.Add("SNo",typeof(string));
            dt.Columns.Add("From_Amount",typeof(decimal));
            dt.Columns.Add("To_Amount", typeof(decimal));
            dt.Columns.Add("No_of_Approvals", typeof(int));
            dt.Columns.Add("Active",typeof(bool));     
            dt.Columns.Add("Is_Delete",typeof(int));
            dr = dt.NewRow();
            dr["SNo"] = 0;
            dr["From_Amount"] = 0;
            dr["To_Amount"] = 0;
            dr["No_of_Approvals"] = 0;
            dr["Active"] = true;
            dr["Is_Delete"] = 0;
            dt.Rows.Add(dr);              
            grvCAPApproval.DataSource = dt;
            grvCAPApproval.DataBind();
            dt.Rows[0].Delete();
            ViewState["currenttable"] = dt;
            grvCAPApproval.Rows[0].Visible = false;
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw new ApplicationException("Unable to initiate the row");
        }
    }

    private void FunPriInitializeDeviationApproval()
    {
        try
        {
            DataTable dt = new DataTable();
            DataRow dr;
            dt.Columns.Add("SNo", typeof(string));
            dt.Columns.Add("RepayCultureID", typeof(int));
            dt.Columns.Add("RepayCulture", typeof(string));
            dt.Columns.Add("LTVPer", typeof(decimal));
            dt.Columns.Add("IRRPer",typeof(decimal));
            dt.Columns.Add("IDVAmount", typeof(decimal));
            dt.Columns.Add("WaiverAmount", typeof(decimal));
            dt.Columns.Add("ARPID", typeof(int));
            dt.Columns.Add("ARP",typeof(string));
            dt.Columns.Add("SCWID",typeof(int));
            dt.Columns.Add("SCW", typeof(string));
            dt.Columns.Add("No_of_Approvals",typeof(int));
            dt.Columns.Add("Active",typeof(bool));
            dt.Columns.Add("Is_Delete", typeof(int));
            dr = dt.NewRow();
            dr["SNo"] = 0;
            dr["RepayCultureID"] = 0;
            dr["RepayCulture"] = string.Empty;
            dr["LTVPer"] = 0;
            dr["IRRPer"] = 0;
            dr["IDVAmount"] = 0;
            dr["WaiverAmount"] = 0;
            dr["ARP"] = 0;
            dr["ARP"] = string.Empty;
            dr["SCW"] = string.Empty;
            dr["SCWID"] = 0;
            dr["No_of_Approvals"] = 0;
            dr["Active"] = true;
            dr["Is_Delete"] = 0;
            dt.Rows.Add(dr);            
            grvCAPDeviation.DataSource = dt;
            grvCAPDeviation.DataBind();
            dt.Rows[0].Delete();
            ViewState["currenttable1"] = dt;
            grvCAPDeviation.Rows[0].Visible = false;
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw new ApplicationException("Unable to initiate the row");
        }
    }
    protected void grvCAPApproval_RowCommand(object sender, GridViewCommandEventArgs e)
    {        
        DataTable dtCAPApproval = (DataTable)ViewState["currenttable"];        
        DataRow dRow;
        if (e.CommandName == "Add")
        {
            Label lbl=(Label)grvCAPApproval.Rows[grvCAPApproval.Rows.Count-1].FindControl("lblSNo");
            int intSNo = (Convert.ToInt32(lbl.Text))+1;
            TextBox txtFromAmount = (TextBox)grvCAPApproval.FooterRow.FindControl("txtFromAmount");
            TextBox txtFToAmount = (TextBox)grvCAPApproval.FooterRow.FindControl("txtFToAmount");
            TextBox txtFApprovalOrder = (TextBox)grvCAPApproval.FooterRow.FindControl("txtFApprovalOrder");

            if (Convert.ToDecimal(txtFromAmount.Text) == 0 && Convert.ToDecimal(txtFToAmount.Text) == 0)
            {
                Utility.FunShowAlertMsg(this, "All values cannot be Zero");
                return;
            }      

            decimal dcToAmount = 0;
            if (!string.IsNullOrEmpty(txtFToAmount.Text.Trim()))
            {
                dcToAmount = Convert.ToDecimal(txtFToAmount.Text.Trim());
                if (!FunProCheckOverlap(txtFromAmount.Text, txtFToAmount.Text))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('From/To Amount should not overlap');", true);
                    txtFromAmount.Focus();
                    return;
                }
            }

            if (dtCAPApproval != null)
            {
                if (((DataTable)(ViewState["dtTempCAPApprover"])) != null)
                {
                    DataRow[] dArray = ((DataTable)(ViewState["dtTempCAPApprover"])).Select("SNo='" + intSNo + "' and ApprovEntity<>'' and ApprovPerson<>''");
                    if (dArray.Length != (Convert.ToInt32(txtFApprovalOrder.Text)))
                    {
                        Utility.FunShowAlertMsg(this, "No. of Approvers selected does not match with No.of Approvals");
                        return;
                    }
                }
                else
                {
                    Utility.FunShowAlertMsg(this, "Assign the Approvers");
                    return;
                }
            }
            dRow = dtCAPApproval.NewRow();
            dRow["SNo"] = (Convert.ToInt32(lbl.Text) + 1);
            dRow["From_Amount"] = Convert.ToDecimal(txtFromAmount.Text);
            dRow["To_Amount"] = Convert.ToDecimal(txtFToAmount.Text);
            dRow["No_of_Approvals"] = Convert.ToInt32(txtFApprovalOrder.Text);
            dRow["Active"] = true;
            dtCAPApproval.Rows.Add(dRow);
            DataView dtView = new DataView(dtCAPApproval);
            dtView.Sort = "From_Amount,To_Amount ASC";
            DataTable dtTemp = dtView.ToTable();
            dtTemp.AcceptChanges();
            grvCAPApproval.DataSource = dtTemp;
            grvCAPApproval.DataBind();
            ViewState["currenttable"] = dtTemp;          

        }
    }

    protected void grvCAPDeviation_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        //Label lblFooter = (Label)grvCAPDeviation.FooterRow.FindControl("lblSNo");
        //int intSNo = Convert.ToInt32(lblFooter.Text);
        DataTable dtDevApproval = (DataTable)ViewState["currenttable1"];
        DataRow dRow;
        DataRow[] dRow1;
        if (e.CommandName == "Add")
        {
            Label lbl = (Label)grvCAPDeviation.Rows[grvCAPDeviation.Rows.Count - 1].FindControl("lblSNo");
            int intSNo = (Convert.ToInt32(lbl.Text))+1;
            DropDownList ddlRepayCulture = (DropDownList)grvCAPDeviation.FooterRow.FindControl("ddlRepayCulture");
            TextBox txtFLTVPercentage = (TextBox)grvCAPDeviation.FooterRow.FindControl("txtFLTVPercentage");
            TextBox txtFIRRPercentage = (TextBox)grvCAPDeviation.FooterRow.FindControl("txtFIRRPercentage");
            TextBox txtFIDV = (TextBox)grvCAPDeviation.FooterRow.FindControl("txtFIDV");
            TextBox txtFWaiverAmount = (TextBox)grvCAPDeviation.FooterRow.FindControl("txtFWaiverAmount");
            DropDownList ddlFRejectedProposal = (DropDownList)grvCAPDeviation.FooterRow.FindControl("ddlFRejectedProposal");
            DropDownList ddlFSpecialConditions = (DropDownList)grvCAPDeviation.FooterRow.FindControl("ddlFSpecialConditions");
            TextBox txtNoofApproval = (TextBox)grvCAPDeviation.FooterRow.FindControl("txtNoofApproval");
            CheckBox chkFActive = (CheckBox)grvCAPDeviation.FooterRow.FindControl("chkFActive");

            if (Convert.ToDecimal(txtFLTVPercentage.Text) == 0 && Convert.ToDecimal(txtFIRRPercentage.Text) == 0 && Convert.ToDecimal(txtFIDV.Text) == 0 && Convert.ToDecimal(txtFWaiverAmount.Text) == 0)
            {
                Utility.FunShowAlertMsg(this, "All values cannot be Zero");
                return;
            }

            dRow1 = dtDevApproval.Select("LTVPer='" + txtFLTVPercentage.Text
                    + "' and IRRPer='" + txtFIRRPercentage.Text 
                    + "' and IDVAmount='" + txtFIDV.Text 
                    + "' and WaiverAmount='" + txtFWaiverAmount.Text 
                    + "' and ARP='" + ddlFRejectedProposal.SelectedItem.Text 
                    + "' and SCW='" + ddlFSpecialConditions.SelectedItem.Text + "' and Active='True'");
            if (dRow1.Length > 0)
            {
                Utility.FunShowAlertMsg(this, "Selected Combination Already Exist");
                ddlRepayCulture.Focus();
                return;
            }
            if (dtDevApproval != null)
            {
                if (((DataTable)(ViewState["dtTempDEVApprover"])) != null)
                {
                    DataRow[] dArray = ((DataTable)(ViewState["dtTempDEVApprover"])).Select("SNo='" + intSNo + "' and ApprovEntity<>'' and ApprovPerson<>''");
                    if (dArray.Length != (Convert.ToInt32(txtNoofApproval.Text)))
                    {
                        Utility.FunShowAlertMsg(this, "No. of Approvers selected does not match with No.of Approvals");
                        return;
                    }
                }
                else
                {
                    Utility.FunShowAlertMsg(this, "Assign the Approvers");
                    return;
                }
            }
            dRow = dtDevApproval.NewRow();
            dRow["SNo"] = (Convert.ToInt32(lbl.Text) + 1).ToString();
            dRow["RepayCulture"] = ddlRepayCulture.SelectedItem.Text;
            dRow["RepayCultureID"] = ddlRepayCulture.SelectedValue;
            dRow["LTVPer"] = txtFLTVPercentage.Text;
            dRow["IRRPer"] = txtFIRRPercentage.Text;
            //dRow["IDVAmount"] = txtFIDV.Text;
            //dRow["WaiverAmount"] = txtFWaiverAmount.Text;
            dRow["ARPID"] = ddlFRejectedProposal.SelectedValue;
            dRow["ARP"] = ddlFRejectedProposal.SelectedItem.Text;
            dRow["SCWID"] = ddlFSpecialConditions.SelectedValue;
            dRow["SCW"] = ddlFSpecialConditions.SelectedItem.Text;
            dRow["No_of_Approvals"] = txtNoofApproval.Text;
            dRow["Active"] = chkFActive.Checked = true;
            dtDevApproval.Rows.Add(dRow);
            grvCAPDeviation.DataSource = dtDevApproval;
            grvCAPDeviation.DataBind();
            ViewState["currenttable1"] = dtDevApproval;
            FunPriLoadRepayCulture();
        }
    }

    protected void grvCAPApproval_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        DataTable dtNew=new DataTable();
        Label lblSNo = (Label)grvCAPApproval.Rows[e.RowIndex].FindControl("lblSNo");
        int intDeletedRow = Convert.ToInt32(lblSNo.Text);
        DataTable dt = (DataTable)ViewState["currenttable"];
        DataTable dt1 = (DataTable)ViewState["dtTempCAPApprover"];        
        var c = from d1 in dt1.AsEnumerable()
                where !dt.AsEnumerable().Any(r2 => r2.Field<string>("SNo").ToString() == d1.Field<string>("SNo").ToString())
                select d1;

        DataRow[] dArray = c.ToArray();
        if (dArray.Length > 0)
        {
            foreach (DataRow drow in dArray)
            {
                dt1.Rows.Remove(drow);
                dt1.AcceptChanges();
            }
        }

        FunProDeleteRowsFromDataTable(ref dt, ref dt1, ref intDeletedRow,e.RowIndex);              
        ViewState["dtTempCAPApprover"] = dt1;        
        if (dt.Rows.Count == 0)
        {
            FunPriInitializeCAPApproval();
        }
        else
        {
            ViewState["currenttable"] = dt;
            grvCAPApproval.DataSource = dt;
            grvCAPApproval.DataBind();
        }
    }

    protected void grvCAPDeviation_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        Label lblSNo = (Label)grvCAPDeviation.Rows[e.RowIndex].FindControl("lblSNo");
        int intDeletedRow = Convert.ToInt32(lblSNo.Text);
        DataTable dt = (DataTable)ViewState["currenttable1"];
        DataTable dt1 = (DataTable)ViewState["dtTempDEVApprover"];        
        var c = from d1 in dt1.AsEnumerable()
                where !dt.AsEnumerable().Any(r2 => r2.Field<string>("SNo").ToString() == d1.Field<string>("SNo").ToString())
                select d1;

        DataRow[] dArray = c.ToArray();
        if (dArray.Length > 0)
        {
            foreach (DataRow drow in dArray)
            {
                dt1.Rows.Remove(drow);
                dt1.AcceptChanges();
            }
        }
        FunProDeleteRowsFromDataTable(ref dt, ref dt1, ref intDeletedRow,e.RowIndex);
        ViewState["dtTempDEVApprover"] = dt1;
        if (dt.Rows.Count == 0)
        {
            FunPriInitializeDeviationApproval();            
        }
        else
        {
            ViewState["currenttable1"] = dt;
            grvCAPDeviation.DataSource = dt;
            grvCAPDeviation.DataBind();            
        }
        FunPriLoadRepayCulture();
    }

    protected void btnFApproverDesig_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        Button btn = (Button)sender;
        GridViewRow gvRow = (GridViewRow)btn.Parent.Parent;
        TextBox txtFApprovalOrder = (TextBox)gvRow.FindControl("txtFApprovalOrder");
        string strSLNo = "";
        Label lblSNo = (Label)grvCAPApproval.Rows[grvCAPApproval.Rows.Count - 1].FindControl("lblSNo");
        strSLNo = (Convert.ToInt32(lblSNo.Text) + 1).ToString();

        if (ViewState["dtTempCAPApprover"] == null)
        {
            ViewState["dtTempCAPApprover"] = FunProInitializePopup();
        }
        DataTable dt1 = (DataTable)ViewState["dtTempCAPApprover"];      
        DataTable dtApprove = new DataTable();
        DataRow[] dRow = dt1.Select("SNo ='" + strSLNo + "'");
        if (dRow.Length > 0)
        {
            dtApprove = dt1.Clone();
            dRow.CopyToDataTable(dtApprove, LoadOption.OverwriteChanges);
        }

        if (dtApprove != null && dtApprove.Rows.Count > 0)
        {
            int intNoofApproval = Convert.ToInt32(txtFApprovalOrder.Text);
            int intRowCount = dtApprove.Rows.Count;

            if (intNoofApproval > intRowCount)
            {
                for (int inti = intRowCount + 1; inti <= (intRowCount + (intNoofApproval - intRowCount)); inti++)
                {
                    DataRow dr;
                    dr = dtApprove.NewRow();
                    dr["SNo"] = strSLNo;
                    dr["SequenceNumber"] = inti;
                    dr["ApprovEntity"] = string.Empty;
                    dr["ApprovPerson"] = string.Empty;
                    dtApprove.Rows.Add(dr);
                }
            }

            if (intNoofApproval < intRowCount)
            {
                for (int inti = 1; inti <= intRowCount - intNoofApproval; inti++)
                {
                    dtApprove.Rows[dtApprove.Rows.Count - 1].Delete();
                    dtApprove.AcceptChanges();
                }
            }

            grvApprover.DataSource = dtApprove;
            grvApprover.DataBind();            
        }
        else
        {
            dtApprove=FunProInitializePopup();
            int intNoofApproval = Convert.ToInt32(txtFApprovalOrder.Text);
            for (int inti = 1; inti <= intNoofApproval; inti++)
            {
                DataRow dr;
                dr = dtApprove.NewRow();
                dr["SNo"] = strSLNo;
                dr["SequenceNumber"] = inti;
                dr["ApprovEntity"] = string.Empty;
                dr["ApprovPerson"] = string.Empty;
                dtApprove.Rows.Add(dr);
            }

            grvApprover.DataSource = dtApprove;
            grvApprover.DataBind();

            dt1.Merge(dtApprove);

            ViewState["dtTempCAPApprover"] = dt1;
        }
        ModalPopupExtenderApprover.Show();
    }
   
    protected void btnIApproverDesig_Click(object sender, EventArgs e)
    {        
        FunProAssignApprover(sender, e);
    }

    protected void btnAssignApprover_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        Button btn = (Button)sender;
        GridViewRow gvRow = (GridViewRow)btn.Parent.Parent;
        TextBox txtNoofApproval = (TextBox)gvRow.FindControl("txtNoofApproval");
        string strSLNo = "";
        Label lblSNo = (Label)grvCAPDeviation.Rows[grvCAPDeviation.Rows.Count - 1].FindControl("lblSNo");
        strSLNo = (Convert.ToInt32(lblSNo.Text) + 1).ToString();

        if (ViewState["dtTempDEVApprover"] == null)
        {
            ViewState["dtTempDEVApprover"] = FunProInitializePopup();
        }
        DataTable dt1 = (DataTable)ViewState["dtTempDEVApprover"];
        DataTable dtApprove = new DataTable();
        DataRow[] dRow = dt1.Select("SNo ='" + strSLNo + "'");
        if (dRow.Length > 0)
        {
            dtApprove = dt1.Clone();
            dRow.CopyToDataTable(dtApprove, LoadOption.OverwriteChanges);
        }

        if (dtApprove != null && dtApprove.Rows.Count > 0)
        {
            int intNoofApproval = Convert.ToInt32(txtNoofApproval.Text);
            int intRowCount = dtApprove.Rows.Count;

            if (intNoofApproval > intRowCount)
            {
                for (int inti = intRowCount + 1; inti <= (intRowCount + (intNoofApproval - intRowCount)); inti++)
                {
                    DataRow dr;
                    dr = dtApprove.NewRow();
                    dr["SNo"] = strSLNo;
                    dr["SequenceNumber"] = inti;
                    dr["ApprovEntity"] = string.Empty;
                    dr["ApprovPerson"] = string.Empty;
                    dtApprove.Rows.Add(dr);
                }
            }

            if (intNoofApproval < intRowCount)
            {
                for (int inti = 1; inti <= intRowCount - intNoofApproval; inti++)
                {
                    dtApprove.Rows[dtApprove.Rows.Count - 1].Delete();
                    dtApprove.AcceptChanges();
                }
            }

            grvDEVApprover.DataSource = dtApprove;
            grvDEVApprover.DataBind();
        }
        else
        {
            dtApprove = FunProInitializePopup();
            int intNoofApproval = Convert.ToInt32(txtNoofApproval.Text);
            for (int inti = 1; inti <= intNoofApproval; inti++)
            {
                DataRow dr;
                dr = dtApprove.NewRow();
                dr["SNo"] = strSLNo;
                dr["SequenceNumber"] = inti;
                dr["ApprovEntity"] = string.Empty;
                dr["ApprovPerson"] = string.Empty;
                dtApprove.Rows.Add(dr);
            }

            grvDEVApprover.DataSource = dtApprove;
            grvDEVApprover.DataBind();

            dt1.Merge(dtApprove);

            ViewState["dtTempDEVApprover"] = dt1;
        }
        ModalPopupExtenderDEVApprover.Show();
    }

    protected void btnIAssignApprover_Click(object sender, EventArgs e)
    {
        FunProAssignApprover(sender, e);
    }

    protected void FunProAssignApprover(object sender,EventArgs e)
    {
        DataTable dt = new DataTable();
        DataTable dtApprove = new DataTable();
        Button btn = (Button)sender;
        GridViewRow gvRow = (GridViewRow)btn.Parent.Parent;
        GridView grv = (GridView)gvRow.Parent.Parent;
        if (grv.ClientID == "ctl00_ContentPlaceHolder1_tcCAPMaster_TabPanel2_grvCAPDeviation")
        {           
            dt = (DataTable)ViewState["dtTempDEVApprover"];
            LinkButton lnkCAPDelete = (LinkButton)gvRow.FindControl("lnkCAPDelete");
            if (lnkCAPDelete.Enabled == true)
            {
                btnDEVModalAdd.Enabled = true;
            }
            else
            {
                btnDEVModalAdd.Enabled = false;
            }
        }
        else
        {           
            dt = (DataTable)ViewState["dtTempCAPApprover"];
            LinkButton lnkDelete = (LinkButton)gvRow.FindControl("lnkDelete");
            if (lnkDelete.Enabled == true)
            {
                btnModalAdd.Enabled = true;
            }
            else
            {
                btnModalAdd.Enabled = false;
            }
        }
        Label lblINoofApprovals = (Label)gvRow.FindControl("lblINoofApprovals");
        Label lblSNo = (Label)gvRow.FindControl("lblSNo");
        int intSNo = Convert.ToInt32(lblSNo.Text);        
        DataRow[] drCAPApprover = dt.Select("SNo='" + intSNo + "'");
        dtApprove = FunProInitializePopup();
        if (drCAPApprover.Length > 0)
        {
            drCAPApprover.CopyToDataTable(dtApprove, LoadOption.OverwriteChanges);
        }
        if (grv.ClientID == "ctl00_ContentPlaceHolder1_tcCAPMaster_TabPanel2_grvCAPDeviation")
        {
            grvDEVApprover.DataSource = dtApprove;
            grvDEVApprover.DataBind();
            ModalPopupExtenderDEVApprover.Show();
        }
        else
        {
            grvApprover.DataSource = dtApprove;
            grvApprover.DataBind();
            ModalPopupExtenderApprover.Show();
        }        
    }

    protected DataTable FunProInitializePopup()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("SNo");
        dt.Columns.Add("SequenceNumber");
        dt.Columns.Add("ApprovEntity");
        dt.Columns.Add("ApprovPerson");

        return dt;
    }

    public void FunPubGetDesignationDetails()
    {
        dictParam = new Dictionary<string, string>();
        dictParam.Add("@Company_ID", Convert.ToString(intCompanyId));
        ViewState["ApprovalEntity"] = Utility.GetDefaultData("S3G_ORG_GETAPPROVALENTITY", dictParam);
        ViewState["Designation"] = Utility.GetDefaultData("S3G_ORG_GETDESIGNATION", dictParam);
    }

    protected void grvCAPApproval_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (ViewState["currenttable"] != null)
        {
            DataTable dt = (DataTable)ViewState["currenttable"];
            DataView dtView = new DataView(dt);
            dtView.Sort = "From_Amount,To_Amount ASC";
            dt = dtView.ToTable();
            dt.AcceptChanges();
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                TextBox txtFromAmount = (TextBox)e.Row.FindControl("txtFromAmount");
                TextBox txtFToAmount = (TextBox)e.Row.FindControl("txtFToAmount");
                txtFromAmount.SetPercentagePrefixSuffix(6, 2, false, "From Amount");
                txtFToAmount.SetPercentagePrefixSuffix(6, 2, false, "To Amount");
                if (dt != null && dt.Rows.Count == 0)
                {
                    txtFromAmount.Text = "0.01";
                    txtFromAmount.Enabled = false;
                }
                else if (dt != null && dt.Rows.Count > 0)
                {
                    txtFromAmount.Text = dt.Rows[dt.Rows.Count - 1]["To_Amount"].ToString();
                    if (!string.IsNullOrEmpty(txtFromAmount.Text))
                        txtFromAmount.Text = Convert.ToString(Convert.ToDecimal(txtFromAmount.Text) + Convert.ToDecimal(0.01));
                    txtFromAmount.Enabled = false;
                }
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblIsDelete = (Label)e.Row.FindControl("lblIsDelete");
                LinkButton lnkDelete = (LinkButton)e.Row.FindControl("lnkDelete");
                Button btnIApproverDesig = (Button)e.Row.FindControl("btnIApproverDesig");
                CheckBox chkCAPIActive = (CheckBox)e.Row.FindControl("chkCAPIActive");
                if (ViewState["Mode"].ToString() == "M")
                {
                    if (lblIsDelete.Text == "0" || lblIsDelete.Text == string.Empty)
                    {
                        lnkDelete.Enabled = true;
                        btnIApproverDesig.Enabled = true;
                        chkCAPIActive.Enabled = true;
                    }
                    else
                    {
                        lnkDelete.Enabled = false;
                        lnkDelete.OnClientClick = "";
                        btnIApproverDesig.Enabled = true;
                        chkCAPIActive.Enabled = false;
                    }
                }
                else
                {
                    lnkDelete.Enabled = false;
                    lnkDelete.OnClientClick = "";
                    btnIApproverDesig.Enabled = true;
                    chkCAPIActive.Enabled = false;
                }
            }
        }
    }

    protected void grvCAPDeviation_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            TextBox txtFLTVPercentage = (TextBox)e.Row.FindControl("txtFLTVPercentage");
            TextBox txtFIRRPercentage = (TextBox)e.Row.FindControl("txtFIRRPercentage");
            TextBox txtFIDV = (TextBox)e.Row.FindControl("txtFIDV");
            TextBox txtFWaiverAmount = (TextBox)e.Row.FindControl("txtFWaiverAmount");           
            txtFLTVPercentage.SetPercentagePrefixSuffix(3, 2, false, "LTV %");
            txtFIRRPercentage.SetPercentagePrefixSuffix(3, 2, false, "IRR %");
            txtFIDV.SetPercentagePrefixSuffix(7, 2, false, "IDV Amount");
            txtFWaiverAmount.SetPercentagePrefixSuffix(7, 2, false, "Waiver Amount");                
        }
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    LinkButton lnkCAPDelete = (LinkButton)e.Row.FindControl("lnkCAPDelete");
        //    CheckBox chkActive = (CheckBox)e.Row.FindControl("chkActive");
        //    if (strMode == "Q")
        //    {
        //        chkActive.Enabled = false;
        //        lnkCAPDelete.Enabled = false;
        //        lnkCAPDelete.OnClientClick = "";
        //    }       
        //}
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblDevIsDelete = (Label)e.Row.FindControl("lblDevIsDelete");
            LinkButton lnkCAPDelete = (LinkButton)e.Row.FindControl("lnkCAPDelete");
            Button btnIAssignApprover = (Button)e.Row.FindControl("btnIAssignApprover");
            CheckBox chkActive = (CheckBox)e.Row.FindControl("chkActive");
            if (ViewState["Mode"].ToString() == "M")
            {
                if (lblDevIsDelete.Text == "0" || lblDevIsDelete.Text == string.Empty)
                {
                    lnkCAPDelete.Enabled = true;
                    btnIAssignApprover.Enabled = true;
                    chkActive.Enabled = true;
                }
                else
                {
                    lnkCAPDelete.Enabled = false;
                    lnkCAPDelete.OnClientClick = "";
                    btnIAssignApprover.Enabled = true;
                    chkActive.Enabled = false;
                }
            }
            else
            {
                lnkCAPDelete.Enabled = false;
                lnkCAPDelete.OnClientClick = "";
                btnIAssignApprover.Enabled = true;
                chkActive.Enabled = false;
            }
        }
    }

    protected void grvApprover_RowDataBound(object sender,GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblApprovalEntity = (Label)e.Row.FindControl("lblApprovalEntity");
            Label lblApprovalPerson = (Label)e.Row.FindControl("lblApprovalPerson");
            DropDownList ddlApprovalEntity = (DropDownList)e.Row.FindControl("ddlApprovalEntity");
            DropDownList ddlApprovPerson = (DropDownList)e.Row.FindControl("ddlApprovPerson");
            ddlApprovalEntity.BindDataTable((DataTable)ViewState["ApprovalEntity"]);
            ddlApprovPerson.BindDataTable((DataTable)ViewState["Designation"]);

            if (ddlApprovalEntity.Items.Count == 2)
            {
                ddlApprovalEntity.Items.RemoveAt(0);
            }
            if (ddlApprovPerson.Items.Count == 2)
            {
                ddlApprovPerson.Items.RemoveAt(0);
            }
            ddlApprovalEntity.SelectedValue = lblApprovalEntity.Text;
            ddlApprovPerson.SelectedValue = lblApprovalPerson.Text;
            if (btnModalAdd.Enabled == false)
            {
                ddlApprovalEntity.ClearDropDownList();
                ddlApprovPerson.ClearDropDownList();
            }
        }    
    }

    protected void grvDEVApprover_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblApprovalEntity = (Label)e.Row.FindControl("lblApprovalEntity");
            Label lblApprovalPerson = (Label)e.Row.FindControl("lblApprovalPerson");
            DropDownList ddlApprovalEntity = (DropDownList)e.Row.FindControl("ddlApprovalEntity");
            DropDownList ddlApprovPerson = (DropDownList)e.Row.FindControl("ddlApprovPerson");
            ddlApprovalEntity.BindDataTable((DataTable)ViewState["ApprovalEntity"]);
            ddlApprovPerson.BindDataTable((DataTable)ViewState["Designation"]);

            if (ddlApprovalEntity.Items.Count == 2)
            {
                ddlApprovalEntity.Items.RemoveAt(0);
            }
            if (ddlApprovPerson.Items.Count == 2)
            {
                ddlApprovPerson.Items.RemoveAt(0);
            }
            ddlApprovalEntity.SelectedValue = lblApprovalEntity.Text;
            ddlApprovPerson.SelectedValue = lblApprovalPerson.Text;
            if (btnDEVModalAdd.Enabled == false)
            {
                ddlApprovalEntity.ClearDropDownList();
                ddlApprovPerson.ClearDropDownList();
            }
        }
    }

    protected void btnModalCancel_Click(object sender, EventArgs e)
    {
        ModalPopupExtenderApprover.Hide();
    }    
    protected void btnDEVModalCancel_Click(object sender, EventArgs e)
    {
        ModalPopupExtenderDEVApprover.Hide();
    }
    protected void btnModalAdd_Click(object sender, EventArgs e)
    {        
        Button btn = (Button)sender;        
        DataTable dtModal = new DataTable();        
        foreach (GridViewRow GvRow in grvApprover.Rows)
        {
            DropDownList ddlApprovalEntity = (DropDownList)GvRow.FindControl("ddlApprovalEntity");
            DropDownList ddlApprovPerson = (DropDownList)GvRow.FindControl("ddlApprovPerson");
            Label lblSequenceNumber = (Label)GvRow.FindControl("lblSequenceNumber");
            if (ddlApprovalEntity.Items.Count > 1 && ddlApprovalEntity.SelectedValue == "0")
            {
                Utility.FunShowAlertMsg(this, "Select Approval Entity");
                ddlApprovalEntity.Focus();
                return;
            }
            if (ddlApprovPerson.Items.Count > 1 && ddlApprovPerson.SelectedValue == "0")
            {
                Utility.FunShowAlertMsg(this, "Select Approval Authority");
                ddlApprovPerson.Focus();
                return;
            }
            //string strApprovalEntity = ddlApprovalEntity.SelectedValue;
            //string strApprovalPerson = ddlApprovPerson.SelectedValue;
            //for (int inti = 0; inti <= grvApprover.Rows.Count-1; inti++)
            //{
            //    Label lblSequenceNumber1 = (Label)grvApprover.Rows[inti].FindControl("lblSequenceNumber");
            //    DropDownList ddlApprovalEntity1 = (DropDownList)grvApprover.Rows[inti].FindControl("ddlApprovalEntity");
            //    DropDownList ddlApprovPerson1 = (DropDownList)grvApprover.Rows[inti].FindControl("ddlApprovPerson");
            //    if (lblSequenceNumber.Text != lblSequenceNumber1.Text && ddlApprovalEntity1.SelectedValue == strApprovalEntity && ddlApprovPerson1.SelectedValue == strApprovalPerson)
            //    {
            //        Utility.FunShowAlertMsg(this, "Selected combination already exist");
            //        ddlApprovPerson1.Focus();
            //        return;
            //    }
            //}
        }
        dtModal = (DataTable)ViewState["dtTempCAPApprover"];  

        int intMasterSeq = Convert.ToInt32(((Label)grvApprover.Rows[0].FindControl("LblSNo")).Text);
        /*Write Coding To delete Existing record*/

        DataRow[] Drow = dtModal.Select("SNo='" + intMasterSeq + "'");
        foreach (DataRow dr in Drow)
        {
            dtModal.Rows.Remove(dr);
        }

        foreach (GridViewRow GvRow in grvApprover.Rows)
        {
            DropDownList ddlApprovalEntity = (DropDownList)GvRow.FindControl("ddlApprovalEntity");
            DropDownList ddlApprovPerson = (DropDownList)GvRow.FindControl("ddlApprovPerson");
            Label lblSequenceNumber = (Label)GvRow.FindControl("lblSequenceNumber");            
            DataRow dRow = dtModal.NewRow();

            dRow["SNo"] = intMasterSeq;
            dRow["SequenceNumber"] = lblSequenceNumber.Text;
            dRow["ApprovEntity"] = ddlApprovalEntity.SelectedValue;
            dRow["ApprovPerson"] = ddlApprovPerson.SelectedValue;
            dtModal.Rows.Add(dRow);          
        }                
        ViewState["dtTempCAPApprover"]=dtModal;        
        ModalPopupExtenderApprover.Hide();
    }

    protected void btnDEVModalAdd_Click(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        DataTable dtModal = new DataTable();
        foreach (GridViewRow GvRow in grvDEVApprover.Rows)
        {
            DropDownList ddlApprovalEntity = (DropDownList)GvRow.FindControl("ddlApprovalEntity");
            DropDownList ddlApprovPerson = (DropDownList)GvRow.FindControl("ddlApprovPerson");
            Label lblSequenceNumber = (Label)GvRow.FindControl("lblSequenceNumber");
            if (ddlApprovalEntity.Items.Count > 1 && ddlApprovalEntity.SelectedValue == "0")
            {
                Utility.FunShowAlertMsg(this, "Select Approval Entity");
                ddlApprovalEntity.Focus();
                return;
            }
            if (ddlApprovPerson.Items.Count > 1 && ddlApprovPerson.SelectedValue == "0")
            {
                Utility.FunShowAlertMsg(this, "Select Approval Authority");
                ddlApprovPerson.Focus();
                return;
            }
            //string strApprovalEntity = ddlApprovalEntity.SelectedValue;
            //string strApprovalPerson = ddlApprovPerson.SelectedValue;
            //for (int inti = 0; inti <= grvDEVApprover.Rows.Count - 1; inti++)
            //{
            //    Label lblSequenceNumber1 = (Label)grvDEVApprover.Rows[inti].FindControl("lblSequenceNumber");
            //    DropDownList ddlApprovalEntity1 = (DropDownList)grvDEVApprover.Rows[inti].FindControl("ddlApprovalEntity");
            //    DropDownList ddlApprovPerson1 = (DropDownList)grvDEVApprover.Rows[inti].FindControl("ddlApprovPerson");
            //    if (lblSequenceNumber.Text != lblSequenceNumber1.Text && ddlApprovalEntity1.SelectedValue == strApprovalEntity && ddlApprovPerson1.SelectedValue == strApprovalPerson)
            //    {
            //        Utility.FunShowAlertMsg(this, "Selected combination already exist");
            //        ddlApprovPerson1.Focus();
            //        return;
            //    }
            //}
        }
        dtModal = (DataTable)ViewState["dtTempDEVApprover"];        

        int intMasterSeq = Convert.ToInt32(((Label)grvDEVApprover.Rows[0].FindControl("LblSNo")).Text);
        /*Write Coding To delete Existing record*/

        DataRow[] Drow = dtModal.Select("SNo='" + intMasterSeq + "'");
        foreach (DataRow dr in Drow)
        {
            dtModal.Rows.Remove(dr);
        }

        foreach (GridViewRow GvRow in grvDEVApprover.Rows)
        {

            DropDownList ddlApprovalEntity = (DropDownList)GvRow.FindControl("ddlApprovalEntity");
            DropDownList ddlApprovPerson = (DropDownList)GvRow.FindControl("ddlApprovPerson");
            Label lblSequenceNumber = (Label)GvRow.FindControl("lblSequenceNumber");
            DataRow dRow = dtModal.NewRow();

            dRow["SNo"] = intMasterSeq;
            dRow["SequenceNumber"] = lblSequenceNumber.Text;
            dRow["ApprovEntity"] = ddlApprovalEntity.SelectedValue;
            dRow["ApprovPerson"] = ddlApprovPerson.SelectedValue;
            dtModal.Rows.Add(dRow);

        }        
        ViewState["dtTempDEVApprover"] = dtModal;

        ModalPopupExtenderDEVApprover.Hide();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        FunPubSaveCAPMaster();    
    }
    private void FunPubSaveCAPMaster()
    {
        return;
        TextBox txtFromAmount = (TextBox)grvCAPApproval.FooterRow.FindControl("txtFromAmount");
        decimal FromAmount, ToAmount;
        Session["ToAmount"] = null;
        objApprovalMgtServicesClient = new ApprovalMgtServicesClient();
        try
        {
            if (((DataTable)ViewState["currenttable"]).Rows.Count == 0)
            {
                Utility.FunShowAlertMsg(this, "Add atleast one CAP Approval details");
                tcCAPMaster.ActiveTabIndex = 0;
                return;
            }           

            if (((DataTable)ViewState["currenttable1"]).Rows.Count == 0)
            {
                Utility.FunShowAlertMsg(this, "Add atleast one Deviation Approval details");
                tcCAPMaster.ActiveTabIndex = 1;
                return;
            }

            if (((DataTable)ViewState["currenttable"]).Rows.Count > 0 && ((DataTable)ViewState["currenttable"]).Select("Active='True'").Length == 0)
            {
                Utility.FunShowAlertMsg(this, "Atleast one Active record should be present in CAP Approval");
                tcCAPMaster.ActiveTabIndex = 0;
                return;
            }

            if (((DataTable)ViewState["currenttable1"]).Rows.Count > 0 && ((DataTable)ViewState["currenttable1"]).Select("Active='True'").Length == 0)
            {
                Utility.FunShowAlertMsg(this, "Atleast one Active record should be present in Deviation Approval");
                tcCAPMaster.ActiveTabIndex = 1;
                return;
            }            
            foreach (GridViewRow grvRow in grvCAPApproval.Rows)
            {
                Label lblFromAmount = (Label)grvRow.FindControl("lblFromAmount");                
                if (Session["ToAmount"] != null)
                {
                    ToAmount = Convert.ToDecimal(Session["ToAmount"]) + Convert.ToDecimal("0.01");
                    FromAmount = Convert.ToDecimal(lblFromAmount.Text.Trim());
                    if (ToAmount != FromAmount)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('The difference between amount should be equal to 0.01');", true);
                        txtFromAmount.Text = Convert.ToString(Convert.ToDecimal(Session["ToAmount"]) + Convert.ToDecimal(0.01));
                        txtFromAmount.ReadOnly = true;
                        txtFromAmount.Focus();
                        return;
                    }
                }
                Label lblToAmount = (Label)grvRow.FindControl("lblIAmount");
                FromAmount = Convert.ToDecimal(lblFromAmount.Text.Trim());
                ToAmount = Convert.ToDecimal(lblToAmount.Text.Trim());
                Session["ToAmount"] = ToAmount.ToString();              
            }

            //ObjS3G_CAPMasterListDataTable = new ApprovalMgtServices.S3G_ORG_CAPDataTable();
            //S3GBusEntity.LoanAdmin.ApprovalMgtServices.S3G_ORG_CAPRow objS3G_ORG_CAPRow;
            //objS3G_ORG_CAPRow = ObjS3G_CAPMasterListDataTable.NewS3G_ORG_CAPRow();
            //objS3G_ORG_CAPRow.Created_By = Convert.ToString(intUserId);
            //objS3G_ORG_CAPRow.Modified_By = Convert.ToString(intUserId);
            //objS3G_ORG_CAPRow.xmlCAP = Convert.ToString(((DataTable)ViewState["currenttable"]).FunPubFormXml());
            //objS3G_ORG_CAPRow.xmlDesigCAP = Convert.ToString(((DataTable)ViewState["dtTempCAPApprover"]).FunPubFormXml());
            //objS3G_ORG_CAPRow.xmlDeviation = Convert.ToString(((DataTable)ViewState["currenttable1"]).FunPubFormXml());
            //objS3G_ORG_CAPRow.xmlDesigDevi = Convert.ToString(((DataTable)ViewState["dtTempDEVApprover"]).FunPubFormXml());
            //ObjS3G_CAPMasterListDataTable.Rows.Add(objS3G_ORG_CAPRow);
            //byte[] objbyteDataTable=ClsPubSerialize.Serialize(ObjS3G_CAPMasterListDataTable,SerializationMode.Binary);
            //intErrCode = objApprovalMgtServicesClient.FunPubCreateCAPMaster(out strErrorMsg,SerializationMode.Binary,objbyteDataTable);
            switch (intErrCode)
            {
                case 0:
                    {
                        if (intCAPDetailID > 0)
                        {
                            strKey = "Modify";
                            strAlert = "CAP Master details updated successfully";                            
                            strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                            strRedirectPageView = "";
                            //Added by Thangam M on 17/Oct/2012 to avoid double save click
                            btnSave.Enabled = false;
                            //End here
                        }
                        else
                        {
                            strAlert = "CAP Master details added successfully";                            
                            strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                            strRedirectPageView = "";
                            //Added by Thangam M on 17/Oct/2012 to avoid double save click
                            btnSave.Enabled = false;
                            //End here

                        }
                        ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                    }
                    break;
                 case 20: Utility.FunShowAlertMsg(this.Page, "Error in Adding CAP Master Details");
                    break;
            }
            
        }
        catch (Exception e)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(e);
            throw e;
        }
        finally
        {
            objApprovalMgtServicesClient.Close();
        }
    }
    private void FunPriLoadRepayCulture()
    {
        DropDownList ddlRepayCulture = (DropDownList)grvCAPDeviation.FooterRow.FindControl("ddlRepayCulture");
        ddlRepayCulture.Items.Clear();
        dictParam = new Dictionary<string, string>();
        dictParam.Add("@COMPANY_ID", Convert.ToString(1));
        DataSet dsRepayCulture = Utility.GetDataset("S3G_ORG_GETREPAYCULTURE", dictParam);        
        ddlRepayCulture.BindDataTable(dsRepayCulture.Tables[0]);
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Common/HomePage.aspx");
    }

    private void FunPriDisableControls(int intModeID)
    {        
        switch (intModeID)
        {
            case 0: // Modify Mode                
                btnCancel.Enabled = true;
                btnSave.Enabled = true;
                grvCAPApproval.FooterRow.Visible = true;
                grvCAPDeviation.FooterRow.Visible = true;
                btnModalAdd.Enabled = true;
                btnDEVModalAdd.Enabled = true;
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);                
                break;          

            case -1:// Query Mode
                btnCancel.Enabled = true;
                btnSave.Enabled = false;
                btnModalAdd.Enabled = false;
                btnDEVModalAdd.Enabled = false;
                grvCAPApproval.FooterRow.Visible = false;
                grvCAPDeviation.FooterRow.Visible = false;                                
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                break;
        }
    }

    protected void chkCAPIActive_CheckedChanged(object sender, EventArgs e)
    {
        int intRowIndex = Utility.FunPubGetGridRowID("grvCAPApproval", ((CheckBox)sender).ClientID.ToString());

        CheckBox chkCAPIActive = (CheckBox)grvCAPApproval.Rows[intRowIndex].FindControl("chkCAPIActive");
        Label lblFromAmount = (Label)grvCAPApproval.Rows[intRowIndex].FindControl("lblFromAmount");
        Label lblIAmount = (Label)grvCAPApproval.Rows[intRowIndex].FindControl("lblIAmount");

        DataTable dtCAPApproval = (DataTable)ViewState["currenttable"];

        if (chkCAPIActive.Checked)
        {
            if (!FunProCheckOverlap(lblFromAmount.Text, lblIAmount.Text))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('From/To Amount should not overlap');", true);
                chkCAPIActive.Checked = false;
                return;
            }
            dtCAPApproval.Rows[intRowIndex]["Active"] = true;
        }
        else
        {
            dtCAPApproval.Rows[intRowIndex]["Active"] = false;
        }

        ViewState["currenttable"] = dtCAPApproval;
    }

    protected void chkActive_CheckedChanged(object sender, EventArgs e)
    {
        int intRowIndex = Utility.FunPubGetGridRowID("grvCAPDeviation", ((CheckBox)sender).ClientID.ToString());
        CheckBox chkActive = (CheckBox)grvCAPDeviation.Rows[intRowIndex].FindControl("chkActive");        
        Label lblRepayCulture = (Label)grvCAPDeviation.Rows[intRowIndex].FindControl("lblRepayCulture");
        Label lblILTVPercentage = (Label)grvCAPDeviation.Rows[intRowIndex].FindControl("lblILTVPercentage");
        Label lblIIRRPercentage = (Label)grvCAPDeviation.Rows[intRowIndex].FindControl("lblIIRRPercentage");
        Label lblIIDV = (Label)grvCAPDeviation.Rows[intRowIndex].FindControl("lblIIDV");
        Label lblIWaiverAmount = (Label)grvCAPDeviation.Rows[intRowIndex].FindControl("lblIWaiverAmount");
        Label lblARP = (Label)grvCAPDeviation.Rows[intRowIndex].FindControl("lblARP");
        Label lblSCW = (Label)grvCAPDeviation.Rows[intRowIndex].FindControl("lblSCW");        
       
        DataTable dtDEVApproval = (DataTable)ViewState["currenttable1"];        

        if (chkActive.Checked==true)
        {
            //DataRow[] dRow = dtDEVApproval.Select("RepayCulture='" + lblRepayCulture.Text + "' and Active=True");
            DataRow[] dRow = dtDEVApproval.Select("RepayCulture='" + lblRepayCulture.Text
                    + "' and LTVPer='" + lblILTVPercentage.Text
                    + "' and IRRPer='" + lblIIRRPercentage.Text
                    + "' and IDVAmount='" + lblIIDV.Text
                    + "' and WaiverAmount='" + lblIWaiverAmount.Text
                    + "' and ARP='" + lblARP.Text
                    + "' and SCW='" + lblSCW.Text + "' and Active='True'");
            if (dRow.Length > 0)
            {
                Utility.FunShowAlertMsg(this, "Already one active entry for the selected Combination exist");
                chkActive.Checked = false;
                return;
            }
            dtDEVApproval.Rows[intRowIndex]["Active"] = true;
        }
        else
        {
            dtDEVApproval.Rows[intRowIndex]["Active"] = false;
        }

        ViewState["currenttable1"] = dtDEVApproval;
    }

    protected bool FunProCheckOverlap(string strFromAmount, string strToAmount)
    {
        DataTable dtCAPMaster = (DataTable)ViewState["currenttable"];

        DataRow[] dRow = dtCAPMaster.Select("From_Amount <=" + strFromAmount + " and To_Amount >= " + strFromAmount + "and Active='True'");
        if (dRow.Length > 0)
        {
            return false;
        }
        dRow = dtCAPMaster.Select("From_Amount <=" + strToAmount + " and To_Amount >=" + strToAmount + "and Active='True'");
        if (dRow.Length > 0)
        {
            return false;
        }

        return true;
    }

    protected void FunProDeleteRowsFromDataTable(ref DataTable dtApproval, ref DataTable dtApprover, ref int intDeletedRow, int intRowIndex)
    {
        DataRow[] dRow = dtApprover.Select("SNo='" + intDeletedRow + "'");
        if (dRow.Length > 0)
        {
            foreach (DataRow dr in dRow)
            {
                dtApprover.Rows.Remove(dr);
            }
        }         
        dtApprover.AcceptChanges();
        dtApproval.Rows[intRowIndex].Delete();
        dtApproval.AcceptChanges();
    }
}
