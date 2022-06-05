using HotelManagement.Entity.Payroll;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace HotelManagement.Data.Payroll
{
    public class PayrollEmployeeBalanceTransferDA : BaseService
    {
        public Boolean SaveUpdateEmployeeBalanceTransferInfo(PayrollEmployeeBalanceTransferBO payrollEmployeeBalanceTransferBO, out long OutId)
        {
            Boolean status = false;

            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SavePayrollEmployeeBalanceTransferInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@Id", DbType.Int64, payrollEmployeeBalanceTransferBO.Id);

                        if (payrollEmployeeBalanceTransferBO.TransferFrom > 0)
                            dbSmartAspects.AddInParameter(command, "@TransferFrom", DbType.String, payrollEmployeeBalanceTransferBO.TransferFrom);
                        else
                            dbSmartAspects.AddInParameter(command, "@TransferFrom", DbType.String, DBNull.Value);

                        if (payrollEmployeeBalanceTransferBO.TransferTo > 0)
                            dbSmartAspects.AddInParameter(command, "@TransferTo", DbType.String, payrollEmployeeBalanceTransferBO.TransferTo);
                        else
                            dbSmartAspects.AddInParameter(command, "@TransferTo", DbType.String, DBNull.Value);

                        if (payrollEmployeeBalanceTransferBO.TransferAmount > 0)
                            dbSmartAspects.AddInParameter(command, "@TransferAmount", DbType.String, payrollEmployeeBalanceTransferBO.TransferAmount);
                        else
                            dbSmartAspects.AddInParameter(command, "@TransferAmount", DbType.String, DBNull.Value);

                        if (payrollEmployeeBalanceTransferBO.Description != "")
                            dbSmartAspects.AddInParameter(command, "@Description", DbType.String, payrollEmployeeBalanceTransferBO.Description);
                        else
                            dbSmartAspects.AddInParameter(command, "@Description", DbType.String, DBNull.Value);

                        
                        if (payrollEmployeeBalanceTransferBO.Id == 0)
                            dbSmartAspects.AddInParameter(command, "@UserId", DbType.String, payrollEmployeeBalanceTransferBO.CreatedBy);
                        else
                            dbSmartAspects.AddInParameter(command, "@UserId", DbType.String, payrollEmployeeBalanceTransferBO.LastModifiedBy);

                        dbSmartAspects.AddOutParameter(command, "@OutId", DbType.Int64, sizeof(Int64));

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
        public List<PayrollEmployeeBalanceTransferBO> GetEmployeeBalanceTransferBySearchCriteriaForPagination( int TransferFrom, int TransferTo, int userId, int recordPerPage, int pageIndex, out int totalRecords)
        {
            bool actStat;

            List<PayrollEmployeeBalanceTransferBO> BalanceTransfeList = new List<PayrollEmployeeBalanceTransferBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {

                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPayrollEmpBalanceTransferForPaging_SP"))
                {
                    //GetPaidServiceInfoBySearchCriteriaForPagination_SP
                    
                    if (TransferFrom != 0)
                        dbSmartAspects.AddInParameter(cmd, "@TransferFrom", DbType.Int32, TransferFrom);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@TransferFrom", DbType.Int32, DBNull.Value);

                    if (TransferTo != 0)
                        dbSmartAspects.AddInParameter(cmd, "@TransferTo", DbType.Int32, TransferTo);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@TransferTo", DbType.Int32, DBNull.Value);

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
                                PayrollEmployeeBalanceTransferBO BalanceTransferBO = new PayrollEmployeeBalanceTransferBO();
                                BalanceTransferBO.Id = Convert.ToInt32(reader["Id"]);
                                BalanceTransferBO.TransferAmount    = Convert.ToDecimal(reader["TransferAmount"]);
                                BalanceTransferBO.TransferFromEmp   = (reader["TransferFromEmp"].ToString());
                                BalanceTransferBO.TransferToEmp = (reader["TransferToEmp"].ToString());
                                BalanceTransferBO.Description   = (reader["Description"].ToString());
                                BalanceTransferBO.Status = (reader["Status"].ToString());
                                BalanceTransferBO.IsCanEdit     = Convert.ToBoolean(reader["IsCanEdit"]);
                                BalanceTransferBO.IsCanDelete   = Convert.ToBoolean(reader["IsCanDelete"]);
                                BalanceTransferBO.IsCanChecked  = Convert.ToBoolean(reader["IsCanChecked"]);
                                BalanceTransferBO.IsCanApproved = Convert.ToBoolean(reader["IsCanApproved"]);

                                BalanceTransfeList.Add(BalanceTransferBO);
                            }
                        }
                    }
                    totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                }
            }
            return BalanceTransfeList;
        }
        public bool BalanceTransferApproval(int Id, string approvedStatus, int checkedOrApprovedBy)
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
                        using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("EmpBalanceTransferApproval_SP"))
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
        public PayrollEmployeeBalanceTransferBO GetBalanceTransferById(long id)
        {
            PayrollEmployeeBalanceTransferBO BalanceTransfer = new PayrollEmployeeBalanceTransferBO();
            string query = string.Format("SELECT * FROM PayrollEmployeeBalanceTransfer WHERE Id = {0}", id);

            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetSqlStringCommand(query))
                    {
                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {

                                    BalanceTransfer.Id = Convert.ToInt64(reader["Id"]);
                                    BalanceTransfer.TransferFrom= Convert.ToInt64(reader["TransferFrom"]);
                                    BalanceTransfer.TransferTo = Convert.ToInt64(reader["TransferTo"]);
                                    BalanceTransfer.TransferAmount = Convert.ToDecimal(reader["TransferAmount"]);
                                    BalanceTransfer.Description = (reader["Description"].ToString());                                 

                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return BalanceTransfer;

        }
    }
}
