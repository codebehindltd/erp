using HotelManagement.Entity.GeneralLedger;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace HotelManagement.Data.GeneralLedger
{
    public class SupplierCompanyBalanceTransferDA : BaseService
    {
        public Boolean SaveSupplierCompanyBalanceTransfer(SupplierCompanyBalanceTransferBO SCBalanceTransferInfo)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveSupplierCompanyBalanceTransfer_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@Id", DbType.Int32, SCBalanceTransferInfo.Id);
                    dbSmartAspects.AddInParameter(command, "@TransactionType", DbType.String, SCBalanceTransferInfo.TransactionType);
                    dbSmartAspects.AddInParameter(command, "@FromTransactionId", DbType.Int32, SCBalanceTransferInfo.FromTransactionId);
                    dbSmartAspects.AddInParameter(command, "@ToTransactionId", DbType.Int32, SCBalanceTransferInfo.ToTransactionId);
                    dbSmartAspects.AddInParameter(command, "@Amount", DbType.Decimal, SCBalanceTransferInfo.Amount);
                    dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, SCBalanceTransferInfo.CreatedBy);
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public List<SupplierCompanyBalanceTransferBO> GetTransactionsBySearch(string transactionTypeSearch, int fromTransaction, int toTransaction, DateTime? dateFrom, DateTime? dateTo)
        {
            List<SupplierCompanyBalanceTransferBO> transactionInfo = new List<SupplierCompanyBalanceTransferBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetTransactionsBySearch_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@TransactionTypeSearch", DbType.String, transactionTypeSearch);

                    if (fromTransaction != 0)
                        dbSmartAspects.AddInParameter(cmd, "@FromTransaction", DbType.Int32, fromTransaction);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@FromTransaction", DbType.Int32, DBNull.Value);

                    if (toTransaction != 0)
                        dbSmartAspects.AddInParameter(cmd, "@ToTransaction", DbType.Int32, toTransaction);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ToTransaction", DbType.Int32, DBNull.Value);

                    if (dateFrom != null)
                        dbSmartAspects.AddInParameter(cmd, "@DateFrom", DbType.DateTime, dateFrom);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DateFrom", DbType.DateTime, DBNull.Value);

                    if (dateTo != null)
                        dbSmartAspects.AddInParameter(cmd, "@DateTo", DbType.DateTime, dateTo);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DateTo", DbType.DateTime, DBNull.Value);

                    IDataReader reader = dbSmartAspects.ExecuteReader(cmd);
                    while (reader.Read())
                    {
                        SupplierCompanyBalanceTransferBO bo = new SupplierCompanyBalanceTransferBO();
                        bo.Id = Int32.Parse(reader["Id"].ToString());
                        bo.TransactionType = reader["TransactionType"].ToString();
                        bo.FromTransactionId = Int32.Parse(reader["FromTransactionId"].ToString());
                        bo.FromTransactionText = reader["FromTransactionText"].ToString();
                        bo.ToTransactionId = Int32.Parse(reader["ToTransactionId"].ToString());
                        bo.ToTransactionText = reader["ToTransactionText"].ToString();
                        bo.Amount = Decimal.Parse(reader["Amount"].ToString());
                        transactionInfo.Add(bo);
                    }
                }
            }

            return transactionInfo;
        }

        public SupplierCompanyBalanceTransferBO GetCurrentSupplierCompanyInfoForEdit(int Id)
        {
            SupplierCompanyBalanceTransferBO infoForEdit = new SupplierCompanyBalanceTransferBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCurrentSupplierCompanyInfoForEdit_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@Id", DbType.Int32, Id);

                    IDataReader reader = dbSmartAspects.ExecuteReader(cmd);
                    if (reader.Read())
                    {
                        infoForEdit.Id = Int32.Parse(reader["Id"].ToString());
                        infoForEdit.TransactionType = reader["TransactionType"].ToString();
                        infoForEdit.FromTransactionId = Int32.Parse(reader["FromTransactionId"].ToString());
                        infoForEdit.ToTransactionId = Int32.Parse(reader["ToTransactionId"].ToString());
                        infoForEdit.Amount = Decimal.Parse(reader["Amount"].ToString());
                    }
                }
            }

            return infoForEdit;
        }

        public Boolean DeleteTransactionInfo(int Id)
        {
            Boolean status = false;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("DeleteTransactionInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@Id", DbType.Int32, Id);
                    status = dbSmartAspects.ExecuteNonQuery(cmd) > 0 ? true : false;
                }
            }

            return status;
        }
    }
}
