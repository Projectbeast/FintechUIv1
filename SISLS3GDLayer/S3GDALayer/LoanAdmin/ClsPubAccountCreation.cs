﻿#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Loan Admin
/// Screen Name			: Account Creation DAL Class
/// Created By			: Prabhu K
/// Created Date		: 06-Sep-2010
/// Purpose	            : DAL Class for Account Creation Methods
/// Last Updated By		: NULL
/// Last Updated Date   : NULL
/// Reason              : NULL
/// <Program Summary>
#endregion

#region Namespaces
using System;using S3GDALayer.S3GAdminServices;
using S3GDALayer.S3GAdminServices;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using S3GBusEntity;
using Entity_LoanAdmin = S3GBusEntity.LoanAdmin;
using System.Data.OracleClient;
#endregion

namespace S3GDALayer.LoanAdmin
{
    namespace ContractMgtServices
    {
        
        public class ClsPubAccountCreation
        {
            int intRowsAffected;

            //Code added for getting common connection string  from config file
            Database db;
            public ClsPubAccountCreation()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }

            public DataSet FunPubGetApplicationProcessDetails(int intApplicationProcessId)
            {
                try
                {
                    DataSet ObjDS = new DataSet();
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_LoanAd_LoadAccountDetails");
                    db.AddInParameter(command, "@ApplicationProcessId", DbType.Int32, intApplicationProcessId);
                    db.FunPubLoadDataSet(command, ObjDS, "ApplicationProcessDetails");
                    return ObjDS;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            public DataSet FunPubGetMLASLAApplicable(int intLobId,int intCompanyId)
            {
                try
                {
                    DataSet ObjDS = new DataSet();
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3g_LoanAd_GetMLASLAApplicable");
                    db.AddInParameter(command, "@CompanyId", DbType.Int32, intCompanyId);
                    db.AddInParameter(command, "@LobId", DbType.Int32, intLobId);
                    db.FunPubLoadDataSet(command, ObjDS, "ApplicationProcessDetails");
                    return ObjDS;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            public int FunPubInsertAccountCreationInt(AccountCreationEntity objAccountCreationEntity , out string strPASANo)
            {
                int intErrorCode = 0;
                strPASANo = "";
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand(SPNames.S3G_LOANAD_InsertAccountCreation);

                    db.AddInParameter(command, "@Company_ID", DbType.Int32, objAccountCreationEntity.intCompanyId);
                    db.AddInParameter(command, "@LOB_ID", DbType.Int32, objAccountCreationEntity.intLobId);
                    db.AddInParameter(command, "@Location_ID", DbType.Int32, objAccountCreationEntity.intBranchId);
                    db.AddInParameter(command, "@PANum", DbType.String, objAccountCreationEntity.strPANumber);
                    db.AddInParameter(command, "@Product_ID", DbType.Int32, objAccountCreationEntity.intProductId);

                    db.AddInParameter(command, "@Application_Process_ID", DbType.Int32, objAccountCreationEntity.intApplicationProcessId);
                    db.AddInParameter(command, "@Creation_Date", DbType.DateTime, objAccountCreationEntity.dtCreationDate);
                    db.AddInParameter(command, "@Customer_ID", DbType.Int32, objAccountCreationEntity.intCustomerId);
                    db.AddInParameter(command, "@Sales_Person_ID", DbType.Int32, objAccountCreationEntity.intSalesPersonId);
                    db.AddInParameter(command, "@Finance_Amount", DbType.Decimal, objAccountCreationEntity.dcmFinanceAmount);

                    db.AddInParameter(command, "@Refinance_Contract", DbType.Boolean, true );
                    db.AddInParameter(command, "@Constitution_ID", DbType.Int32, objAccountCreationEntity.intConstitutionId);
                    db.AddInParameter(command, "@Lease_Type", DbType.Int32, objAccountCreationEntity.intLeaseType);
                    db.AddInParameter(command, "@PA_Statustype_Code", DbType.Int32, objAccountCreationEntity.intPAStatusTypeCode);
                    db.AddInParameter(command, "@PA_Status_Code", DbType.Int32, objAccountCreationEntity.intPAStatusCode);

                    db.AddInParameter(command, "@Txn_id", DbType.Int32, objAccountCreationEntity.intTxnId);
                    db.AddInParameter(command, "@Offer_Residual_Value", DbType.Decimal, objAccountCreationEntity.dcmOfferResidualValue);
                    db.AddInParameter(command, "@Offer_Residual_Value_Amount", DbType.Decimal, objAccountCreationEntity.dcmOfferResidualValueAmount);
                    db.AddInParameter(command, "@Offer_Margin", DbType.Decimal, objAccountCreationEntity.dcmOfferMargin);
                    db.AddInParameter(command, "@Offer_Margin_Amount", DbType.Decimal, objAccountCreationEntity.dcmOfferMarginAmount);
                    db.AddInParameter(command, "@Created_By", DbType.Int32, objAccountCreationEntity.intUserId);
                    db.AddInParameter(command, "@Modified_By", DbType.Int32, objAccountCreationEntity.intUserId);

                    
                    db.AddInParameter(command, "@ROIRuleId", DbType.Int32, objAccountCreationEntity.intROIRuleID);
                    db.AddInParameter(command, "@PaymentRuleCardId", DbType.Int32, objAccountCreationEntity.intPaymentRuleId);
                    db.AddInParameter(command, "@LoanAmount", DbType.Decimal, objAccountCreationEntity.dcmLoanAmount);
                    db.AddInParameter(command, "@TenureTypeCode", DbType.Int32, objAccountCreationEntity.intTenureTypeCode);
                    db.AddInParameter(command, "@TenureCode", DbType.Int32, objAccountCreationEntity.intTenureCode);
                    db.AddInParameter(command, "@Tenure", DbType.Int32, objAccountCreationEntity.intTenure);
                    db.AddInParameter(command, "@RepaymentTypecode", DbType.Int32, objAccountCreationEntity.intRepaymentTypecode);
                    db.AddInParameter(command, "@RepaymentCode", DbType.Int32, objAccountCreationEntity.intRepaymentCode);
                    db.AddInParameter(command, "@RepaymentTimeTypeCode", DbType.Int32, objAccountCreationEntity.intRepaymentTimeTypeCode);
                    db.AddInParameter(command, "@RepaymentTimeCode", DbType.Int32, objAccountCreationEntity.intRepaymentTimeCode);
                    db.AddInParameter(command, "@FBDate", DbType.Int32, objAccountCreationEntity.intFBDate);
                    db.AddInParameter(command, "@AdvanceInstallments", DbType.Int32, objAccountCreationEntity.intAdvanceInstallments);
                    db.AddInParameter(command, "@IsDORequired", DbType.Boolean, objAccountCreationEntity.blnIsDORequired);
                    if(!objAccountCreationEntity.dtLastODICalcDate.ToShortDateString().Contains("1/1/0001"))
                    db.AddInParameter(command, "@LastODICalcDate", DbType.DateTime, objAccountCreationEntity.dtLastODICalcDate);
                    db.AddInParameter(command, "@BusinessIRR", DbType.Decimal, objAccountCreationEntity.dcmBusinessIRR);
                    db.AddInParameter(command, "@CompanyIRR", DbType.Decimal, objAccountCreationEntity.dcmCompanyIRR);
                    db.AddInParameter(command, "@AccountingIRR", DbType.Decimal, objAccountCreationEntity.dcmAccountingIRR);

                    db.AddInParameter(command, "@SA_Internal_code_Ref", DbType.String, objAccountCreationEntity.strSAInternal_code_Ref);
                    db.AddInParameter(command, "@SA_User_Name", DbType.String, objAccountCreationEntity.strSA_User_Name);
                    db.AddInParameter(command, "@SA_User_Address1", DbType.String, objAccountCreationEntity.strSA_User_Address1);
                    db.AddInParameter(command, "@SA_User_Address2", DbType.String, objAccountCreationEntity.strSA_User_Address2);
                    db.AddInParameter(command, "@SA_User_City", DbType.String, objAccountCreationEntity.strSA_User_City);
                    db.AddInParameter(command, "@SA_User_State", DbType.String, objAccountCreationEntity.strSA_User_State);
                    db.AddInParameter(command, "@SA_User_Country", DbType.String, objAccountCreationEntity.strSA_User_Country);
                    db.AddInParameter(command, "@SA_User_Pincode", DbType.String, objAccountCreationEntity.strSA_User_Pincode);
                    db.AddInParameter(command, "@SA_User_Phone", DbType.String, objAccountCreationEntity.strSA_User_Phone);
                    db.AddInParameter(command, "@SA_User_Mobile", DbType.String, objAccountCreationEntity.strSA_User_Mobile);
                    db.AddInParameter(command, "@SA_User_Email", DbType.String, objAccountCreationEntity.strSA_User_Email);
                    db.AddInParameter(command, "@SA_User_Website", DbType.String, objAccountCreationEntity.strSA_User_Website);

                    S3GDALDBType enumDBType = Common.ClsIniFileAccess.S3G_DBType;
                    if (enumDBType == S3GDALDBType.ORACLE)
                    {
                        OracleParameter param;
                        if (objAccountCreationEntity.XmlRepaymentStructure != null)
                        {
                            param = new OracleParameter("@XML_RepaymentStructure", OracleType.Clob,
                                objAccountCreationEntity.XmlRepaymentStructure.Length, ParameterDirection.Input, true,
                                0, 0, String.Empty, DataRowVersion.Default, objAccountCreationEntity.XmlRepaymentStructure);
                            command.Parameters.Add(param);
                        }

                        if (objAccountCreationEntity.XmlConstitutionDocDetails != null)
                        {
                            param = new OracleParameter("@XmlConstitutionDocDetails", OracleType.Clob,
                                objAccountCreationEntity.XmlConstitutionDocDetails.Length, ParameterDirection.Input, true,
                                0, 0, String.Empty, DataRowVersion.Default, objAccountCreationEntity.XmlConstitutionDocDetails);
                            command.Parameters.Add(param);
                        }

                        if (objAccountCreationEntity.XmlROIDetails != null)
                        {
                            param = new OracleParameter("@XmlROIDetails", OracleType.Clob,
                                objAccountCreationEntity.XmlROIDetails.Length, ParameterDirection.Input, true,
                                0, 0, String.Empty, DataRowVersion.Default, objAccountCreationEntity.XmlROIDetails);
                            command.Parameters.Add(param);
                        }

                        if (objAccountCreationEntity.XmlFollowDetails != null)
                        {
                            param = new OracleParameter("@XmlFollowupDetail", OracleType.Clob,
                                objAccountCreationEntity.XmlFollowDetails.Length, ParameterDirection.Input, true,
                                0, 0, String.Empty, DataRowVersion.Default, objAccountCreationEntity.XmlFollowDetails);
                            command.Parameters.Add(param);
                        }

                        if (objAccountCreationEntity.XmlAlertDetails != null)
                        {
                            param = new OracleParameter("@XmlAlertDetails", OracleType.Clob,
                                objAccountCreationEntity.XmlAlertDetails.Length, ParameterDirection.Input, true,
                                0, 0, String.Empty, DataRowVersion.Default, objAccountCreationEntity.XmlAlertDetails);
                            command.Parameters.Add(param);
                        }

                        if (objAccountCreationEntity.XmlGuarantorDetails != null)
                        {
                            param = new OracleParameter("@XmlGuarantorDetails", OracleType.Clob,
                                objAccountCreationEntity.XmlGuarantorDetails.Length, ParameterDirection.Input, true,
                                0, 0, String.Empty, DataRowVersion.Default, objAccountCreationEntity.XmlGuarantorDetails);
                            command.Parameters.Add(param);
                        }

                        if (objAccountCreationEntity.XmlMoratoriumDetails != null)
                        {
                            param = new OracleParameter("@XmlMoratoriumDetails", OracleType.Clob,
                                objAccountCreationEntity.XmlMoratoriumDetails.Length, ParameterDirection.Input, true,
                                0, 0, String.Empty, DataRowVersion.Default, objAccountCreationEntity.XmlMoratoriumDetails);
                            command.Parameters.Add(param);
                        }

                        if (objAccountCreationEntity.XmlRepaymentDetails != null)
                        {
                            param = new OracleParameter("@XmlRepaymentDetails", OracleType.Clob,
                                objAccountCreationEntity.XmlRepaymentDetails.Length, ParameterDirection.Input, true,
                                0, 0, String.Empty, DataRowVersion.Default, objAccountCreationEntity.XmlRepaymentDetails);
                            command.Parameters.Add(param);
                        }

                        if (objAccountCreationEntity.XmlCashInflowDetails != null)
                        {
                            param = new OracleParameter("@XmlInflowDetails", OracleType.Clob,
                                objAccountCreationEntity.XmlCashInflowDetails.Length, ParameterDirection.Input, true,
                                0, 0, String.Empty, DataRowVersion.Default, objAccountCreationEntity.XmlCashInflowDetails);
                            command.Parameters.Add(param);
                        }

                        if (objAccountCreationEntity.XmlOutFlowDetails != null)
                        {
                            param = new OracleParameter("@XmlOutflowDetails", OracleType.Clob,
                                objAccountCreationEntity.XmlOutFlowDetails.Length, ParameterDirection.Input, true,
                                0, 0, String.Empty, DataRowVersion.Default, objAccountCreationEntity.XmlOutFlowDetails);
                            command.Parameters.Add(param);
                        }

                        if (objAccountCreationEntity.XmlAssetDetails != null)
                        {
                            param = new OracleParameter("@XmlAssetDetails", OracleType.Clob,
                                objAccountCreationEntity.XmlAssetDetails.Length, ParameterDirection.Input, true,
                                0, 0, String.Empty, DataRowVersion.Default, objAccountCreationEntity.XmlAssetDetails);
                            command.Parameters.Add(param);
                        }

                        if (objAccountCreationEntity.XmlInvoiceDetails != null)
                        {
                            param = new OracleParameter("@XmlInvoiceDetails", OracleType.Clob,
                                objAccountCreationEntity.XmlInvoiceDetails.Length, ParameterDirection.Input, true,
                                0, 0, String.Empty, DataRowVersion.Default, objAccountCreationEntity.XmlInvoiceDetails);
                            command.Parameters.Add(param);
                        }

                        if (objAccountCreationEntity.XMLRepayDetailsOthers != null)
                        {
                            param = new OracleParameter("@XMLRepayDetailsOthers", OracleType.Clob,
                                objAccountCreationEntity.XMLRepayDetailsOthers.Length, ParameterDirection.Input, true,
                                0, 0, String.Empty, DataRowVersion.Default, objAccountCreationEntity.XMLRepayDetailsOthers);
                            command.Parameters.Add(param);
                        }
                    }
                    else
                    {
                        db.AddInParameter(command, "@XmlConstitutionDocDetails", DbType.String, objAccountCreationEntity.XmlConstitutionDocDetails);
                        db.AddInParameter(command, "@XmlROIDetails", DbType.String, objAccountCreationEntity.XmlROIDetails);
                        db.AddInParameter(command, "@XmlFollowupDetail", DbType.String, objAccountCreationEntity.XmlFollowDetails);
                        db.AddInParameter(command, "@XmlAlertDetails", DbType.String, objAccountCreationEntity.XmlAlertDetails);
                        db.AddInParameter(command, "@XmlGuarantorDetails", DbType.String, objAccountCreationEntity.XmlGuarantorDetails);
                        db.AddInParameter(command, "@XmlMoratoriumDetails", DbType.String, objAccountCreationEntity.XmlMoratoriumDetails);
                        db.AddInParameter(command, "@XmlRepaymentDetails", DbType.String, objAccountCreationEntity.XmlRepaymentDetails);
                        db.AddInParameter(command, "@XmlInflowDetails", DbType.String, objAccountCreationEntity.XmlCashInflowDetails);
                        db.AddInParameter(command, "@XmlOutflowDetails", DbType.String, objAccountCreationEntity.XmlOutFlowDetails);
                        db.AddInParameter(command, "@XmlAssetDetails", DbType.String, objAccountCreationEntity.XmlAssetDetails);
                        db.AddInParameter(command, "@XmlInvoiceDetails", DbType.String, objAccountCreationEntity.XmlInvoiceDetails);                        
                        db.AddInParameter(command, "@XML_RepaymentStructure", DbType.String, objAccountCreationEntity.XmlRepaymentStructure);
                        db.AddInParameter(command, "@XMLRepayDetailsOthers", DbType.String, objAccountCreationEntity.XMLRepayDetailsOthers);
                    }

                    if (!string.IsNullOrEmpty(objAccountCreationEntity.strConsSplitNo))
                    {
                        db.AddInParameter(command, "@DocumentNo", DbType.String, objAccountCreationEntity.strConsSplitNo);
                    }
                    if (!string.IsNullOrEmpty(objAccountCreationEntity.strSplit_RefNo))
                    {
                        db.AddInParameter(command, "@Split_RefNo", DbType.String, objAccountCreationEntity.strSplit_RefNo);
                    }
                    db.AddOutParameter(command, "@AccountNumber", DbType.String, 100);
                    db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                    using (DbConnection conn = db.CreateConnection())
                    {
                        conn.Open();
                        DbTransaction trans = conn.BeginTransaction();
                        try
                        {
                            db.FunPubExecuteNonQuery(command, ref trans);
                            if ((int)command.Parameters["@ErrorCode"].Value != 0)
                            {
                                intErrorCode = (int)command.Parameters["@ErrorCode"].Value;
                                
                            }
                            else
                            {
                                strPASANo = Convert.ToString(command.Parameters["@AccountNumber"].Value);
                                trans.Commit();
                            }

                        }
                        catch (Exception ex)
                        {
                            if (intErrorCode == 0)
                                intErrorCode = 50;
                             ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                            trans.Rollback();
                        }
                        finally
                        {
                            conn.Close();
                        }
                    }

                   
                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return intErrorCode;
            }
 
  

            public int FunPubModifyAccountCreationInt(AccountCreationEntity objAccountCreationEntity)
            {
                int intErrorCode = 0;
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("LA_UPD_AC_CREAT_ASNTAG");

                    db.AddInParameter(command, "@AccountCreationId", DbType.Int32, objAccountCreationEntity.intAccountCreationId);
                    db.AddInParameter(command, "@Company_ID", DbType.Int32, objAccountCreationEntity.intCompanyId);
                    db.AddInParameter(command, "@AssigneeName", DbType.String, objAccountCreationEntity.strAssigneeName);
                    db.AddInParameter(command, "@AssigneeEffDate", DbType.DateTime, objAccountCreationEntity.dtAssigneeEffDate);
                    db.AddInParameter(command, "@PANum", DbType.String, objAccountCreationEntity.strPANumber);
                    db.AddInParameter(command, "@Modified_By", DbType.Int32, objAccountCreationEntity.intUserId);
                    db.AddInParameter(command, "@assignee_Remarks", DbType.String, objAccountCreationEntity.strAssigneeRemarks);
                    db.AddInParameter(command, "@assignee_Status", DbType.Int32, objAccountCreationEntity.intAssigneeStatus);

                    //db.AddInParameter(command, "@LOB_ID", DbType.Int32, objAccountCreationEntity.intLobId);
                    //db.AddInParameter(command, "@Location_ID", DbType.Int32, objAccountCreationEntity.intBranchId);
                 
                    //db.AddInParameter(command, "@SANumber", DbType.String, objAccountCreationEntity.strSANum);
                    //db.AddInParameter(command, "@Creation_Date", DbType.DateTime, objAccountCreationEntity.dtCreationDate);
                    //db.AddInParameter(command, "@Modified_By", DbType.Int32, objAccountCreationEntity.intUserId);
                    //db.AddInParameter(command, "@ROIRuleId", DbType.Int32, objAccountCreationEntity.intROIRuleID);
                    //db.AddInParameter(command, "@Tenure", DbType.Int32, objAccountCreationEntity.intTenure);
                    
                    //db.AddInParameter(command, "@RepaymentCode", DbType.Int32, objAccountCreationEntity.intRepaymentCode);
                    //db.AddInParameter(command, "@RepaymentTimeCode", DbType.Int32, objAccountCreationEntity.intRepaymentTimeCode);
                    //db.AddInParameter(command, "@FBDate", DbType.Int32, objAccountCreationEntity.intFBDate);
                    //db.AddInParameter(command, "@AdvanceInstallments", DbType.Int32, objAccountCreationEntity.intAdvanceInstallments);
                    //db.AddInParameter(command, "@IsDORequired", DbType.Boolean, objAccountCreationEntity.blnIsDORequired);
                    //if (!objAccountCreationEntity.dtLastODICalcDate.ToShortDateString().Contains("1/1/0001"))
                    //    db.AddInParameter(command, "@LastODICalcDate", DbType.DateTime, objAccountCreationEntity.dtLastODICalcDate);

                    //db.AddInParameter(command, "@BusinessIRR", DbType.Decimal, objAccountCreationEntity.dcmBusinessIRR);
                    //db.AddInParameter(command, "@CompanyIRR", DbType.Decimal, objAccountCreationEntity.dcmCompanyIRR);
                    //db.AddInParameter(command, "@AccountingIRR", DbType.Decimal, objAccountCreationEntity.dcmAccountingIRR);

                    //db.AddInParameter(command, "@XmlConstitutionDocDetails", DbType.String, objAccountCreationEntity.XmlConstitutionDocDetails);
                    //db.AddInParameter(command, "@XmlROIDetails", DbType.String, objAccountCreationEntity.XmlROIDetails);
                    //db.AddInParameter(command, "@XmlFollowupDetail", DbType.String, objAccountCreationEntity.XmlFollowDetails);
                    //db.AddInParameter(command, "@XmlAlertDetails", DbType.String, objAccountCreationEntity.XmlAlertDetails);
                    
                    //db.AddInParameter(command, "@XmlRepaymentDetails", DbType.String, objAccountCreationEntity.XmlRepaymentDetails);
                    //db.AddInParameter(command, "@XmlInflowDetails", DbType.String, objAccountCreationEntity.XmlCashInflowDetails);
                    //db.AddInParameter(command, "@XmlOutflowDetails", DbType.String, objAccountCreationEntity.XmlOutFlowDetails);
                    //db.AddInParameter(command, "@XmlInvoiceDetails", DbType.String, objAccountCreationEntity.XmlInvoiceDetails);

                    //S3GDALDBType enumDBType = Common.ClsIniFileAccess.S3G_DBType;
                    //if (enumDBType == S3GDALDBType.ORACLE)
                    //{
                    //    OracleParameter param = new OracleParameter("@XML_RepaymentStructure", OracleType.Clob,
                    //        objAccountCreationEntity.XmlRepaymentStructure.Length, ParameterDirection.Input, true,
                    //        0, 0, String.Empty, DataRowVersion.Default, objAccountCreationEntity.XmlRepaymentStructure);
                    //    command.Parameters.Add(param);
                    //}
                    //else
                    //{
                    //    db.AddInParameter(command, "@XML_RepaymentStructure", DbType.String, objAccountCreationEntity.XmlRepaymentStructure);
                    //}

                    S3GDALDBType enumDBType = Common.ClsIniFileAccess.S3G_DBType;
                    if (enumDBType == S3GDALDBType.ORACLE)
                    {
                        OracleParameter param;
                        //if (objAccountCreationEntity.XmlConstitutionDocDetails != null)
                        //{
                        //    param = new OracleParameter("@XmlConstitutionDocDetails", OracleType.Clob,
                        //        objAccountCreationEntity.XmlConstitutionDocDetails.Length, ParameterDirection.Input, true,
                        //        0, 0, String.Empty, DataRowVersion.Default, objAccountCreationEntity.XmlConstitutionDocDetails);
                        //    command.Parameters.Add(param);
                        //}

                        //if (objAccountCreationEntity.XmlROIDetails != null)
                        //{
                        //    param = new OracleParameter("@XmlROIDetails", OracleType.Clob,
                        //        objAccountCreationEntity.XmlROIDetails.Length, ParameterDirection.Input, true,
                        //        0, 0, String.Empty, DataRowVersion.Default, objAccountCreationEntity.XmlROIDetails);
                        //    command.Parameters.Add(param);
                        //}

                        //if (objAccountCreationEntity.XmlFollowDetails != null)
                        //{
                        //    param = new OracleParameter("@XmlFollowupDetail", OracleType.Clob,
                        //        objAccountCreationEntity.XmlFollowDetails.Length, ParameterDirection.Input, true,
                        //        0, 0, String.Empty, DataRowVersion.Default, objAccountCreationEntity.XmlFollowDetails);
                        //    command.Parameters.Add(param);
                        //}

                        //if (objAccountCreationEntity.XmlAlertDetails != null)
                        //{
                        //    param = new OracleParameter("@XmlAlertDetails", OracleType.Clob,
                        //        objAccountCreationEntity.XmlAlertDetails.Length, ParameterDirection.Input, true,
                        //        0, 0, String.Empty, DataRowVersion.Default, objAccountCreationEntity.XmlAlertDetails);
                        //    command.Parameters.Add(param);
                        //}

                        //if (objAccountCreationEntity.XmlRepaymentDetails != null)
                        //{
                        //    param = new OracleParameter("@XmlRepaymentDetails", OracleType.Clob,
                        //        objAccountCreationEntity.XmlRepaymentDetails.Length, ParameterDirection.Input, true,
                        //        0, 0, String.Empty, DataRowVersion.Default, objAccountCreationEntity.XmlRepaymentDetails);
                        //    command.Parameters.Add(param);
                        //}

                        //if (objAccountCreationEntity.XmlCashInflowDetails != null)
                        //{
                        //    param = new OracleParameter("@XmlInflowDetails", OracleType.Clob,
                        //        objAccountCreationEntity.XmlCashInflowDetails.Length, ParameterDirection.Input, true,
                        //        0, 0, String.Empty, DataRowVersion.Default, objAccountCreationEntity.XmlCashInflowDetails);
                        //    command.Parameters.Add(param);
                        //}

                        //if (objAccountCreationEntity.XmlOutFlowDetails != null)
                        //{
                        //    param = new OracleParameter("@XmlOutflowDetails", OracleType.Clob,
                        //        objAccountCreationEntity.XmlOutFlowDetails.Length, ParameterDirection.Input, true,
                        //        0, 0, String.Empty, DataRowVersion.Default, objAccountCreationEntity.XmlOutFlowDetails);
                        //    command.Parameters.Add(param);
                        //}

                        //if (objAccountCreationEntity.XmlInvoiceDetails != null)
                        //{
                        //    param = new OracleParameter("@XmlInvoiceDetails", OracleType.Clob,
                        //        objAccountCreationEntity.XmlInvoiceDetails.Length, ParameterDirection.Input, true,
                        //        0, 0, String.Empty, DataRowVersion.Default, objAccountCreationEntity.XmlInvoiceDetails);
                        //    command.Parameters.Add(param);
                        //}

                        //if (objAccountCreationEntity.XmlRepaymentStructure != null)
                        //{
                        //    param = new OracleParameter("@XML_RepaymentStructure", OracleType.Clob,
                        //        objAccountCreationEntity.XmlRepaymentStructure.Length, ParameterDirection.Input, true,
                        //        0, 0, String.Empty, DataRowVersion.Default, objAccountCreationEntity.XmlRepaymentStructure);
                        //    command.Parameters.Add(param);
                        //}

                        //if (objAccountCreationEntity.XMLRepayDetailsOthers != null)
                        //{
                        //    param = new OracleParameter("@XMLRepayDetailsOthers", OracleType.Clob,
                        //        objAccountCreationEntity.XMLRepayDetailsOthers.Length, ParameterDirection.Input, true,
                        //        0, 0, String.Empty, DataRowVersion.Default, objAccountCreationEntity.XMLRepayDetailsOthers);
                        //    command.Parameters.Add(param);
                        //}


                        if (objAccountCreationEntity.XmlAssetDetails != null)
                        {
                            param = new OracleParameter("@xmlassetdetailsExists", OracleType.Clob,
                                objAccountCreationEntity.XmlAssetDetails.Length, ParameterDirection.Input, true,
                                0, 0, String.Empty, DataRowVersion.Default, objAccountCreationEntity.XmlAssetDetails);
                            command.Parameters.Add(param);
                        }
                        if (objAccountCreationEntity.XmlInvoiceDetails != null)
                        {
                            param = new OracleParameter("@xmlassetdetailsNew", OracleType.Clob,
                                objAccountCreationEntity.XmlInvoiceDetails.Length, ParameterDirection.Input, true,
                                0, 0, String.Empty, DataRowVersion.Default, objAccountCreationEntity.XmlInvoiceDetails);
                            command.Parameters.Add(param);
                        }
                         //Enabled by Arunkumar K for GuarantorDetails update on 13-Sep-2016
                        if (objAccountCreationEntity.XmlGuarantorDetails != null)
                        {
                            param = new OracleParameter("@XmlGuarantorDetails", OracleType.Clob,
                                objAccountCreationEntity.XmlGuarantorDetails.Length, ParameterDirection.Input, true,
                                0, 0, String.Empty, DataRowVersion.Default, objAccountCreationEntity.XmlGuarantorDetails);
                            command.Parameters.Add(param);
                        }
                        //Enabled by Arunkumar K for GuarantorDetails update on 13-Sep-2016

                        //if (objAccountCreationEntity.XmlMoratoriumDetails != null)
                        //{
                        //    param = new OracleParameter("@XmlMoratoriumDetails", OracleType.Clob,
                        //        objAccountCreationEntity.XmlMoratoriumDetails.Length, ParameterDirection.Input, true,
                        //        0, 0, String.Empty, DataRowVersion.Default, objAccountCreationEntity.XmlMoratoriumDetails);
                        //    command.Parameters.Add(param);
                        //}
                       
                    }
                    else
                    {
                        db.AddInParameter(command, "@XmlConstitutionDocDetails", DbType.String, objAccountCreationEntity.XmlConstitutionDocDetails);
                        db.AddInParameter(command, "@XmlROIDetails", DbType.String, objAccountCreationEntity.XmlROIDetails);
                        db.AddInParameter(command, "@XmlFollowupDetail", DbType.String, objAccountCreationEntity.XmlFollowDetails);
                        db.AddInParameter(command, "@XmlAlertDetails", DbType.String, objAccountCreationEntity.XmlAlertDetails);
                        db.AddInParameter(command, "@XmlGuarantorDetails", DbType.String, objAccountCreationEntity.XmlGuarantorDetails);//Enabled by Arunkumar K for GuarantorDetails update on 13-Sep-2016
                        //db.AddInParameter(command, "@XmlMoratoriumDetails", DbType.String, objAccountCreationEntity.XmlMoratoriumDetails);
                        db.AddInParameter(command, "@XmlRepaymentDetails", DbType.String, objAccountCreationEntity.XmlRepaymentDetails);
                        db.AddInParameter(command, "@XmlInflowDetails", DbType.String, objAccountCreationEntity.XmlCashInflowDetails);
                        db.AddInParameter(command, "@XmlOutflowDetails", DbType.String, objAccountCreationEntity.XmlOutFlowDetails);
                        db.AddInParameter(command, "@XmlAssetDetails", DbType.String, objAccountCreationEntity.XmlAssetDetails);
                        db.AddInParameter(command, "@XmlInvoiceDetails", DbType.String, objAccountCreationEntity.XmlInvoiceDetails);
                        db.AddInParameter(command, "@XML_RepaymentStructure", DbType.String, objAccountCreationEntity.XmlRepaymentStructure);
                        db.AddInParameter(command, "@XMLRepayDetailsOthers", DbType.String, objAccountCreationEntity.XMLRepayDetailsOthers);
                    }

                    //db.AddInParameter(command, "@SA_Internal_code_Ref", DbType.String, objAccountCreationEntity.strSAInternal_code_Ref);
                    //db.AddInParameter(command, "@SA_User_Name", DbType.String, objAccountCreationEntity.strSA_User_Name);
                    //db.AddInParameter(command, "@SA_User_Address1", DbType.String, objAccountCreationEntity.strSA_User_Address1);
                    //db.AddInParameter(command, "@SA_User_Address2", DbType.String, objAccountCreationEntity.strSA_User_Address2);
                    //db.AddInParameter(command, "@SA_User_City", DbType.String, objAccountCreationEntity.strSA_User_City);
                    //db.AddInParameter(command, "@SA_User_State", DbType.String, objAccountCreationEntity.strSA_User_State);
                    //db.AddInParameter(command, "@SA_User_Country", DbType.String, objAccountCreationEntity.strSA_User_Country);
                    //db.AddInParameter(command, "@SA_User_Pincode", DbType.String, objAccountCreationEntity.strSA_User_Pincode);
                    //db.AddInParameter(command, "@SA_User_Phone", DbType.String, objAccountCreationEntity.strSA_User_Phone);
                    //db.AddInParameter(command, "@SA_User_Mobile", DbType.String, objAccountCreationEntity.strSA_User_Mobile);
                    //db.AddInParameter(command, "@SA_User_Email", DbType.String, objAccountCreationEntity.strSA_User_Email);
                    //db.AddInParameter(command, "@SA_User_Website", DbType.String, objAccountCreationEntity.strSA_User_Website);
                    
                    db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                    using (DbConnection conn = db.CreateConnection())
                    {
                        conn.Open();
                        DbTransaction trans = conn.BeginTransaction();
                        try
                        {
                            db.FunPubExecuteNonQuery(command, ref trans);
                            if ((int)command.Parameters["@ErrorCode"].Value > 0)
                            {
                                intErrorCode = (int)command.Parameters["@ErrorCode"].Value;
                                throw new Exception("Error in Creating Account " + intErrorCode.ToString());
                            }
                            else
                            {
                                trans.Commit();
                            }

                        }
                        catch (Exception ex)
                        {
                            if (intErrorCode == 0)
                                intErrorCode = 50;
                             //ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                            ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                            trans.Rollback();
                        }
                        finally
                        {
                            conn.Close();
                        }
                    }
                   
                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return intErrorCode;
            }

            public int FunPubUpdateAccountStatus(AccountCreationEntity objAccountCreationEntity)
            {
                int intErrorCode = 0;
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_LoanAd_CancelAccountCreation");

                    db.AddInParameter(command, "@CompanyId", DbType.Int32, objAccountCreationEntity.intCompanyId);
                    db.AddInParameter(command, "@PANum", DbType.String, objAccountCreationEntity.strPANumber);
                    db.AddInParameter(command, "@SANum", DbType.String, objAccountCreationEntity.strSANum);
                    db.AddInParameter(command, "@UserId", DbType.Int32, objAccountCreationEntity.intUserId);
                    db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                    db.FunPubExecuteNonQuery(command);
                    if (command.Parameters["@ErrorCode"].Value != null)
                    {
                        intErrorCode = Convert.ToInt32(command.Parameters["@ErrorCode"].Value);
                    }
                    return intErrorCode;

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        
        }
    }
}
