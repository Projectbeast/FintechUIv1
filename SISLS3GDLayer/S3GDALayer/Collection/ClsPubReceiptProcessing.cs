﻿using System;
using S3GDALayer.S3GAdminServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.IO;
using System.Data;
using S3GDALayer.Common;
using S3GBusEntity;
using System.Data.OracleClient;


namespace S3GDALayer.Collection
{
    namespace ClnReceiptMgtServices
    {
        public class ClsPubReceiptProcessing
        {
            S3GBusEntity.Collection.ClnReceiptMgtServices.S3G_CLN_ReceiptProcessingDataTable objReceiptProcessingTable;
            S3GBusEntity.Collection.ClnReceiptMgtServices.S3G_CLN_TempReceiptProcessingDataTable objTempReceiptProcessingTable;
            S3GBusEntity.Collection.ClnReceiptMgtServices.S3G_CLN_UTPAReceiptProcessingDataTable objUTPAReceiptProcessingTable;

            //Code added for getting common connection string  from config file
            Database db;
            public ClsPubReceiptProcessing()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }

            /// <summary>
            /// Generate the Receipts
            /// </summary>
            /// <param name="SerMode"></param>
            /// <param name="bytesObjReceiptProcessing_DataTable"></param>
            /// <param name="strReceiptNumber"></param>
            /// <returns></returns>
            public int FunPubCreateReceipt(SerializationMode SerMode, byte[] bytesObjReceiptProcessing_DataTable, out string strReceiptNumber)
            {
                int intErrorCode = 0;
                strReceiptNumber = "";
                try
                {
                    objReceiptProcessingTable = (S3GBusEntity.Collection.ClnReceiptMgtServices.S3G_CLN_ReceiptProcessingDataTable)
                                        ClsPubSerialize.DeSerialize(bytesObjReceiptProcessing_DataTable,
                                    SerMode, typeof(S3GBusEntity.Collection.ClnReceiptMgtServices.S3G_CLN_ReceiptProcessingDataTable));

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    foreach (S3GBusEntity.Collection.ClnReceiptMgtServices.S3G_CLN_ReceiptProcessingRow objReceiptRow in objReceiptProcessingTable)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_CLN_INSERTRECEIPT");
                        db.AddInParameter(command, "@COMPANYID", DbType.Int32, objReceiptRow.Company_ID);
                        db.AddInParameter(command, "@LOB_ID", DbType.Int32, objReceiptRow.LOB_ID);
                        db.AddInParameter(command, "@Location_ID", DbType.Int32, objReceiptRow.Branch_ID);
                        db.AddInParameter(command, "@ENTITY_TYPE", DbType.Int32, objReceiptRow.Entity_Type);
                        db.AddInParameter(command, "@ENTITY_ID", DbType.Int32, objReceiptRow.Entity_ID);
                        db.AddInParameter(command, "@PAYMENT_MODE", DbType.Int32, objReceiptRow.Payment_Mode);
                        db.AddInParameter(command, "@TOTAL_RECEIPT_AMOUNT", DbType.Decimal, objReceiptRow.Total_Receipt_Amount);
                        db.AddInParameter(command, "@RECEIPT_DATE", DbType.DateTime, objReceiptRow.Receipt_Date);
                        db.AddInParameter(command, "@VALUE_DATE", DbType.DateTime, objReceiptRow.Value_Date);
                        db.AddInParameter(command, "@FAC_CUSTOMER_ID", DbType.Int32, objReceiptRow.Customer_ID);
                        db.AddInParameter(command, "@Cheque_Mode", DbType.Int32, objReceiptRow.Cheque_Mode);
                        db.AddInParameter(command, "@Cheque_Type", DbType.Int32, objReceiptRow.Cheque_Type);

                        if (!objReceiptRow.IsPayment_Gateway_RefNull())
                            db.AddInParameter(command, "@PAYMENT_GATEWAY_REF", DbType.String, objReceiptRow.Payment_Gateway_Ref);
                        if (!objReceiptRow.IsTemp_Receipt_IDNull())
                            db.AddInParameter(command, "@TEMP_RECEIPT_ID", DbType.Int32, objReceiptRow.Temp_Receipt_ID);
                        if (!objReceiptRow.IsInstrument_NoNull())
                            db.AddInParameter(command, "@INSTRUMENT_NO", DbType.String, objReceiptRow.Instrument_No);
                        if (!objReceiptRow.IsInstrument_DateNull())
                            db.AddInParameter(command, "@INSTRUMENT_DATE", DbType.DateTime, objReceiptRow.Instrument_Date);
                        if (!objReceiptRow.IsLocationNull())
                            db.AddInParameter(command, "@LOCATION", DbType.String, objReceiptRow.Location);

                        if (!objReceiptRow.IsDrawee_Bank_NameNull())
                            db.AddInParameter(command, "@DRAWEE_BANK_Name", DbType.String, objReceiptRow.Drawee_Bank_Name);
                        if (!objReceiptRow.IsAccount_Link_KeyNull())
                            db.AddInParameter(command, "@ACCOUNT_LINK_KEY", DbType.Int32, objReceiptRow.Account_Link_Key);
                        if (!objReceiptRow.IsAuthorized_ByNull())
                            db.AddInParameter(command, "@AUTHORIZED_BY", DbType.Int32, objReceiptRow.Authorized_By);
                        if (!objReceiptRow.IsAuthorized_DateNull())
                            db.AddInParameter(command, "@AUTHORIZED_DATE", DbType.DateTime, objReceiptRow.Authorized_Date);
                        if (!objReceiptRow.IsAck_NoNull())
                            db.AddInParameter(command, "@ACK_NO", DbType.String, objReceiptRow.Ack_No);
                        db.AddInParameter(command, "@CREATED_BY", DbType.Int32, objReceiptRow.Created_By);



                        S3GDALDBType enumDBType = Common.ClsIniFileAccess.S3G_DBType;

                        if (enumDBType == S3GDALDBType.ORACLE)
                        {

                            OracleParameter param1;
                            if (objReceiptRow.XmlReceiptDetails != null)
                            {
                                param1 = new OracleParameter("@XMLACCOUNTDETAILS", OracleType.Clob,
                                    objReceiptRow.XmlReceiptDetails.Length, ParameterDirection.Input, true,
                                    0, 0, String.Empty, DataRowVersion.Default, objReceiptRow.XmlReceiptDetails);
                                command.Parameters.Add(param1);
                            }
                            OracleParameter param2;
                            if (objReceiptRow.XmlAddLessDetails != null)
                            {
                                param2 = new OracleParameter("@XMLADDLESSDETAILS", OracleType.Clob,
                                    objReceiptRow.XmlAddLessDetails.Length, ParameterDirection.Input, true,
                                    0, 0, String.Empty, DataRowVersion.Default, objReceiptRow.XmlAddLessDetails);
                                command.Parameters.Add(param2);
                            }

                            if (!objReceiptRow.IsInstallment_XMLNull())
                            {
                                OracleParameter param;
                                if (objReceiptRow.Installment_XML != null)
                                {
                                    param = new OracleParameter("@XMLINSTALLMENTDETAILS", OracleType.Clob,
                                        objReceiptRow.Installment_XML.Length, ParameterDirection.Input, true,
                                        0, 0, String.Empty, DataRowVersion.Default, objReceiptRow.Installment_XML);
                                    command.Parameters.Add(param);
                                }
                            }

                            if (!objReceiptRow.IsXmlDebitCreditNull())
                            {

                                OracleParameter param3;
                                if (objReceiptRow.XmlDebitCredit != null)
                                {
                                    param3 = new OracleParameter("@XmlDebitCredit", OracleType.Clob,
                                        objReceiptRow.XmlDebitCredit.Length, ParameterDirection.Input, true,
                                        0, 0, String.Empty, DataRowVersion.Default, objReceiptRow.XmlDebitCredit);
                                    command.Parameters.Add(param3);
                                }
                            }

                            if (!objReceiptRow.IsXmlDenominationNull())
                            {
                                OracleParameter param4;
                                if (objReceiptRow.XmlDenomination != null)
                                {
                                    param4 = new OracleParameter("@XmlDenomination", OracleType.Clob,
                                        objReceiptRow.XmlDenomination.Length, ParameterDirection.Input, true,
                                        0, 0, String.Empty, DataRowVersion.Default, objReceiptRow.XmlDenomination);
                                    command.Parameters.Add(param4);
                                }
                            }

                            if (!objReceiptRow.IsXmlCurrencyNull())
                            {
                                OracleParameter param5;
                                if (objReceiptRow.XmlCurrency != null)
                                {
                                    param5 = new OracleParameter("@XmlCurrency", OracleType.Clob,
                                        objReceiptRow.XmlCurrency.Length, ParameterDirection.Input, true,
                                        0, 0, String.Empty, DataRowVersion.Default, objReceiptRow.XmlCurrency);
                                    command.Parameters.Add(param5);
                                }
                            }

                            if (!objReceiptRow.IsXmlInvoiceNull())
                            {
                                OracleParameter param6;
                                if (objReceiptRow.XmlInvoice != null)
                                {
                                    param6 = new OracleParameter("@XmlInvoice", OracleType.Clob,
                                        objReceiptRow.XmlInvoice.Length, ParameterDirection.Input, true,
                                        0, 0, String.Empty, DataRowVersion.Default, objReceiptRow.XmlInvoice);
                                    command.Parameters.Add(param6);
                                }
                            }

                        }
                        else
                        {
                            db.AddInParameter(command, "@XMLACCOUNTDETAILS", DbType.String, objReceiptRow.XmlReceiptDetails);
                            db.AddInParameter(command, "@XMLADDLESSDETAILS", DbType.String, objReceiptRow.XmlAddLessDetails);
                            db.AddInParameter(command, "@XMLINSTALLMENTDETAILS", DbType.String, objReceiptRow.Installment_XML);
                            if (!objReceiptRow.IsXmlDebitCreditNull())
                            {
                                db.AddInParameter(command, "@XmlDebitCredit", DbType.String, objReceiptRow.XmlDebitCredit);
                            }

                            if (!objReceiptRow.IsXmlDenominationNull())
                            {
                                db.AddInParameter(command, "@XmlDenomination", DbType.String, objReceiptRow.XmlDenomination);
                            }

                            if (!objReceiptRow.IsXmlCurrencyNull())
                            {
                                db.AddInParameter(command, "@XmlCurrency", DbType.String, objReceiptRow.XmlCurrency);
                            }

                            if (!objReceiptRow.IsXmlInvoiceNull())
                            {
                                db.AddInParameter(command, "@XmlInvoice", DbType.String, objReceiptRow.XmlInvoice);
                            }
                        }

                        db.AddOutParameter(command, "@ERRORCODE", DbType.Int32, sizeof(Int64));
                        db.AddOutParameter(command, "@RECEIPT_NO", DbType.String, 200);
                        db.AddInParameter(command, "@RECEIPTID", DbType.Int32, objReceiptRow.Receipt_ID);
                        db.AddInParameter(command, "@Account_Based", DbType.Int32, objReceiptRow.Account_Based);
                        if (!objReceiptRow.IsOther_Bank_NameNull())
                            db.AddInParameter(command, "@Other_Bank_Name", DbType.String, objReceiptRow.Other_Bank_Name);

                        db.AddInParameter(command, "@DRAWEEBANKBRANCH_ID", DbType.Int32, objReceiptRow.DRAWEEBANKBRANCH_ID);
                        db.AddInParameter(command, "@Given_By", DbType.Int32, objReceiptRow.Given_By);

                        if (!objReceiptRow.IsFACTORED_AMOUNTNull())
                            db.AddInParameter(command, "@FACTORED_AMOUNT", DbType.Decimal, objReceiptRow.FACTORED_AMOUNT);
                        if (!objReceiptRow.IsUNFACTORED_AMOUNTNull())
                            db.AddInParameter(command, "@UNFACTORED_AMOUNT", DbType.Decimal, objReceiptRow.UNFACTORED_AMOUNT);
                        if (!objReceiptRow.IsGivenBy_NameNull())
                            db.AddInParameter(command, "@GivenBy_Name", DbType.String, objReceiptRow.GivenBy_Name);
                        if (!objReceiptRow.IsDrawee_Bank_AccNoNull())
                            db.AddInParameter(command, "@Drawee_Bank_AccNo", DbType.String, objReceiptRow.Drawee_Bank_AccNo);
                        if (!objReceiptRow.IsBank_IDNull())
                            db.AddInParameter(command, "@BANK_ID", DbType.Int32, objReceiptRow.Bank_ID);
                        if (!objReceiptRow.IsAccount_IDNull())
                            db.AddInParameter(command, "@ACCOUNT_ID", DbType.Int32, objReceiptRow.Account_ID);
                        if (!objReceiptRow.IsCard_Ref_NumberNull())
                            db.AddInParameter(command, "@CARD_REF_NUMBER", DbType.String, objReceiptRow.Card_Ref_Number);
                        using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                                //db.ExecuteNonQuery(command, trans);
                                db.FunPubExecuteNonQuery(command, ref trans);
                                if ((int)command.Parameters["@ERRORCODE"].Value > 0 && (int)command.Parameters["@ERRORCODE"].Value != 1)
                                {
                                    intErrorCode = (int)command.Parameters["@ERRORCODE"].Value;
                                    //7466 start 8000 added 
                                    if (intErrorCode == 15 || intErrorCode == 8000 || intErrorCode == 800)
                                    //7466 end 
                                    {
                                        strReceiptNumber = Convert.ToString(command.Parameters["@RECEIPT_NO"].Value);
                                        trans.Rollback();
                                    }
                                    else
                                    {
                                        strReceiptNumber = string.Empty;
                                        trans.Rollback();
                                    }
                                }
                                else if ((int)command.Parameters["@ERRORCODE"].Value < 0)
                                {
                                    intErrorCode = (int)command.Parameters["@ERRORCODE"].Value;
                                }
                                else if ((int)command.Parameters["@ERRORCODE"].Value == 1)
                                {
                                    strReceiptNumber = Convert.ToString(command.Parameters["@RECEIPT_NO"].Value);
                                    trans.Commit();
                                    intErrorCode = (int)command.Parameters["@ERRORCODE"].Value;
                                }
                                else
                                {
                                    strReceiptNumber = Convert.ToString(command.Parameters["@RECEIPT_NO"].Value);
                                    trans.Commit();
                                }
                            }
                            catch (Exception ex)
                            {
                                if (intErrorCode == 0)
                                    intErrorCode = 50;
                                trans.Rollback();
                                ClsPubCommErrorLogDal.CustomErrorRoutine(ex);

                            }
                            finally
                            {
                                conn.Close();
                            }
                        }




                    }
                }
                catch (Exception ex)
                {
                    intErrorCode = 50;
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                return intErrorCode;

            }




            public int FunPubCreateTempReceipt(SerializationMode SerMode, byte[] bytesObjReceiptProcessing_DataTable, out string strReceiptNumber)
            {
                int intErrorCode = 0;
                strReceiptNumber = "";
                try
                {
                    objTempReceiptProcessingTable = (S3GBusEntity.Collection.ClnReceiptMgtServices.S3G_CLN_TempReceiptProcessingDataTable)
                                        ClsPubSerialize.DeSerialize(bytesObjReceiptProcessing_DataTable,
                                    SerMode, typeof(S3GBusEntity.Collection.ClnReceiptMgtServices.S3G_CLN_TempReceiptProcessingDataTable));

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    foreach (S3GBusEntity.Collection.ClnReceiptMgtServices.S3G_CLN_TempReceiptProcessingRow objTempReceiptRow in objTempReceiptProcessingTable)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_CLN_INSERTTEMPRECEIPT");

                        db.AddInParameter(command, "@COMPANYID", DbType.Int32, objTempReceiptRow.Company_ID);
                        db.AddInParameter(command, "@LOB_ID", DbType.Int32, objTempReceiptRow.LOB_ID);
                        db.AddInParameter(command, "@Location_ID", DbType.Int32, objTempReceiptRow.Branch_ID);
                        db.AddInParameter(command, "@CUSTOMER_ID", DbType.Int32, objTempReceiptRow.Customer_ID);
                        db.AddInParameter(command, "@TEMP_RECEIPT_AMOUNT", DbType.Double, objTempReceiptRow.Temp_Receipt_Amount);
                        db.AddInParameter(command, "@TEMP_RECEIPT_DATE", DbType.DateTime, objTempReceiptRow.Temp_Reciept_Date);
                        db.AddInParameter(command, "@VALUE_DATE", DbType.DateTime, objTempReceiptRow.Value_Date);
                        if (!objTempReceiptRow.IsInstrument_NoNull())
                            db.AddInParameter(command, "@INSTRUMENT_NO", DbType.Int64, objTempReceiptRow.Instrument_No);

                        if (!objTempReceiptRow.IsInstrument_DateNull())
                            db.AddInParameter(command, "@INSTRUMENT_DATE", DbType.DateTime, objTempReceiptRow.Instrument_Date);

                        db.AddInParameter(command, "@PAYMENT_TYPE", DbType.Int32, objTempReceiptRow.Payment_Type);
                        if (!objTempReceiptRow.IsLocationNull())
                            db.AddInParameter(command, "@LOCATION", DbType.String, objTempReceiptRow.Location);
                        if (!objTempReceiptRow.IsDrawee_Bank_NameNull())
                            db.AddInParameter(command, "@DRAWEE_BANK_Name", DbType.String, objTempReceiptRow.Drawee_Bank_Name);
                        if (!objTempReceiptRow.IsPayment_Gateway_RefNull())
                            db.AddInParameter(command, "@PAYMENT_GATEWAY_REF", DbType.String, objTempReceiptRow.Payment_Gateway_Ref);
                        if (!objTempReceiptRow.IsOther_Bank_NameNull())
                            db.AddInParameter(command, "@Other_Bank_Name", DbType.String, objTempReceiptRow.Other_Bank_Name);
                        if (!objTempReceiptRow.IsACK_NoNull())
                            db.AddInParameter(command, "@ACK_NO", DbType.String, objTempReceiptRow.ACK_No);

                        db.AddInParameter(command, "@TEMP_RECEIPTBOOK_NO", DbType.Int32, objTempReceiptRow.Temp_ReceiptBook_No);
                        db.AddInParameter(command, "@BOOKPAGENO", DbType.Int32, objTempReceiptRow.Temp_Receipt_BookPage);

                        db.AddInParameter(command, "@CREATED_BY", DbType.Int32, objTempReceiptRow.Created_By);
                        db.AddInParameter(command, "@TEMPRECEIPTID", DbType.Int32, objTempReceiptRow.Temp_Receipt_ID);
                        db.AddOutParameter(command, "@ERRORCODE", DbType.Int32, sizeof(Int64));
                        db.AddOutParameter(command, "@TEMP_RECEIPTNO", DbType.String, 200);
                        using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                                //db.ExecuteNonQuery(command, trans);
                                db.FunPubExecuteNonQuery(command, ref trans);
                                if ((int)command.Parameters["@ERRORCODE"].Value != 0)
                                {
                                    intErrorCode = (int)command.Parameters["@ERRORCODE"].Value;

                                }
                                else
                                {
                                    strReceiptNumber = Convert.ToString(command.Parameters["@TEMP_RECEIPTNO"].Value);
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
                }
                catch (Exception ex)
                {
                    intErrorCode = 50;
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                return intErrorCode;

            }

            public int FunPubCancelReceipt(SerializationMode SerMode, byte[] bytesObjReceiptProcessing_DataTable)
            {
                int intErrorCode = 0;
                try
                {
                    objReceiptProcessingTable = (S3GBusEntity.Collection.ClnReceiptMgtServices.S3G_CLN_ReceiptProcessingDataTable)
                                        ClsPubSerialize.DeSerialize(bytesObjReceiptProcessing_DataTable,
                                    SerMode, typeof(S3GBusEntity.Collection.ClnReceiptMgtServices.S3G_CLN_ReceiptProcessingDataTable));

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    foreach (S3GBusEntity.Collection.ClnReceiptMgtServices.S3G_CLN_ReceiptProcessingRow objReceiptRow in objReceiptProcessingTable)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_CLN_CANCELRECEIPT");
                        db.AddInParameter(command, "@ReceiptId", DbType.Int32, objReceiptRow.Receipt_ID);
                        db.AddInParameter(command, "@UserId", DbType.Int32, objReceiptRow.Modified_By);
                        db.AddOutParameter(command, "@ERRORCODE", DbType.Int32, sizeof(Int64));
                        using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                                //db.ExecuteNonQuery(command, trans);
                                db.FunPubExecuteNonQuery(command, ref trans);
                                if ((int)command.Parameters["@ERRORCODE"].Value != 0)
                                {
                                    intErrorCode = (int)command.Parameters["@ERRORCODE"].Value;

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
                                ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                                trans.Rollback();
                            }
                            finally
                            {
                                conn.Close();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    intErrorCode = 50;
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                return intErrorCode;
            }

            public int FunPubCancelTempReceipt(SerializationMode SerMode, byte[] bytesObjReceiptProcessing_DataTable)
            {
                int intErrorCode = 0;
                try
                {
                    objTempReceiptProcessingTable = (S3GBusEntity.Collection.ClnReceiptMgtServices.S3G_CLN_TempReceiptProcessingDataTable)
                                        ClsPubSerialize.DeSerialize(bytesObjReceiptProcessing_DataTable,
                                    SerMode, typeof(S3GBusEntity.Collection.ClnReceiptMgtServices.S3G_CLN_TempReceiptProcessingDataTable));

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    foreach (S3GBusEntity.Collection.ClnReceiptMgtServices.S3G_CLN_TempReceiptProcessingRow objReceiptRow in objTempReceiptProcessingTable)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_CLN_CANCELTEMPRECEIPT");
                        db.AddInParameter(command, "@TempReceiptId", DbType.Int32, objReceiptRow.Temp_Receipt_ID);
                        db.AddInParameter(command, "@UserId", DbType.Int32, objReceiptRow.Modified_By);
                        db.AddInParameter(command, "@BookNo", DbType.Int32, objReceiptRow.Temp_ReceiptBook_No);
                        db.AddInParameter(command, "@PageNo", DbType.Int32, objReceiptRow.Temp_Receipt_BookPage);
                        db.AddInParameter(command, "@ReasonId", DbType.Int32, objReceiptRow.ReasonId);
                        db.AddOutParameter(command, "@ERRORCODE", DbType.Int32, sizeof(Int64));
                        using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                                //db.ExecuteNonQuery(command, trans);
                                db.FunPubExecuteNonQuery(command, ref trans);
                                if ((int)command.Parameters["@ERRORCODE"].Value != 0)
                                {
                                    intErrorCode = (int)command.Parameters["@ERRORCODE"].Value;

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
                                ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                                trans.Rollback();
                            }
                            finally
                            {
                                conn.Close();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    intErrorCode = 50;
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                return intErrorCode;
            }

            public int FunPubCreateUTPAReceipt(SerializationMode SerMode, byte[] bytesObjUTPAReceiptProcessing_DataTable, out string strUTPAReceiptNumber)
            {
                int intErrorCode = 0;
                strUTPAReceiptNumber = "";
                try
                {
                    objUTPAReceiptProcessingTable = (S3GBusEntity.Collection.ClnReceiptMgtServices.S3G_CLN_UTPAReceiptProcessingDataTable)
                                        ClsPubSerialize.DeSerialize(bytesObjUTPAReceiptProcessing_DataTable,
                                    SerMode, typeof(S3GBusEntity.Collection.ClnReceiptMgtServices.S3G_CLN_UTPAReceiptProcessingDataTable));

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    foreach (S3GBusEntity.Collection.ClnReceiptMgtServices.S3G_CLN_UTPAReceiptProcessingRow objUTPAReceiptRow in objUTPAReceiptProcessingTable)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_CLN_INSERTUTPARECEIPT");
                        db.AddInParameter(command, "@COMPANYID", DbType.Int32, objUTPAReceiptRow.Company_ID);
                        db.AddInParameter(command, "@LOB_ID", DbType.Int32, objUTPAReceiptRow.LOB_ID);
                        db.AddInParameter(command, "@Location_ID", DbType.Int32, objUTPAReceiptRow.Branch_ID);
                        db.AddInParameter(command, "@CUSTOMER_ID", DbType.Int32, objUTPAReceiptRow.Customer_ID);
                        db.AddInParameter(command, "@PAYMENT_TYPE", DbType.Int32, objUTPAReceiptRow.Payment_Type);
                        db.AddInParameter(command, "@TOTAL_RECEIPT_AMOUNT", DbType.Decimal, objUTPAReceiptRow.UTPA_Receipt_Amount);
                        db.AddInParameter(command, "@RECEIPT_DATE", DbType.DateTime, objUTPAReceiptRow.UTPA_Reciept_Date);
                        db.AddInParameter(command, "@VALUE_DATE", DbType.DateTime, objUTPAReceiptRow.Value_Date);

                        if (!objUTPAReceiptRow.IsPayment_Gateway_RefNull())
                            db.AddInParameter(command, "@PAYMENT_GATEWAY_REF", DbType.String, objUTPAReceiptRow.Payment_Gateway_Ref);
                        db.AddInParameter(command, "@Collector_ID", DbType.Int32, objUTPAReceiptRow.Collector_ID);
                        if (!objUTPAReceiptRow.IsInstrument_NoNull())
                            db.AddInParameter(command, "@INSTRUMENT_NO", DbType.Int32, objUTPAReceiptRow.Instrument_No);
                        if (!objUTPAReceiptRow.IsInstrument_DateNull())
                            db.AddInParameter(command, "@INSTRUMENT_DATE", DbType.DateTime, objUTPAReceiptRow.Instrument_Date);
                        if (!objUTPAReceiptRow.IsLocationNull())
                            db.AddInParameter(command, "@LOCATION", DbType.String, objUTPAReceiptRow.Location);

                        if (!objUTPAReceiptRow.IsDrawee_Bank_NameNull())
                            db.AddInParameter(command, "@DRAWEE_BANK_Name", DbType.String, objUTPAReceiptRow.Drawee_Bank_Name);
                        if (!objUTPAReceiptRow.IsAuthorized_ByNull())
                            db.AddInParameter(command, "@AUTHORIZED_BY", DbType.Int32, objUTPAReceiptRow.Authorized_By);
                        if (!objUTPAReceiptRow.IsAuthorized_DateNull())
                            db.AddInParameter(command, "@AUTHORIZED_DATE", DbType.DateTime, objUTPAReceiptRow.Authorized_Date);
                        if (!objUTPAReceiptRow.IsAck_NoNull())
                            db.AddInParameter(command, "@ACK_NO", DbType.String, objUTPAReceiptRow.Ack_No);
                        db.AddInParameter(command, "@CREATED_BY", DbType.Int32, objUTPAReceiptRow.Created_By);
                        db.AddInParameter(command, "@XMLACCOUNTDETAILS", DbType.String, objUTPAReceiptRow.XmlAccountDetails);
                        db.AddOutParameter(command, "@ERRORCODE", DbType.Int32, sizeof(Int64));
                        db.AddOutParameter(command, "@RECEIPT_NO", DbType.String, 200);
                        db.AddInParameter(command, "@RECEIPTID", DbType.Int32, objUTPAReceiptRow.Receipt_ID);
                        if (!objUTPAReceiptRow.IsOther_Bank_NameNull())
                            db.AddInParameter(command, "@Other_Bank_Name", DbType.String, objUTPAReceiptRow.Other_Bank_Name);
                        using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                                //db.ExecuteNonQuery(command, trans);
                                db.FunPubExecuteNonQuery(command, ref trans);
                                if ((int)command.Parameters["@ERRORCODE"].Value > 0)
                                {
                                    intErrorCode = (int)command.Parameters["@ERRORCODE"].Value;
                                    if (intErrorCode == 15)
                                    {
                                        strUTPAReceiptNumber = Convert.ToString(command.Parameters["@RECEIPT_NO"].Value);
                                    }
                                }
                                else
                                {
                                    strUTPAReceiptNumber = Convert.ToString(command.Parameters["@RECEIPT_NO"].Value);
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
                }
                catch (Exception ex)
                {
                    intErrorCode = 50;
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                return intErrorCode;

            }

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
                int intErrorCode = 0;
                strReceiptID = "";
                try
                {
                    objReceiptProcessingTable = (S3GBusEntity.Collection.ClnReceiptMgtServices.S3G_CLN_ReceiptProcessingDataTable)
                    ClsPubSerialize.DeSerialize(bytesObjReceiptProcessing_DataTable, SerMode, typeof(S3GBusEntity.Collection.ClnReceiptMgtServices.S3G_CLN_ReceiptProcessingDataTable));

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_CLN_ReceiptAuthorize");

                    db.AddInParameter(command, "@COMPANYID", DbType.Int32, objReceiptProcessingTable[0].Company_ID);
                    db.AddInParameter(command, "@Receipt_ID", DbType.Int32, objReceiptProcessingTable[0].Receipt_ID);
                    db.AddInParameter(command, "@User_Id", DbType.Int32, objReceiptProcessingTable[0].Created_By);
                    db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));

                    using (DbConnection conn = db.CreateConnection())
                    {
                        conn.Open();
                        DbTransaction trans = conn.BeginTransaction();
                        try
                        {
                            //db.ExecuteNonQuery(command, trans);
                            db.FunPubExecuteNonQuery(command, ref trans);
                            if ((int)command.Parameters["@ERRORCODE"].Value > 0)
                            {
                                intErrorCode = (int)command.Parameters["@ERRORCODE"].Value;
                                throw new Exception("Error in Receipt Authorization " + intErrorCode.ToString());
                            }
                            else
                            {
                                strReceiptID = Convert.ToString(command.Parameters["@Receipt_ID"].Value);
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
                    intErrorCode = 50;
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                return intErrorCode;
            }

            /// <summary>
            /// Created By Tamilselvan.S
            /// Created Date 14/03/2011
            /// For Receipt Authorization           
            /// </summary>
            /// <param name="SerMode"></param>
            /// <param name="intCompany_ID"></param>
            /// <param name="intUser_ID"></param>
            /// <param name="intReceipt_ID"></param>
            /// <param name="strReceiptID"></param>
            /// <returns></returns>
            public int FunPubReceiptAuthorize(int intCompany_ID, int intUser_ID, int intReceipt_ID, out string strReceiptID)
            {
                int intErrorCode = 0;
                strReceiptID = "";
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_CLN_ReceiptAuthorize");

                    db.AddInParameter(command, "@Company_ID", DbType.Int32, intCompany_ID);
                    db.AddInParameter(command, "@Receipt_ID", DbType.Int32, intReceipt_ID);
                    db.AddInParameter(command, "@User_Id", DbType.Int32, intUser_ID);
                    db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));

                    using (DbConnection conn = db.CreateConnection())
                    {
                        conn.Open();
                        //DbTransaction trans = conn.BeginTransaction();
                        try
                        {
                            //db.ExecuteNonQuery(command);
                            db.FunPubExecuteNonQuery(command);
                            if ((int)command.Parameters["@ERRORCODE"].Value > 0)
                            {
                                intErrorCode = (int)command.Parameters["@ERRORCODE"].Value;
                                throw new Exception("Error in Receipt Authorization " + intErrorCode.ToString());
                            }
                            else
                            {
                                strReceiptID = Convert.ToString(command.Parameters["@Receipt_ID"].Value);
                                // trans.Commit();
                            }
                        }
                        catch (Exception ex)
                        {
                            if (intErrorCode == 0)
                                intErrorCode = 50;
                            ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                            // trans.Rollback();
                        }
                        finally
                        {
                            conn.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    intErrorCode = 50;
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                return intErrorCode;
            }

            /// <summary>
            /// Created By Tamilselvan.S
            /// Created Date 14/03/2011
            /// For Revoke the Receipt Authorization           
            /// </summary>
            /// <param name="intCompany_ID"></param>
            /// <param name="intUser_ID"></param>
            /// <param name="intReceipt_ID"></param>
            /// <param name="strReceiptID"></param>
            /// <returns></returns>
            public int FunPubRevokeReceiptAuthorize(int intCompany_ID, int intUser_ID, int intReceipt_ID, out string strReceiptID)
            {
                int intErrorCode = 0;
                strReceiptID = "";
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_CLN_RovokeReceiptAuthorize");

                    db.AddInParameter(command, "@Company_ID", DbType.Int32, intCompany_ID);
                    db.AddInParameter(command, "@Receipt_ID", DbType.Int32, intReceipt_ID);
                    db.AddInParameter(command, "@User_Id", DbType.Int32, intUser_ID);
                    db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));

                    using (DbConnection conn = db.CreateConnection())
                    {
                        conn.Open();
                        try
                        {
                            //db.ExecuteNonQuery(command);
                            db.FunPubExecuteNonQuery(command);
                            if ((int)command.Parameters["@ERRORCODE"].Value > 0)
                            {
                                intErrorCode = (int)command.Parameters["@ERRORCODE"].Value;
                                throw new Exception("Error in Receipt Authorization " + intErrorCode.ToString());
                            }
                            else
                            {
                                strReceiptID = Convert.ToString(command.Parameters["@Receipt_ID"].Value);
                            }
                        }
                        catch (Exception ex)
                        {
                            if (intErrorCode == 0)
                                intErrorCode = 50;
                            ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                        }
                        finally
                        {
                            conn.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    intErrorCode = 50;
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                return intErrorCode;
            }

            #endregion [Receipt Authorization]

            #region [UTPA Receipt Authorization And Revoke]

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
                int intErrorCode = 0;
                strUTPAReceiptID = "";
                try
                {
                    DbCommand command = db.GetStoredProcCommand("S3G_CLN_UTPAReceiptAuthorize");
                    db.AddInParameter(command, "@Company_ID", DbType.Int32, intCompany_ID);
                    db.AddInParameter(command, "@UTPA_Receipt_ID", DbType.Int32, intUTPAReceipt_ID);
                    db.AddInParameter(command, "@User_Id", DbType.Int32, intUser_ID);
                    db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));

                    using (DbConnection conn = db.CreateConnection())
                    {
                        conn.Open();
                        //DbTransaction trans = conn.BeginTransaction();
                        try
                        {
                            //db.ExecuteNonQuery(command);
                            db.FunPubExecuteNonQuery(command);
                            if ((int)command.Parameters["@ERRORCODE"].Value > 0)
                            {
                                intErrorCode = (int)command.Parameters["@ERRORCODE"].Value;
                                throw new Exception("Error in Receipt Authorization " + intErrorCode.ToString());
                            }
                            else
                            {
                                strUTPAReceiptID = Convert.ToString(command.Parameters["@UTPA_Receipt_ID"].Value);
                            }
                        }
                        catch (Exception ex)
                        {
                            if (intErrorCode == 0)
                                intErrorCode = 50;
                            ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                        }
                        finally
                        {
                            conn.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    intErrorCode = 50;
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                return intErrorCode;
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
                int intErrorCode = 0;
                strUTPAReceiptID = "";
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_CLN_RevokeUTPAReceiptAuthorize");

                    db.AddInParameter(command, "@Company_ID", DbType.Int32, intCompany_ID);
                    db.AddInParameter(command, "@UTPA_Receipt_ID", DbType.Int32, intUTPAReceipt_ID);
                    db.AddInParameter(command, "@User_Id", DbType.Int32, intUser_ID);
                    db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));

                    using (DbConnection conn = db.CreateConnection())
                    {
                        conn.Open();
                        try
                        {
                            //db.ExecuteNonQuery(command);
                            db.FunPubExecuteNonQuery(command);
                            if ((int)command.Parameters["@ERRORCODE"].Value > 0)
                            {
                                intErrorCode = (int)command.Parameters["@ERRORCODE"].Value;
                                throw new Exception("Error in Receipt Authorization " + intErrorCode.ToString());
                            }
                            else
                            {
                                strUTPAReceiptID = Convert.ToString(command.Parameters["@UTPA_Receipt_ID"].Value);
                            }
                        }
                        catch (Exception ex)
                        {
                            if (intErrorCode == 0)
                                intErrorCode = 50;
                            ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                        }
                        finally
                        {
                            conn.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    intErrorCode = 50;
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                return intErrorCode;
            }

            #endregion [UTPA Receipt Authorization And Revoke]

            public DataTable FunPubCheckDocDate(string strProcName, int intCompanyId, DateTime dtDocDate, string strRecType)
            {

                DataSet ds = new DataSet();
                try
                {
                    DbCommand command = db.GetStoredProcCommand(strProcName);
                    db.AddInParameter(command, "@CompanyId", DbType.Int32, intCompanyId);
                    db.AddInParameter(command, "@RecDocDate", DbType.DateTime, dtDocDate);
                    if (strRecType != "")
                    {
                        db.AddInParameter(command, "@RecType", DbType.String, strRecType);
                    }
                    //db.LoadDataSet(command, ds, "ListTable");
                    db.FunPubLoadDataSet(command, ds, "ListTable");

                }
                catch (Exception ex)
                {
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                return ds.Tables["ListTable"];

            }

            public DataTable FunPubLoadBookNo(string strProcName, int intOption, int intCompanyId, DateTime dtValueDate, DateTime dtDocDate, int intLobId, int intLocationId)
            {

                DataSet ds = new DataSet();
                try
                {
                    DbCommand command = db.GetStoredProcCommand(strProcName);
                    db.AddInParameter(command, "@Option", DbType.Int32, intOption);
                    db.AddInParameter(command, "@CompanyId", DbType.Int32, intCompanyId);
                    db.AddInParameter(command, "@LobId", DbType.Int32, intLobId);
                    db.AddInParameter(command, "@LocationId", DbType.Int32, intLocationId);
                    db.AddInParameter(command, "@DocDate", DbType.DateTime, dtDocDate);
                    db.AddInParameter(command, "@ValueDate", DbType.DateTime, dtValueDate);
                    //db.LoadDataSet(command, ds, "BookNoTable");
                    db.FunPubLoadDataSet(command, ds, "BookNoTable");

                }
                catch (Exception ex)
                {
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                return ds.Tables["BookNoTable"];

            }
            public int FunPubInsertPopupInvoice(SerializationMode SerMode, byte[] bytesObjReceiptProcessing_DataTable)
            {
                int intErrorCode = 0;
                try
                {
                    objReceiptProcessingTable = (S3GBusEntity.Collection.ClnReceiptMgtServices.S3G_CLN_ReceiptProcessingDataTable)
                                        ClsPubSerialize.DeSerialize(bytesObjReceiptProcessing_DataTable,
                                    SerMode, typeof(S3GBusEntity.Collection.ClnReceiptMgtServices.S3G_CLN_ReceiptProcessingDataTable));

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    foreach (S3GBusEntity.Collection.ClnReceiptMgtServices.S3G_CLN_ReceiptProcessingRow objReceiptRow in objReceiptProcessingTable)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_CLN_CANCELTEMPRECEIPT");
                        if (!objReceiptRow.IsXmlInvoiceNull())
                        {
                            db.AddInParameter(command, "@RECEIPTID", DbType.Int32, objReceiptRow.Receipt_ID);
                            db.AddInParameter(command, "@ReasonId", DbType.String, objReceiptRow.XmlInvoice);
                        }
                        db.AddOutParameter(command, "@ERRORCODE", DbType.Int32, sizeof(Int64));
                        using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                                //db.ExecuteNonQuery(command, trans);
                                db.FunPubExecuteNonQuery(command, ref trans);
                                if ((int)command.Parameters["@ERRORCODE"].Value != 0)
                                {
                                    intErrorCode = (int)command.Parameters["@ERRORCODE"].Value;

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
                                ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                                trans.Rollback();
                            }
                            finally
                            {
                                conn.Close();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    intErrorCode = 50;
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                return intErrorCode;
            }
        }
    }
}
