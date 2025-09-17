﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using S3GBusEntity;
using System.Data;
using S3GDALayer.Collection;
using System.ServiceModel.Activation;

namespace S3GServiceLayer.Collection
{
    // To Implement Service Compatablity
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple)]
    // NOTE: If you change the class name "ClnReceiptMgtServicesReference" here, you must also update the reference to "ClnReceiptMgtServicesReference" in Web.config.
    public class ClnReceiptMgtServices : IClnReceiptMgtServices
    {
        byte[] bytesDataTable;
        int intResult;
        int intErrorCode;

        #region "Appropriation Logic"
        /// <summary>
        /// Inserting records in Appropriation Logic details table
        /// </summary>
        /// <param name="SMode">Pass SerializationMode</param>
        /// <param name="byteEnquiryService">Pass bype object</param>
        public int FunPubCreateAppropriationLogic(SerializationMode SMode, byte[] byteAppropriationLogicService, out string strApproID)
        {
            try
            {
                S3GDALayer.Collection.ClnReceiptMgtServices.ClsPubAppropriationLogic objClsPubAppropriationLogic = new S3GDALayer.Collection.ClnReceiptMgtServices.ClsPubAppropriationLogic();
                return objClsPubAppropriationLogic.FunPubCreateAppropriationLogic(SMode, byteAppropriationLogicService, out strApproID);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in Appropriation Logic Creation :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

        /// <summary>
        /// Getting Appropriation Logic details 
        /// </summary>
        /// <param name="SMode">Pass SerializationMode</param>
        /// <param name="byteEnquiryService">Pass bype object</param>

        public byte[] FunAppropriationLogicForModification(string strApproID, int Company_ID)
        {
            DataTable dtAppLogicable = new DataTable();
            S3GDALayer.Collection.ClnReceiptMgtServices.ClsPubAppropriationLogic objClsPubAppropriationLogic = new S3GDALayer.Collection.ClnReceiptMgtServices.ClsPubAppropriationLogic();
            dtAppLogicable = objClsPubAppropriationLogic.FunAppropriationLogicForModification(strApproID, Company_ID);
            bytesDataTable = ClsPubSerialize.Serialize(dtAppLogicable, SerializationMode.Binary);
            return bytesDataTable;
        }
        #endregion

        #region "Challan Generation"
        /// <summary>
        /// Inserting records in Challan Generation details table
        /// </summary>
        /// <param name="SMode">Pass SerializationMode</param>
        /// <param name="byteEnquiryService">Pass bype object</param>
        public int FunPubCreateChallanGenerationLogic(SerializationMode SMode, byte[] byteChallanGenerationService, out string strChallanNumber)
        {
            try
            {
                S3GDALayer.Collection.ClnReceiptMgtServices.ClsPubChallanGeneration objClsPubChallanGeneration = new S3GDALayer.Collection.ClnReceiptMgtServices.ClsPubChallanGeneration();
                return objClsPubChallanGeneration.FunPubCreateChallanGenerationLogic(SMode, byteChallanGenerationService, out strChallanNumber);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in Challan Generation :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }
        #endregion

        #region "Cheque Return"
        /// <summary>
        /// Inserting records in Cheque REturn table
        /// </summary>
        /// <param name="SMode">Pass SerializationMode</param>
        /// <param name="byteEnquiryService">Pass bype object</param>
        //public int FunPubCreateChequeReturns(SerializationMode SMode, byte[] bytesChequeReturnDAL)
        //{
        //    try
        //    {
        //        S3GDALayer.Collection.ClnReceiptMgtServices.ClsPubChequeReturn objClsPubChequeReturn = new S3GDALayer.Collection.ClnReceiptMgtServices.ClsPubChequeReturn();
        //        return objClsPubChequeReturn.FunPubCreateChequeReturns(SMode, bytesChequeReturnDAL);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        /// <summary>
        /// Created by Tamilselvan.S
        /// Created Date 19/01/2011
        /// Purpose To Cheque return process 
        /// </summary>
        /// <param name="SMode"></param>
        /// <param name="bytesChequeReturnDAL"></param>
        /// <param name="bytesMemorandumBookingDAL"></param>
        /// <returns></returns>
        public int FunPubCreateChequeReturns(SerializationMode SerMode, byte[] bytesChequeReturnDAL_SERLAY, byte[] bytesMemorandumbookingDAL_SERLAY, out string strChequeReturnNo)
        {
            try
            {
                S3GDALayer.Collection.ClnReceiptMgtServices.ClsPubChequeReturn objClsPubChequeReturn = new S3GDALayer.Collection.ClnReceiptMgtServices.ClsPubChequeReturn();
                return objClsPubChequeReturn.FunPubCreateChequeReturns(SerMode, bytesChequeReturnDAL_SERLAY, bytesMemorandumbookingDAL_SERLAY, out strChequeReturnNo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int FunPubChequeAuthorization(SerializationMode SMode, byte[] bytesChequeAuthorizeDAL)
        {
            try
            {
                S3GDALayer.Collection.ClnReceiptMgtServices.ClsPubChequeReturn objClsPubChequeReturn = new S3GDALayer.Collection.ClnReceiptMgtServices.ClsPubChequeReturn();
                return objClsPubChequeReturn.FunPubChequeAuthorization(SMode, bytesChequeAuthorizeDAL);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int FunPubCreateChequeReturnsMulti(SerializationMode SerMode, byte[] bytesObjS3G_CLN_ChequeTable, out string ChequeReturnNo)
        {
            try
            {
                S3GDALayer.Collection.ClnReceiptMgtServices.ClsPubChequeReturn objClsPubChequeReturn = new S3GDALayer.Collection.ClnReceiptMgtServices.ClsPubChequeReturn();
                return objClsPubChequeReturn.FunPubCreateChequeReturnsMulti(SerMode, bytesObjS3G_CLN_ChequeTable,out ChequeReturnNo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #endregion
        #region "Cheque Return Through Excel"
        public int FunPubCreateChequeReturnsExcel(SerializationMode SerMode, byte[] bytesChequeReturnExcelDAL_SERLAY, out string strChequeReturnNo)
        {
            try
            {
                S3GDALayer.Collection.ClnReceiptMgtServices.ClsPubChequeReturn objClsPubChequeReturnExcel = new S3GDALayer.Collection.ClnReceiptMgtServices.ClsPubChequeReturn();
                return objClsPubChequeReturnExcel.FunPubCreateChequeReturnsExcel(SerMode, bytesChequeReturnExcelDAL_SERLAY, out strChequeReturnNo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region ECS

        public int FunPubCreateECSFormat(SerializationMode SMode, byte[] byteECSFomatService)
        {
            try
            {
                S3GDALayer.Collection.ClnReceiptMgtServices.ClsPubECSSpoolFormats objClsPubECS = new S3GDALayer.Collection.ClnReceiptMgtServices.ClsPubECSSpoolFormats();
                return objClsPubECS.FunPubCreateECSFormat(SMode, byteECSFomatService);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public int FunPubModifyECSFormat(SerializationMode SMode, byte[] byteECSFomatService)
        {
            try
            {
                S3GDALayer.Collection.ClnReceiptMgtServices.ClsPubECSSpoolFormats objClsPubECS = new S3GDALayer.Collection.ClnReceiptMgtServices.ClsPubECSSpoolFormats();
                return objClsPubECS.FunPubModifyECSFormat(SMode, byteECSFomatService);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Erro in Cashflow Creation :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return intResult;
        }

        #endregion

        #region ECS Process

        public int FunPubCreateECSProcess(SerializationMode SMode, byte[] byteECSProcesservice, out string strECSNumber)
        {
            try
            {
                S3GDALayer.Collection.ClnReceiptMgtServices.ClsPubECSProcess objClsPubECS = new S3GDALayer.Collection.ClnReceiptMgtServices.ClsPubECSProcess();
                return objClsPubECS.FunPubCreateECSProcess(SMode, byteECSProcesservice, out strECSNumber);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in ECS Process Creation :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }


        #endregion

        #region Challan_Rule_Creation
        // ----Challan Rule Creation(Created By- Irsathameen K)-------
        // ----Created On 13-Oct-2010------
        //Begin

        public int FunPubCreateChallanRuleDetails(SerializationMode SerMode, byte[] bytesObjS3G_CLN_ChallanRuleDataTable, out string strChallanNo)
        {
            try
            {
                S3GDALayer.Collection.ClnReceiptMgtServices.ClsPubChallanRuleCreation objClsPubChallanRule = new S3GDALayer.Collection.ClnReceiptMgtServices.ClsPubChallanRuleCreation();
                return objClsPubChallanRule.FunPubCreateChallanRuleDetails(SerMode, bytesObjS3G_CLN_ChallanRuleDataTable, out strChallanNo);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }

        }
        //End
        #endregion

        #region PDC Entry
        // ----PDC Entry(Created By- Irsathameen K)-------
        // ----Created On 11-Oct-2010------
        //Begin
        public int FunPubCreatePDCModuleDetails(SerializationMode SerMode, byte[] bytesObjS3G_CLN_PDCModuleDataTable, out string strPDCNo, out string strchequeNo, out string strexistingdate)
        {
            try
            {
                S3GDALayer.Collection.ClnReceiptMgtServices.ClsPubPDCModule objClsPubPDCModule = new S3GDALayer.Collection.ClnReceiptMgtServices.ClsPubPDCModule();
                return objClsPubPDCModule.FunPubCreatePDCModuleDetails(SerMode, bytesObjS3G_CLN_PDCModuleDataTable, out strPDCNo, out strchequeNo, out strexistingdate);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }

        }

        //public int FunPubModifyPDCModuleDetails(SerializationMode SerMode, byte[] bytesObjS3G_CLN_PDCModuleDataTable)
        //{
        //    try
        //    {
        //        S3GDALayer.Collection.ClnReceiptMgtServices.ClsPubPDCModule objClsPubPDCModule = new S3GDALayer.Collection.ClnReceiptMgtServices.ClsPubPDCModule();
        //        return objClsPubPDCModule.FunPubModifyPDCModuleDetails(SerMode, bytesObjS3G_CLN_PDCModuleDataTable);
        //    }
        //    catch (Exception objExp)
        //    {
        //        ClsPubFaultException objFault = new ClsPubFaultException();
        //        objFault.ProReasonRW = objExp.Message.ToString();
        //        throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
        //    }

        //}
        //End


        #endregion


        //public byte[] FunPubGetPDC(SerializationMode SerMode, byte[] bytesObjAccountDetailsDataTable_SERLAY)
        //{
        //    S3GBusEntity.Collection.ClnReceiptMgtServices.S3G_CLN_GetPDCDetailsDataTable dtPDCDetails;
        //    S3GDALayer.Collection.ClnReceiptMgtServices.ClsPubPDCReceipts ObjPDCDetails = new S3GDALayer.Collection.ClnReceiptMgtServices.ClsPubPDCReceipts();
        //    dtPDCDetails = ObjPDCDetails.FunPubGetPDC(SerMode, bytesObjAccountDetailsDataTable_SERLAY);
        //    byte[] bytesAccountDetails = ClsPubSerialize.Serialize(dtPDCDetails, SerializationMode.Binary);
        //    return bytesAccountDetails;
        //}

        public int FunPubCreatePDCBulkReceipt(SerializationMode SMode, byte[] bytesobjS3G_cln_PDCDataTable)
        {
            try
            {
                S3GDALayer.Collection.ClnReceiptMgtServices.ClsPubPDCReceipts objClsPubECS = new S3GDALayer.Collection.ClnReceiptMgtServices.ClsPubPDCReceipts();
                return objClsPubECS.FunPubCreatePDCBulkReceiptProcess(SMode, bytesobjS3G_cln_PDCDataTable);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }
        #region Temporary Book Maintenance
        public int FunPubCreateTempBookDetails(SerializationMode SerMode, byte[] byteObjS3G_Collection_TempBook)
        {
            try
            {
                S3GDALayer.Collection.ClnReceiptMgtServices.ClsPubTemporaryBook objClsPubTemporaryBook = new S3GDALayer.Collection.ClnReceiptMgtServices.ClsPubTemporaryBook();
                return objClsPubTemporaryBook.FunPubCreateTempBookDetails(SerMode, byteObjS3G_Collection_TempBook);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

        public int FunPubUpdateTempBookDetails(SerializationMode SerMode, byte[] byteObjS3G_Collection_TempReceiptBookMaster_DataTable, out string strUsedleafsNumber)
        {
            try
            {
                S3GDALayer.Collection.ClnReceiptMgtServices.ClsPubTemporaryBook objClsPubTemporaryBook = new S3GDALayer.Collection.ClnReceiptMgtServices.ClsPubTemporaryBook();
                return objClsPubTemporaryBook.FunPubUpdateTempBookDetails(SerMode, byteObjS3G_Collection_TempReceiptBookMaster_DataTable, out strUsedleafsNumber);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

        #endregion

        #region Receipt Process
        public int FunPubCreateReceipt(SerializationMode SerMode, byte[] bytesObjReceiptProcessing_DataTable, out string strReceiptNumber)
        {
            try
            {
                S3GDALayer.Collection.ClnReceiptMgtServices.ClsPubReceiptProcessing objReceiptProcessing = new S3GDALayer.Collection.ClnReceiptMgtServices.ClsPubReceiptProcessing();
                return objReceiptProcessing.FunPubCreateReceipt(SerMode, bytesObjReceiptProcessing_DataTable, out strReceiptNumber);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int FunPubCreateTempReceipt(SerializationMode SerMode, byte[] bytesObjReceiptProcessing_DataTable, out string strTempReceiptNumber)
        {
            try
            {
                S3GDALayer.Collection.ClnReceiptMgtServices.ClsPubReceiptProcessing objReceiptProcessing = new S3GDALayer.Collection.ClnReceiptMgtServices.ClsPubReceiptProcessing();
                return objReceiptProcessing.FunPubCreateTempReceipt(SerMode, bytesObjReceiptProcessing_DataTable, out strTempReceiptNumber);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int FunPubCancelReceipt(SerializationMode SerMode, byte[] bytesObjReceiptProcessing_DataTable)
        {
            try
            {
                S3GDALayer.Collection.ClnReceiptMgtServices.ClsPubReceiptProcessing objReceiptProcessing = new S3GDALayer.Collection.ClnReceiptMgtServices.ClsPubReceiptProcessing();
                return objReceiptProcessing.FunPubCancelReceipt(SerMode, bytesObjReceiptProcessing_DataTable);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable FunPubCheckDocDate(string strProcName, int intCompanyId, DateTime dtDocDate, string strRecType)
        {
            try
            {
                S3GDALayer.Collection.ClnReceiptMgtServices.ClsPubReceiptProcessing objReceiptProcessing = new S3GDALayer.Collection.ClnReceiptMgtServices.ClsPubReceiptProcessing();
                return objReceiptProcessing.FunPubCheckDocDate(strProcName, intCompanyId, dtDocDate, strRecType);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int FunPubCancelTempReceipt(SerializationMode SerMode, byte[] bytesObjReceiptProcessing_DataTable)
        {
            try
            {
                S3GDALayer.Collection.ClnReceiptMgtServices.ClsPubReceiptProcessing objReceiptProcessing = new S3GDALayer.Collection.ClnReceiptMgtServices.ClsPubReceiptProcessing();
                return objReceiptProcessing.FunPubCancelTempReceipt(SerMode, bytesObjReceiptProcessing_DataTable);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int FunPubCreateUTPAReceipt(SerializationMode SerMode, byte[] bytesObjUTPAReceiptProcessing_DataTable, out string strUTPAReceiptNumber)
        {
            try
            {
                S3GDALayer.Collection.ClnReceiptMgtServices.ClsPubReceiptProcessing objReceiptProcessing = new S3GDALayer.Collection.ClnReceiptMgtServices.ClsPubReceiptProcessing();
                return objReceiptProcessing.FunPubCreateUTPAReceipt(SerMode, bytesObjUTPAReceiptProcessing_DataTable, out strUTPAReceiptNumber);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable FunPubLoadBookNo(string strProcName, int intOption, int intCompanyId, DateTime dtValueDate, DateTime dtDocDate, int intLobId, int intLocationId)
        {
            try
            {
                S3GDALayer.Collection.ClnReceiptMgtServices.ClsPubReceiptProcessing objReceiptProcessing = new S3GDALayer.Collection.ClnReceiptMgtServices.ClsPubReceiptProcessing();
                return objReceiptProcessing.FunPubLoadBookNo(strProcName, intOption, intCompanyId, dtValueDate, dtDocDate, intLobId, intLocationId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region IClnReceiptMgtServices Members




        #endregion

        #region [Receipt Authorization]
        /// <summary>
        /// Created By Tamilselvan.S
        /// Created Date 14/03/2011
        /// For Receipt Authorization
        /// </summary> 
        /// <param name="SerMode"></param>
        /// <param name="byteobjTable"></param>
        /// <param name="intReceiptID"></param>
        /// <returns></returns>
        public int FunPubReceiptAuthorize(SerializationMode SerMode, byte[] bytesObjReceiptProcessing_DataTable, out string strReceiptID)
        {
            try
            {
                S3GDALayer.Collection.ClnReceiptMgtServices.ClsPubReceiptProcessing objReceiptProcessing = new S3GDALayer.Collection.ClnReceiptMgtServices.ClsPubReceiptProcessing();
                return objReceiptProcessing.FunPubReceiptAuthorize(SerMode, bytesObjReceiptProcessing_DataTable, out strReceiptID);
            }
            catch (Exception ex)
            {
                ClsPubCommErrorLog.CustomErrorRoutine(ex);
                throw ex;
            }
        }

        /// <summary>
        /// Created By Tamilselvan.S
        /// Created Date 14/03/2011
        /// For Receipt Authorization
        /// </summary> 
        /// <param name="SerMode"></param>
        /// <param name="byteobjTable"></param>
        /// <param name="intReceiptID"></param>
        /// <returns></returns>
        public int FunPubReceiptAuthorize(int intCompany_ID, int intUser_ID, int intReceipt_ID, out string strReceiptID)
        {
            try
            {
                S3GDALayer.Collection.ClnReceiptMgtServices.ClsPubReceiptProcessing objReceiptProcessing = new S3GDALayer.Collection.ClnReceiptMgtServices.ClsPubReceiptProcessing();
                return objReceiptProcessing.FunPubReceiptAuthorize(intCompany_ID, intUser_ID, intReceipt_ID, out strReceiptID);
            }
            catch (Exception ex)
            {
                ClsPubCommErrorLog.CustomErrorRoutine(ex);
                throw ex;
            }
        }

        /// <summary>
        /// Created By Tamilselvan.S
        /// Created Date 18/05/2011
        /// For Revoke the Receipt Authorization
        /// </summary>
        /// <param name="intCompany_ID"></param>
        /// <param name="intUser_ID"></param>
        /// <param name="intReceipt_ID"></param>
        /// <param name="strReceiptID"></param>
        /// <returns></returns>
        public int FunPubRevokeReceiptAuthorize(int intCompany_ID, int intUser_ID, int intReceipt_ID, out string strReceiptID)
        {
            try
            {
                S3GDALayer.Collection.ClnReceiptMgtServices.ClsPubReceiptProcessing objReceiptProcessing = new S3GDALayer.Collection.ClnReceiptMgtServices.ClsPubReceiptProcessing();
                return objReceiptProcessing.FunPubRevokeReceiptAuthorize(intCompany_ID, intUser_ID, intReceipt_ID, out strReceiptID);
            }
            catch (Exception ex)
            {
                ClsPubCommErrorLog.CustomErrorRoutine(ex);
                throw ex;
            }
        }

        #endregion [Receipt Authorization]

        #region [UTPA Receipt Authorization and Revoke]

        /// <summary>
        /// Created By Tamilselvan.S
        /// Created Date 12/10/2011
        /// For UTPA Receipt Authorization
        /// </summary>
        /// <param name="intCompany_ID"></param>
        /// <param name="intUser_ID"></param>
        /// <param name="intUTPAReceipt_ID"></param>
        /// <param name="strUTPAReceiptID"></param>
        /// <returns></returns>
        public int FunPubUTPAReceiptAuthorize(int intCompany_ID, int intUser_ID, int intUTPAReceipt_ID, out string strUTPAReceiptID)
        {
            try
            {
                S3GDALayer.Collection.ClnReceiptMgtServices.ClsPubReceiptProcessing objReceiptProcessing = new S3GDALayer.Collection.ClnReceiptMgtServices.ClsPubReceiptProcessing();
                return objReceiptProcessing.FunPubUTPAReceiptAuthorize(intCompany_ID, intUser_ID, intUTPAReceipt_ID, out strUTPAReceiptID);
            }
            catch (Exception ex)
            {
                ClsPubCommErrorLog.CustomErrorRoutine(ex);
                throw ex;
            }
        }

        /// <summary>
        /// Created By Tamilselvan.S
        /// Created Date 12/10/2011
        /// For Revoke the UTPA Receipt Authorization
        /// </summary>
        /// <param name="intCompany_ID"></param>
        /// <param name="intUser_ID"></param>
        /// <param name="intUTPAReceipt_ID"></param>
        /// <param name="strUTPAReceiptID"></param>
        /// <returns></returns>
        public int FunPubRevokeUTPAReceiptAuthorize(int intCompany_ID, int intUser_ID, int intUTPAReceipt_ID, out string strUTPAReceiptID)
        {
            try
            {
                S3GDALayer.Collection.ClnReceiptMgtServices.ClsPubReceiptProcessing objReceiptProcessing = new S3GDALayer.Collection.ClnReceiptMgtServices.ClsPubReceiptProcessing();
                return objReceiptProcessing.FunPubRevokeUTPAReceiptAuthorize(intCompany_ID, intUser_ID, intUTPAReceipt_ID, out strUTPAReceiptID);
            }
            catch (Exception ex)
            {
                ClsPubCommErrorLog.CustomErrorRoutine(ex);
                throw ex;
            }
        }

        #endregion [UTPA Receipt Authorization and Revoke]

        #region PDC Bulk Replacement
        // ----PDC Entry(Created By- Palani Kumar.A)-------
        // ----Created On 11-Sep-2013------
        //Begin
        public int FunPubCreatePDCBulkModuleDetails(SerializationMode SerMode, byte[] bytesObjS3G_CLN_PDCModuleDataTable, out string strPDCNo, out string strchequeNo, out string strexistingdate)
        {
            try
            {
                S3GDALayer.Collection.ClnReceiptMgtServices.ClsPubPDCModule objClsPubPDCModule = new S3GDALayer.Collection.ClnReceiptMgtServices.ClsPubPDCModule();
                return objClsPubPDCModule.FunPubCreatePDCBulkModuleDetails(SerMode, bytesObjS3G_CLN_PDCModuleDataTable, out strPDCNo, out strchequeNo, out strexistingdate);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }

        }

        #endregion
        #region
        public int FunDCIncentiveMasterInsertInt(SerializationMode SerMode, byte[] bytesobjNCDClosure_DTB, string strConnectionName)
        {
            S3GDALayer.Collection.ClsPubDCIncentives objFunderTransaction = new S3GDALayer.Collection.ClsPubDCIncentives(strConnectionName);
            intResult = objFunderTransaction.FunDCIncentiveMasterInsertInt(SerMode, bytesobjNCDClosure_DTB);
            return intResult;
        }

        public int FunDCIncentiveMasterUpdateInt(SerializationMode SerMode, byte[] bytesobjNCDClosure_DTB, string strConnectionName)
        {
            S3GDALayer.Collection.ClsPubDCIncentives objFunderTransaction = new S3GDALayer.Collection.ClsPubDCIncentives(strConnectionName);
            intResult = objFunderTransaction.FunDCIncentiveMasterUpdateInt(SerMode, bytesobjNCDClosure_DTB);
            return intResult;
        }
        #endregion

        #region DC Incentive File upload
        public int FunPubCreateDCIncentiveUploadInt(SerializationMode SerMode, byte[] bytesChequeReturnExcelDAL_SERLAY, out string strChequeReturnNo, string strConnectionName)
        {
            try
            {
                S3GDALayer.Collection.ClsPubDCIncentives objClsPubDCINExcel = new S3GDALayer.Collection.ClsPubDCIncentives(strConnectionName);
                return objClsPubDCINExcel.FunPubCreateDCIncentiveUpload(SerMode, bytesChequeReturnExcelDAL_SERLAY, out strChequeReturnNo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        //Added by Suseela -For Cheque return from vault/Vault Approval/Vault option/Vault option acknowledgement/Handing over acknowledgement by DC/legal/Cheque Handing over by Cheque custodian/-code starts
        #region ChequeMovement
        public int FunPubChequeMovement(SerializationMode SerMode, byte[] byteChequeMovementFomatService, string strConnectionName)
        {
            S3GDALayer.Collection.ClnReceiptMgtServices.ClsPubECSSpoolFormats objFunderMaster = new S3GDALayer.Collection.ClnReceiptMgtServices.ClsPubECSSpoolFormats();
            intErrorCode = objFunderMaster.FunPubCreateChequeMovement(SerMode, byteChequeMovementFomatService, strConnectionName);
            return intErrorCode;
        }
        #endregion

        //Added by Suseela -For Cheque return-code ends

        //Added by Suseela -For Cheque Representation-code starts
        #region ChequeMovement
        public int FunPubChequeRepresentation(SerializationMode SerMode, byte[] byteChequeRepresentFomatService, string strConnectionName)
        {
            S3GDALayer.Collection.ClnReceiptMgtServices.ClsPubECSSpoolFormats objFunderMaster = new S3GDALayer.Collection.ClnReceiptMgtServices.ClsPubECSSpoolFormats();
            intErrorCode = objFunderMaster.FunPubCreateChequeRepresentation(SerMode, byteChequeRepresentFomatService, strConnectionName);
            return intErrorCode;
        }
        #endregion

        //Added by Suseela -For Cheque Representation-code ends

        #region "Marketing Slab Master"
        //Added on 08-Sep-2018 Starts here
        public int FunPubCreateOrModifySlabMaster(SerializationMode SerMode, byte[] bytesObjSlabMaster_DataTable, out string strSlabDocNumber)
        {
            try
            {
                S3GDALayer.Collection.ClnReceiptMgtServices.ClsPubMarketingSlabMaster objClsPubSlabMaster = new S3GDALayer.Collection.ClnReceiptMgtServices.ClsPubMarketingSlabMaster();
                return objClsPubSlabMaster.FunPubCreateOrModifySlabMaster(SerMode, bytesObjSlabMaster_DataTable, out strSlabDocNumber);
            }
            catch (Exception objException)
            {
                throw objException;
            }
        }
        //Added on 08-Sep-2018 Ends here
        #endregion


        #region "Marketing Incentive Master"
        //Added on 19-Sep-2018 Starts here
        public int FunPubCreateOrModifyIncentiveMaster(SerializationMode SerMode, byte[] bytesObjMaster_DataTable, out string strIncentiveDocNumber)
        {
            try
            {
                S3GDALayer.Collection.ClnReceiptMgtServices.ClsPubMarketingIncentiveMaster objClsPubIncentiveMaster = new S3GDALayer.Collection.ClnReceiptMgtServices.ClsPubMarketingIncentiveMaster();
                return objClsPubIncentiveMaster.FunPubCreateOrModifyIncentiveMaster(SerMode, bytesObjMaster_DataTable, out strIncentiveDocNumber);
            }
            catch (Exception objException)
            {
                throw objException;
            }
        }
        //Added on 19-Sep-2018 Ends here
        #endregion

        #region "Dealer Sales Commission Master"
        //Added on 29-Sep-2018 Starts here
        public int FunPubCreateOrModifyDSCMaster(SerializationMode SerMode, byte[] bytesObjMaster_DataTable, out string strDSCCode)
        {
            try
            {
                S3GDALayer.Collection.ClnReceiptMgtServices.ClsPubDSCMaster objClsPubDSCMaster = new S3GDALayer.Collection.ClnReceiptMgtServices.ClsPubDSCMaster();
                return objClsPubDSCMaster.FunPubCreateOrModifyDSCMaster(SerMode, bytesObjMaster_DataTable, out strDSCCode);
            }
            catch (Exception objException)
            {
                throw objException;
            }
        }
        //Added on 29-Sep-2018 Ends here
        #endregion

        #region "Dealer Sepcial Scheme Rate Master"
        public int FunPubCreateOrModifyDSScheme(SerializationMode SerMode, byte[] bytesObjMaster_DataTable, out string strDSCCode)
        {
            try
            {
                S3GDALayer.Collection.ClnReceiptMgtServices.ClsPubDSCMaster objClsPubDSCMaster = new S3GDALayer.Collection.ClnReceiptMgtServices.ClsPubDSCMaster();
                return objClsPubDSCMaster.FunPubCreateOrModifyDSScheme(SerMode, bytesObjMaster_DataTable, out strDSCCode);
            }
            catch (Exception objException)
            {
                throw objException;
            }
        }
        #endregion

        #region "File Upload"
        //Added on 11-Oct-2018 Starts here
        public int FunPubCreateFileUpload(SerializationMode SerMode, byte[] bytesObjPDCUpload_DataTable, out Int32 intUploadID)
        {
            try
            {
                S3GDALayer.Collection.ClnReceiptMgtServices.ClsPubPDCUpload objPDCUUpload = new S3GDALayer.Collection.ClnReceiptMgtServices.ClsPubPDCUpload();
                return objPDCUUpload.FunPubCreateFileUpload(SerMode, bytesObjPDCUpload_DataTable, out intUploadID);
            }
            catch (Exception objException)
            {
                throw objException;
            }
        }
        //Added on 11-Oct-2018 Ends here

        //Added on 12-Oct-2018 Starts here
        public int FunPubCreatePDCUploadDetails(SerializationMode SerMode, byte[] bytesObjPDCUpload_DataTable, out Int32 intUploadID)
        {
            try
            {
                S3GDALayer.Collection.ClnReceiptMgtServices.ClsPubPDCUpload objPDCUUpload = new S3GDALayer.Collection.ClnReceiptMgtServices.ClsPubPDCUpload();
                return objPDCUUpload.FunPubCreatePDCUploadDetails(SerMode, bytesObjPDCUpload_DataTable, out intUploadID);
            }
            catch (Exception objException)
            {
                throw objException;
            }
        }
        //Added on 12-Oct-2018 Ends here
        #endregion

        #region "PDC Maintenance"
        //Added on 15-Oct-2018 Starts here
        public int FunPubCreatePDCMaintenance(SerializationMode SerMode, byte[] bytesObjS3G_CLN_PDCMntncDataTable)
        {
            try
            {
                S3GDALayer.Collection.ClnReceiptMgtServices.ClsPubPDCModule objClsPubMaster = new S3GDALayer.Collection.ClnReceiptMgtServices.ClsPubPDCModule();
                return objClsPubMaster.FunPubCreatePDCMaintenance(SerMode, bytesObjS3G_CLN_PDCMntncDataTable);
            }
            catch (Exception objException)
            {
                throw objException;
            }
        }
        //Added on 15-Oct-2018 Ends here
        #endregion

        #region "PDC Picklist"

        //Added on 31-Oct-2018 Starts here
        public int FunPubCreatePDCPicklistDetails(SerializationMode SerMode, byte[] bytesObjPDCPicklist_DataTable, out Int32 intPicklistID, out string strPicklistNumber)
        {
            try
            {
                S3GDALayer.Collection.ClnReceiptMgtServices.ClsPubPDCPicklist objPDCUPicklist = new S3GDALayer.Collection.ClnReceiptMgtServices.ClsPubPDCPicklist();
                return objPDCUPicklist.FunPubCreatePDCPicklistDetails(SerMode, bytesObjPDCPicklist_DataTable, out intPicklistID, out strPicklistNumber);
            }
            catch (Exception objException)
            {
                throw objException;
            }
        }
        //Added on 31-Oct-2018 Ends here

        #endregion

        #region "Cheque Return Upload"

        //Added on 17-Nov-2018 Starts here
        public int FunPubCreateChequeReturnUploadDetails(SerializationMode SerMode, byte[] bytesObjUpload_DataTable, out Int32 intUploadID)
        {
            try
            {
                S3GDALayer.Collection.ClnReceiptMgtServices.ClsPubPDCUpload objUUpload = new S3GDALayer.Collection.ClnReceiptMgtServices.ClsPubPDCUpload();
                return objUUpload.FunPubCreateChequeReturnUploadDetails(SerMode, bytesObjUpload_DataTable, out intUploadID);
            }
            catch (Exception objException)
            {
                throw objException;
            }
        }
        //Added on 11-Nov-2018 Ends here

        #endregion

    }
}
