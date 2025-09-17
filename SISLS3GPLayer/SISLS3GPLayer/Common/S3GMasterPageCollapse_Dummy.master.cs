﻿#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Common
/// Screen Name			: Master Page
/// Created By			: Nataraj Y
/// Created Date		: 
/// Purpose	            : 
/// Last Updated By		: Nataraj Y
/// Last Updated Date   : 13-May-2010
/// Reason              : For Localization
/// Last Updated By		: Nataraj Y
/// Last Updated Date   : 17-May-2010
/// Reason              : For Localization
/// <Program Summary>
#endregion

#region NameSpaces
using System;
using System.Threading;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using AjaxControlToolkit;
using System.Configuration;
using System.IO;
using S3GBusEntity;
#endregion

namespace SmartLend3G
{
    public partial class S3GMasterPageCollapse_Dummy : System.Web.UI.MasterPage
    {
        #region Intialization
        string strLocalization;
        DataSet ds_Menu;
        UserInfo ObjUserInfo = new UserInfo();
        string strMenuText;
        string strModuleNameNew = string.Empty;
        #endregion

        #region Page Events

        #region Page Load
        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (ObjUserInfo.ProUserNameRW != null)
                {
                    string strPageUrl = Request.Url.ToString();
                    string strPageName = strPageUrl.Substring(strPageUrl.LastIndexOf("/") + 1);
                    strPageUrl = strPageUrl.Replace(strPageName, "");
                    strPageUrl = strPageUrl.TrimEnd('/');
                    string strModuleName = strPageUrl.Substring(strPageUrl.LastIndexOf("/") + 1);
                    if (!IsPostBack)
                    {
                        strLocalization = ObjUserInfo.ProLocalizationRW;
                        //ddlTheme.SelectedValue = ObjUserInfo.ProUserThemeRW;

                        lblUser.Text = ObjUserInfo.ProUserNameRW.ToUpper();
                        lblCompany.Text = ObjUserInfo.ProCompanyNameRW.ToUpper();
                        lblLastLoginDate.Text = ObjUserInfo.ProLastLoginRW.ToString("dd-MM-yyyy hh:mm:ss tt");
                        FnPubLoadControllblnameFrmResourceFile();

                        //Accordion1.SelectedIndex = Convert.ToInt32(Session["MenuIndex"]);
                        imgHideMenu.Attributes.Add("onmouseup", "showMenu('F');SetMenuVisibility(0);");
                        imgHideMenu.Attributes.Add("onmouseover", "showtooltip('H')");
                        imgHideMenu.Attributes.Add("onmouseout", "hidetooltip('H')");
                        imgShowMenu.Attributes.Add("onmouseup", "showMenu('T');SetMenuVisibility(1);");
                        imgShowMenu.Attributes.Add("onmouseover", "showtooltip('S');");
                        imgShowMenu.Attributes.Add("onmouseout", "hidetooltip('S')");
                        switch (ObjUserInfo.ProUserThemeRW.ToUpper())
                        {
                            case "S3GTHEME_BLUE2":
                                imgHideMenu.ImageUrl = "~/Images/Blue_2/layout_button_left_blue.gif";
                                imgShowMenu.ImageUrl = "~/Images/Blue_2/layout_button_right_blue.gif";
                                imgLogo.ImageUrl = "~/Images/Blue_2/s3g_logo.png";
                                break;
                            case "S3GTHEME_SILVER":
                                imgHideMenu.ImageUrl = "~/Images/layout_button_left_silver.gif";
                                imgShowMenu.ImageUrl = "~/Images/layout_button_right_blue.gif";
                                imgLogo.ImageUrl = "~/Images/smartlend-logo_gray.jpg";
                                break;
                            case "S3GTHEME_BLUE":
                                imgHideMenu.ImageUrl = "~/Images/layout_button_left.gif";
                                imgShowMenu.ImageUrl = "~/Images/layout_button_right.gif";
                                imgLogo.ImageUrl = "~/Images/logo_blue.jpg";
                                break;
                            default:
                                imgHideMenu.ImageUrl = "~/Images/Orange/layout_button_left_orange.gif";
                                imgShowMenu.ImageUrl = "~/Images/Orange/layout_button_right_orange.gif";
                                imgLogo.ImageUrl = "~/Images/Orange/logo_orange_new.jpg";
                                break;
                        }


                        if (Accordion1.SelectedIndex != 1)
                        {
                            FunPriSetPageTitle();
                        }



                    }
                    FunPriLoadMenu(strModuleName, strPageName);
                }
                else
                {
                    Response.Redirect("~/SessionExpired.aspx");
                }

            }
            catch (Exception ex)
            {
                  ClsPubCommErrorLogDB.CustomErrorRoutine(ex, "Master Page");
            }

        }

        #endregion



        /// <summary>
        /// Event triggered onPage Intialization
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {


            //Ramesh M for cultureinfo
            base.OnInit(e);
            //Modified by Nataraj Y to add localization to be obtained from Session variable 
            if (strLocalization != null)
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(strLocalization);
                Thread.CurrentThread.CurrentCulture = new CultureInfo(strLocalization);
            }
            else
            {
                strLocalization = "en";//ConfigurationManager.AppSettings["localization"].ToString();

            }
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
            Response.Cache.SetNoStore();
            // FunPriLoadMenu();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Menu", "javascript:GetMenuVisibility();", true);

        }

        /// <summary>
        /// Event To handle Home link click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkHome_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Common/HomePage.aspx");
        }

        protected void lnkChangePassword_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Common/Changepassword.aspx");
        }

        /// <summary>
        /// Event to handle Theme Combo selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTheme_SelectedIndexChanged(object sender, EventArgs e)
        {
            ObjUserInfo.ProUserThemeRW = ddlTheme.SelectedValue;
            Server.Transfer(Request.FilePath);
        }

        /// <summary>
        /// Event to handle Signout Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkSignOut_Click(object sender, EventArgs e)
        {
            Utility.FunPubKillSession();
            string strPath;
            if (ObjUserInfo.ProUserTypeRW == "USER")
            {
                strPath = Server.MapPath(@"~\Data\UserManagement\");
            }
            else
            {
                strPath = Server.MapPath(@"~\Data\UserManagement\UTPA\");
            }
            try
            {
                File.Delete(strPath + ObjUserInfo.ProUserIdRW.ToString() + ".xml");
            }
            catch
            {

            }
            //Added by Thangam M on 28-Sep-2013 for Row lock
            if (Session["RemoveLock"] != null)
            {
                Utility.FunPriRemoveLockedRow(ObjUserInfo.ProUserIdRW, "0", "0");
                Session.Remove("RemoveLock");
            }
            //End here
            Response.Redirect("~/LoginPage.aspx");
        }

        #endregion

        #region Page Methods
        /// <summary>
        /// Method to load Menu from db 
        /// </summary>
        private void FunPriLoadMenu(string strModuleNameCur, string strPageName)
        {
            try
            {
                // ObjUserInfo = new UserInfo();
                ds_Menu = new DataSet();

                string strPath;
                if (ObjUserInfo.ProUserTypeRW == "USER")
                {
                    strPath = Server.MapPath(@"~\Data\UserManagement\");
                }
                else
                {
                    strPath = Server.MapPath(@"~\Data\UserManagement\UTPA\");
                }
                if (!Directory.Exists(strPath))
                {
                    Directory.CreateDirectory(strPath);
                }

                if (!File.Exists(strPath + ObjUserInfo.ProUserIdRW.ToString() + ".xml"))
                {
                    S3GAdminServicesReference.S3GAdminServicesClient ObjMenuLoading = new S3GAdminServicesReference.S3GAdminServicesClient();
                    byte[] byte_Menu = ObjMenuLoading.FunPubGetUserMenu(ObjUserInfo.ProUserIdRW, ObjUserInfo.ProUserNameRW.ToUpper(), ObjUserInfo.ProUserTypeRW);
                    ds_Menu = (DataSet)S3GBusEntity.ClsPubSerialize.DeSerialize(byte_Menu, S3GBusEntity.SerializationMode.Binary, typeof(DataSet));
                    ds_Menu.WriteXml(strPath + ObjUserInfo.ProUserIdRW.ToString() + ".xml");
                }
                else
                {
                    ds_Menu.ReadXml(strPath + ObjUserInfo.ProUserIdRW.ToString() + ".xml");
                }
                if (ds_Menu != null && (ds_Menu.Tables.Count > 0) && (ds_Menu.Tables[0].Rows.Count > 0))
                {
                    if (ds_Menu.Tables.Count > 1)
                    {
                        for (int intModuleCount = 0; intModuleCount < ds_Menu.Tables["Module_Header"].Rows.Count; intModuleCount++)
                        {
                            AccordionPane accpaneProgram = new AccordionPane();
                            accpaneProgram.ID = "Accpane" + intModuleCount.ToString();
                            string strModuleName = ds_Menu.Tables["Module_Header"].Rows[intModuleCount].ItemArray[2].ToString();
                            if (strModuleNameCur.ToUpper() == strModuleName.ToUpper())
                            {
                                Accordion1.SelectedIndex = intModuleCount;
                            }
                            //Code Added by Kali to change module name on 23-Dec-2011
                            if (strModuleName == "LoanAdmin")
                                strModuleNameNew = "Loan Admin";
                            else if (strModuleName == "LegalRepossession")
                                strModuleNameNew = "Legal Repossession";
                            //Added by Thangam M 0n 02/Nov/2012 for TA
                            else if (strModuleName == "TradeAdvance")
                                strModuleNameNew = "Trade Advance";
                            else
                                strModuleNameNew = strModuleName;
                            accpaneProgram.HeaderContainer.Controls.Add(new LiteralControl(strModuleNameNew));
                            //Code End
                            for (int intProgCount = 0; intProgCount < ds_Menu.Tables[strModuleName].Rows.Count; intProgCount++)
                            {
                                LinkButton lbtnProgram = new LinkButton();
                                Image imgMenuItems = new Image();
                                imgMenuItems.ID = "Img" + intProgCount.ToString();
                                lbtnProgram.ID = "lbtn" + intProgCount.ToString();
                                lbtnProgram.Click += new EventHandler(lbtnProgram_Click);
                                switch (ObjUserInfo.ProUserThemeRW.ToUpper())
                                {
                                    case "S3GTHEME_BLUE2":
                                        imgMenuItems.ImageUrl = "~/images/spacer.gif";
                                        break;
                                    default:
                                        imgMenuItems.ImageUrl = "~/images/bullet_button.jpg";
                                        //lbtnProgram.ForeColor = System.Drawing.Color.Orange;
                                        break;
                                }
                                strMenuText = ds_Menu.Tables[strModuleName].Rows[intProgCount].ItemArray[1].ToString();

                                lbtnProgram.PostBackUrl = "~/" + ds_Menu.Tables[strModuleName].Rows[intProgCount].ItemArray[4].ToString() + "/" + ds_Menu.Tables[strModuleName].Rows[intProgCount].ItemArray[3].ToString();
                                lbtnProgram.Text = strMenuText;

                                //(!string.IsNullOrEmpty(Convert.ToString(Session["CurrentPage"])) && Session["CurrentPage"].ToString() == strMenuText && strModuleName.ToUpper() == strModuleNameCur.ToUpper())

                                if ((strPageName.ToUpper().Contains(ds_Menu.Tables[strModuleName].Rows[intProgCount].ItemArray[7].ToString().ToUpper()) && strModuleName.ToUpper() == strModuleNameCur.ToUpper())
                                    || (strPageName.ToUpper().Contains(ds_Menu.Tables[strModuleName].Rows[intProgCount].ItemArray[3].ToString().ToUpper()))
                                    || (strPageName.ToUpper().Contains(ds_Menu.Tables[strModuleName].Rows[intProgCount].ItemArray[3].ToString().Replace("_View", "_Add").ToUpper()))
                                    && strModuleName.ToUpper() == strModuleNameCur.ToUpper())
                                {

                                    //Session["CurrentPage"] = ds_Menu.Tables[strModuleName].Rows[intProgCount].ItemArray[1].ToString();

                                    switch (ObjUserInfo.ProUserThemeRW.ToUpper())
                                    {
                                        case "S3GTHEME_BLUE2":
                                            imgMenuItems.ImageUrl = "~/images/Blue_2/menu_arrow_blue.gif";
                                            //lbtnProgram.ForeColor=;
                                            lbtnProgram.Font.Bold = true;
                                            break;
                                        case "S3GTHEME_SILVER":
                                            lbtnProgram.ForeColor = System.Drawing.Color.FromArgb(172, 174, 189);
                                            break;
                                        default:
                                            //imgMenuItems.ImageUrl = "~/images/bullet_button.jpg";
                                            //lbtnProgram.ForeColor = System.Drawing.Color.Orange;
                                            lbtnProgram.Font.Bold = true;
                                            break;
                                    }
                                }
                                lbtnProgram.CausesValidation = false;
                                accpaneProgram.ContentContainer.Controls.Add(imgMenuItems);
                                accpaneProgram.ContentContainer.Controls.Add(new LiteralControl("&nbsp;&nbsp;&nbsp;"));
                                accpaneProgram.ContentContainer.Controls.Add(lbtnProgram);
                                accpaneProgram.ContentContainer.Controls.Add(new LiteralControl("<br>"));

                            }
                            this.Accordion1.Panes.Add(accpaneProgram);
                            //  FunPriSetPageTitle(strMenuText);

                        }
                    }
                    else
                    {
                        AccordionPane accpaneProgram = new AccordionPane();
                        accpaneProgram.ID = "Accpane0";
                        string strModuleName;
                        if (ObjUserInfo.ProUserTypeRW == "USER")
                        {
                            strModuleName = "System Admin";
                        }
                        else
                        {
                            strModuleName = "Menu";
                            Accordion1.SelectedIndex = 0;
                        }
                        if (strModuleNameCur.ToUpper() == strModuleName.ToUpper())
                        {
                            Accordion1.SelectedIndex = 0;
                        }
                        accpaneProgram.HeaderContainer.Controls.Add(new LiteralControl(strModuleName));
                        for (int intProgCount = 0; intProgCount < ds_Menu.Tables[strModuleName].Rows.Count; intProgCount++)
                        {
                            LinkButton lbtnProgram = new LinkButton();
                            Image imgMenuItems = new Image();
                            imgMenuItems.ID = "Img" + intProgCount.ToString();
                            lbtnProgram.ID = "lbtn" + intProgCount.ToString();
                            switch (ObjUserInfo.ProUserThemeRW.ToUpper())
                            {
                                case "S3GTHEME_BLUE2":
                                    imgMenuItems.ImageUrl = "~/images/spacer.gif";
                                    break;
                                default:
                                    imgMenuItems.ImageUrl = "~/images/bullet_button.jpg";
                                    //lbtnProgram.ForeColor = System.Drawing.Color.Orange;
                                    break;
                            }
                            strMenuText = ds_Menu.Tables[strModuleName].Rows[intProgCount].ItemArray[1].ToString();

                            lbtnProgram.PostBackUrl = "~/" + ds_Menu.Tables[strModuleName].Rows[intProgCount].ItemArray[4].ToString() + "/" + ds_Menu.Tables[strModuleName].Rows[intProgCount].ItemArray[3].ToString();
                            lbtnProgram.Text = strMenuText;
                            if ((strPageName.ToUpper().Contains(ds_Menu.Tables[strModuleName].Rows[intProgCount].ItemArray[6].ToString().ToUpper()) && strModuleName.ToUpper() == strModuleNameCur.ToUpper())
                                || (strPageName.ToUpper().Contains(ds_Menu.Tables[strModuleName].Rows[intProgCount].ItemArray[1].ToString().ToUpper().Replace(" ", "")))
                                || (strPageName.ToUpper().Contains(ds_Menu.Tables[strModuleName].Rows[intProgCount].ItemArray[3].ToString().ToUpper())) || (strPageName.ToUpper().Contains(ds_Menu.Tables[strModuleName].Rows[intProgCount].ItemArray[3].ToString().Replace("_View", "_Add").ToUpper()))
                                && strModuleName.ToUpper() == strModuleNameCur.ToUpper())
                            {

                                switch (ObjUserInfo.ProUserThemeRW.ToUpper())
                                {
                                    case "S3GTHEME_BLUE2":
                                        imgMenuItems.ImageUrl = "~/images/Blue_2/menu_arrow_blue.gif";
                                        //lbtnProgram.ForeColor=;
                                        lbtnProgram.Font.Bold = true;
                                        break;
                                    case "S3GTHEME_SILVER":
                                        lbtnProgram.ForeColor = System.Drawing.Color.FromArgb(172, 174, 189);
                                        break;
                                    default:
                                        //imgMenuItems.ImageUrl = "~/images/bullet_button.jpg";
                                        lbtnProgram.Font.Bold = true;
                                        //lbtnProgram.ForeColor = System.Drawing.Color.Orange;
                                        break;
                                }

                            }
                            lbtnProgram.CausesValidation = false;
                            accpaneProgram.ContentContainer.Controls.Add(imgMenuItems);
                            accpaneProgram.ContentContainer.Controls.Add(new LiteralControl("&nbsp;&nbsp;&nbsp;"));
                            accpaneProgram.ContentContainer.Controls.Add(lbtnProgram);
                            accpaneProgram.ContentContainer.Controls.Add(new LiteralControl("<br>"));

                        }
                        this.Accordion1.Panes.Add(accpaneProgram);

                    }
                }
                else
                {
                    //Changed By Thangam M on 06/Mar/2012 to fix Bug ID - 6011
                    string strRedirectPageView = "window.location.href='../LoginPage.aspx';";

                    File.Delete(strPath + ObjUserInfo.ProUserIdRW.ToString() + ".xml");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('No program is mapped');" + strRedirectPageView, true);
                    //Response.Redirect("~/LoginPage.aspx");
                    //End here
                }

                //Session["CurrentPage"] = null;
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to load Menu");
            }


        }

        protected void lbtnProgram_Click(object sender, EventArgs e)
        {
            Session["CurrentPage"] = ((LinkButton)sender).Text.ToString();
        }

        private void FunPriSetPageTitle()
        {
            try
            {
                string strPageTitle = SiteMapPath1.Provider.CurrentNode.Description;
                strPageTitle = "S3G - " + strPageTitle;
                this.Page.Title = strPageTitle;
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to Set page Title");
            }
        }
        //private void FunPriSetPageTitle(string strTitle)
        //{
        //    //string strPageTitle = SiteMapPath1.Provider.CurrentNode.Description;
        //    string strPageTitle = "S3G - " + strTitle;
        //    this.Page.Title = strPageTitle;
        //}

        /// <summary>
        /// Method To laod Values from Resource file
        /// </summary>
        protected void FnPubLoadControllblnameFrmResourceFile()
        {
            lnkHome.Text = Resources.LocalizationResources.lnkHome;
            lnkAbout.Text = Resources.LocalizationResources.lnkAbout;
            lnkMore.Text = Resources.LocalizationResources.lnkMore;
            //lnkSettings.Text = Resources.LocalizationResources.lnkSettings;
            lnkChangePassword.Text = Resources.LocalizationResources.lnkChangePassword;
            lnkHelp.Text = Resources.LocalizationResources.lnkHelp;
            lnkSignOut.Text = Resources.LocalizationResources.lnkSignOut;

            lblApplyTheme.InnerText = Resources.LocalizationResources.lblApplyTheme;
            // lblCompany.Text = Resources.LocalizationResources.lblCompany;
            lblCompanyName.InnerText = Resources.LocalizationResources.lblCompanyName;
            lblWelcome.InnerText = Resources.LocalizationResources.lblWelcome;
            lblLastLogin.InnerText = Resources.LocalizationResources.lblLastLogin;
            //lblMarquee.InnerText = Resources.LocalizationResources.lblMarquee;
            lblMarquee.InnerText = ObjUserInfo.ProMarqueeRW;
            lnkSiteMap.InnerText = Resources.LocalizationResources.lnkSiteMap;
            lnkAboutUs.InnerText = Resources.LocalizationResources.lnkAboutUs;
            lnkTerms.InnerText = Resources.LocalizationResources.lnkTerms;
            lnkContact.InnerText = Resources.LocalizationResources.lnkContact;
            lnkDisclaimer.InnerText = Resources.LocalizationResources.lnkDisclaimer;
            lnkPrivacy.InnerText = Resources.LocalizationResources.lnkPrivacy;
            lnkFooterMore.InnerText = Resources.LocalizationResources.lnkMore;
            lblCopyRights.InnerText = Resources.LocalizationResources.lblCopyRights;
        }

        #endregion



        protected void btnHidden_Click(object sender, EventArgs e)
        {
            Session["MenuIndex"] = hdnMenuLoad.Value;
        }
        protected void lnkHelp_Click(object sender, EventArgs e)
        {
            string strFileName = "/Help/HLP_SISSL_Company_Master.chm";
            string strScipt = "window.open('../Common/S3GDownloadPage.aspx?qsFileName=" + strFileName + "', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "master", strScipt, true);
        }
    }
}