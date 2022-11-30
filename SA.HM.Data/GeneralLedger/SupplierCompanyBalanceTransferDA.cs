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
                    dbSmartAspects.AddInParameter(command, "@TransactionDate", DbType.DateTime, SCBalanceTransferInfo.TransactionDate);
                    dbSmartAspects.AddInParameter(command, "@Amount", DbType.Decimal, SCBalanceTransferInfo.Amount);
                    dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, SCBalanceTransferInfo.Remarks);
                    dbSmartAspects.AddInParameter(command, "@ApprovedStatus", DbType.String, SCBalanceTransferInfo.ApprovedStatus);
                    dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, SCBalanceTransferInfo.CreatedBy);
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public List<SupplierCompanyBalanceTransferBO> GetTransactionsBySearch(string transactionTypeSearch, int fromTransaction, int toTransaction, DateTime? dateFrom, DateTime? dateTo, int userInfoId)
        {
            List<SupplierCompanyBalanceTransferBO> transactionInfoList = new List<SupplierCompanyBalanceTransferBO>();

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

                    dbSmartAspects.AddInParameter(cmd, "@UserInfoId", DbType.Int32, userInfoId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "TransactionInfo");
                    DataTable Table = ds.Tables["TransactionInfo"];

                    transactionInfoList = Table.AsEnumerable().Select(r => new SupplierCompanyBalanceTransferBO
                    {
                        Id = r.Field<Int64>("Id"),
                        TransactionType = r.Field<string>("TransactionType"),
                        FromTransactionId = r.Field<Int32>("FromTransactionId"),
                        FromTransactionText = r.Field<string>("FromTransactionText"),
                        ToTransactionId = r.Field<Int32>("ToTransactionId"),
                        ToTransactionText = r.Field<string>("ToTransactionText"),
                        TransactionDate = r.Field<DateTime>("TransactionDate"),
                        Amount = r.Field<decimal>("Amount"),
                        Remarks = r.Field<string>("Remarks"),
                        ApprovedStatus = r.Field<string>("ApprovedStatus"),
                        CreatedBy = r.Field<Int32>("CreatedBy"),
                        CheckedBy = r.Field<Int32>("CheckedBy"),
                        ApprovedBy = r.Field<Int32>("ApprovedBy"),
                        IsCanEdit = r.Field<bool>("IsCanEdit"),
                        IsCanDelete = r.Field<bool>("IsCanDelete"),
                        IsCanChecked = r.Field<bool>("IsCanChecked"),
                        IsCanApproved = r.Field<bool>("IsCanApproved")

                    }).ToList();     
                }
            }

            return transactionInfoList;
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

                        if(reader["TransactionDate"] == DBNull.Value)
                        {
                            infoForEdit.TransactionDate = null;
                        }
                        else
                        {
                            infoForEdit.TransactionDate = Convert.ToDateTime(reader["TransactionDate"]);
                        }
                        
                        infoForEdit.Amount = Decimal.Parse(reader["Amount"].ToString());
                        infoForEdit.Remarks = reader["Remarks"].ToString();
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

        public bool CheckedTransfer(Int64 transferId, int checkedBy)
        {
            int status = 0;
            Int64 supplierIdCompanyId = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("CheckedSupplierCompanyBalanceTransfer_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@TransferId", DbType.String, transferId);
                            dbSmartAspects.AddInParameter(command, "@CheckedBy", DbType.String, checkedBy);

                            status = dbSmartAspects.ExecuteNonQuery(command, transaction);
                            supplierIdCompanyId = Convert.ToInt32(command.Parameters["@TransferId"].Value);
                        }

                        if (status > 0)
                        {
                            transaction.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        status = 0;
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }

            return status > 0 ? true : false;
        }

        public bool ApprovedTransfer(Int64 transferId, int approvedBy)
        {
            int status = 0;
            Int64 supplierIdCompanyId = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("ApprovedSupplierCompanyBalanceTransfer_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@TransferId", DbType.String, transferId);
                            dbSmartAspects.AddInParameter(command, "@ApprovedBy", DbType.String, approvedBy);

                            status = dbSmartAspects.ExecuteNonQuery(command, transaction);
                            supplierIdCompanyId = Convert.ToInt32(command.Parameters["@TransferId"].Value);
                        }

                        if (status > 0)
                        {
                            transaction.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        status = 0;
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }

            return status > 0 ? true : false;
        }
    }
}
