using HotelManagement.Entity.Payroll;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace HotelManagement.Data.Payroll
{
    public class CashRequisitionDA : BaseService
    {
        public bool SaveOrUpdateCashRequisition(CashRequisitionBO cashRequisitionBO, out long id, out string TransactionNo, out string TransactionTypeCash, out string ApproveStatusCash)
        {
            Boolean status = false;

            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveOrUpdateCashRequisition_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@Id", DbType.Int32, cashRequisitionBO.Id);
                        dbSmartAspects.AddInParameter(command, "@CompanyId", DbType.Int32, cashRequisitionBO.CompanyId);
                        dbSmartAspects.AddInParameter(command, "@ProjectId", DbType.Int32, cashRequisitionBO.ProjectId);
                        dbSmartAspects.AddInParameter(command, "@EmployeeId", DbType.Int32, cashRequisitionBO.EmployeeId);
                        dbSmartAspects.AddInParameter(command, "@TransactionFromAccountHeadId", DbType.Int32, cashRequisitionBO.TransactionFromAccountHeadId);
                        dbSmartAspects.AddInParameter(command, "@Amount", DbType.Decimal, cashRequisitionBO.Amount);
                        dbSmartAspects.AddInParameter(command, "@RequireDate", DbType.DateTime, cashRequisitionBO.RequireDate);
                        dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, cashRequisitionBO.Remarks);
                        dbSmartAspects.AddInParameter(command, "@UserInfoId", DbType.Int32, cashRequisitionBO.CreatedBy);
                        dbSmartAspects.AddInParameter(command, "@TransactionType", DbType.String, cashRequisitionBO.TransactionType);
                        dbSmartAspects.AddInParameter(command, "@ApproveStatus", DbType.String, cashRequisitionBO.ApprovedStatus);
                        if (cashRequisitionBO.RefId == null)
                            cashRequisitionBO.RefId = 0;
                        dbSmartAspects.AddInParameter(command, "@RefId", DbType.Int32, cashRequisitionBO.RefId);

                        dbSmartAspects.AddOutParameter(command, "@OutId", DbType.Int32, sizeof(Int32));
                        dbSmartAspects.AddOutParameter(command, "@TransactionNo", DbType.String, 100);
                        dbSmartAspects.AddOutParameter(command, "@TransactionTypeCash", DbType.String, 100);
                        dbSmartAspects.AddOutParameter(command, "@ApproveStatusCash", DbType.String, 100);

                        status = (dbSmartAspects.ExecuteNonQuery(command) > 0);
                        id = Convert.ToInt64(command.Parameters["@OutId"].Value);
                        TransactionNo = command.Parameters["@TransactionNo"].Value.ToString();
                        TransactionTypeCash = command.Parameters["@TransactionTypeCash"].Value.ToString();
                        ApproveStatusCash = command.Parameters["@ApproveStatusCash"].Value.ToString();


                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return status;
        }
        public bool DeleteCashRequisition(long id, int lastModifiedBy)
        {
            Boolean status = false;

            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteCashRequisitionMasterWithDetails_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@Id", DbType.Int32, id);
                        dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, lastModifiedBy);

                        status = (dbSmartAspects.ExecuteNonQuery(command) > 0);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return status;
        }
        public bool AdjustmentWithReturnById(long id, int adjustmentBy)
        {
            Boolean status = false;

            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("AdjustmentWithReturnById_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@Id", DbType.Int32, id);
                        dbSmartAspects.AddInParameter(command, "@AdjustmentBy", DbType.Int32, adjustmentBy);

                        status = (dbSmartAspects.ExecuteNonQuery(command) > 0);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return status;
        }

        public bool SaveOrUpdateBillVoucher(CashRequisitionBO cashRequisitionBO, List<CashRequisitionBO> RequisitionForBillVoucherNewlyAdded,
                                                  List<CashRequisitionBO> RequisitionForBillVoucherDeleted, out long id, out string TransactionNo, out string TransactionTypeCash, out string ApproveStatusCash)
        {
            Boolean status = false;
            int Recentstatus = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveOrUpdateCashRequisition_SP"))
                        {
                            command.Parameters.Clear();

                            dbSmartAspects.AddInParameter(command, "@Id", DbType.Int32, cashRequisitionBO.Id);
                            dbSmartAspects.AddInParameter(command, "@CompanyId", DbType.Int32, cashRequisitionBO.CompanyId);
                            dbSmartAspects.AddInParameter(command, "@ProjectId", DbType.Int32, cashRequisitionBO.ProjectId);
                            dbSmartAspects.AddInParameter(command, "@EmployeeId", DbType.Int32, cashRequisitionBO.EmployeeId);
                            dbSmartAspects.AddInParameter(command, "@TransactionFromAccountHeadId", DbType.Int32, cashRequisitionBO.TransactionFromAccountHeadId);
                            dbSmartAspects.AddInParameter(command, "@Amount", DbType.Decimal, cashRequisitionBO.Amount);
                            dbSmartAspects.AddInParameter(command, "@RequireDate", DbType.DateTime, DBNull.Value);
                            dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, cashRequisitionBO.Remarks);
                            dbSmartAspects.AddInParameter(command, "@UserInfoId", DbType.Int32, cashRequisitionBO.CreatedBy);
                            dbSmartAspects.AddInParameter(command, "@TransactionType", DbType.String, cashRequisitionBO.TransactionType);
                            dbSmartAspects.AddInParameter(command, "@ApproveStatus", DbType.String, cashRequisitionBO.ApprovedStatus);
                            if (cashRequisitionBO.RefId == null)
                                cashRequisitionBO.RefId = 0;
                            dbSmartAspects.AddInParameter(command, "@RefId", DbType.Int32, cashRequisitionBO.RefId);

                            dbSmartAspects.AddOutParameter(command, "@OutId", DbType.Int32, sizeof(Int32));
                            dbSmartAspects.AddOutParameter(command, "@TransactionNo", DbType.String, 100);
                            dbSmartAspects.AddOutParameter(command, "@TransactionTypeCash", DbType.String, 100);
                            dbSmartAspects.AddOutParameter(command, "@ApproveStatusCash", DbType.String, 100);

                            Recentstatus = dbSmartAspects.ExecuteNonQuery(command, transction);
                            id = Convert.ToInt64(command.Parameters["@OutId"].Value);
                            TransactionNo = command.Parameters["@TransactionNo"].Value.ToString();
                            TransactionTypeCash = command.Parameters["@TransactionTypeCash"].Value.ToString();
                            ApproveStatusCash = command.Parameters["@ApproveStatusCash"].Value.ToString();


                        }

                        if (Recentstatus > 0 && RequisitionForBillVoucherNewlyAdded.Count > 0)
                        {
                            foreach (CashRequisitionBO bVoucher in RequisitionForBillVoucherNewlyAdded)
                            {
                                using (DbCommand cmdDetails = dbSmartAspects.GetStoredProcCommand("SaveOrUpdateCashRequisitionDetails_SP"))
                                {
                                    cmdDetails.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(cmdDetails, "@CashRequisitionId", DbType.Int32, id);
                                    dbSmartAspects.AddInParameter(cmdDetails, "@RequisitionForHeadId", DbType.Int32, bVoucher.CostCenterId);
                                    if (cashRequisitionBO.TransactionType == "Bill Voucher")
                                    {
                                        dbSmartAspects.AddInParameter(cmdDetails, "@CompanyId", DbType.Int32, cashRequisitionBO.CompanyId);
                                        dbSmartAspects.AddInParameter(cmdDetails, "@ProjectId", DbType.Int32, cashRequisitionBO.ProjectId);

                                    }
                                    else
                                    {
                                        dbSmartAspects.AddInParameter(cmdDetails, "@CompanyId", DbType.Int32, bVoucher.CompanyId);
                                        dbSmartAspects.AddInParameter(cmdDetails, "@ProjectId", DbType.Int32, bVoucher.ProjectId);

                                    }

                                    if (bVoucher.SupplierId > 0)
                                    {
                                        dbSmartAspects.AddInParameter(cmdDetails, "@SupplierId", DbType.Int32, bVoucher.SupplierId);
                                    }
                                    else
                                    {
                                        dbSmartAspects.AddInParameter(cmdDetails, "@SupplierId", DbType.Int32, DBNull.Value);
                                    }

                                    dbSmartAspects.AddInParameter(cmdDetails, "@RequisitionForHeadName", DbType.String, bVoucher.CostCenterName);
                                    dbSmartAspects.AddInParameter(cmdDetails, "@RequsitionAmount", DbType.Decimal, bVoucher.RequsitionAmount);
                                    dbSmartAspects.AddInParameter(cmdDetails, "@Remarks", DbType.String, bVoucher.Remarks);
                                    dbSmartAspects.AddInParameter(cmdDetails, "@DetailId", DbType.Int32, bVoucher.DetailId);

                                    Recentstatus = dbSmartAspects.ExecuteNonQuery(cmdDetails, transction);
                                }
                            }
                        }
                        if (Recentstatus > 0 && RequisitionForBillVoucherDeleted.Count > 0)
                        {
                            using (DbCommand commandDelete = dbSmartAspects.GetStoredProcCommand("DeleteCashRequisitionDetails_SP"))
                            {
                                foreach (CashRequisitionBO bVoucher in RequisitionForBillVoucherDeleted)
                                {
                                    commandDelete.Parameters.Clear();
                                    dbSmartAspects.AddInParameter(commandDelete, "@CashRequisitionId", DbType.Int32, id);
                                    dbSmartAspects.AddInParameter(commandDelete, "@RequisitionForHeadId", DbType.Int32, bVoucher.CostCenterId);
                                    dbSmartAspects.AddInParameter(commandDelete, "@RequisitionForHeadName", DbType.String, bVoucher.CostCenterName);
                                    dbSmartAspects.AddInParameter(commandDelete, "@RequsitionAmount", DbType.Decimal, bVoucher.RequsitionAmount);
                                    dbSmartAspects.AddInParameter(commandDelete, "@Remarks", DbType.String, bVoucher.Remarks);
                                    dbSmartAspects.AddInParameter(commandDelete, "@DetailId", DbType.Int32, bVoucher.DetailId);

                                    Recentstatus = dbSmartAspects.ExecuteNonQuery(commandDelete, transction);
                                }
                            }
                        }
                        if (Recentstatus > 0)
                        {
                            status = true;
                            transction.Commit();
                        }
                        else
                        {
                            status = false;
                            transction.Rollback();
                        }
                    }
                    catch (Exception ex)
                    {
                        status = false;
                        transction.Rollback();
                        throw ex;
                    }
                }
            }

            return status;
        }
        public bool ApprovalStatusUpdate(int isAdminUser, long id, string ApprovalStatusUpdate, int checkedOrApprovedBy, out string TransactionNo, out string TransactionTypeCash, out string ApproveStatusCash)
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
                        using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("ApprovalStatusUpdate_SP"))
                        {
                            dbSmartAspects.AddInParameter(commandMaster, "@IsAdminUser", DbType.Int32, isAdminUser);
                            dbSmartAspects.AddInParameter(commandMaster, "@Id", DbType.Int32, id);
                            dbSmartAspects.AddInParameter(commandMaster, "@ApproveStatus", DbType.String, ApprovalStatusUpdate);
                            dbSmartAspects.AddInParameter(commandMaster, "@CheckedOrApprovedBy", DbType.Int32, checkedOrApprovedBy);
                            dbSmartAspects.AddOutParameter(commandMaster, "@TransactionNo", DbType.String, 100);
                            dbSmartAspects.AddOutParameter(commandMaster, "@TransactionTypeCash", DbType.String, 100);
                            dbSmartAspects.AddOutParameter(commandMaster, "@ApproveStatusCash", DbType.String, 100);

                            status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);
                            TransactionNo = commandMaster.Parameters["@TransactionNo"].Value.ToString();
                            TransactionTypeCash = commandMaster.Parameters["@TransactionTypeCash"].Value.ToString();
                            ApproveStatusCash = commandMaster.Parameters["@ApproveStatusCash"].Value.ToString();
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
        public CashRequisitionBO GetCashRequisitionByTransactionNo(string transactionNo)
        {
            CashRequisitionBO cashRequisitionBO = new CashRequisitionBO();

            string query = string.Format("SELECT * FROM CashRequisition WHERE TransactionNo = {0}", "'" + transactionNo + "'");

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
                                cashRequisitionBO.Id = Convert.ToInt64(reader["Id"]);
                                cashRequisitionBO.EmployeeId = Convert.ToInt32(reader["EmployeeId"]);
                                cashRequisitionBO.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                cashRequisitionBO.ProjectId = Convert.ToInt32(reader["ProjectId"]);
                                cashRequisitionBO.ApprovedStatus = reader["ApproveStatus"].ToString();
                                if (reader["Amount"] != DBNull.Value)
                                {
                                    cashRequisitionBO.Amount = Convert.ToDecimal(reader["Amount"]);
                                }
                                if (reader["RequireDate"] != DBNull.Value)
                                {
                                    cashRequisitionBO.RequireDate = Convert.ToDateTime(reader["RequireDate"]);
                                }
                                if (reader["RemainingBalance"] != DBNull.Value)
                                {
                                    cashRequisitionBO.RemainingAmount = Convert.ToDecimal(reader["RemainingBalance"]);
                                }
                                if (reader["Remarks"] != DBNull.Value)
                                {
                                    cashRequisitionBO.Remarks = reader["Remarks"].ToString();
                                }
                            }
                        }
                    }

                }
            }
            return cashRequisitionBO;
        }
        public List<CashRequisitionBO> GetCashRequisitionForGridPaging(int userInfoId, int empId, int CompanyId, int ProjectId, int AssignEmployeeId, String transactionType, DateTime? fromDate, DateTime? toDate,
                                                                        string fromAmount, string toAmount, string transactionNo, string adjustmentNo, string status, string Remarks, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<CashRequisitionBO> CashRequisitionBOList = new List<CashRequisitionBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCashRequisitionForGridPaging_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    if (userInfoId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@UserId", DbType.Int32, userInfoId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@UserId", DbType.Int32, DBNull.Value);

                    if (empId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, empId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, DBNull.Value);

                    if (CompanyId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, CompanyId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, DBNull.Value);

                    if (ProjectId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int32, ProjectId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int32, DBNull.Value);

                    if (AssignEmployeeId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@AssignEmployeeId", DbType.Int32, AssignEmployeeId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@AssignEmployeeId", DbType.Int32, DBNull.Value);

                    if (!string.IsNullOrEmpty(transactionType))
                        dbSmartAspects.AddInParameter(cmd, "@transactionType", DbType.String, transactionType);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@transactionType", DbType.String, DBNull.Value);

                    if (fromDate != null)
                        dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, Convert.ToDateTime(fromDate));
                    else
                        dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, DBNull.Value);

                    if (toDate != null)
                        dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, Convert.ToDateTime(toDate));
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, DBNull.Value);

                    if (!string.IsNullOrEmpty(fromAmount))
                        dbSmartAspects.AddInParameter(cmd, "@FromAmount", DbType.Decimal, Convert.ToDecimal(fromAmount));
                    else
                        dbSmartAspects.AddInParameter(cmd, "@FromAmount", DbType.Decimal, DBNull.Value);

                    if (!string.IsNullOrEmpty(toAmount))
                        dbSmartAspects.AddInParameter(cmd, "@ToAmount", DbType.Decimal, Convert.ToDecimal(toAmount));
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ToAmount", DbType.Decimal, DBNull.Value);
                    if (!string.IsNullOrEmpty(status))
                        dbSmartAspects.AddInParameter(cmd, "@Status", DbType.String, status);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@Status", DbType.String, DBNull.Value);
                    if (!string.IsNullOrEmpty(transactionNo))
                        dbSmartAspects.AddInParameter(cmd, "@TransactionNo", DbType.String, transactionNo);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@TransactionNo", DbType.String, DBNull.Value);

                    if (!string.IsNullOrEmpty(adjustmentNo))
                        dbSmartAspects.AddInParameter(cmd, "@AdjustmentNo", DbType.String, adjustmentNo);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@AdjustmentNo", DbType.String, DBNull.Value);

                    if (!string.IsNullOrEmpty(Remarks))
                        dbSmartAspects.AddInParameter(cmd, "@Remarks", DbType.String, Remarks);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@Remarks", DbType.String, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);

                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                    DataSet CashDS = new DataSet();

                    dbSmartAspects.LoadDataSet(cmd, CashDS, "Cash");
                    DataTable Table = CashDS.Tables["Cash"];

                    CashRequisitionBOList = Table.AsEnumerable().Select(r => new CashRequisitionBO
                    {
                        Id = r.Field<Int64>("Id"),
                        TransactionNo = r.Field<string>("TransactionNo"),
                        TransactionType = r.Field<string>("TransactionType"),
                        RequsitionBy = r.Field<string>("RequsitionBy"),
                        EmployeeName = r.Field<string>("EmployeeName"),
                        EmployeeId = r.Field<int>("EmployeeId"),
                        Amount = r.Field<decimal>("Amount"),
                        RequireDate = r.Field<DateTime?>("RequireDate"),
                        ApprovedStatus = r.Field<string>("ApproveStatus"),
                        Remarks = r.Field<string>("Remarks"),
                        CreatedBy = r.Field<int>("CreatedBy"),
                        CreatedDate = r.Field<DateTime>("CreatedDate"),
                        RemainingAmount = r.Field<decimal>("RemainingBalance"),
                        LastAdjustmentDate = r.Field<DateTime?>("LastAdjustmentDate"),
                        RemainingAdjustmentDay = r.Field<int>("RemainingAdjustmentDay"),
                        HaveCashRequisitionAdjustment = r.Field<bool>("HaveCashRequisitionAdjustment"),
                        IsCanEdit = r.Field<bool>("IsCanEdit"),
                        IsCanDelete = r.Field<bool>("IsCanDelete"),
                        IsCanCheck = r.Field<bool>("IsCanCheck"),
                        SerialNumber = r.Field<Int64>("SerialNumber"),
                        IsCanApprove = r.Field<bool>("IsCanApprove"),
                        HasPermissionForChild = r.Field<bool>("HasPermissionForChild"),
                        AdjustmentIdList = r.Field<string>("AdjustmentIdList"),
                        AuthorizedByList = r.Field<string>("AuthorizedByList")
                    }).ToList();

                    totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                }
            }
            return CashRequisitionBOList;
        }

        public List<CashRequisitionBO> GetALLAdjustmentForCashRequisitionById(int userInfoId, int CashRequisitionId, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<CashRequisitionBO> CashRequisitionBOList = new List<CashRequisitionBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetALLAdjustmentForCashRequisitionByIdForGridPaging_SP"))
                {
                    if (userInfoId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@UserId", DbType.Int32, userInfoId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@UserId", DbType.Int32, DBNull.Value);


                    if (CashRequisitionId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@CashRequisitionId", DbType.Int32, CashRequisitionId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CashRequisitionId", DbType.Int32, DBNull.Value);


                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);

                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                    DataSet CashDS = new DataSet();

                    dbSmartAspects.LoadDataSet(cmd, CashDS, "Cash");
                    DataTable Table = CashDS.Tables["Cash"];

                    CashRequisitionBOList = Table.AsEnumerable().Select(r => new CashRequisitionBO
                    {
                        Id = r.Field<Int64>("Id"),
                        TransactionType = r.Field<string>("TransactionType"),
                        TransactionNo = r.Field<string>("TransactionNo"),
                        RequsitionBy = r.Field<string>("RequsitionBy"),
                        EmployeeName = r.Field<string>("EmployeeName"),
                        EmployeeId = r.Field<int>("EmployeeId"),
                        Amount = r.Field<decimal>("Amount"),
                        ApprovedStatus = r.Field<string>("ApproveStatus"),
                        Remarks = r.Field<string>("Remarks"),
                        CreatedBy = r.Field<int>("CreatedBy"),
                        CreatedDate = r.Field<DateTime>("CreatedDate"),
                        IsCanEdit = r.Field<bool>("IsCanEdit"),
                        IsCanDelete = r.Field<bool>("IsCanDelete"),
                        IsCanCheck = r.Field<bool>("IsCanCheck"),
                        IsCanApprove = r.Field<bool>("IsCanApprove"),
                        AuthorizedByList = r.Field<string>("AuthorizedByList")
                    }).ToList();

                    totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                }
            }
            return CashRequisitionBOList;
        }

        public List<CashRequisitionBO> GetBillVoucherById(long id)
        {
            List<CashRequisitionBO> CashRequisitionBOList = new List<CashRequisitionBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetBillVoucherById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@Id", DbType.Int64, id);

                    DataSet CashDS = new DataSet();

                    dbSmartAspects.LoadDataSet(cmd, CashDS, "Cash");
                    DataTable Table = CashDS.Tables["Cash"];

                    CashRequisitionBOList = Table.AsEnumerable().Select(r => new CashRequisitionBO
                    {
                        Id = r.Field<Int64>("Id"),
                        CompanyId = r.Field<Int32>("CompanyId"),
                        ProjectId = r.Field<Int32>("ProjectId"),
                        TransactionType = r.Field<string>("TransactionType"),
                        RequsitionBy = r.Field<string>("RequsitionBy"),
                        EmployeeName = r.Field<string>("EmployeeName"),
                        EmployeeId = r.Field<int>("EmployeeId"),
                        TransactionFromAccountHeadId = r.Field<int>("TransactionFromAccountHeadId"),
                        Amount = r.Field<decimal>("Amount"),
                        ApprovedStatus = r.Field<string>("ApproveStatus"),
                        Remarks = r.Field<string>("Remarks"),
                        CreatedBy = r.Field<int>("CreatedBy"),
                        CreatedDate = r.Field<DateTime>("CreatedDate"),
                        DetailId = r.Field<Int64>("CRDetailsId"),
                        RequsitionAmount = r.Field<decimal>("RequisitionAmount"),
                        RequisitionForHeadId = r.Field<int>("RequisitionForHeadId"),
                        RequisitionForHeadName = r.Field<string>("RequisitionForHeadName"),
                        SupplierId = r.Field<int>("SupplierId"),
                        IndividualRemarks = r.Field<string>("IndividualRemarks"),
                        IndividualCompanyId = r.Field<int>("IndividualCompanyId"),
                        IndividualProjectId = r.Field<int>("IndividualProjectId"),
                        IndividualCompanyName = r.Field<string>("IndividualCompanyName"),
                        IndividualProjectName = r.Field<string>("IndividualProjectName")

                    }).ToList();

                }
            }
            return CashRequisitionBOList;
        }

        public CashRequisitionBO GetRequsitionById(long id)
        {
            CashRequisitionBO cashRequisitionBO = new CashRequisitionBO();

            string query = string.Format("SELECT * FROM CashRequisition WHERE Id = {0}", id);

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
                                cashRequisitionBO.Id = Convert.ToInt64(reader["Id"]);
                                cashRequisitionBO.EmployeeId = Convert.ToInt32(reader["EmployeeId"]);
                                cashRequisitionBO.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                cashRequisitionBO.ProjectId = Convert.ToInt32(reader["ProjectId"]);
                                cashRequisitionBO.ApprovedStatus = reader["ApproveStatus"].ToString();
                                if (reader["Amount"] != DBNull.Value)
                                {
                                    cashRequisitionBO.Amount = Convert.ToDecimal(reader["Amount"]);
                                }
                                if (reader["TransactionFromAccountHeadId"] != DBNull.Value)
                                {
                                    cashRequisitionBO.TransactionFromAccountHeadId = Convert.ToInt32(reader["TransactionFromAccountHeadId"]);
                                }
                                else
                                {
                                    cashRequisitionBO.TransactionFromAccountHeadId = 0;
                                }
                                if (reader["RequireDate"] != DBNull.Value)
                                {
                                    cashRequisitionBO.RequireDate = Convert.ToDateTime(reader["RequireDate"]);
                                }
                                if (reader["RemainingBalance"] != DBNull.Value)
                                {
                                    cashRequisitionBO.RemainingAmount = Convert.ToDecimal(reader["RemainingBalance"]);
                                }
                                if (reader["Remarks"] != DBNull.Value)
                                {
                                    cashRequisitionBO.Remarks = reader["Remarks"].ToString();
                                }
                            }
                        }
                    }

                }
            }
            return cashRequisitionBO;
        }

        public List<CashRequisitionBO> GetCashRequisitionNAdjustmentForReport(int companyId, int projectId, int employeeId, string transactionTypeId, DateTime? fromDate, DateTime? toDate, string fromAmount, string toAmount, string transactionNo, string adjustmentNo, string status, string remarks)
        {
            List<CashRequisitionBO> CashRequisitionBOList = new List<CashRequisitionBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCashRequisitionNAdjustmentForReport_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    if (companyId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, companyId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, DBNull.Value);

                    if (projectId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int32, projectId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int32, DBNull.Value);

                    if (employeeId > 0)
                        dbSmartAspects.AddInParameter(cmd, "@EmployeeId", DbType.Int32, employeeId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@EmployeeId", DbType.Int32, DBNull.Value);

                    if (!string.IsNullOrEmpty(transactionTypeId))
                        dbSmartAspects.AddInParameter(cmd, "@TransactionType", DbType.String, transactionTypeId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@TransactionType", DbType.String, DBNull.Value);

                    if (fromDate != null)
                        dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, Convert.ToDateTime(fromDate));
                    else
                        dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, DBNull.Value);

                    if (toDate != null)
                        dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, Convert.ToDateTime(toDate));
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, DBNull.Value);
                    if (!string.IsNullOrEmpty(fromAmount))
                        dbSmartAspects.AddInParameter(cmd, "@FromAmount", DbType.Decimal, Convert.ToDecimal(fromAmount));
                    else
                        dbSmartAspects.AddInParameter(cmd, "@FromAmount", DbType.Decimal, DBNull.Value);

                    if (!string.IsNullOrEmpty(toAmount))
                        dbSmartAspects.AddInParameter(cmd, "@ToAmount", DbType.Decimal, Convert.ToDecimal(toAmount));
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ToAmount", DbType.Decimal, DBNull.Value);
                    if (!string.IsNullOrEmpty(status))
                        dbSmartAspects.AddInParameter(cmd, "@Status", DbType.String, status);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@Status", DbType.String, DBNull.Value);
                    if (!string.IsNullOrEmpty(transactionNo))
                        dbSmartAspects.AddInParameter(cmd, "@TransactionNo", DbType.String, transactionNo);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@TransactionNo", DbType.String, DBNull.Value);

                    if (!string.IsNullOrEmpty(adjustmentNo))
                        dbSmartAspects.AddInParameter(cmd, "@AdjustmentNo", DbType.String, adjustmentNo);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@AdjustmentNo", DbType.String, DBNull.Value);

                    if (!string.IsNullOrEmpty(remarks))
                        dbSmartAspects.AddInParameter(cmd, "@Remarks", DbType.String, remarks);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@Remarks", DbType.String, DBNull.Value);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                CashRequisitionBO CashRequisitionBo = new CashRequisitionBO();

                                CashRequisitionBo.TransactionNo = Convert.ToString(reader["TransactionNo"]);
                                //CashRequisitionBo.RequisitionDate = Convert.ToDateTime(reader["RequisitionDate"]);
                                //CashRequisitionBo.RequisitionDateDisplay = reader["RequisitionDateDisplay"].ToString();
                                CashRequisitionBo.Employee = Convert.ToString(reader["Employee"]);
                                CashRequisitionBo.Amount = Convert.ToDecimal(reader["Amount"]);
                                CashRequisitionBo.Company = Convert.ToString(reader["Company"]);
                                CashRequisitionBo.Project = Convert.ToString(reader["Project"]);
                                CashRequisitionBo.RequisitionFor = Convert.ToString(reader["RequisitionFor"]);
                                if (reader["VoucherDate"] != DBNull.Value)
                                {
                                    CashRequisitionBo.VoucherDate = Convert.ToDateTime(reader["VoucherDate"]);
                                    CashRequisitionBo.VoucherDateDisplay = reader["VoucherDateDisplay"].ToString();
                                }
                                
                                CashRequisitionBo.VoucherNo = Convert.ToString(reader["VoucherNo"]);
                                CashRequisitionBo.VoucherNarration = Convert.ToString(reader["VoucherNarration"]);
                                CashRequisitionBo.RemainingBalance = Convert.ToDecimal(reader["RemainingBalance"]);
                                CashRequisitionBo.AdjustmentNo = Convert.ToString(reader["AdjustmentNo"]);
                                if (reader["AdjustmentDate"] != DBNull.Value)
                                {
                                    CashRequisitionBo.AdjustmentDate = Convert.ToDateTime(reader["AdjustmentDate"]);
                                    CashRequisitionBo.AdjustmentDateDisplay = reader["AdjustmentDateDisplay"].ToString();
                                }
                                
                                CashRequisitionBo.AdjustmentAmount = Convert.ToDecimal(reader["AdjustmentAmount"]);
                                CashRequisitionBo.AdjustmentCompany = Convert.ToString(reader["AdjustmentCompany"]);
                                CashRequisitionBo.AdjustmentProject = Convert.ToString(reader["AdjustmentProject"]);
                                CashRequisitionBo.AdjustmentPurpose = Convert.ToString(reader["AdjustmentPurpose"]);

                                if (reader["TransactionNo"] != DBNull.Value)
                                {
                                    CashRequisitionBo.TransactionNo = Convert.ToString(reader["TransactionNo"]);
                                }
                                if (reader["RequisitionDate"] != DBNull.Value)
                                {
                                    CashRequisitionBo.RequisitionDate = Convert.ToDateTime(reader["RequisitionDate"]);
                                }
                                if (reader["RequisitionDateDisplay"] != DBNull.Value)
                                {
                                    CashRequisitionBo.RequisitionDateDisplay = reader["RequisitionDateDisplay"].ToString();
                                }
                                if (reader["Employee"] != DBNull.Value)
                                {
                                    CashRequisitionBo.Employee = Convert.ToString(reader["Employee"]);
                                }
                                if (reader["Amount"] != DBNull.Value)
                                {
                                    CashRequisitionBo.Amount = Convert.ToDecimal(reader["Amount"]);
                                }
                                if (reader["Company"] != DBNull.Value)
                                {
                                    CashRequisitionBo.Company = Convert.ToString(reader["Company"]);
                                }
                                if (reader["Project"] != DBNull.Value)
                                {
                                    CashRequisitionBo.Project = Convert.ToString(reader["Project"]);
                                }
                                if (reader["RequisitionFor"] != DBNull.Value)
                                {
                                    CashRequisitionBo.RequisitionFor = Convert.ToString(reader["RequisitionFor"]);
                                }
                                //if (reader["VoucherDate"] != DBNull.Value)
                                //{
                                //    CashRequisitionBo.VoucherDate = Convert.ToDateTime(reader["VoucherDate"]);
                                //}
                                //if (reader["VoucherDateDisplay"] != DBNull.Value)
                                //{
                                //    CashRequisitionBo.VoucherDateDisplay = Convert.ToDateTime(reader["VoucherDateDisplay"]).ToString();
                                //}
                                if (reader["VoucherNo"] != DBNull.Value)
                                {
                                    CashRequisitionBo.VoucherNo = Convert.ToString(reader["VoucherNo"]);
                                }
                                if (reader["VoucherNarration"] != DBNull.Value)
                                {
                                    CashRequisitionBo.VoucherNarration = Convert.ToString(reader["VoucherNarration"]);
                                }
                                if (reader["RemainingBalance"] != DBNull.Value)
                                {
                                    CashRequisitionBo.RemainingBalance = Convert.ToDecimal(reader["RemainingBalance"]);
                                }
                                if (reader["AdjustmentNo"] != DBNull.Value)
                                {
                                    CashRequisitionBo.AdjustmentNo = Convert.ToString(reader["AdjustmentNo"]);
                                }
                                //if (reader["AdjustmentDate"] != DBNull.Value)
                                //{
                                //    CashRequisitionBo.AdjustmentDate = Convert.ToDateTime(reader["AdjustmentDate"]);
                                //}
                                //if (reader["AdjustmentDateDisplay"] != DBNull.Value)
                                //{
                                //    CashRequisitionBo.AdjustmentDateDisplay = Convert.ToDateTime(reader["AdjustmentDateDisplay"]).ToString();
                                //}
                                if (reader["AdjustmentAmount"] != DBNull.Value)
                                {
                                    CashRequisitionBo.AdjustmentAmount = Convert.ToDecimal(reader["AdjustmentAmount"]);
                                }
                                if (reader["AdjustmentCompany"] != DBNull.Value)
                                {
                                    CashRequisitionBo.AdjustmentCompany = Convert.ToString(reader["AdjustmentCompany"]);
                                }
                                if (reader["AdjustmentProject"] != DBNull.Value)
                                {
                                    CashRequisitionBo.AdjustmentProject = Convert.ToString(reader["AdjustmentProject"]);
                                }
                                if (reader["AdjustmentPurpose"] != DBNull.Value)
                                {
                                    CashRequisitionBo.AdjustmentPurpose = Convert.ToString(reader["AdjustmentPurpose"]);
                                }

                                CashRequisitionBOList.Add(CashRequisitionBo);
                            }
                        }
                    }
                }
            }
            return CashRequisitionBOList;
        }

        public List<CashRequisitionBO> GetCashRequisitionByIdForInvoice(int id)
        {
            List<CashRequisitionBO> CashRequisitionBOList = new List<CashRequisitionBO>();
            CashRequisitionBO BO = new CashRequisitionBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCashRequisitionInvoiceInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@Id", DbType.Int32, id);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {

                                BO.Id = Convert.ToInt32(reader["Id"]);
                                if ((reader["RequireDate"] != DBNull.Value))
                                {
                                    BO.RequireDate = Convert.ToDateTime(reader["RequireDate"]);
                                }
                                if ((reader["CreatedDate"] != DBNull.Value))
                                {
                                    BO.CreatedDate = Convert.ToDateTime(reader["CreatedDate"]);
                                }
                                BO.EmployeeName = reader["EmployeeName"].ToString();
                                BO.IndividualProjectName = reader["IndividualProjectName"].ToString();
                                BO.IndividualCompanyName = reader["IndividualCompanyName"].ToString();
                                BO.TransactionFromAccountHeadId = Convert.ToInt32(reader["TransactionFromAccountHeadId"]);
                                BO.TransactionFromAccountHead = reader["TransactionFromAccountHead"].ToString();
                                BO.Remarks = reader["Remarks"].ToString();
                                BO.DepartmentName = reader["DepartmentName"].ToString();
                                BO.DesignationName = reader["DesignationName"].ToString();
                                BO.Amount = Convert.ToDecimal(reader["Amount"]);
                                BO.TransactionNo = (reader["TransactionNo"].ToString());
                                BO.TransactionType = (reader["TransactionType"].ToString());

                                BO.RequsitionDateString = reader["RequsitionDateString"].ToString();
                                BO.RequireDateString = reader["RequireDateString"].ToString();
                                BO.ApproveDateString = reader["ApproveDateString"].ToString();
                                BO.OfficialEmail = reader["OfficialEmail"].ToString();
                                BO.OfficialPhone = reader["OfficialPhone"].ToString();
                                BO.RemainingBalance = Convert.ToDecimal(reader["RemainingBalance"]);
                            }
                        }
                    }
                }
                CashRequisitionBOList.Add(BO);
            }
            return CashRequisitionBOList;
        }

        public List<CashRequisitionBO> GetCashRequisitionDetailsByIdForInvoice(int id)
        {
            List<CashRequisitionBO> CashRequisitionBOList = new List<CashRequisitionBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCashRequisitionInvoiceDetailsInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@Id", DbType.Int32, id);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                CashRequisitionBO BO = new CashRequisitionBO();
                                BO.Id = Convert.ToInt32(reader["Id"]);
                                BO.RequisitionForHeadName = reader["RequisitionForHeadName"].ToString();
                                BO.Amount = Convert.ToDecimal(reader["Amount"]);
                                CashRequisitionBOList.Add(BO);
                            }
                        }
                    }
                }
            }
            return CashRequisitionBOList;
        }
    }
}
