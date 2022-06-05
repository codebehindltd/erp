using HotelManagement.Entity.LCManagement;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace HotelManagement.Data.LCManagement
{
    public class CNFTransactionDA : BaseService
    {
        public bool SaveOrUpdateCNFTransaction(CNFTransactionBO CNFTransactionBO, out int OutId)
        {
            Boolean status = false;
            OutId = 0;

            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveOrUpdateCNFTransaction_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@Id", DbType.Int64, CNFTransactionBO.Id);

                        if (CNFTransactionBO.CNFId != 0)
                            dbSmartAspects.AddInParameter(command, "@CNFId", DbType.Int32, CNFTransactionBO.CNFId);
                        else
                            dbSmartAspects.AddInParameter(command, "@CNFId", DbType.Int32, DBNull.Value);

                        if (!string.IsNullOrEmpty(CNFTransactionBO.TransactionType))
                            dbSmartAspects.AddInParameter(command, "@TransactionType", DbType.String, CNFTransactionBO.TransactionType);
                        else
                            dbSmartAspects.AddInParameter(command, "@TransactionType", DbType.String, DBNull.Value);

                        if (!string.IsNullOrEmpty(CNFTransactionBO.PaymentMode))
                            dbSmartAspects.AddInParameter(command, "@PaymentMode", DbType.String, CNFTransactionBO.PaymentMode);
                        else
                            dbSmartAspects.AddInParameter(command, "@PaymentMode", DbType.String, DBNull.Value);

                        dbSmartAspects.AddInParameter(command, "@PaymentDate", DbType.DateTime, CNFTransactionBO.PaymentDate);

                        if (!string.IsNullOrEmpty(CNFTransactionBO.ChequeNumber))
                            dbSmartAspects.AddInParameter(command, "@ChequeNumber", DbType.String, CNFTransactionBO.ChequeNumber);
                        else
                            dbSmartAspects.AddInParameter(command, "@ChequeNumber", DbType.String, DBNull.Value);

                        if (CNFTransactionBO.BankId != 0)
                            dbSmartAspects.AddInParameter(command, "@BankId", DbType.Int32, CNFTransactionBO.BankId);
                        else
                            dbSmartAspects.AddInParameter(command, "@BankId", DbType.Int32, DBNull.Value);

                        if (CNFTransactionBO.CurrencyId != 0)
                            dbSmartAspects.AddInParameter(command, "@CurrencyId", DbType.Int32, CNFTransactionBO.CurrencyId);
                        else
                            dbSmartAspects.AddInParameter(command, "@CurrencyId", DbType.Int32, DBNull.Value);

                        if (CNFTransactionBO.PaymentAmount != 0)
                            dbSmartAspects.AddInParameter(command, "@PaymentAmount", DbType.Decimal, CNFTransactionBO.PaymentAmount);
                        else
                            dbSmartAspects.AddInParameter(command, "@PaymentAmount", DbType.Decimal, DBNull.Value);

                        if (CNFTransactionBO.ConversionRate != 0)
                            dbSmartAspects.AddInParameter(command, "@ConversionRate", DbType.Decimal, CNFTransactionBO.ConversionRate);
                        else
                            dbSmartAspects.AddInParameter(command, "@ConversionRate", DbType.Decimal, DBNull.Value);

                        if (CNFTransactionBO.Remarks != "")
                            dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, CNFTransactionBO.Remarks);
                        else
                            dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, DBNull.Value);
                        if (CNFTransactionBO.Id == 0)
                            dbSmartAspects.AddInParameter(command, "@UserId", DbType.Int32, CNFTransactionBO.CreatedBy);
                        else
                            dbSmartAspects.AddInParameter(command, "@UserId", DbType.Int32, CNFTransactionBO.LastModifiedBy);

                        dbSmartAspects.AddOutParameter(command, "@OutId", DbType.Int32, sizeof(Int32));

                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                        OutId = Convert.ToInt32(command.Parameters["@OutId"].Value);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }
        public List<CNFTransactionBO> GetTransactionInformationPagination(DateTime fromDate, DateTime toDate, int UserInfoId, string transactionType, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<CNFTransactionBO> TransactionInformationList = new List<CNFTransactionBO>();
            try
            {

                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetTransactionInformationForPaging_SP"))
                    {

                        dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                        dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);
                        dbSmartAspects.AddInParameter(cmd, "@UserInfoId", DbType.Int32, UserInfoId);
                        if (transactionType == "0")
                        {
                            dbSmartAspects.AddInParameter(cmd, "@TransactionType", DbType.String, DBNull.Value);
                        }
                        else
                            dbSmartAspects.AddInParameter(cmd, "@TransactionType", DbType.String, transactionType);


                        dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                        dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                        dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    CNFTransactionBO TransactionInformation = new CNFTransactionBO();

                                    TransactionInformation.Id = Convert.ToInt32(reader["Id"]);
                                    TransactionInformation.TransactionNo = (reader["TransactionNo"].ToString());
                                    TransactionInformation.PaymentDate = Convert.ToDateTime(reader["PaymentDate"]);
                                    TransactionInformation.TransactionType = reader["TransactionType"].ToString();
                                    TransactionInformation.PaymentAmount = Convert.ToDecimal(reader["PaymentAmount"]);
                                    TransactionInformation.IsCanEdit = Convert.ToBoolean(reader["IsCanEdit"]);
                                    TransactionInformation.IsCanDelete = Convert.ToBoolean(reader["IsCanDelete"]);
                                    TransactionInformation.IsCanChecked = Convert.ToBoolean(reader["IsCanChecked"]);
                                    TransactionInformation.IsCanApproved = Convert.ToBoolean(reader["IsCanApproved"]);

                                    TransactionInformationList.Add(TransactionInformation);
                                }
                            }
                        }
                        totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return TransactionInformationList;
        }
        public CNFTransactionBO GetCNFTransactionById(int id)
        {
            CNFTransactionBO CNFTransaction = new CNFTransactionBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCNFTransactionInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@Id", DbType.Int32, id);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                CNFTransaction.Id = Int32.Parse(reader["Id"].ToString());
                                CNFTransaction.CNFId = Int32.Parse(reader["CNFId"].ToString());
                                CNFTransaction.TransactionType = reader["TransactionType"].ToString();
                                CNFTransaction.PaymentMode = reader["PaymentMode"].ToString();
                                CNFTransaction.PaymentDate = Convert.ToDateTime(reader["PaymentDate"]);
                                CNFTransaction.ChequeNumber = reader["ChequeNumber"].ToString();
                                CNFTransaction.BankId = Convert.ToInt32(reader["BankId"]);
                                CNFTransaction.CurrencyId = Convert.ToInt32(reader["CurrencyId"]);
                                CNFTransaction.PaymentAmount = Convert.ToDecimal(reader["PaymentAmount"]);
                                CNFTransaction.ConversionRate = Convert.ToDecimal(reader["ConversionRate"]);
                                CNFTransaction.Remarks = reader["Remarks"].ToString();
                            }
                        }
                    }
                }
            }
            return CNFTransaction;
        }
        public bool TransactionApproval(int Id, string approvedStatus, int checkedOrApprovedBy)
        {
            bool retVal = false;
            int status = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("TransactionApproval_SP"))
                        {
                            dbSmartAspects.AddInParameter(commandMaster, "@Id", DbType.Int32, Id);
                            dbSmartAspects.AddInParameter(commandMaster, "@ApprovedStatus", DbType.String, approvedStatus);
                            dbSmartAspects.AddInParameter(commandMaster, "@CheckedOrApprovedBy", DbType.Int32, checkedOrApprovedBy);

                            status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);
                        }                        
                        if (status > 0)
                        {
                            transction.Commit();
                            retVal = true;
                        }
                        else
                        {
                            transction.Rollback();
                            retVal = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        transction.Rollback();
                        retVal = false;
                        throw ex;
                    }
                }
            }
            return retVal;
        }

    }
}
