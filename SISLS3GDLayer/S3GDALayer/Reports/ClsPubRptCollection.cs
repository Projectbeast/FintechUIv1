﻿using System;
using S3GDALayer.S3GAdminServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using S3GBusEntity.Reports;
using S3GBusEntity;
using Microsoft.Practices.EnterpriseLibrary;
using Microsoft.Practices.Unity;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using S3GDALayer.Constants;

namespace S3GDALayer.Reports
{
    public class ClsPubRptCollection:ClsPubDalReportBase
    {
        public List<ClsPubCollectionDetails> FunPubGetCollectionPreciseDetails(ClsPubCollectionHeader HeaderDetails)
        {
            List<ClsPubCollectionDetails> objCollection = new List<ClsPubCollectionDetails>();
            try
            {
                DbCommand command = db.GetStoredProcCommand(SPNames.GetCollectionReportDetails);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, HeaderDetails.CompanyId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOBID, DbType.Int32, HeaderDetails.LOBID);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMUSERID, DbType.Int32, HeaderDetails.UserId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMFROMMONTH, DbType.DateTime, HeaderDetails.FromMonth);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMTOMONTH, DbType.DateTime, HeaderDetails.ToMonth);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMDENOMINATION, DbType.Decimal, HeaderDetails.Denomination);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMISDETAILED, DbType.String, HeaderDetails.IsDetailed);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMDEBTCOLLECTORCODE, DbType.String, HeaderDetails.DebtCollector);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMISCHEQUE, DbType.Int32, HeaderDetails.ChequeReturn);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPROGRAMID, DbType.Int32, HeaderDetails.ProgramId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATIONID1, DbType.Int32, HeaderDetails.LocationId1);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATIONID2, DbType.Int32, HeaderDetails.LocationId2);
                db.AddInParameter(command, ClsPubDALConstants.INPUTDRAWEEBANK, DbType.String, HeaderDetails.DraweeBank);

                db.AddInParameter(command, ClsPubDALConstants.ReceiptMode, DbType.Int32, HeaderDetails.ReceiptMode);
                db.AddInParameter(command, ClsPubDALConstants.SMECustomer, DbType.Int32, HeaderDetails.SMECustomer);
                db.AddInParameter(command, ClsPubDALConstants.ComplianceId, DbType.Int32, HeaderDetails.ComplianceId);
                db.AddInParameter(command, ClsPubDALConstants.NTDType, DbType.Int32, HeaderDetails.NTDType);
                db.AddInParameter(command, ClsPubDALConstants.CashRangeFrom, DbType.Decimal, HeaderDetails.CashRangeFrom);
                db.AddInParameter(command, ClsPubDALConstants.CashRangeTo, DbType.Decimal, HeaderDetails.CashRangeTo);
                db.AddInParameter(command, ClsPubDALConstants.LEGALTYPE, DbType.Int32, HeaderDetails.LegalType);
                IDataReader reader = db.FunPubExecuteReader(command);
                while (reader.Read())
                {
                    ClsPubCollectionDetails Collection = new ClsPubCollectionDetails();
                    Collection.LineOfBusiness = StringParse(reader[ClsPubDALConstants.LOBNAME]);
                    Collection.Location = StringParse(reader[ClsPubDALConstants.LOCATION]);
                    //Collection.Branch = StringParse(reader[ClsPubDALConstants.BRANCHNAME]);
                    Collection.DebtCollectorCode = StringParse(reader[ClsPubDALConstants.DEBTCOLLECTORCODE]);
                    Collection.TotalCollectionAmount = DecimalParse(reader[ClsPubDALConstants.TOTALCOLLECTIONAMOUNT]);
                    Collection.CurrentCollection = DecimalParse(reader[ClsPubDALConstants.CURRENTCOLLECTION]);
                    Collection.ArrearsCollection = DecimalParse(reader[ClsPubDALConstants.ARREARCOLLECTION]);
                    Collection.Insurance = DecimalParse(reader[ClsPubDALConstants.INSURANCEAMOUNT]);
                    Collection.Interest = DecimalParse(reader[ClsPubDALConstants.INTEREST]);
                    Collection.OverDueInterest = DecimalParse(reader[ClsPubDALConstants.OVERDUEINTEREST]);
                    Collection.MemoCharges = DecimalParse(reader[ClsPubDALConstants.MEMOCHARGES]);
                    Collection.Others = DecimalParse(reader[ClsPubDALConstants.OTHERS]);
                    Collection.GPSSuffix = StringParse(reader[ClsPubDALConstants.Gpssuffix]);
                     
                    objCollection.Add(Collection);
                }
            }

            catch (Exception e)
            {
                 ClsPubCommErrorLogDal.CustomErrorRoutine(e);
                throw e;
            }

            return objCollection;
        }

        public List<ClsPubCollectionDetails> FunPubGetCollectionDetails(ClsPubCollectionHeader HeaderDetails)
        {
            List<ClsPubCollectionDetails> objCollectionDetails = new List<ClsPubCollectionDetails>();
            try
            {
                DbCommand command = db.GetStoredProcCommand(SPNames.GetCollectionReportDetails);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, HeaderDetails.CompanyId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOBID, DbType.Int32, HeaderDetails.LOBID);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMUSERID, DbType.Int32, HeaderDetails.UserId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMFROMMONTH, DbType.DateTime, HeaderDetails.FromMonth);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMTOMONTH, DbType.DateTime, HeaderDetails.ToMonth);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMDENOMINATION, DbType.Decimal, HeaderDetails.Denomination);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMISDETAILED, DbType.String, HeaderDetails.IsDetailed);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMDEBTCOLLECTORCODE, DbType.String, HeaderDetails.DebtCollector);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMISCHEQUE, DbType.Int32, HeaderDetails.ChequeReturn);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPROGRAMID, DbType.Int32, HeaderDetails.ProgramId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATIONID1, DbType.Int32, HeaderDetails.LocationId1);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATIONID2, DbType.Int32, HeaderDetails.LocationId2);
                db.AddInParameter(command, ClsPubDALConstants.INPUTDRAWEEBANK, DbType.String, HeaderDetails.DraweeBank);

                db.AddInParameter(command, ClsPubDALConstants.ReceiptMode, DbType.Int32, HeaderDetails.ReceiptMode);
                db.AddInParameter(command, ClsPubDALConstants.SMECustomer, DbType.Int32, HeaderDetails.SMECustomer);
                db.AddInParameter(command, ClsPubDALConstants.ComplianceId, DbType.Int32, HeaderDetails.ComplianceId);
                db.AddInParameter(command, ClsPubDALConstants.NTDType, DbType.Int32, HeaderDetails.NTDType);
                db.AddInParameter(command, ClsPubDALConstants.CashRangeFrom, DbType.Decimal, HeaderDetails.CashRangeFrom);
                db.AddInParameter(command, ClsPubDALConstants.CashRangeTo, DbType.Decimal, HeaderDetails.CashRangeTo);
                db.AddInParameter(command, ClsPubDALConstants.LEGALTYPE, DbType.Int32, HeaderDetails.LegalType);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMISOVERDUE, DbType.Int32, HeaderDetails.IsOverdue);

                IDataReader reader = db.FunPubExecuteReader(command);
                while (reader.Read())
                {
                    ClsPubCollectionDetails CollectionDetails = new ClsPubCollectionDetails();
                    CollectionDetails.Location = StringParse(reader[ClsPubDALConstants.LOCATION]); //CollectionDetails.Region = StringParse(reader[ClsPubDALConstants.REGIONNAME]);
                    CollectionDetails.LineOfBusiness = StringParse(reader[ClsPubDALConstants.LOBNAME]);
                    //CollectionDetails.Branch = StringParse(reader[ClsPubDALConstants.BRANCHNAME]);                   
                    CollectionDetails.CurrentCollection = DecimalParse(reader[ClsPubDALConstants.CURRENTCOLLECTION]);
                    CollectionDetails.ArrearsCollection = DecimalParse(reader[ClsPubDALConstants.ARREARCOLLECTION]);
                    CollectionDetails.Insurance = DecimalParse(reader[ClsPubDALConstants.INSURANCEAMOUNT]);
                    CollectionDetails.Interest = DecimalParse(reader[ClsPubDALConstants.INTEREST]);
                    CollectionDetails.OverDueInterest = DecimalParse(reader[ClsPubDALConstants.OVERDUEINTEREST]);
                    CollectionDetails.MemoCharges = DecimalParse(reader[ClsPubDALConstants.MEMOCHARGES]);
                    CollectionDetails.Others = DecimalParse(reader[ClsPubDALConstants.OTHERS]);
                    CollectionDetails.PrimeAccountNumber = StringParse(reader[ClsPubDALConstants.PRIMEACCOUNTNUMBER]);
                    CollectionDetails.SubAccountNumber = StringParse(reader[ClsPubDALConstants.SUBACCOUNTNUMBER]);
                    CollectionDetails.DebtCollectorCode = StringParse(reader[ClsPubDALConstants.DEBTCOLLECTORCODE]);
                    CollectionDetails.ReceiptAmount = DecimalParse(reader[ClsPubDALConstants.RECEIPTAMOUNT]);
                    CollectionDetails.ReceiptDate = StringParse(reader[ClsPubDALConstants.RECEIPTDATE]);
                    CollectionDetails.ReceiptNumber = StringParse(reader[ClsPubDALConstants.RECEIPTNUMBER]);
                    CollectionDetails.CustomerName = StringParse(reader[ClsPubDALConstants.CUSTOMERNAME]);
                    CollectionDetails.Mobile = StringParse(reader[ClsPubDALConstants.MOBILENUMBER]);
                    CollectionDetails.GPSSuffix = StringParse(reader[ClsPubDALConstants.Gpssuffix]);
                    CollectionDetails.Instrument_Number = StringParse(reader[ClsPubDALConstants.Instrument_Number]);
                    CollectionDetails.Bank = StringParse(reader[ClsPubDALConstants.BANK]);
                    CollectionDetails.CHK_RETDATE = StringParse(reader[ClsPubDALConstants.CHK_RETDATE]);
                    CollectionDetails.CRC = DecimalParse(reader[ClsPubDALConstants.CRC]);
                    CollectionDetails.LIP = DecimalParse(reader[ClsPubDALConstants.LIP]);
                  
                    objCollectionDetails.Add(CollectionDetails);
                }
            }

            catch (Exception e)
            {
                 ClsPubCommErrorLogDal.CustomErrorRoutine(e);
                throw e;
            }

            return objCollectionDetails;
        }
    }
}
