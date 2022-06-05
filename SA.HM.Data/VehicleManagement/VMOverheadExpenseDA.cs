using HotelManagement.Entity.VehicleManagement;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace HotelManagement.Data.VehicleManagement
{
    public class VMOverheadExpenseDA : BaseService
    {
        public Boolean SaveVMOverHeadExpenseInfo(VMOverheadExpenseBO serviceBO, out int tmpserviceId)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveVMOverHeadExpenseInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@Id", DbType.Int32, serviceBO.Id);
                        dbSmartAspects.AddInParameter(command, "@VehicleId", DbType.Int32, serviceBO.VehicleId);
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
                        dbSmartAspects.AddInParameter(command, "@OverHeadId", DbType.Int32, serviceBO.OverheadId);
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
        public List<VMOverheadExpenseBO> GetOverHeadExpenseInfoBySearchCriteriaForPagination(DateTime fromDate, DateTime toDate, int VehicleId, int overHeadId, int userId, int recordPerPage, int pageIndex, out int totalRecords)
        {
            bool actStat;

            List<VMOverheadExpenseBO> paidServiceList = new List<VMOverheadExpenseBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {

                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetVMOverHeadExpenseForPaging_SP"))
                {
                    //GetPaidServiceInfoBySearchCriteriaForPagination_SP
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);
                    if (VehicleId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@VehicleId", DbType.Int32, VehicleId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@VehicleId", DbType.Int32, DBNull.Value);

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
                                VMOverheadExpenseBO paidServiceBO = new VMOverheadExpenseBO();
                                paidServiceBO.Id = Convert.ToInt32(reader["Id"]);
                                paidServiceBO.VehicleId = Convert.ToInt32(reader["VehicleId"]);
                                paidServiceBO.NumberPlate = reader["NumberPlate"].ToString();
                                paidServiceBO.OverheadId = Convert.ToInt32(reader["OverHeadId"]);
                                paidServiceBO.OverheadName = reader["OverheadName"].ToString();
                                paidServiceBO.ExpenseDate = Convert.ToDateTime(reader["ExpenseDate"].ToString());
                                paidServiceBO.ExpenseAmount = Convert.ToDecimal(reader["ExpenseAmount"]);
                                paidServiceBO.Description = reader["Description"].ToString();
                                paidServiceBO.TransactionNo = reader["TransactionNo"].ToString();

                                paidServiceBO.IsCanEdit = Convert.ToBoolean(reader["IsCanEdit"]);
                                paidServiceBO.IsCanDelete = Convert.ToBoolean(reader["IsCanDelete"]);
                                paidServiceBO.IsCanChecked = Convert.ToBoolean(reader["IsCanChecked"]);
                                paidServiceBO.IsCanApproved = Convert.ToBoolean(reader["IsCanApproved"]);
                                paidServiceList.Add(paidServiceBO);
                            }
                        }
                    }
                    totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                }
            }
            return paidServiceList;
        }
        public VMOverheadExpenseBO GetLCOverHeadExpenseInfoById(int serviceId)
        {
            VMOverheadExpenseBO guestHouseService = new VMOverheadExpenseBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetVMOverHeadExpenseInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@Id", DbType.Int32, serviceId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                guestHouseService.Id = Int32.Parse(reader["Id"].ToString());
                                guestHouseService.VehicleId = Int32.Parse(reader["VehicleId"].ToString());
                                guestHouseService.TransactionType = reader["TransactionType"].ToString();
                                guestHouseService.PaymentMode = reader["PaymentMode"].ToString();
                                guestHouseService.ChequeNumber = reader["ChequeNumber"].ToString();
                                guestHouseService.BankId = Convert.ToInt32(reader["BankId"]);
                                guestHouseService.CurrencyId = Convert.ToInt32(reader["CurrencyId"]);
                                guestHouseService.OverheadId = Convert.ToInt32(reader["OverHeadId"]);
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
                        using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("VMOverHeadExpenseApproval_SP"))
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
