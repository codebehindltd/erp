using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.LCManagement;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.LCManagement
{
    public class OverHeadExpenseDA : BaseService
    {
        //public List<OverHeadNameBO> GetLCOverHeadNameInfo()
        //{
        //    List<OverHeadNameBO> guestHouseServiceList = new List<OverHeadNameBO>();
        //    using (DbConnection conn = dbSmartAspects.CreateConnection())
        //    {
        //        using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetLCOverHeadNameInfo_SP"))
        //        {
        //            using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
        //            {
        //                if (reader != null)
        //                {
        //                    while (reader.Read())
        //                    {
        //                        OverHeadNameBO guestHouseService = new OverHeadNameBO();

        //                        guestHouseService.OverHeadId = Convert.ToInt32(reader["OverHeadId"]);
        //                        guestHouseService.OverHeadName = reader["OverHeadName"].ToString();
        //                        guestHouseService.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
        //                        guestHouseService.ActiveStatus = reader["ActiveStatus"].ToString();

        //                        guestHouseServiceList.Add(guestHouseService);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return guestHouseServiceList;
        //}
        //public List<OverHeadNameBO> GetActiveGuestHouseServiceInfo()
        //{
        //    List<OverHeadNameBO> guestHouseServiceList = new List<OverHeadNameBO>();
        //    using (DbConnection conn = dbSmartAspects.CreateConnection())
        //    {
        //        using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetActiveLCOverHeadNameInfo_SP"))
        //        {
        //            using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
        //            {
        //                if (reader != null)
        //                {
        //                    while (reader.Read())
        //                    {
        //                        OverHeadNameBO guestHouseService = new OverHeadNameBO();

        //                        guestHouseService.OverHeadId = Convert.ToInt32(reader["OverHeadId"]);
        //                        guestHouseService.OverHeadName = reader["OverHeadName"].ToString();
        //                        guestHouseService.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
        //                        guestHouseService.ActiveStatus = reader["ActiveStatus"].ToString();
        //                        //guestHouseService.VatAmount = Convert.ToDecimal(reader["VatAmount"]);
        //                        //guestHouseService.ServiceCharge = Convert.ToDecimal(reader["ServiceCharge"]);

        //                        guestHouseServiceList.Add(guestHouseService);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return guestHouseServiceList;
        //}
        public Boolean SaveLCOverHeadExpenseInfo(OverHeadExpenseBO serviceBO, out int tmpserviceId)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveLCOverHeadExpenseInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@ExpenseId", DbType.Int32, serviceBO.ExpenseId);
                        dbSmartAspects.AddInParameter(command, "@LCId", DbType.Int32, serviceBO.LCId);
                        dbSmartAspects.AddInParameter(command, "@TransactionType", DbType.String, serviceBO.TransactionType);

                        if (!string.IsNullOrEmpty(serviceBO.PaymentMode))
                            dbSmartAspects.AddInParameter(command, "@PaymentMode", DbType.String, serviceBO.PaymentMode);
                        else
                            dbSmartAspects.AddInParameter(command, "@PaymentMode", DbType.String, DBNull.Value);
                                                
                        if (!string.IsNullOrEmpty(serviceBO.ChequeNumber))
                            dbSmartAspects.AddInParameter(command, "@ChequeNumber", DbType.String, serviceBO.ChequeNumber);
                        else
                            dbSmartAspects.AddInParameter(command, "@ChequeNumber", DbType.String, DBNull.Value);

                        if (serviceBO.BankId != 0)
                            dbSmartAspects.AddInParameter(command, "@BankId", DbType.Int32, serviceBO.BankId);
                        else
                            dbSmartAspects.AddInParameter(command, "@BankId", DbType.Int32, DBNull.Value);

                        if (serviceBO.CurrencyId != 0)
                            dbSmartAspects.AddInParameter(command, "@CurrencyId", DbType.Int32, serviceBO.CurrencyId);
                        else
                            dbSmartAspects.AddInParameter(command, "@CurrencyId", DbType.Int32, DBNull.Value);
                                                
                        if (serviceBO.ConversionRate != 0)
                            dbSmartAspects.AddInParameter(command, "@ConversionRate", DbType.Decimal, serviceBO.ConversionRate);
                        else
                            dbSmartAspects.AddInParameter(command, "@ConversionRate", DbType.Decimal, DBNull.Value);

                        dbSmartAspects.AddInParameter(command, "@TransactionAccountHeadId", DbType.Int32, serviceBO.TransactionAccountHeadId);
                        dbSmartAspects.AddInParameter(command, "@OverHeadId", DbType.Int32, serviceBO.OverHeadId);
                        dbSmartAspects.AddInParameter(command, "@CNFId", DbType.Int32, serviceBO.CNFId);
                        dbSmartAspects.AddInParameter(command, "@ExpenseDate", DbType.DateTime, serviceBO.ExpenseDate);
                        dbSmartAspects.AddInParameter(command, "@ExpenseAmount", DbType.Decimal, serviceBO.ExpenseAmount);
                        dbSmartAspects.AddInParameter(command, "@Description", DbType.String, serviceBO.Description);
                        dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, serviceBO.CreatedBy);
                        dbSmartAspects.AddOutParameter(command, "@OutId", DbType.Int32, sizeof(Int32));

                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                        tmpserviceId = Convert.ToInt32(command.Parameters["@OutId"].Value);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }
        public Boolean UpdateLCOverHeadExpenseInfo(OverHeadExpenseBO serviceBO)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateLCOverHeadExpenseInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@ExpenseId", DbType.Int32, serviceBO.ExpenseId);
                    dbSmartAspects.AddInParameter(command, "@LCId", DbType.Int32, serviceBO.LCId);
                    dbSmartAspects.AddInParameter(command, "@OverHeadId", DbType.Int32, serviceBO.OverHeadId);
                    dbSmartAspects.AddInParameter(command, "@ExpenseDate", DbType.DateTime, serviceBO.ExpenseDate);
                    dbSmartAspects.AddInParameter(command, "@ExpenseAmount", DbType.Decimal, serviceBO.ExpenseAmount);
                    dbSmartAspects.AddInParameter(command, "@Description", DbType.String, serviceBO.Description);
                    dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, serviceBO.LastModifiedBy);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public OverHeadExpenseBO GetLCOverHeadExpenseInfoById(int serviceId)
        {
            OverHeadExpenseBO guestHouseService = new OverHeadExpenseBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetLCOverHeadExpenseInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ExpenseId", DbType.Int32, serviceId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                guestHouseService.ExpenseId = Int32.Parse(reader["ExpenseId"].ToString());
                                guestHouseService.LCId = Int32.Parse(reader["LCId"].ToString());
                                guestHouseService.TransactionType = reader["TransactionType"].ToString();
                                guestHouseService.PaymentMode = reader["PaymentMode"].ToString();
                                guestHouseService.ChequeNumber = reader["ChequeNumber"].ToString();
                                guestHouseService.BankId = Convert.ToInt32(reader["BankId"]);
                                guestHouseService.CurrencyId = Convert.ToInt32(reader["CurrencyId"]);
                                guestHouseService.OverHeadId = Convert.ToInt32(reader["OverHeadId"]);
                                guestHouseService.CNFId = Convert.ToInt32(reader["CNFId"]);
                                guestHouseService.ExpenseDate = Convert.ToDateTime(reader["ExpenseDate"].ToString());
                                guestHouseService.ConversionRate = Convert.ToDecimal(reader["ConversionRate"].ToString());
                                guestHouseService.ExpenseAmount = Convert.ToDecimal(reader["ExpenseAmount"].ToString());
                                guestHouseService.Description = reader["Description"].ToString();
                                guestHouseService.TransactionAccountHeadId = Convert.ToInt32(reader["TransactionAccountHeadId"]);
                            }
                        }
                    }
                }
            }
            return guestHouseService;
        }
        //public List<OverHeadNameBO> GetGuestHouseServiceInfoBySearchCriteria(string overHeadName, bool activeStat)
        //{
        //    List<OverHeadNameBO> guestHouseServiceList = new List<OverHeadNameBO>();
        //    using (DbConnection conn = dbSmartAspects.CreateConnection())
        //    {
        //        using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetLCOverHeadNameInfoBySearchCriteria_SP"))
        //        {
        //            dbSmartAspects.AddInParameter(cmd, "@OverHeadName", DbType.String, overHeadName);
        //            dbSmartAspects.AddInParameter(cmd, "@ActiveStat", DbType.Boolean, activeStat);
        //            using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
        //            {
        //                if (reader != null)
        //                {
        //                    while (reader.Read())
        //                    {
        //                        OverHeadNameBO guestHouseService = new OverHeadNameBO();

        //                        guestHouseService.OverHeadId = Convert.ToInt32(reader["OverHeadId"]);
        //                        guestHouseService.OverHeadName = reader["OverHeadName"].ToString();
        //                        guestHouseService.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
        //                        guestHouseService.ActiveStatus = reader["ActiveStatus"].ToString();

        //                        guestHouseServiceList.Add(guestHouseService);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return guestHouseServiceList;
        //}       
        //public OverHeadNameBO GetGuestHouseServiceInfoDetailsById(int serviceId)
        //{
        //    OverHeadNameBO guestHouseService = new OverHeadNameBO();
        //    using (DbConnection conn = dbSmartAspects.CreateConnection())
        //    {
        //        using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetLCOverHeadNameInfoDetailsById_SP"))
        //        {
        //            dbSmartAspects.AddInParameter(cmd, "@ServiceId", DbType.Int32, serviceId);

        //            using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
        //            {
        //                if (reader != null)
        //                {
        //                    while (reader.Read())
        //                    {
        //                        guestHouseService.OverHeadId = Convert.ToInt32(reader["OverHeadId"]);
        //                        guestHouseService.OverHeadName = reader["OverHeadName"].ToString();
        //                        guestHouseService.Description = reader["Description"].ToString();
        //                        guestHouseService.NodeId = Int32.Parse(reader["NodeId"].ToString());
        //                        guestHouseService.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
        //                        guestHouseService.ActiveStatus = reader["ActiveStatus"].ToString();
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return guestHouseService;
        //}
        public List<OverHeadExpenseBO> GetOverHeadExpenseInfoBySearchCriteriaForPagination(string LCId, int recordPerPage, int pageIndex, out int totalRecords)
        {
            string Where = GenarateWhereCondition(LCId);
            List<OverHeadExpenseBO> paidServiceList = new List<OverHeadExpenseBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetOverHeadExpenseInfoBySearchCriteriaForPagination_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@SearchCriteria", DbType.String, Where);
                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                OverHeadExpenseBO paidServiceBO = new OverHeadExpenseBO();
                                paidServiceBO.ExpenseId = Convert.ToInt32(reader["ExpenseId"]);
                                paidServiceBO.LCId = Convert.ToInt32(reader["LCId"]);
                                paidServiceBO.LCNumber = reader["LCNumber"].ToString();
                                paidServiceBO.OverHeadId = Convert.ToInt32(reader["OverHeadId"]);
                                paidServiceBO.OverHeadName = reader["OverHeadName"].ToString();
                                paidServiceBO.ExpenseDate = Convert.ToDateTime(reader["ExpenseDate"].ToString());
                                paidServiceBO.ExpenseAmount = Convert.ToDecimal(reader["ExpenseAmount"]);
                                paidServiceBO.CurrencyName = reader["CurrencyName"].ToString();
                                paidServiceBO.ConvertionRate = Convert.ToDecimal(reader["ConvertionRate"]);
                                paidServiceBO.LocalCurrencyAmount = Convert.ToDecimal(reader["LocalCurrencyAmount"]);
                                paidServiceBO.Description = reader["Description"].ToString();
                                paidServiceBO.IsDayClosed = Convert.ToInt32(reader["IsDayClosed"]);
                                paidServiceList.Add(paidServiceBO);
                            }
                        }
                    }
                    totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                }
            }
            return paidServiceList;
        }
        public List<OverHeadExpenseBO> GetOverHeadExpenseInfoBySearchCriteriaForPagination(string transactionType, DateTime fromDate, DateTime toDate, int LCId, int overHeadId, int userId, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<OverHeadExpenseBO> paidServiceList = new List<OverHeadExpenseBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetLCOverHeadExpenseForPaging_SP"))
                {
                    if (transactionType != "0")
                    {
                        dbSmartAspects.AddInParameter(cmd, "@TransactionType", DbType.String, transactionType);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@TransactionType", DbType.String, DBNull.Value);
                    }

                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);
                    if (LCId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@LCId", DbType.Int32, LCId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@LCId", DbType.Int32, DBNull.Value);

                    if (overHeadId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@OverHeadId", DbType.Int32, overHeadId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@OverHeadId", DbType.Int32, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@UserInfoId", DbType.Int32, userId);
                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                OverHeadExpenseBO paidServiceBO = new OverHeadExpenseBO();
                                paidServiceBO.ExpenseId = Convert.ToInt32(reader["ExpenseId"]);
                                paidServiceBO.LCId = Convert.ToInt32(reader["LCId"]);
                                paidServiceBO.LCNumber = reader["LCNumber"].ToString();
                                paidServiceBO.OverHeadId = Convert.ToInt32(reader["OverHeadId"]);
                                paidServiceBO.OverHeadName = reader["OverHeadName"].ToString();
                                paidServiceBO.ExpenseDate = Convert.ToDateTime(reader["ExpenseDate"].ToString());
                                paidServiceBO.ExpenseAmount = Convert.ToDecimal(reader["ExpenseAmount"]);
                                paidServiceBO.CurrencyName = reader["CurrencyName"].ToString();
                                paidServiceBO.Description = reader["Description"].ToString();
                                paidServiceBO.TransactionNo = reader["TransactionNo"].ToString();
                                paidServiceBO.TransactionType = reader["TransactionType"].ToString();
                                paidServiceBO.IsDayClosed = Convert.ToInt32(reader["IsDayClosed"]);
                                paidServiceBO.IsCanEdit = Convert.ToBoolean(reader["IsCanEdit"]);
                                paidServiceBO.IsCanDelete = Convert.ToBoolean(reader["IsCanDelete"]);
                                paidServiceBO.IsCanChecked = Convert.ToBoolean(reader["IsCanChecked"]);
                                paidServiceBO.IsCanApproved = Convert.ToBoolean(reader["IsCanApproved"]);
                                paidServiceBO.Status = reader["Status"].ToString();
                                paidServiceList.Add(paidServiceBO);
                            }
                        }
                    }
                    totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                }
            }
            return paidServiceList;
        }

        private string GenarateWhereCondition(string LCId)
        {
            string Where = string.Empty;
            if (!string.IsNullOrEmpty(LCId.ToString()))
            {
                Where += " LCExpense.LCId = '" + LCId + "'";
            }

            if (!string.IsNullOrWhiteSpace(Where))
            {
                Where = " WHERE " + Where + " AND LCExpense.Status = 'Approved'";
            }
            return Where;
        }
        public List<OverHeadExpenseBO> GetOverHeadExpenseInfoForReport(DateTime fromDate, DateTime toDate, int overheadId, int LCId)
        {
            bool actStat;
            List<OverHeadExpenseBO> paidServiceList = new List<OverHeadExpenseBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {

                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetOverHeadExpenseInfoForReport_SP"))
                {
                    //GetPaidServiceInfoBySearchCriteriaForPagination_SP
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);
                    if (overheadId == 0)
                        dbSmartAspects.AddInParameter(cmd, "@OverHeadId", DbType.Int32, DBNull.Value);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@OverHeadId", DbType.Int32, overheadId);
                    if (LCId == 0)
                        dbSmartAspects.AddInParameter(cmd, "@LcId", DbType.Int32, DBNull.Value);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@LcId", DbType.Int32, LCId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                OverHeadExpenseBO paidServiceBO = new OverHeadExpenseBO();
                                paidServiceBO.ExpenseId = Convert.ToInt32(reader["ExpenseId"]);
                                paidServiceBO.LCId = Convert.ToInt32(reader["LCId"]);
                                paidServiceBO.LCNumber = reader["LCNumber"].ToString();
                                paidServiceBO.OverHeadId = Convert.ToInt32(reader["OverHeadId"]);
                                paidServiceBO.OverHeadName = reader["OverHeadName"].ToString();
                                paidServiceBO.ExpenseDate = Convert.ToDateTime(reader["ExpenseDate"].ToString());
                                paidServiceBO.ExpenseAmount = Convert.ToDecimal(reader["ExpenseAmount"]);
                                paidServiceBO.Description = reader["Description"].ToString();
                                paidServiceBO.IsDayClosed = Convert.ToInt32(reader["IsDayClosed"]);
                                paidServiceBO.TransactionType = reader["TransactionType"].ToString();
                                paidServiceBO.Status = reader["Status"].ToString();

                                paidServiceList.Add(paidServiceBO);
                            }
                        }
                    }
                }
            }
            return paidServiceList;
        }
        public bool ExpenseApproval(int Id, string approvedStatus, int checkedOrApprovedBy)
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
                        using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("OverHeadExpenseApproval_SP"))
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
        public List<OverHeadExpenseBO> GetLCOverheadExpenseInfoByExpenseId(int expenseId)
        {
            List<OverHeadExpenseBO> paymentInfo = new List<OverHeadExpenseBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetLCOverheadExpenseInfoByExpenseId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ExpenseId", DbType.Int32, expenseId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PaymentInfo");
                    DataTable Table = ds.Tables["PaymentInfo"];

                    paymentInfo = Table.AsEnumerable().Select(r => new OverHeadExpenseBO
                    {
                        ExpenseId = r.Field<Int32>("ExpenseId"),
                        OverHeadName = r.Field<string>("OverHeadName"),
                        TransactionNo = r.Field<string>("TransactionNo"),
                        ExpenseDate = r.Field<DateTime>("ExpenseDate"),
                        ExpenseDateDisplay = r.Field<string>("ExpenseDateDisplay"),
                        LCId = r.Field<int>("LCId"),
                        Description = r.Field<string>("Description"),
                        TransactionType = r.Field<string>("TransactionType"),
                        AccountingPostingHead = r.Field<string>("AccountingPostingHead"),
                        ChequeNumber = r.Field<string>("ChequeNumber"),
                        CurrencyType = r.Field<string>("CurrencyType"),
                        ConversionRate = r.Field<decimal>("ConversionRate"),
                        ChecqueDateDisplay = r.Field<string>("ChecqueDateDisplay"),
                        Status = r.Field<string>("Status"),
                        ExpenseAmount = r.Field<decimal>("ExpenseAmount"),
                        CreatedByName = r.Field<string>("CreatedByName"),
                    }).ToList();
                }
            }

            return paymentInfo;
        }
    }
}
